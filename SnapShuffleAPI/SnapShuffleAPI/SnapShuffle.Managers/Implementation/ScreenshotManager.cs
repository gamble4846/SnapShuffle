﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SnapShuffle.DataAccess.Interface;
using SnapShuffle.Managers.Interface;
using SnapShuffle.Models;
using SnapShuffle.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;

namespace SnapShuffle.Managers.Implementation
{
    public class ScreenshotManager : IScreenshotManager
    {
        private CommonFunctions _CommonFunctions { get; set; }
        private ITbScreenShotDataAccess _TbScreenShotDataAccess { get; set; }

        public ScreenshotManager(IHttpContextAccessor httpContextAccessor, ITbScreenShotDataAccess tbScreenShotDataAccess)
        {
            _CommonFunctions = new CommonFunctions(httpContextAccessor);
            _TbScreenShotDataAccess = tbScreenShotDataAccess;
        }

        public async Task<APIResponse> GetRandom()
        {
            var CurrentId = _CommonFunctions.GetRandomPrintScreenId();

            var TbScreenShotData = _TbScreenShotDataAccess.GetByPrintScreenId(CurrentId);
            bool IsNew = false;

            if(TbScreenShotData == null)
            {
                string src = null;
                byte[] imageBytes = null;
                while (src == null || imageBytes == null)
                {
                    CurrentId = _CommonFunctions.GetRandomPrintScreenId();
                    var PrintScreenURl = "https://prnt.sc/" + CurrentId;
                    WebClient client = new WebClient();
                    client.Headers.Add("user-agent", "Only a test!");
                    string htmlCode = client.DownloadString(PrintScreenURl);
                    src = _CommonFunctions.GetSrcOfTagFromHTMLById(htmlCode, "screenshot-image");
                    imageBytes = _CommonFunctions.ConvertUrlToByteArray(src);
                }

                var ImgurResponse = await _CommonFunctions.UploadImageToImgur(imageBytes);

                if (ImgurResponse == null)
                {
                    return new APIResponse(ResponseCode.ERROR, "Error Uploading Image to Imgur", null, false);
                }

                var ToAddModel = new tbScreenShotModel()
                {
                    ScreenshotGUID = Guid.NewGuid(),
                    PrintScreenId = CurrentId,
                    NewImgurLink = ImgurResponse.data.link
                };

                var NewGUID = _TbScreenShotDataAccess.Add(ToAddModel);
                TbScreenShotData = _TbScreenShotDataAccess.GetByPrintScreenId(CurrentId);

                if (TbScreenShotData == null)
                {
                    return new APIResponse(ResponseCode.ERROR, "Some Error Occured", NewGUID, false);
                }
                IsNew = true;
            }



            dynamic FinalResponse = new System.Dynamic.ExpandoObject();
            FinalResponse.TbScreenShotData = TbScreenShotData;
            FinalResponse.IsNew = IsNew;

            return new APIResponse(ResponseCode.SUCCESS, JsonConvert.SerializeObject(FinalResponse), false); ;
        }
    }
}

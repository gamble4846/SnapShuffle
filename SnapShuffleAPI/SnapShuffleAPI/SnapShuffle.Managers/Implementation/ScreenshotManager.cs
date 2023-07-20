using Microsoft.AspNetCore.Http;
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

namespace SnapShuffle.Managers.Implementation
{
    public class ScreenshotManager : IScreenshotManager
    {
        private CommonFunctions _CommonFunctions { get; set; }

        public ScreenshotManager(IHttpContextAccessor httpContextAccessor)
        {
            _CommonFunctions = new CommonFunctions(httpContextAccessor);
        }

        public async Task<APIResponse> GetRandom()
        {
            var CurrentId = _CommonFunctions.GetRandomPrintScreenId();

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

            if(ImgurResponse == null)
            {
                return new APIResponse(ResponseCode.ERROR, "Error Uploading Image to Imgur", null, false);
            }

            return new APIResponse(ResponseCode.SUCCESS, "Image Found", "https://i.imgur.com/" + ImgurResponse.data.id, false); ;
        }
    }
}

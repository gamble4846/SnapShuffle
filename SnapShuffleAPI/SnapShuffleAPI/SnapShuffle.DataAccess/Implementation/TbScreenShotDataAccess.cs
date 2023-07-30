using EasyCrudLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnapShuffle.DataAccess.Interface;
using SnapShuffle.Models;
using SnapShuffle.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapShuffle.DataAccess.Implementation
{
    public class TbScreenShotDataAccess : ITbScreenShotDataAccess
    {
        private CommonFunctions _CommonFunctions { get; set; }
        private string ConnectionString { get; set; }
        public TbScreenShotDataAccess(IHttpContextAccessor httpContextAccessor)
        {
            _CommonFunctions = new CommonFunctions(httpContextAccessor);
            ConnectionString = _CommonFunctions.GetConntectionString();
        }

        public tbScreenShotModel GetByPrintScreenId(string PrintScreenId)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@PrintScreenId", PrintScreenId));
            var WhereCondition = " WHERE OldImageLink = @PrintScreenId ";
            return _EC.GetFirstOrDefault<tbScreenShotModel>(WhereCondition, Parameters, GSEnums.WithInQuery.NoLock);
        }

        public string Add(tbScreenShotModel model)
        {
            var _EC = new EasyCrud(ConnectionString);
            return _EC.Add<tbScreenShotModel>(model, "ScreenshotGUID", "ScreenshotGUID", true);
        }
    }
}

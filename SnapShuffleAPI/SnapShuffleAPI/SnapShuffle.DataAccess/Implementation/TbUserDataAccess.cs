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
    public class TbUserDataAccess : ITbUserDataAccess
    {
        private CommonFunctions _CommonFunctions { get; set; }
        private string ConnectionString { get; set; }
        public TbUserDataAccess(IHttpContextAccessor httpContextAccessor)
        {
            _CommonFunctions = new CommonFunctions(httpContextAccessor);
            ConnectionString = _CommonFunctions.GetConntectionString();
        }

        public string Register(tbUserModel model)
        {
            var _EC = new EasyCrud(ConnectionString);
            return _EC.Add<tbUserModel>(model, "UserGuid", "UserGuid",true);
        }

        public tbUserModel GetByEmail(string email)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@Email", email));
            var WhereCondition = " WHERE Email = @Email ";
            return _EC.GetFirstOrDefault<tbUserModel>(WhereCondition, Parameters,GSEnums.WithInQuery.NoLock);
        }

        public tbUserModel GetByUsername(string username)
        {
            var _EC = new EasyCrud(ConnectionString);
            List<SqlParameter> Parameters = new List<SqlParameter>();
            Parameters.Add(new SqlParameter("@UserName", username));
            var WhereCondition = " WHERE UserName = @UserName ";
            return _EC.GetFirstOrDefault<tbUserModel>(WhereCondition, Parameters, GSEnums.WithInQuery.NoLock);
        }
    }
}

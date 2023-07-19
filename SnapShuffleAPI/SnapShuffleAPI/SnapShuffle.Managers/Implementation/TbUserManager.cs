using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SnapShuffle.DataAccess.Interface;
using SnapShuffle.Managers.Interface;
using SnapShuffle.Models;
using SnapShuffle.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapShuffle.Managers.Implementation
{
    public class TbUserManager : ITbUserManager
    {
        ITbUserDataAccess UserDataAccess { get; set; }
        private CommonFunctions _CommonFunctions { get; set; }

        public TbUserManager(ITbUserDataAccess userDataAccess, IHttpContextAccessor httpContextAccessor) 
        {
            UserDataAccess = userDataAccess;
            _CommonFunctions = new CommonFunctions(httpContextAccessor);
        }

        public APIResponse Register(tbUserRegisterModel model)
        {
            model.Password = _CommonFunctions.DecryptRSAEncryptedString(model.Password);
            var EmailCheck = UserDataAccess.GetByEmail(model.Email);
            if (EmailCheck != null)
            {
                return new APIResponse(ResponseCode.ERROR, "Email Already Exists");
            }

            var UsernameCheck = UserDataAccess.GetByUsername(model.UserName);
            if (UsernameCheck != null)
            {
                return new APIResponse(ResponseCode.ERROR, "Username Already Exists");
            }

            var ToInsertModel = new tbUserModel()
            {
                UserGuid = Guid.NewGuid(),
                UserName = model.UserName,
                PasswordHash = SecureHasher.Hash(model.Password),
                Email = model.Email
            };

            var data = UserDataAccess.Register(ToInsertModel);
            return new APIResponse(ResponseCode.SUCCESS, "Register Success", data);
        }

        public APIResponse Login(tbUserLoginModel model)
        {
            model.Password = _CommonFunctions.DecryptRSAEncryptedString(model.Password);
            var UsernameCheck = UserDataAccess.GetByUsername(model.UserName);
            if (UsernameCheck == null)
            {
                return new APIResponse(ResponseCode.ERROR, "Username And Password did not match");
            }

            var VerifyPassword = SecureHasher.Verify(model.Password, UsernameCheck.PasswordHash);

            if (VerifyPassword)
            {
                var claims = new List<ClaimModel>();
                model.Password = _CommonFunctions.Encrypt(model.Password);
                claims.Add(new ClaimModel() { ClaimName = "LoginData", Data = model });
                var token = _CommonFunctions.CreateJWTToken(claims);
                return new APIResponse(ResponseCode.SUCCESS, "Login Success", token);
            }
            else
            {
                return new APIResponse(ResponseCode.ERROR, "Username And Password did not match");
            }
        }
    }
}

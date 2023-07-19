using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SnapShuffle.Managers.Interface;
using SnapShuffle.Models;
using SnapShuffle.Utility;

namespace SnapShuffleAPI.Controllers
{
    [ApiController]
    public class TbUserController : ControllerBase
    {
        ITbUserManager UserManager { get; set; }
        public TbUserController(ITbUserManager userManager)
        {
            UserManager = userManager;
        }

        [HttpPost]
        [Route("/api/User/Register")]
        public ActionResult Register(tbUserRegisterModel model)
        {
            try
            {
                return Ok(UserManager.Register(model));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, JsonConvert.SerializeObject(ex)));
            }
        }

        [HttpPost]
        [Route("/api/User/Login")]
        public ActionResult Login(tbUserLoginModel model)
        {
            try
            {
                return Ok(UserManager.Login(model));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, JsonConvert.SerializeObject(ex)));
            }
        }
    }
}

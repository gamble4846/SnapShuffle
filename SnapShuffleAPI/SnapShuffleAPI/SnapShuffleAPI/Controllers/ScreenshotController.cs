using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SnapShuffle.Managers.Interface;
using SnapShuffle.Models;
using SnapShuffle.Utility;

namespace SnapShuffleAPI.Controllers
{
    [ApiController]
    //[Authorize]
    public class ScreenshotController : ControllerBase
    {
        IScreenshotManager ScreenshotManager { get; set; }
        public ScreenshotController(IScreenshotManager screenshotManager)
        {
            ScreenshotManager = screenshotManager;
        }

        [HttpGet]
        [Route("/api/Screenshot/GetRandom")]
        public async Task<ActionResult> GetRandom()
        {
            try
            {
                return Ok(await ScreenshotManager.GetRandom());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, JsonConvert.SerializeObject(ex)));
            }
        }
    }
}

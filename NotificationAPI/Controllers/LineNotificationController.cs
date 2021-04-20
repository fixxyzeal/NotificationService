using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServicesLibrary.LineServices;
using ServicesLibrary.CacheServices;
using ServicesLibrary.Models.Line;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace NotificationAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("[controller]")]
    public class LineNotificationController : ControllerBase
    {
        private readonly ILineMessageService _lineMessageService;

        public LineNotificationController(ILineMessageService lineMessageService)
        {
            _lineMessageService = lineMessageService;
        }

        [HttpGet("userprofile")]
        public async Task<IActionResult> GetLineUserProfile([FromQuery, Required] string userId)
        {
            return Ok(await _lineMessageService.GetProfile(Environment.GetEnvironmentVariable("LineChannelAccessToken"), userId).ConfigureAwait(false));
        }

        [HttpPost("sendtextmessage")]
        public async Task<IActionResult> SendTextMessage([FromBody] LineMessageRequestModel req)
        {
            await _lineMessageService.SendTextMessage(req).ConfigureAwait(false);
            return Ok();
        }

        [HttpPost("sendtextmessage/nowait")]
        public async Task<IActionResult> SendTextMessageWithNoWait([FromBody] LineMessageRequestModel req)
        {
            await _lineMessageService.SendTextMessageNoWait(req).ConfigureAwait(false);
            return Ok();
        }
    }
}
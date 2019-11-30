using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SSEBackend.Security;
using SSECommon;
using SSECommon.Types;

namespace SSEBackend.Controllers
{
    [Route("api/generic")]
    [ApiController]
    public class GenericController : ControllerBase
    {
        //simply returns some text so the client knows the server can be reached.
        [HttpGet("ping")]
        public ActionResult Ping() {
            return new ObjectResult("PONG!");
        }

        
        [HttpGet("time")]
        public ActionResult Time() {
            return new ObjectResult(DateTime.UtcNow.ToLongTimeString());
        }
    }
}
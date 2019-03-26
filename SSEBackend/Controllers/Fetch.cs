using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SSECommon;
using SSECommon.Types;
using SSEBackend.Types;
using SSEBackend.Security;

namespace SSEBackend.Controllers
{
    [Route("api/fetch")]
    [ApiController]
    public class Fetch : ControllerBase
    {
        [HttpPost("readme")]
        public ActionResult Readme([FromBody] GenericEncryptedMessage message) {

            //make sure the team requesting a key is legitimate with a legitimate RID
            if (!Globals.VerifyTeamAuthenticity(message.TeamUUID, message.RuntimeID)) {
                return new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }

            //make sure the team and runtime has a valid communication key.
            if (!Globals.VerifyRuntimeHasValidCommsKey(message.TeamUUID, message.RuntimeID)) {
                return new StatusCodeResult(StatusCodes.Status419AuthenticationTimeout);
            }

            string plaintext = Encryption.DecryptMessage(message.Ciphertext, message.IV, message.TeamUUID, message.RuntimeID);

            if (plaintext != Constants.KEY_EXCHANGE_SANITY_CHECK) {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }

            byte[] readme = Globals.GetReadme(message.TeamUUID, message.RuntimeID);


            byte[] iv;
            byte[] ciphertext = Encryption.EncryptMessage(readme.ToHex(), out iv, message.TeamUUID, message.RuntimeID);

            return new ObjectResult(new GenericEncryptedMessage(ciphertext, iv, "", message.TeamUUID, message.RuntimeID).ToJson());
        }
    }
}
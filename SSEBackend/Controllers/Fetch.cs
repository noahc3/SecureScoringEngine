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

using Newtonsoft.Json;

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

            //the body doesnt actually contain any useful information, just a sanity check to make sure they comms key stored on the server matches that of the client.
            if (plaintext != Constants.KEY_EXCHANGE_SANITY_CHECK) {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }

            FileTransferWrapper readme = Globals.GetReadme(message.TeamUUID, message.RuntimeID);


            byte[] iv;
            byte[] ciphertext = Encryption.EncryptMessage(JsonConvert.SerializeObject(readme), out iv, message.TeamUUID, message.RuntimeID);

            return new ObjectResult(new GenericEncryptedMessage(ciphertext, iv, "", message.TeamUUID, message.RuntimeID).ToJson());
        }
    }
}
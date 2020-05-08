using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SSEBackend.Security;
using SSECommon;
using SSECommon.Types;
using System.IO;
using System.IO.Compression;
using Newtonsoft.Json;

namespace SSEBackend.Controllers
{
    [Route("api/debug")]
    [ApiController]
    public class DebugSvcsController : ControllerBase
    {
        [HttpPost("debugsvcstatus")]
        public ActionResult Ping([FromBody] GenericEncryptedMessage message) {

            //make sure the team requesting a key is legitimate with a legitimate RID
            if (!Globals.IsTeamDebug(message.TeamUUID)) {
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


            byte[] iv; 
            byte[] ciphertext = Encryption.EncryptMessage(Globals.config.DebugSvcs.ToString().ToLower(), out iv, message.TeamUUID, message.RuntimeID);

            return new ObjectResult(new GenericEncryptedMessage(ciphertext, iv, "", message.TeamUUID, message.RuntimeID).ToJson());
        }

        [HttpPost("addruntime")]
        public ActionResult AddRuntime([FromBody] GenericEncryptedMessage message) {
            //make sure the team requesting a key is legitimate with a legitimate RID
            if (!Globals.IsTeamDebug(message.TeamUUID)) {
                return new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }

            //make sure the team and runtime has a valid communication key.
            if (!Globals.VerifyRuntimeHasValidCommsKey(message.TeamUUID, message.RuntimeID)) {
                return new StatusCodeResult(StatusCodes.Status419AuthenticationTimeout);
            }

            FileTransferWrapper wrapper = JsonConvert.DeserializeObject<FileTransferWrapper>(Encryption.DecryptMessage(message.Ciphertext, message.IV, message.TeamUUID, message.RuntimeID));
            Stream stream = new MemoryStream(wrapper.Blob);
            using (ZipArchive archive = new ZipArchive(stream)) {
                archive.ExtractToDirectory((Globals.RUNTIME_CONFIG_DIRECTORY + "/" + wrapper.Path).AsPath());
            }

            return new StatusCodeResult(StatusCodes.Status202Accepted);
            
        }

        [HttpPost("hotreload")]
        public ActionResult HotReload([FromBody] GenericEncryptedMessage message) {
            //make sure the team requesting a key is legitimate with a legitimate RID
            if (!Globals.IsTeamDebug(message.TeamUUID)) {
                return new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }

            Globals.StopPassiveTasks();
            Globals.SaveData();
            Globals.LoadData();
            Globals.StartPassiveTasks();

            return new StatusCodeResult(StatusCodes.Status200OK);
        }
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

using SSECommon;
using SSECommon.Types;
using SSEBackend.Types;
using SSEBackend.Security;

namespace SSEBackend.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class Authentication : ControllerBase
    {
        [HttpGet("isteamuuidvalid")]
        public ActionResult IsTeamUUIDValid([FromHeader] IsTeamUUIDValidInputModel model) {
            //STUB: Implement proper checking for valid team UUIDs
            return new StatusCodeResult(StatusCodes.Status200OK);
        }

        [HttpGet("keyexchange")]
        public ActionResult KeyExchange([FromHeader] KeyExchangeInputModel model) {

            byte[] theirPublicKey = model.DHKEPublicKey.FromHexToByteArray();
            string teamUuid = model.TeamUUID;
            string runtimeId = model.RunetimeID;

            //make sure the team requesting a key is legitimate with a legitimate RID
            if (!Globals.VerifyTeamAuthenticity(teamUuid, runtimeId)) {
                return new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }

            Team team = Globals.GetTeam(teamUuid);
            Runtime runtime = Globals.GetRuntime(teamUuid, runtimeId);

            //initialize the key exchange
            using (ECDiffieHellmanCng exchange = new ECDiffieHellmanCng()) {
                exchange.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
                exchange.HashAlgorithm = CngAlgorithm.Sha256;

                //try to read clients's public keyblob into DHKE and derive the key material
                try {
                    Encryption.SetRuntimeKeySlot(teamUuid, runtimeId, exchange.DeriveKeyMaterial(CngKey.Import(theirPublicKey, CngKeyBlobFormat.EccPublicBlob)));
                } catch (Exception e) {
                    //if this fails the public key sent by the client was either invalid or could not be parsed.
                    return new StatusCodeResult(StatusCodes.Status400BadRequest);
                }

                //key exchange was successful! send back the server's public key with a sanity check
                byte[] iv;
                byte[] sanityCheck = Encryption.EncryptMessage(Constants.KEY_EXCHANGE_SANITY_CHECK, out iv, teamUuid, runtimeId);
                return new ObjectResult(new GenericEncryptedMessage(sanityCheck, iv, exchange.PublicKey.ToByteArray().ToHex(), teamUuid, runtimeId).ToJson());
            }
        }

        //can be used to ping the server and verify key refresh is working
        //this endpoint is NOT plaintext! use /api/generic/ping for plaintext ping!
        //TODO: implement /api/generic/ping
        //this endpoint WILL invalidate the key! it should only be used to verify the key exchange is working, not to verify keys!
        [HttpPost("ping")]
        public ActionResult Ping([FromBody] GenericEncryptedMessage message) {

            //make sure the team requesting a key is legitimate with a legitimate RID
            if (!Globals.VerifyTeamAuthenticity(message.TeamUUID, message.RuntimeID)) {
                return new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }

            //make sure the team and runtime has a valid communication key.
            if (!Globals.VerifyRuntimeHasValidCommsKey(message.TeamUUID, message.RuntimeID)) {
                return new StatusCodeResult(StatusCodes.Status419AuthenticationTimeout);
            }

            string plaintext = Encryption.DecryptMessage(message.Ciphertext, message.IV, message.TeamUUID, message.RuntimeID);

            if (plaintext != "PING!") {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }


            byte[] iv;
            byte[] ciphertext = Encryption.EncryptMessage("PONG!", out iv, message.TeamUUID, message.RuntimeID);

            return new ObjectResult(new GenericEncryptedMessage(ciphertext, iv, "", message.TeamUUID, message.RuntimeID).ToJson());
        }
    }

    public class IsTeamUUIDValidInputModel {
        [FromHeader(Name = "TEAM-UUID")]
        [Required]
        public string TeamUUID { get; set; }
    }

    public class KeyExchangeInputModel {
        [FromHeader(Name = "TEAM-UUID")]
        [Required]
        public string TeamUUID { get; set; }

        [FromHeader(Name = "RUNTIME-ID")]
        [Required]
        public string RunetimeID { get; set; }

        [FromHeader(Name = "DHKE-PUBLIC-KEY")]
        [Required]
        public string DHKEPublicKey { get; set; }
    }
}
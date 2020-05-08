using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading.Tasks;

using SSECommon;
using SSECommon.Types;
using SSEBackend.Types;
using SSEBackend.Security;

using Newtonsoft.Json;

namespace SSEBackend.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class Authentication : ControllerBase
    {
        [HttpGet("isteamuuidvalid")]
        public ActionResult IsTeamUUIDValid([FromHeader] IsTeamUUIDValidInputModel model) {
            if (Globals.VerifyTeamAuthenticity(model.TeamUUID, model.RuntimeID)) {
                return new StatusCodeResult(StatusCodes.Status200OK);
            } else {
                return new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }

        }

        //Cross-platform (server & client) elliptic-curve diffie-helman key exchange.
        [HttpGet("keyexchange")]
        public ActionResult KeyExchange([FromHeader] KeyExchangeInputModel model) {

            byte[] theirPublicKey = model.DHKEPublicKey.FromHexToByteArray();
            string teamUuid = model.TeamUUID;
            string runtimeId = model.RuntimeID;

            //make sure the team requesting a key is legitimate with a legitimate RID
            if (!Globals.VerifyTeamAuthenticity(teamUuid, runtimeId)) {
                return new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }

            Team team = Globals.GetTeam(teamUuid);
            Runtime runtime = Globals.GetRuntime(teamUuid, runtimeId);

            //create server exchange
            using (ECDiffieHellman exchange = ECDiffieHellman.Create()) {
                int read;
                //import the other party's serialized public key info and export the public key as the object we need
                ECDiffieHellman otherParty = ECDiffieHellman.Create();
                otherParty.ImportSubjectPublicKeyInfo(model.DHKEPublicKey.FromHexToByteArray(), out read);
                ECDiffieHellmanPublicKey otherPartyKey = otherParty.PublicKey;
                //derive key material and populate the keyslot
                byte[] privateKey = exchange.DeriveKeyMaterial(otherPartyKey);
                Encryption.SetRuntimeKeySlot(teamUuid, runtimeId, privateKey);

                //key exchange was successful! send back the server's public key with a sanity check
                byte[] iv;
                byte[] sanityCheck = Encryption.EncryptMessage(Constants.KEY_EXCHANGE_SANITY_CHECK, out iv, teamUuid, runtimeId);

                //return server public key
                return new ObjectResult(new GenericEncryptedMessage(sanityCheck, iv, exchange.ExportSubjectPublicKeyInfo().ToHex(), teamUuid, runtimeId).ToJson());
            }
        }

        //can be used to ping the server and verify key refresh is working
        //this endpoint is NOT plaintext! use /api/generic/ping for plaintext ping!
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

    public class IsTeamUUIDValidInputModel
    {
        [FromHeader(Name = "TEAM-UUID")]
        [Required]
        public string TeamUUID { get; set; }

        [FromHeader(Name = "RUNTIME-ID")]
        [Required]
        public string RuntimeID { get; set; }
    }

    public class KeyExchangeInputModel
    {
        [FromHeader(Name = "TEAM-UUID")]
        [Required]
        public string TeamUUID { get; set; }

        [FromHeader(Name = "RUNTIME-ID")]
        [Required]
        public string RuntimeID { get; set; }

        [FromHeader(Name = "DHKE-PUBLIC-KEY")]
        [Required]
        public string DHKEPublicKey { get; set; }
    }
}
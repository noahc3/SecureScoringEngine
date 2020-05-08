using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SSECommon;
using SSECommon.Enums;
using SSECommon.Types;
using SSEBackend.Types;
using SSEBackend.Security;

using Newtonsoft.Json;

namespace SSEBackend.Controllers
{
    [Route("api/scoring")]
    [ApiController]
    public class ScoringController : ControllerBase
    {
        [HttpPost("start")]
        public ActionResult Start([FromBody] GenericEncryptedMessage message) {
            
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

            Team team = Globals.GetTeam(message.TeamUUID);

            ScoringProgressTracker tracker = new ScoringProgressTracker();
            tracker.scored = new List<bool>();
            tracker.scoringProgress = -1;

            if (team.ScoringProgressTrackers == null) team.ScoringProgressTrackers = new Dictionary<string, ScoringProgressTracker>();

            team.ScoringProgressTrackers[Globals.GetRuntime(message.TeamUUID, message.RuntimeID).ID] = tracker;

            return new StatusCodeResult(StatusCodes.Status200OK);
        }

        [HttpPost("continuescoringprocess")]
        public ActionResult ContinueScoringProcess([FromBody] GenericEncryptedMessage message) {

            //make sure the team requesting a key is legitimate with a legitimate RID
            if (!Globals.VerifyTeamAuthenticity(message.TeamUUID, message.RuntimeID)) {
                return new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }

            //make sure the team and runtime has a valid communication key.
            if (!Globals.VerifyRuntimeHasValidCommsKey(message.TeamUUID, message.RuntimeID)) {
                return new StatusCodeResult(StatusCodes.Status419AuthenticationTimeout);
            }

            //TODO: consider moving this validation to a method in Globals

            Dictionary<string, ScoringProgressTracker> scoringProgressTrackers = Globals.GetTeam(message.TeamUUID).ScoringProgressTrackers;

            //[ -- 1 -- ]
            //the client messed up and needs to request to start scoring first.
            //TODO: for anticheat, this is a bannable offense as it means the client is modifed or custom
            //(or a very strange bug occurred and the client straight up skipped instructions).
            //THIS SHOULD NEVER HAPPEN!!!
            if (scoringProgressTrackers == null) {
                return new StatusCodeResult(StatusCodes.Status424FailedDependency);
            }

            Runtime runtime = Globals.GetRuntime(message.TeamUUID, message.RuntimeID);
            string sanitizedRuntimeId = runtime.ID;

            //[ -- 1 -- ]
            if (!scoringProgressTrackers.ContainsKey(sanitizedRuntimeId)) {
                return new StatusCodeResult(StatusCodes.Status424FailedDependency);
            }

            //end TODO

            ScoringProgressTracker tracker = scoringProgressTrackers[sanitizedRuntimeId];
            
            
            //the tag will either contain Constants.SCORING_PROCESS_START if it is the first request for a payload
            //or it will contain Constants.SCORING_PROCESS_CONTINUE along with the previous payloads output if it is trying to continue the scoring process.
            if (message.Tag == Constants.SCORING_PROCESS_START) {

                tracker.scoringProgress++;
                //[ -- 1 -- ]
                if (tracker.scoringProgress != 0) {
                    return new StatusCodeResult(StatusCodes.Status424FailedDependency);
                }

                //encrypt the scoring payload and send it back to the client
                //tag is set to Constants.SCORING_PROCESS_EXECUTE_PAYLOAD
                byte[] iv;
                byte[] ciphertext = Encryption.EncryptMessage(runtime.scoredItems[tracker.scoringProgress].ClientPayload, out iv, message.TeamUUID, message.RuntimeID);

                return new ObjectResult(new GenericEncryptedMessage(ciphertext, iv, Constants.SCORING_PROCESS_EXECUTE_PAYLOAD, message.TeamUUID, message.RuntimeID).ToJson());



            } else if (message.Tag == Constants.SCORING_PROCESS_CONTINUE) {

                //[ -- 1 -- ]
                if (tracker.scoringProgress < 0) {
                    return new StatusCodeResult(StatusCodes.Status424FailedDependency);
                }

                //decrypt the result made by the client payload
                string clientPayloadOutput = Encryption.DecryptMessage(message.Ciphertext, message.IV, message.TeamUUID, message.RuntimeID);

                //pass the client payload output into the server payload and save the result
                bool serverPayloadResult = runtime.scoredItems[tracker.scoringProgress].ServerPayload.ExecuteAsCode<bool>(clientPayloadOutput);


                //add to tracker whether or not the server payload result matches the intended value.
                tracker.scored.Add(serverPayloadResult == runtime.scoredItems[tracker.scoringProgress].ScoreIfBool);

                //increase the scoring progress value and check if it exceeds the number of items to score
                //if so, let the client know the scoring process is finished
                //if not, continue with the next payload
                tracker.scoringProgress++;

                if (tracker.scoringProgress >= runtime.scoredItems.Count) {

                    tracker.safeScored = tracker.scored;
                    tracker.scored = null;
                    tracker.safeToGenerateReport = true;

                    byte[] iv;
                    byte[] ciphertext = Encryption.EncryptMessage(Constants.KEY_EXCHANGE_SANITY_CHECK, out iv, message.TeamUUID, message.RuntimeID);

                    return new ObjectResult(new GenericEncryptedMessage(ciphertext, iv, Constants.SCORING_PROCESS_FINISHED, message.TeamUUID, message.RuntimeID).ToJson());

                } else {
                    //encrypt the scoring payload and send it back to the client
                    //tag is set to Constants.SCORING_PROCESS_EXECUTE_PAYLOAD
                    byte[] iv;
                    byte[] ciphertext = Encryption.EncryptMessage(runtime.scoredItems[tracker.scoringProgress].ClientPayload, out iv, message.TeamUUID, message.RuntimeID);

                    return new ObjectResult(new GenericEncryptedMessage(ciphertext, iv, Constants.SCORING_PROCESS_EXECUTE_PAYLOAD, message.TeamUUID, message.RuntimeID).ToJson());
                }
            } else {
                //this should definitely never happen
                //TODO: anticheat
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
        }

        [HttpPost("getscoringreport")]
        public ActionResult GetScoringReport([FromBody] GenericEncryptedMessage message) {

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

            Team team = Globals.GetTeam(message.TeamUUID);
            Dictionary<string, ScoringProgressTracker> scoringProgressTrackers = team.ScoringProgressTrackers;

            //[ -- 1 -- ]
            //the client messed up and needs to request to start scoring first.
            //TODO: for anticheat, this is a bannable offense as it means the client is modifed or custom
            //(or a very strange bug occurred and the client straight up skipped instructions).
            //THIS SHOULD NEVER HAPPEN!!!
            if (scoringProgressTrackers == null) {
                return new StatusCodeResult(StatusCodes.Status424FailedDependency);
            }

            Runtime runtime = Globals.GetRuntime(message.TeamUUID, message.RuntimeID);
            string sanitizedRuntimeId = runtime.ID;

            //[ -- 1 -- ]
            if (!scoringProgressTrackers.ContainsKey(sanitizedRuntimeId)) {
                return new StatusCodeResult(StatusCodes.Status424FailedDependency);
            }

            //end TODO

            ScoringProgressTracker tracker = scoringProgressTrackers[sanitizedRuntimeId];

            if (!tracker.safeToGenerateReport) {
                return new StatusCodeResult(StatusCodes.Status424FailedDependency);
            }

            ScoringReport report = new ScoringReport();
            List<string> serverreport = null;
            if (Globals.config.SaveScoringReportsOnServer) serverreport = new List<string>();

            int i = 0;

            foreach(ScoringPayloadMetadata p in runtime.scoredItems) {
                if (p.Type == ScoreType.Reward) {
                    report.totalRewards++;
                    report.totalScore += p.ScoreValue;

                    if (tracker.safeScored[i]) {
                        report.score += p.ScoreValue;
                        report.rewardsFound++;

                        ClientScoreMetadata meta = new ClientScoreMetadata();
                        if (Globals.config.DetailedScoringReports) {
                            meta.Name = p.Description;
                            meta.Description = p.Description;
                        } else {
                            meta.Name = p.Name;
                            meta.Description = p.Name;
                        }
                        meta.ScoreValue = p.ScoreValue;
                        meta.Type = p.Type;
                        report.rewards.Add(meta);
                        if (Globals.config.SaveScoringReportsOnServer) serverreport.Add(p.Description + " : " + meta.ScoreValue);
                    }
                } else if (p.Type == ScoreType.Penalty) {
                    if (tracker.safeScored[i]) {
                        report.score += -p.ScoreValue;
                        report.penaltiesGained++;

                        ClientScoreMetadata meta = new ClientScoreMetadata();
                        if (Globals.config.DetailedScoringReports) {
                            meta.Name = p.Description;
                            meta.Description = p.Description;
                        } else {
                            meta.Name = p.Name;
                            meta.Description = p.Name;
                        }
                        meta.ScoreValue = p.ScoreValue;
                        meta.Type = p.Type;
                        report.penalties.Add(meta);
                        if (Globals.config.SaveScoringReportsOnServer) serverreport.Add(p.Description + " : -" + meta.ScoreValue);
                    }
                }

                i++;
            }

            report.lastTotalScore = team.RuntimeLastScores.ContainsKey(sanitizedRuntimeId) ? team.RuntimeLastScores[sanitizedRuntimeId] : 0;

            team.RuntimeLastScores[sanitizedRuntimeId] = report.score;
            team.RuntimeLastTimestamps[sanitizedRuntimeId] = DateTime.UtcNow.Ticks;
            team.TeamLastTimestamp = DateTime.UtcNow.Ticks;

            report.teamStartTimestamp = team.TeamStartTimestamp;
            report.runtimeStartTimestamp = team.RuntimeStartTimestamps[sanitizedRuntimeId];

            string serialized = JsonConvert.SerializeObject(report);

            if (Globals.config.SaveScoringReportsOnServer) {
                serverreport.Insert(0, report.score + "/" + report.totalScore + " (" + report.rewardsFound + "/" + report.totalRewards + ")");
                System.IO.File.WriteAllLines((Globals.SCORING_REPORTS_DIRECTORY + "\\" + team.UUID + "_" + runtime.ID + ".txt").AsPath(), serverreport);
            }

            byte[] iv;
            byte[] ciphertext = Encryption.EncryptMessage(serialized, out iv, message.TeamUUID, message.RuntimeID);

            return new ObjectResult(new GenericEncryptedMessage(ciphertext, iv, "", message.TeamUUID, message.RuntimeID));

        }
    }
}
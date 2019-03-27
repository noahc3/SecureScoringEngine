﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using SSEBackend.Types;
using SSECommon;
using SSECommon.Types;

namespace SSEBackend {
    public static class Globals {

        public static string CONFIG_DIRECTORY = (AppContext.BaseDirectory + "\\config").AsPath();
        public static string RUNTIME_CONFIG_DIRECTORY = (CONFIG_DIRECTORY + "\\runtimes\\").AsPath();

        public static Data data;

        public static void LoadData() {
            data = new Data();
            
            foreach (string k in Directory.EnumerateDirectories(RUNTIME_CONFIG_DIRECTORY)) {
                string runtimeJson = File.ReadAllText((k + "\\runtime.conf").AsPath());
                Runtime runtime = Runtime.FromJson(runtimeJson);
                data.runtimes[runtime.ID] = runtime;
            }
        }

        public static bool VerifyTeamAuthenticity(string teamUuid, string runtimeId) {
            Team team;
            if (data.teams.ContainsKey(teamUuid)) {
                team = data.teams[teamUuid];
            } else {
                return false;
            }

            bool validRuntimeId = false;

            foreach(string k in team.ValidRuntimeIDs) {
                if (runtimeId.StartsWith(k)) {
                    validRuntimeId = true;
                    break;
                }
            }

            if (!validRuntimeId) return false;

            return true;
        }

        public static bool VerifyRuntimeHasValidCommsKey(string teamUuid, string runtimeId) {
            Runtime runtime = GetRuntime(teamUuid, runtimeId);
            Team team = GetTeam(teamUuid);

            return team.EncKeys[runtime] != null;
        }

        public static Runtime GetRuntime(string teamUuid, string runtimeId) {

            //TODO: since runtimes can share id's, verify which one is intended using teamUuid
            Runtime runtime = null;
            foreach(string k in data.runtimes.Keys) {
                if (runtimeId.StartsWith(k)) {
                    runtime = data.runtimes[k];
                    break;
                }
            }
            return runtime;
        }

        public static Team GetTeam(string teamUuid) {
            return data.teams[teamUuid];
        }

        public static string GetRuntimeConfigDirectory(Runtime runtime) {
            return (RUNTIME_CONFIG_DIRECTORY + "\\" + runtime.ID + "\\").AsPath();
        }

        public static FileTransferWrapper GetReadme(string teamUuid, string runtimeId) {
            Runtime runtime = GetRuntime(teamUuid, runtimeId);
            string confdir = GetRuntimeConfigDirectory(runtime);

            FileTransferWrapper ftw = new FileTransferWrapper();

            ftw.Blob = File.ReadAllBytes((confdir + "\\readme.bin").AsPath());
            ftw.Path = runtime.readmeLocation;

            return ftw;
        }
    }
}
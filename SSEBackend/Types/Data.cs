using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SSECommon.Types;

namespace SSEBackend.Types {
    public class Data {

        //STUB: Implement proper data import

        public Dictionary<string, Runtime> runtimes = new Dictionary<string, Runtime>();

        public Dictionary<string, Team> teams = new Dictionary<string, Team> {
            { "test", new Team {UUID = "test", ValidRuntimeIDs = new List<string> { "Microsoft Windows 10" }, EncKeys = new Dictionary<Runtime, byte[]>()} }
        };
    }
}

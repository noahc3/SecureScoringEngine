using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSEConfigurationTool.Data {
    public class ConfiguredScoringItem {
        public string Platform;
        public string Category;
        public string Name;
        public List<ScoringVariable> ClientScoringVariables = new List<ScoringVariable>() { new ScoringVariable() };
        public List<ScoringVariable> ServerScoringVariables = new List<ScoringVariable>() { new ScoringVariable() };

        public string ScoringReportName;
        public string DescriptiveName;
        public string ScoreType;
        public int ScoreAmount;
        public bool BanOnInvalid;
    }
}

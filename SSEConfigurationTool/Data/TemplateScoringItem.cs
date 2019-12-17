using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSEConfigurationTool.Data {
    public class TemplateScoringItem {
        public string Platform;
        public string Category;
        public string Name;
        public List<ScoringVariable> ClientScoringVariables = new List<ScoringVariable>() { new ScoringVariable() };
        public List<ScoringVariable> ServerScoringVariables = new List<ScoringVariable>() { new ScoringVariable() };

        public TemplateScoringItem(string Platform, string Category, string Name) {
            this.Platform = Platform;
            this.Category = Category;
            this.Name = Name;

            this.ClientScoringVariables = ScoringItemUtilities.ParseVariables(this.Platform, this.Category, this.Name, false);
            this.ServerScoringVariables = ScoringItemUtilities.ParseVariables(this.Platform, this.Category, this.Name, true);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace SSEConfigurationTool.Data {
    public class ScoringVariable {
        public string Name; //variable name
        public string PrettyName; //name shown in GUI
        public string HelpText; //Help text shown in GUI
        public string Type; //Supported by the UI: bool, int, string, List<string>
        public string Value = "";

        public ScoringVariable Copy() {
            return new ScoringVariable() {
                Name = this.Name,
                PrettyName = this.PrettyName,
                HelpText = this.HelpText,
                Type = this.Type,
                Value = this.Value
            };
        }

        public string GenerateCode() {
            string code = Type + " " + Name + " = ";

            if (Type == "bool") {
                code += Value + ";";
            } else if (Type == "int") {
                code += Value + ";";
            } else if (Type == "string") {
                code += "\"" + Value + "\";";
            } else if (Type == "List<string>") {
                string values = "";
                using (StringReader sr = new StringReader(Value)) {
                    string line = "";
                    while ((line = sr.ReadLine()) != null) {
                        values += "\"" + System.Web.HttpUtility.JavaScriptStringEncode(line) + "\", ";
                    }
                }
                values = "new List<string>() { " + values.TrimEnd(',', ' ') + " };";
                code += values;
            }

            return code;
        }
    }
}

//assembly System;
//assembly System.IO;
//assembly System.Linq;
//assembly System.Text.RegularExpressions;

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

string fileLocation = "/home/bporter/Desktop/Forensics Question 1.txt";

try {  
    string contents = File.ReadAllText(fileLocation);
    string answer = Regex.Split(contents, "Answer:").Last().Replace(" ", "").Replace("<", "").Replace(">", "");
    return answer;
} catch (Exception e) {
    return "{#} CLIENT PAYLOAD FAILED! {#} " + e.Message;
}
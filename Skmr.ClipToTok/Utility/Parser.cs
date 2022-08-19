using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Skmr.ClipToTok.Utility
{
    public static class Parser
    {
        public static void FromTxt(this Highlight highlight, string str)
            => Parse(highlight, str, ' ');


        public static void FromCsv(this Highlight highlight, string str)
            => Parse(highlight, str, ',');


        private static void Parse(this Highlight highlight,string str, char seperator)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"(\d{2}):(\d{2}):(\d{2})");
            sb.Append(seperator);
            sb.Append(@"(\d*)");
            sb.Append(seperator);
            sb.Append(@"([a-zA-Z0-9 /.!?-]*)");

            Regex regex = new Regex(sb.ToString());
            var matches = regex.Matches(str);

            highlight.Start = new TimeSpan(
                int.Parse(matches[0].Groups[1].Value),
                int.Parse(matches[0].Groups[2].Value),
                int.Parse(matches[0].Groups[3].Value));

            highlight.Duration = TimeSpan.FromSeconds(int.Parse(matches[0].Groups[4].Value));
            highlight.Comment = matches[0].Groups[5].Value;
        }
    }
}

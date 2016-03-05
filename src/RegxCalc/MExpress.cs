using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RegxCalc
{
    class MExpress
    {
        private static Regex inBracket = new Regex(@"\(([0-9\+\-\*\/\.]+)\)");
        private static Regex twoNumberMD = new Regex(@"\(?(-?\d+(\.\d+)?)\)?([\*\/])\(?(-?\d+(\.\d+)?)\)?");
        private static Regex twoNumberAE = new Regex(@"\(?(-?\d+(\.\d+)?)\)?([+-])\(?(-?\d+(\.\d+)?)\)?");

        private string calcTwoNumber(string left, string oper, string right)
        {
            switch (oper)
            {
                case "+": return (Convert.ToSingle(left) + Convert.ToSingle(right)).ToString();
                case "-": return (Convert.ToSingle(left) - Convert.ToSingle(right)).ToString();
                case "*": return (Convert.ToSingle(left) * Convert.ToSingle(right)).ToString();
                case "/": return (Convert.ToSingle(left) / Convert.ToSingle(right)).ToString();
                default: return string.Empty;
            }
        }
        private string calcExpressNoBracket(String exp)
        {
            Match m = null;
            while (true)
            {
                m = twoNumberMD.Match(exp);
                if (m.Success)
                    exp = calcReplace(m, exp);
                else
                {
                    m = twoNumberAE.Match(exp);
                    if (m.Success)
                        exp = calcReplace(m, exp);
                    else
                        break;
                }
            }
            return exp;
        }
        private string calcReplace(Match m, string express)
        {
            string result = calcTwoNumber(m.Groups[1].Value, m.Groups[3].Value, m.Groups[4].Value);
            express = express.Replace(m.Groups[0].Value, result);
            return express;
        }
        public string runExpress(string exp)
        {
            Match m = null;
            while (true)
            {
                m = inBracket.Match(exp);
                if (m.Success)
                    exp = exp.Replace(m.Groups[0].Value, calcExpressNoBracket(m.Groups[1].Value));
                else
                    break;
            }
            return calcExpressNoBracket(exp);
        }
    }
}

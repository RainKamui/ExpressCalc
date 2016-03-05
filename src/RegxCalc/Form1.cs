using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RegxCalc
{
    public partial class Form1 : Form
    {
        private static Regex inBracket = new Regex(@"\(([0-9\+\-\*\/\.\^]+)\)");
        private static Regex number = new Regex(@"^[+-]?\d+\.?\d+\d$");
        private static Regex twoNumberMD = new Regex(@"\(?(-?\d+(\.\d+)?)\)?([\*\/])\(?(-?\d+(\.\d+)?)\)?");
        private static Regex twoNumberAE = new Regex(@"\(?(-?\d+(\.\d+)?)\)?([+-])\(?(-?\d+(\.\d+)?)\)?");

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private string calcTowNumber(string left, string oper, string right)
        {
            float leftValue = Convert.ToSingle(left);
            float rightValue = Convert.ToSingle(right);
            switch(oper)
            {
                case "+": return (leftValue + rightValue).ToString();
                case "-": return (leftValue - rightValue).ToString();
                case "*": return (leftValue * rightValue).ToString();
                case "/": return (leftValue / rightValue).ToString();
                default: return string.Empty;
            }
        }

        /**
         * 计算两个数字加减乘除
         * 参数为X+Y类似的表达式
         */
        private string calcExpressNoBracket(String exp)
        {
            Match m = null;
            string express = exp;
            while (true)
            {
                m = twoNumberMD.Match(express);
                if (m.Success)
                {
                    express = calcReplace(m, express);
                }
                else
                {
                    m = twoNumberAE.Match(express);
                    if (m.Success)
                    {
                        express = calcReplace(m, express);
                    }
                    else
                        break;
                }
            }

            return express;

        }

        private string calcReplace(Match m, string express)
        {
            string twoNumberExp = m.Groups[0].Value;
            string leftValue = m.Groups[1].Value;
            string operatorStr = m.Groups[3].Value;
            string rightValue = m.Groups[4].Value;
            string result = calcTowNumber(leftValue, operatorStr, rightValue);
            express = express.Replace(twoNumberExp, result);
            return express;
        }

        private string runExpress(string exp)
        {
            Match m = null;
            string express = exp;
            while (true)
            {
                m = inBracket.Match(express);
                if (m.Success)
                {
                    string bracketExp = m.Groups[0].Value;
                    string calcExp = m.Groups[1].Value;
                    string result = calcExpressNoBracket(calcExp);
                    express = express.Replace(bracketExp, result);
                }
                else
                {
                    break;
                }
            }
            return calcExpressNoBracket(express);
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            ExtendMExpress me2 = new ExtendMExpress();
            textBox2.Text = me2.runExpress(express.Text);
        }
    }
}

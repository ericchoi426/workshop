using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question1
{
    class ValidatorLauncher
    {
        static void Main(string[] args)
        {
            Validator validator = new Validator();
            string strLine, strID, strPsw;

            while (true)
            {
                strLine = Console.ReadLine();
                if (strLine.Length < 10)
                {
                    Console.WriteLine("Input Error");
                    continue;
                }

                strID = strLine.Substring(0, 8);
                strPsw = strLine.Substring(9);

                if (validator.CheckIdPsw(strID, strPsw))
                {
                    Console.WriteLine("LOGIN SUCCESS");
                    break;
                }
                else
                {
                    Console.WriteLine("LOGIN FAIL");
                }
            }
        }
    }
}

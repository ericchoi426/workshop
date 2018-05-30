using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Question2
{
    class ValidatorLauncher
    {
        static void Main(string[] args)
        {
            Validator validator = new Validator();
            string strLine, str_id, str_psw, str_busid, str_cardinfo;

            while (true)
            {
                strLine = Console.ReadLine();
                if (strLine.Length < 10)
                {
                    Console.WriteLine("Input Error");
                    continue;
                }

                str_id = strLine.Substring(0, 8);
                str_psw = strLine.Substring(9);

                if (validator.CheckIdPsw(str_id, str_psw))
                {
                    Console.WriteLine("LOGIN SUCCESS");
                    break;
                }
                else
                {
                    Console.WriteLine("LOGIN FAIL");
                }
            }

            // Inspection
            while (true)
            {
                str_busid = Console.ReadLine();  // busid 

                if (str_busid.Equals("LOGOUT"))
                    break;
                else if (str_busid.Length < 7 || !str_busid.Substring(0,4).Equals("BUS_"))
                {
                    Console.WriteLine("Wrong Bus ID");
                    continue;
                }
                
                // Get Start Time
                string strTime = DateTime.Now.ToString("yyyyMMddHHmmss");

                // Card Validation
                while (true)
                {
                    str_cardinfo = Console.ReadLine();
                    if (str_cardinfo.Equals("DONE"))
                    {
                        break;
                    }
                    else if (str_cardinfo.Length < 30)
                    {
                        Console.WriteLine("Wrong Card Info");
                        continue;
                    }
                    validator.InspectCard(strTime, str_id, str_busid, str_cardinfo);
                }
            }
        }
    }
}

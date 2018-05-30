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
        const string logDataPath = "..\\..\\..\\CLIENT\\INSPECTOR.txt";
        public static void processLogIn()
        {
            while (true)
            {
                string input = Console.ReadLine();
                if (input != null)
                {
                    string[] words = input.Split(' ');

                    string id = words[0];
                    string pwd = words[1];

                    if (CLogIn.isCollect(id, pwd))
                    {
                        CLogIn.doLogIn(id);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("LOGIN FAIL");
                    }
                }
            }
        }

        public static bool processCardInfo()
        {
            string line;
            while (true)
            {
                line = Console.ReadLine();

                if (line == null) continue;                

                if (line.Equals("DONE"))
                {
                    return true;
                }
                else
                {
                    if (line.Length < 29)
                    {
                        Console.WriteLine("Wrong Card input");
                        continue;
                    }

                    CLogIn.validate(line);
                }
            }

            return false;
        }

        static void Main(string[] args)
        {
            CLogIn.load(logDataPath);
            processLogIn();
            while(true)
            {
                if (!CLogIn.getBusId()) return;
                processCardInfo();
            } 
        }
    }
}

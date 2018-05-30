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

        static void Main(string[] args)
        {
            CLogIn.load(logDataPath);
            processLogIn();
            CLogIn.getBusId();


        }
    }
}

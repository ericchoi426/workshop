using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question1
{
    class ValidatorLauncher
    {
       
        static void Main(string[] args)
        {
            const string logDataPath = "..\\..\\..\\CLIENT\\INSPECTOR.txt";
            
            CLogIn.load(logDataPath);

            while (true)
            {
                string input = Console.ReadLine();
                if(input != null)
                {
                    string[] words = input.Split(' ');

                    string id = words[0];
                    string pwd = words[1];
                    //Console.WriteLine("id:{0} pwd:{1}", id, pwd);
                    bool test = CLogIn.doLogIn(id, pwd);
                    if (test)
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
            //Console.ReadKey();
        }
    }
}

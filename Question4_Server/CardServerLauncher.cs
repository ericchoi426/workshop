using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Question4_Server
{
    class CReport
    {
        private int card_num;
        private int strange;
        private int date;

        public int Strange { get => strange; set => strange = value; }
        public int Card_num { get => card_num; set => card_num = value; }
        public int Date { get => date; set => date = value; }
    }
    

    class CardServerLauncher
    {
        private static Dictionary<string, CReport> m_dict = new Dictionary<string, CReport>();

        public static void MakeReport()
        {
            string path = "..\\..\\..\\SERVER";
            string[] files = Directory.GetFiles(path);
            foreach(string fi in files)
            {
                // using문을 사용하면 Diposal를 자동 처리 즉 file close를 알아서 처리해줌
                using (StreamReader reader = new StreamReader(fi))
                {
                    while (true)
                    {
                        string line = reader.ReadLine();
                        if (line == null)
                            return;
                        string[] words = line.Split('#');
                        string key = words[0];
                        if (m_dict.ContainsKey(key))
                        {

                        }
                        else
                        {
                            CReport data = new CReport()
                            {
                                Card_num = 1,
                                Strange = 0,
                                Date = 0
                           
                            };
                            m_dict.Add(key, data);
                        }
                    }
                }
            }
        }

        static void Main(string[] args)
        {
   
        }
    }
}

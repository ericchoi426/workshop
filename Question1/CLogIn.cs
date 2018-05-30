using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class CLogIn
{
    private static Dictionary<string, string> m_log_info_dict = new Dictionary<string, string>();

    public static bool load(string path)
    {
        if (File.Exists(path))
        {
            // using문을 사용하면 Diposal를 자동 처리 즉 file close를 알아서 처리해줌
            using (StreamReader reader = new StreamReader(path))
            {
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                        return true;
                    string[] words = line.Split(' ');

                    if (!m_log_info_dict.ContainsKey(words[0]))
                    {
                        m_log_info_dict.Add(words[0], words[1]);
                    }
                }
            }
        }
        return false;

    }

    public CLogIn(string logInfo)
    {
    }

    public static bool doLogIn(string id, string pwd)
    {
        if (m_log_info_dict.ContainsKey(id))
        {
            string encPwd = m_log_info_dict[id];
            string input_encPwd = CardUtility.passwordEncryption_SHA256(pwd);

            if (encPwd.Equals(input_encPwd))
            {
                return true;
            }
        }

        return false;
    }
}

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

    public static bool isCollect(string id, string pwd)
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
    private static void CreateFolder(string path)
    {
        try
        {
            // Determine whether the directory exists.
            if (Directory.Exists(path))
            {
                return;
            }
            // Try to create the directory.
            DirectoryInfo di = Directory.CreateDirectory(path);
        }
        catch (Exception e)
        {
            Console.WriteLine("The process failed: {0}", e.ToString());
        }
        finally { }
    }

    public static void doLogIn(string id)
    {
        Inspector = id;
        LogInStatus = true;
        Out_folder = "..\\..\\" + Inspector + "\\";
        CreateFolder(Out_folder);
    }

    public static bool getBusId()
    {
        string line = Console.ReadLine();
        if(line.Equals("LOGOUT"))
        {
            return false;
        }
        else
        {
            Bus_id = line;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            string fileName = Inspector + "_" + date + ".txt";
            PathString = Path.Combine(Out_folder, fileName);
            if (!File.Exists(PathString))
            {
                using (FileStream fs = System.IO.File.Create(PathString))
                { }
            }
            else
            {
                return true;
            }
        }
        return true;        
    }
    public static void validate(string inputStr)
    {
        string card_id = inputStr.Substring(0, 8);
        string bus_id = inputStr.Substring(8, 7);
        string code = inputStr.Substring(15, 1);
        string time = inputStr.Substring(16, 14);
        string insptTime = DateTime.Now.ToString("yyyyMMddHHmmss");
        string inspecting;

        if (Bus_id.Equals(bus_id))
        {
            // 2. Check Aboard Code
            if (code.Equals("N"))
            {
                // 3. Time Difference
                if (CardUtility.HourDiff(insptTime, time) < 3)
                {
                    inspecting = "R1";
                }
                else
                {
                    inspecting = "R4";
                }
            }
            else
            {
                inspecting = "R3";
            }
        }
        else
        {
            inspecting = "R2";
        }

        string result = Inspector + "#" + bus_id + "#" + inputStr + "#" + inspecting + "#" + insptTime+"\n";

        if (!File.Exists(PathString))
        {
            // Create a file to write to.
            File.WriteAllText(PathString, result);
        }
        else
        {
            File.AppendAllText(PathString, result);
        }      
        
    }

    private static bool m_LogIn = false;
    private static string m_inspector;
    private static string m_bus_id;
    private static string m_out_folder;
    private static string m_pathString;
    

    public static bool LogInStatus{ get => m_LogIn; set => m_LogIn = value; }
    public static string Inspector { get => m_inspector; set => m_inspector = value; }
    public static string Bus_id { get => m_bus_id; set => m_bus_id = value; }
    public static string Out_folder { get => m_out_folder; set => m_out_folder = value; }
    public static string PathString { get => m_pathString; set => m_pathString = value; }
}

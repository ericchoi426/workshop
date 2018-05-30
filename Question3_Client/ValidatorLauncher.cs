using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Question2
{
    class ValidatorLauncher
    {
        enum DataPacketType { TEXT = 1, IMAGE };
        static byte[] m_clientData = null;

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

        private static void SendToServer(String strFilename)
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            byte[] fileName = Encoding.UTF8.GetBytes(strFilename);
            string strFolder = CLogIn.Out_folder;
            string strPath = Path.Combine(strFolder, strFilename);
            byte[] fileData = File.ReadAllBytes(strPath);
           
            byte[] fileNameLen = BitConverter.GetBytes(fileName.Length);
            byte[] fileType = BitConverter.GetBytes((int)DataPacketType.IMAGE);
            // IMAGE(4 byte) + 파일이름(4 byte) + 파일이름길이(4 byte) + 데이타 길이
            m_clientData = new byte[fileType.Length + 4 + fileName.Length + fileData.Length];

            fileType.CopyTo(m_clientData, 0);
            fileNameLen.CopyTo(m_clientData, 4);
            fileName.CopyTo(m_clientData, 8);
            fileData.CopyTo(m_clientData, 8 + fileName.Length);

            clientSocket.Connect(IPAddress.Parse("127.0.0.1"), 27015);
            clientSocket.Send(m_clientData);
            clientSocket.Close();
        }
        static void sendFilesToServer()
        {
            DirectoryInfo di = new DirectoryInfo(CLogIn.Out_folder);
            foreach (var fi in di.GetFiles("*", SearchOption.AllDirectories))
            {
                SendToServer(fi.Name);
                Thread.Sleep(100);
            }
            string targetPath = "..\\..\\..\\BACKUP";
            foreach (var fi in di.GetFiles("*", SearchOption.AllDirectories))
            {
                string orign = Path.Combine(CLogIn.Out_folder, fi.Name);
                string target = Path.Combine(targetPath, fi.Name);

                File.Move(orign, target);
            }
        }
        static void Main(string[] args)
        {
            CLogIn.load(logDataPath);
            processLogIn();
            while(true)
            {
                if (!CLogIn.getBusId()) break;
                processCardInfo();
            }
            Thread t_handler = new Thread(sendFilesToServer);
            t_handler.IsBackground = true;
            t_handler.Start();
            t_handler.Join();
        }
    }
}

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Question3_Server
{
    class StateObject
    {
        // Client socket
        public Socket workSocket = null;

        public const int BufferSize = 4096;

        // Receive buffer
        public byte[] buffer = new byte[BufferSize];
    }


    class CardServerLauncher
    {
        static bool initialFlag = true;
        static string receivedPath = string.Empty;
        enum DataPacketType { TEXT = 1, IMAGE };
        static int dataType = 0;
        static string textData = string.Empty;
        static Socket listener;
        static Mutex mutex = new Mutex();


        static void Main(string[] args)
        {
            Thread t_handler = new Thread(StartListening);
            t_handler.IsBackground = true;
            t_handler.Start();
                        
            string strLine;
            while (true)
            {
                strLine = Console.ReadLine();

                if (strLine.Equals("QUIT"))
                {
                    listener.Close();
                    break;
                }
            }
        }
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        private static void StartListening()
        {
            IPEndPoint localEP = new IPEndPoint(IPAddress.Any, 27015);
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEP);
                listener.Listen(10);

                while (true)
                {
                    allDone.Reset();
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                    allDone.WaitOne();
                }
            }
            catch (SocketException se)
            {
                Trace.WriteLine(string.Format("SocketException :{0}", se.Message));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("Exception :{0}", ex.Message));
            }
        }

        private static void AcceptCallback(IAsyncResult ar)
        {
            allDone.Set();

            Socket listener = ar.AsyncState as Socket;
            Socket handler = listener.EndAccept(ar);

            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
            initialFlag = true;
        }

        private static void ReadCallback(IAsyncResult ar)
        {
            int fileNameLen = 0;
            string content = string.Empty;
            StateObject state = ar.AsyncState as StateObject;
            Socket handler = state.workSocket;
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                if (initialFlag)
                {
                    dataType = BitConverter.ToInt32(state.buffer, 0);
                    if (dataType == (int)DataPacketType.IMAGE)
                    {
                        fileNameLen = BitConverter.ToInt32(state.buffer, 4);
                        string fileName = Encoding.UTF8.GetString(state.buffer, 8, fileNameLen);
                        
                        string pathDownload = "..\\..\\SERVER";

                        receivedPath = Path.Combine(pathDownload, fileName);
                    }
                    else if (dataType == (int)DataPacketType.TEXT)
                    {
                        textData = Encoding.UTF8.GetString(state.buffer, 4, bytesRead - 4);
                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                    }
                }

                if (dataType == (int)DataPacketType.IMAGE)
                {
                    mutex.WaitOne();
                    BinaryWriter bw = new BinaryWriter(File.Open(receivedPath, FileMode.Append));
                    if (initialFlag)
                        bw.Write(state.buffer, 8 + fileNameLen, bytesRead - (8 + fileNameLen));
                    else
                        bw.Write(state.buffer, 0, bytesRead);

                    initialFlag = false;
                    bw.Close();
                    mutex.ReleaseMutex();
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                }
            }
            else
            {
                initialFlag = true;
                //Console.WriteLine("Transfer done!!");
            }
        }
    }
}
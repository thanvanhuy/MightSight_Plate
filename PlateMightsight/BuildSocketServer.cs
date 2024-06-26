using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;

namespace PlateMightsight
{
    public class BuildSocketServer
    {
        public delegate void NewClientConnected(Socket acceptedSocket);

        public delegate void NewDataReceived(string data, IPAddress endpointAddress, int endpointPort);

        private TcpListener tcpListener;

        public string ipAddress { get; set; }

        public int listenPort { get; set; }

        public bool error { get; set; }

        public string errorMessage { get; set; }

        public bool stopWorking { get; set; }

        public event NewClientConnected newClientConnected;

        public event NewDataReceived DataReceivednew;

        public BuildSocketServer()
        {
            ipAddress = "0.0.0.0";
            listenPort = 8085;
        }

        public BuildSocketServer(int listenPort)
        {
            ipAddress = "0.0.0.0";
            this.listenPort = listenPort;
        }

        public BuildSocketServer(string ipAddress, int listenPort)
        {
            this.listenPort = listenPort;
            this.ipAddress = ipAddress;
        }

        public void Start()
        {
            ThreadPool.QueueUserWorkItem(ListenForClient);
        }

        private void ListenForClient(object state)
        {
            error = false;
            errorMessage = string.Empty;
            try
            {
                tcpListener = new TcpListener(IPAddress.Parse(ipAddress), listenPort);
                tcpListener.Start();
                stopWorking = false;
                while (!stopWorking)
                {
                    try
                    {
                        Socket socket = tcpListener.AcceptSocket();
                        if (this.newClientConnected != null)
                        {
                            this.newClientConnected?.Invoke(socket);
                        }

                        ThreadPool.QueueUserWorkItem(GetDataFromClient, socket);
                    }
                    catch (Exception ex)
                    {
                        error = true;
                        errorMessage = "Error in waiting for client's connection: " + ex.Message;
                    }
                }
            }
            catch (Exception ex2)
            {
                error = true;
                errorMessage = "Error in constructing class HttpServer: " + ex2.Message;
            }
        }

        private void GetDataFromClient(object objLcSocket)
        {
            error = false;
            errorMessage = string.Empty;
            Socket socket = (Socket)objLcSocket;
            try
            {
                IPEndPoint iPEndPoint = socket.RemoteEndPoint as IPEndPoint;
                socket.ReceiveBufferSize = 65536 * 2;
                socket.ReceiveTimeout = 100;
                int num = 0;
                int num2 = 0;
                byte[] array = new byte[1048576 * 2];
                while (!stopWorking)
                {
                    try
                    {
                        num = socket.Receive(array, num2, socket.ReceiveBufferSize, SocketFlags.None);
                        num2 += num;
                        if (num2 + 1024 > array.Length)
                        {
                            byte[] array2 = new byte[array.Length + 1048576];
                            Array.Copy(array, 0, array2, 0, num2);
                            array = array2;
                        }

                        if (num == 0)
                        {
                            error = true;
                            errorMessage = "Nhận dữ liệu lỗi : GetDataFromClient";
                        }
                    }
                    catch (SocketException)
                    {
                        if (num2 > 0)
                        {
                            string text = Encoding.ASCII.GetString(array, 0, num2);
                            int num3 = text.IndexOf("\r\n\r\n") + 4;
                            if (num3 >= 0)
                            {
                                text = text.Substring(num3);
                            }

                            if (this.DataReceivednew != null)
                            {
                                this.DataReceivednew?.Invoke(text, iPEndPoint.Address, iPEndPoint.Port);
                            }

                            goto close;
                        }

                        num2 = 0;
                        array = new byte[1048576 * 2];
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                error = true;
                errorMessage = "Nhận dữ liệu lỗi : GetDataFromClient";
            }
            catch (Exception)
            {
                error = true;
                errorMessage = "Nhận dữ liệu lỗi : GetDataFromClient";
            }

            goto close;
        close:
            socket.Close();
        }
    }
}

using System;
using System.Net;
using System.Net.Sockets;

namespace Common.Utils
{
    public class SocketClient
    {
        public string SocketName;

        internal Socket ConnectionSocket { get; set; }

        protected System.Text.Encoding decoder = new System.Text.UTF8Encoding();

        public SocketClient()
        {

        }

        public SocketClient(string name)
        {
            SocketName = name;
        }

        public void ConnectServer(string ip, string prot)
        {
            IPAddress ipAddress = IPAddress.Parse(ip);
            BeginConnect(ipAddress, Int32.Parse(prot));
        }

        private void BeginConnect(IPAddress IPAddress, int Port)
        {
            try
            {
                ConnectionSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ConnectionSocket.Connect(new System.Net.IPEndPoint(IPAddress, Port)); //配置服务器IP与端口 
                byte[] SendDataBuffer = new byte[1024];//decoder.GetBytes(WebSocketContractBuilder.BuildRequestContract(IPAddress.ToString(), Port.ToString()));         
                DoOnSucess("相机连接成功");
                ConnectionSocket.BeginSend(SendDataBuffer, 0, SendDataBuffer.Length, SocketFlags.None, EndConnect, this);
            }
            catch (Exception ex)
            {
                DoOnError(ex.Message);
            }
        }

        internal byte[] ReceivedDataBuffer { get; set; } = new byte[1024 * 1024 * 10];
        private void EndConnect(IAsyncResult ar)
        {
            var client = (SocketClient)ar.AsyncState;
            client.ConnectionSocket.EndSend(ar);
            Array.Clear(ReceivedDataBuffer, 0, ReceivedDataBuffer.Length);
            client.ConnectionSocket.BeginReceive(ReceivedDataBuffer, 0, ReceivedDataBuffer.Length, SocketFlags.None, BeginLogin, client);
        }

        private void BeginLogin(IAsyncResult ar)
        {
            try
            {
                var client = (SocketClient)ar.AsyncState;
                var ReceiveCount = client.ConnectionSocket.EndReceive(ar);
                var ReceiveData = decoder.GetString(ReceivedDataBuffer, 0, ReceiveCount);
                Array.Clear(ReceivedDataBuffer, 0, ReceivedDataBuffer.Length);
                DoOnReceive(ReceiveData);

                if (client.ConnectionSocket.Connected)
                {
                    client.ConnectionSocket.BeginReceive(client.ReceivedDataBuffer, 0, client.ReceivedDataBuffer.Length, SocketFlags.None, BeginLogin, client);
                }
                else
                {
                    DoOnClosed("连接关闭");
                }
            }
            catch (Exception ex)
            {
                DoOnError(ex.Message);
            }
        }

        private void DoOnReceive(string text)
        {
            if (this.OnReceive != null)
            {
                this.OnReceive(this, text);
            }
        }

        private void DoOnError(string text)
        {
            if (this.OnError != null)
            {
                this.OnError(this, text);
            }
        }

        private void DoOnClosed(string text)
        {
            if (this.OnClosed != null)
            {
                this.OnClosed(this, text);
            }
        }

        private void DoOnSucess(string text)
        {
            if (this.OnSucess != null)
            {
                this.OnSucess(this, text);
            }
        }

        public delegate bool SocketEventHandler(object obj, string text);
        public SocketEventHandler OnReceive;
        public SocketEventHandler OnError;
        public SocketEventHandler OnClosed;
        public SocketEventHandler OnSucess;


        public void Dispose()
        {
            this.ConnectionSocket.Close();
            this.ConnectionSocket.Dispose();
        }
    }
}

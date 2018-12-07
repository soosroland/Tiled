using CocosSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Tiled.Droid.Entities;
using Tiled.Shared;

namespace Tiled
{
    public class MainLayer : CCLayer
    {
        MenuLayer menuLayer;
        GameLayer gameLayer;
        HighScoreLayer highScoreLayer;
        HowToPlayLayer howToPlayLayer;
        YouDiedLayer youDiedLayer;
        VictoryLayer victoryLayer;
        PlayMenuLayer PlayMenuLayer;
        SinglePlayerLayer SingelPlayerLayer;
        MultiPlayerLayer MultiPlayerLayer;

        NetworkStream serverStream;
        private static int bufferSize = 2048;
        private static byte[] buffer = new byte[bufferSize];
        private static List<Socket> clientSockets = new List<Socket>();
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public MainLayer()
        {
            System.Diagnostics.Debug.WriteLine("Setting Up Server Plz Wait");
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 8889));
            serverSocket.Listen(10);
            serverSocket.BeginAccept(new AsyncCallback(CallBack), null);
            System.Diagnostics.Debug.WriteLine("Server Made");
            var tcpClient = new TcpClient("192.168.0.248", 8888); // Emulator server address
            //var tcpClient = new TcpClient("10.0.2.2", 8888); // Emulator server address
            serverStream = tcpClient.GetStream();
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();
            this.AddChild(menuLayer = new MenuLayer(this, serverStream));
        }
         
        public void StartGame(String level_num, int hp, String speed)
        {
            this.RemoveChild(PlayMenuLayer);
            this.AddChild(gameLayer = new GameLayer(this, level_num, hp, speed));
        }

        public void PlayMenu()
        {
            this.RemoveChild(menuLayer);
            this.AddChild(PlayMenuLayer = new PlayMenuLayer(this));
        }

        public void SinglePlayer()
        {
            this.RemoveChild(PlayMenuLayer);
            this.AddChild(SingelPlayerLayer = new SinglePlayerLayer(this));
        }

        public void MultiPlayer()
        {
            this.RemoveChild(PlayMenuLayer);
            this.AddChild(MultiPlayerLayer = new MultiPlayerLayer(this, serverStream));
        }

        public void HighScores()
        {
            this.RemoveChild(menuLayer);
            this.AddChild(highScoreLayer = new HighScoreLayer(this));
        }

        public void HowToPlay()
        {
            this.RemoveChild(menuLayer);
            this.AddChild(howToPlayLayer = new HowToPlayLayer(this));
        }



        public void BackToMenu()
        {
            this.RemoveAllChildren();
            this.AddChild(menuLayer);
        }

        public void Victory(String s, String level, int missed_coins, int lost_lives)
        {
            this.RemoveAllChildren();
            this.AddChild(victoryLayer = new VictoryLayer(this, s, level, missed_coins, lost_lives));
        }

        public void Died()
        {
            this.RemoveAllChildren();
            this.AddChild(youDiedLayer = new YouDiedLayer(this));
        }

        void CallBack(IAsyncResult e)
        {
            try
            {
                Socket socket = serverSocket.EndAccept(e);
                clientSockets.Add(socket);
                System.Diagnostics.Debug.WriteLine("Client connected");
                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), socket);
            }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex + " Errooooor"); }
        }

        private static void ReceiveCallBack(IAsyncResult e)
        {
            try
            {
                Socket socket = (Socket)e.AsyncState;
                int received;
                try
                {
                    received = socket.EndReceive(e);
                }
                catch (SocketException)
                {
                    System.Diagnostics.Debug.WriteLine("Server forcefully disconnected");
                    socket.Close();
                    clientSockets.Remove(socket);
                    return;
                }
                byte[] dataBuf = new byte[received];
                Array.Copy(buffer, dataBuf, received);

                string text = System.Text.Encoding.ASCII.GetString(dataBuf);
                System.Diagnostics.Debug.WriteLine("Server request: Menu: " + text);
                socket.BeginReceive(buffer, 0, bufferSize, SocketFlags.None, ReceiveCallBack, socket);
            }
            catch (Exception) { }
        }
    }
}

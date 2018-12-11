﻿using CocosSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Tiled.Droid;
using Tiled.Droid.Entities;
using Tiled.Shared;

namespace Tiled
{
    public class MainLayer : CCLayer
    {
        MenuLayer menuLayer;
        GameLayer gameLayer;
        MultiPlayerGameLayer multiPlayerGameLayer;
        MultiPlayerLoadingLayer multiPlayerLoadingLayer;
        HighScoreLayer highScoreLayer;
        HowToPlayLayer howToPlayLayer;
        YouDiedLayer youDiedLayer;
        VictoryLayer victoryLayer;
        PlayMenuLayer PlayMenuLayer;
        SinglePlayerLayer SinglePlayerLayer;
        MultiPlayerLayer MultiPlayerLayer;

        NetworkStream serverStream;
        private static int bufferSize = 2048;
        private static byte[] buffer = new byte[bufferSize];
        private static List<Socket> clientSockets = new List<Socket>();
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        
        static int _player_id = 0;
        static String _level_num;
        static int _hp;
        static String _speed;
        static int player_count = 2;

        public MainLayer()
        {
            System.Diagnostics.Debug.WriteLine("Setting Up Server Plz Wait");
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 8889));
            serverSocket.Listen(10);
            serverSocket.BeginAccept(new AsyncCallback(CallBack), null);
            System.Diagnostics.Debug.WriteLine("Server Made");
            var tcpClient = new TcpClient("192.168.0.248", 8888); // Emulator server address
            //var tcpClient = new TcpClient("192.168.3.102", 8888); // Emulator server address
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
            this.RemoveChild(SinglePlayerLayer);
            this.AddChild(gameLayer = new GameLayer(this, level_num, hp, speed));
        }

        public void StartMultiPlayerGame2(String level_num, int hp, String speed)
        {
            this.RemoveChild(multiPlayerLoadingLayer);
            this.AddChild(multiPlayerGameLayer = new MultiPlayerGameLayer(this, level_num, hp, speed, serverStream, _player_id, player_count));
        }

        public void StartMultiPlayerGame(String level_num, int hp, String speed)
        {
            _level_num = level_num;
            _hp = hp;
            _speed = speed;
            this.RemoveChild(MultiPlayerLayer);
            this.AddChild(multiPlayerLoadingLayer = new MultiPlayerLoadingLayer(this, serverStream));
        }

        public void PlayMenu()
        {
            this.RemoveChild(menuLayer);
            this.AddChild(PlayMenuLayer = new PlayMenuLayer(this));
        }

        public void SinglePlayer()
        {
            this.RemoveChild(PlayMenuLayer);
            this.AddChild(SinglePlayerLayer = new SinglePlayerLayer(this));
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

        private void ReceiveCallBack(IAsyncResult e)
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
                HandleIncomingEvent(text, socket.RemoteEndPoint.ToString().Split(':')[0]);
                System.Diagnostics.Debug.WriteLine("Server request: Menu: " + text);
                socket.BeginReceive(buffer, 0, bufferSize, SocketFlags.None, ReceiveCallBack, socket);
            }
            catch (Exception) { }
        }

        protected void HandleIncomingEvent(String text, String ip)
        {
            String[] temp = text.Split(';');
            switch (temp[0])
            {
                case "StartGame":
                    _player_id = int.Parse(temp[1]);
                    StartMultiPlayerGame2(_level_num, _hp, _speed);
                    break;
                default:
                    break;
            }
        }
    }
}

using CocosSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Tiled.Droid.Entities;

namespace Tiled
{
    class MultiPlayerLoadingLayer : CCNode
    {
        MainLayer _mainLayer;
        NetworkStream _serverStream;

        Button backGround;
        CCEventListenerTouchAllAtOnce touchListener;

        private static int bufferSize = 2048;
        private static byte[] buffer = new byte[bufferSize];
        private static List<Socket> clientSockets = new List<Socket>();
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


        public MultiPlayerLoadingLayer(MainLayer mainLayer, NetworkStream serverStream)
        {
            _mainLayer = mainLayer;
            _serverStream = serverStream;

            backGround = new Button("WaitingScreen.png");
            backGround.Position = new CCPoint(192, 120);
            AddChild(backGround);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            Schedule(
               (dt) =>
               {
                   if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                   {
                       byte[] outStream = System.Text.Encoding.ASCII.GetBytes("MatchMakingCanceled");
                       _serverStream.Write(outStream, 0, outStream.Length);
                       _serverStream.Flush();
                       serverSocket.Close();
                       _mainLayer.BackToMenu();
                   }
               }
           );

            touchListener = new CCEventListenerTouchAllAtOnce();
            AddEventListener(touchListener, this);
        }
    }
}

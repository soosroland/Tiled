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
        Button start;
        CCEventListenerTouchAllAtOnce touchListener;

        private static int bufferSize = 2048;
        private static byte[] buffer = new byte[bufferSize];
        private static List<Socket> clientSockets = new List<Socket>();
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


        public MultiPlayerLoadingLayer(MainLayer mainLayer, NetworkStream serverStream)
        {
            _mainLayer = mainLayer;
            _serverStream = serverStream;

            backGround = new Button("background.png");
            backGround.Position = new CCPoint(192, 120);
            AddChild(backGround);

            start = new Button("continue.png");
            start.Scale = 1.5f;
            start.Position = new CCPoint(200, 30);
            AddChild(start);

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
            touchListener.OnTouchesBegan = HandleInput;
            AddEventListener(touchListener, this);
        }

        private void HandleInput(System.Collections.Generic.List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                foreach (var touch in touches)
                {
                    if (start.sprite.BoundingBoxTransformedToWorld.ContainsPoint(touch.Location))
                    {
                        //byte[] outStream = System.Text.Encoding.ASCII.GetBytes("Matchmaking;temp[1];temp[2];2;temp[4];temp[5]");
                        //_serverStream.Write(outStream, 0, outStream.Length);
                        //_serverStream.Flush();
                        _mainLayer.StartMultiPlayerGame2(("1").ToString(), 5, "normal");
                    }
                }
            }
        }
    }
}

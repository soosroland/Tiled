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
    class MultiPlayerLayer : CCNode
    {
        MainLayer _mainLayer;
        NetworkStream _serverStream;

        Button backGround;
        Button level_left;
        Button level_right;
        Button maxPlayer_left;
        Button maxPlayer_right;
        Button start;
        CCLabel level;
        CCLabel maxplayer;
        CCEventListenerTouchAllAtOnce touchListener;
        List<String> level_List;
        List<int> maxplayer_List;
        int actual_maxplayer;
        int actual_level;
        static String text;
        static int _player_count;
        
        private static int bufferSize = 2048;
        private static byte[] buffer = new byte[bufferSize];
        private static List<Socket> clientSockets = new List<Socket>();
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static TcpClient tcpClient;
        public MultiPlayerLayer(MainLayer mainLayer, NetworkStream serverStream)
        {
            _mainLayer = mainLayer;
            _serverStream = serverStream;

            backGround = new Button("background.png");
            backGround.Position = new CCPoint(192, 120);
            AddChild(backGround);

            level_List = new List<String>();
            level_List.Add("Level 1");
            level_List.Add("Level 2");
            level_List.Add("Level 3");
            level_List.Add("Level 4");
            level_List.Add("Level 5");
            level_List.Add("Level 6");
            level_List.Add("Level 7");
            level_List.Add("Level 8");
            level_List.Add("Level 9");
            actual_level = 0;

            maxplayer_List = new List<int>();
            maxplayer_List.Add(1);
            maxplayer_List.Add(2);
            actual_maxplayer = 1;

            level_left = new Button("arrow_left.png");
            level_left.Position = new CCPoint(130, 170);
            AddChild(level_left);

            level = new CCLabel("Level 1", "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            level.Color = new CCColor3B(0, 0, 0);
            level.Position = new CCPoint(200, 170);
            AddChild(level);

            level_right = new Button("arrow_right.png");
            level_right.Position = new CCPoint(270, 170);
            AddChild(level_right);

            maxPlayer_left = new Button("arrow_left.png");
            maxPlayer_left.Position = new CCPoint(130, 120);
            AddChild(maxPlayer_left);

            maxplayer = new CCLabel(maxplayer_List[actual_maxplayer].ToString(), "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            maxplayer.Color = new CCColor3B(0, 0, 0);
            maxplayer.Position = new CCPoint(200, 120);
            AddChild(maxplayer);

            maxPlayer_right = new Button("arrow_right.png");
            maxPlayer_right.Position = new CCPoint(270, 120);
            AddChild(maxPlayer_right);

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
                        int hp = 5;
                        _player_count = maxplayer_List[actual_maxplayer];
                        byte[] outStream = System.Text.Encoding.ASCII.GetBytes("Matchmaking;" + level_List[actual_level] + ";normal;"
                            + _player_count + ";normal;normal");
                        _serverStream.Write(outStream, 0, outStream.Length);
                        _serverStream.Flush();
                        _mainLayer.StartMultiPlayerGame((actual_level + 1).ToString(), hp, "normal", _player_count);
                    }
                    else if (level_left.sprite.BoundingBoxTransformedToWorld.ContainsPoint(touch.Location))
                    {
                        if (actual_level > 0)
                        {
                            actual_level--;
                        }
                        else
                        {
                            actual_level = level_List.Count - 1;
                        }
                        level.Text = level_List[actual_level];
                    }
                    else if (level_right.sprite.BoundingBoxTransformedToWorld.ContainsPoint(touch.Location))
                    {
                        if (actual_level < level_List.Count - 1)
                        {
                            actual_level++;
                        }
                        else
                        {
                            actual_level = 0;
                        }
                        level.Text = level_List[actual_level];
                    }
                    else if (maxPlayer_left.sprite.BoundingBoxTransformedToWorld.ContainsPoint(touch.Location))
                    {
                        if (actual_maxplayer > 0)
                        {
                            actual_maxplayer--;
                        }
                        else
                        {
                            actual_maxplayer = maxplayer_List.Count - 1;
                        }
                        maxplayer.Text = maxplayer_List[actual_maxplayer].ToString();
                    }
                    else if (maxPlayer_right.sprite.BoundingBoxTransformedToWorld.ContainsPoint(touch.Location))
                    {
                        if (actual_maxplayer < maxplayer_List.Count - 1)
                        {
                            actual_maxplayer++;
                        }
                        else
                        {
                            actual_maxplayer = 0;
                        }
                        maxplayer.Text = maxplayer_List[actual_maxplayer].ToString();
                    }
                }
            }
        }
    }
}

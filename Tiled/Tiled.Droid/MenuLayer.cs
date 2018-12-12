using System;
using System.Collections.Generic;
using System.Net.Sockets;
using CocosSharp;
using Tiled.Droid.Entities;

namespace Tiled.Shared
{
    public class MenuLayer : CCLayerColor
    {
        //CCLabel label;
        Button play;
        Button highscores;
        Button howtoplay;
        Button backGround;
        MainLayer _mainLayer;
        NetworkStream _serverStream;

        public MenuLayer(MainLayer gameLayer, NetworkStream serverStream)
            : base(CCColor4B.Blue)
        {
            _mainLayer = gameLayer;
            _serverStream = serverStream;

            backGround = new Button("background");
            backGround.Position = new CCPoint(192, 120);
            AddChild(backGround);

            play = new Button("Play");
            play.Scale = 2;
            play.Position = new CCPoint(192, 150);
            AddChild(play);

            highscores = new Button("HighScores")
            {
                Scale = 2,
                Position = new CCPoint(192, 90)
            };
            AddChild(highscores);
            
            howtoplay = new Button("HowToPlay");
            howtoplay.Scale = 2;
            howtoplay.Position = new CCPoint(192, 30);
            AddChild(howtoplay);

        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            // Use the bounds to layout the positioning of our drawable assets
            var bounds = VisibleBoundsWorldspace;
   
            // Register for touch events
            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesEnded = OnTouchesEnded;
            AddEventListener(touchListener, this);
        }

        


        void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            foreach (var touch in touches)
            {
                if (play.sprite.BoundingBoxTransformedToWorld.ContainsPoint(touch.Location))
                {
                    _mainLayer.PlayMenu();
                }
                else if (highscores.sprite.BoundingBoxTransformedToWorld.ContainsPoint(touch.Location))
                {
                    _mainLayer.HighScores();
                }
                else if (howtoplay.sprite.BoundingBoxTransformedToWorld.ContainsPoint(touch.Location))
                {
                    _mainLayer.HowToPlay();
                }
            }
        }
    }
}


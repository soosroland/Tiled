using CocosSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Tiled.Droid.Entities;

namespace Tiled
{
    public class PlayMenuLayer : CCNode
    {
        MainLayer _mainLayer;
        Button backGround;
        Button Single_player;
        Button Multi_player;

        public PlayMenuLayer(MainLayer mainLayer)
        {
            _mainLayer = mainLayer;

            backGround = new Button("background_1")
            {
                Position = new CCPoint(192, 120)
            };
            AddChild(backGround);

            Single_player = new Button("LevelSelect") // Single Player létrehozása
            {
                Scale = 2,
                Position = new CCPoint(192, 150)
            };
            AddChild(Single_player);

            Multi_player = new Button("HowToPlay") // Multi Player létrehozása
            {
                Scale = 2,
                Position = new CCPoint(192, 90)
            };
            AddChild(Multi_player);
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
                if (Single_player.sprite.BoundingBoxTransformedToWorld.ContainsPoint(touch.Location))
                {
                    _mainLayer.SinglePlayer();
                }
                else if (Multi_player.sprite.BoundingBoxTransformedToWorld.ContainsPoint(touch.Location))
                {
                    _mainLayer.MultiPlayer();
                }
            }
        }
    }
}
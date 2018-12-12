using CocosSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Tiled.Droid.Entities;

namespace Tiled.Droid
{
    class MultiPlayerSelector : CCNode
    {
        MainLayer _mainLayer;
        Button backGround;
        Button custom;
        Button Standard;
        Button GoldRush;

        public MultiPlayerSelector(MainLayer mainLayer)
        {
            _mainLayer = mainLayer;

            backGround = new Button("background_1")
            {
                Position = new CCPoint(192, 120)
            };
            AddChild(backGround);

            Standard = new Button("Standard") // Single Player létrehozása
            {
                Scale = 2,
                Position = new CCPoint(192, 180)
            };
            AddChild(Standard);

            GoldRush = new Button("GoldRush") // Multi Player létrehozása
            {
                Scale = 2,
                Position = new CCPoint(192, 120)
            };
            AddChild(GoldRush);

            custom = new Button("Custom");
            custom.Scale = 2;
            custom.Position = new CCPoint(192, 60);
            AddChild(custom);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            Schedule(
               (dt) =>
               {
                   if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                   {
                       _mainLayer.BackToMenu();
                   }
               }
           );

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
                if (Standard.sprite.BoundingBoxTransformedToWorld.ContainsPoint(touch.Location))
                {
                    _mainLayer.MultiPlayer();
                }
                else if (GoldRush.sprite.BoundingBoxTransformedToWorld.ContainsPoint(touch.Location))
                {
                    _mainLayer.GoldRushSelector();
                }
                else if (custom.sprite.BoundingBoxTransformedToWorld.ContainsPoint(touch.Location))
                {
                    _mainLayer.MultiPlayer();
                }
            }
        }
    }
}
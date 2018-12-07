using CocosSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Tiled.Droid.Entities;

namespace Tiled
{
    class YouDiedLayer : CCLayer
    {
        MainLayer _mainLayer;
        public YouDiedLayer(MainLayer mainLayer)
        {
            _mainLayer = mainLayer;
            Button backGround = new Button("youdied.jpg");
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
                       _mainLayer.BackToMenu();
                   }
               }
            );

            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesBegan = OnTouchesBegan;
            AddEventListener(touchListener, this);
        }

        void OnTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
        {
            _mainLayer.BackToMenu();
        }
    }
}

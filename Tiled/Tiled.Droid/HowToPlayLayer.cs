using CocosSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Tiled.Droid.Entities;

namespace Tiled
{
    class HowToPlayLayer : CCNode
    {
        MainLayer _mainLayer;
        CCEventListenerTouchAllAtOnce touchListener;
        Button backGround;
        Button continue_button;
        int page;
        public HowToPlayLayer(MainLayer mainLayer)
        {
            _mainLayer = mainLayer;
            page = 0;
            backGround = new Button("howtoplay_1.png");
            backGround.Position = new CCPoint(192, 120);
            AddChild(backGround);
            continue_button = new Button("continue.png");
            continue_button.Scale = 1.5f;
            continue_button.Position = new CCPoint(330, 20);
            AddChild(continue_button);
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
                    if (continue_button.sprite.BoundingBoxTransformedToWorld.ContainsPoint(touch.Location))
                    {
                        if (page == 0)
                        {
                            this.RemoveChild(backGround);
                            this.RemoveChild(continue_button);
                            backGround = new Button("how_to_play_2.png");
                            backGround.Position = new CCPoint(192, 120);
                            this.AddChild(backGround);
                            this.AddChild(continue_button);
                            page++;
                        }
                        else if(page == 1)
                        {
                            this.RemoveChild(backGround);
                            this.RemoveChild(continue_button);
                            backGround = new Button("how_to_play_3.png");
                            backGround.Position = new CCPoint(192, 120);
                            this.AddChild(backGround);
                            this.AddChild(continue_button);
                            page++;
                        }
                        else
                        {
                            _mainLayer.BackToMenu();
                        }
                    }
                }
            }
        }
    }
}

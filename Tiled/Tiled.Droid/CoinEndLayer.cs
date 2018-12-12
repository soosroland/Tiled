using CocosSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Tiled.Droid.Entities;

namespace Tiled
{
    class CoinEndLayer : CCLayer
    {
        MainLayer _mainLayer;
        NetworkStream _serverStream;
        CCLabel grat;
        CCLabel coin1_label;
        CCLabel coin2_label;
        public CoinEndLayer(MainLayer mainLayer, int time, int coin1, int coin2, int id, NetworkStream serverStream)
        {
            _mainLayer = mainLayer;
            _serverStream = serverStream;
            Button backGround = new Button("vs");
            backGround.Position = new CCPoint(192, 120);
            AddChild(backGround);
            if ((id == 0) && (coin1 > coin2) || (id == 1) && (coin2 > coin1))
            {
                grat = new CCLabel("Congratulations you won!", "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
                grat.Color = new CCColor3B(0, 0, 0);
                grat.Position = new CCPoint(192, 210);
                AddChild(grat);
            }
            else
            {
                grat = new CCLabel("You lost!", "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
                grat.Color = new CCColor3B(0, 0, 0);
                grat.Position = new CCPoint(192, 210);
                AddChild(grat);
            }

            coin1_label = new CCLabel(coin1.ToString(), "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            coin1_label.Color = new CCColor3B(0, 0, 0);
            coin1_label.Position = new CCPoint(60, 115);
            AddChild(coin1_label);
            coin2_label = new CCLabel(coin2.ToString(), "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            coin2_label.Color = new CCColor3B(0,0,0);
            coin2_label.Position = new CCPoint(324, 115);
            AddChild(coin2_label);
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

using CocosSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Tiled.Droid.Entities;

namespace Tiled
{
    class VictoryLayer : CCLayer
    {
        MainLayer _mainLayer;
        CCLabel grat;
        CCLabel time_label;
        CCLabel best_time_label;
        public VictoryLayer(MainLayer mainLayer, String time, String level, int missed_coins, int lost_lives)
        {
            _mainLayer = mainLayer;
            Button backGround = new Button("victory.jpg");
            backGround.Position = new CCPoint(192, 120);
            AddChild(backGround);
            grat = new CCLabel("Congratulations!", "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            grat.Color = new CCColor3B(0, 0, 0);
            grat.Position = new CCPoint(192, 210);
            AddChild(grat);

            int min = Int32.Parse(time.Split(':')[0]);
            int sec = Int32.Parse(time.Split(':')[1]);
            sec = sec + missed_coins*5 + lost_lives*10;
            if (sec > 60)
            {
                min += sec / 60;
                sec -= 60;
            }

            time_label = new CCLabel("Your time was: " + min + ":" + sec, "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            time_label.Color = new CCColor3B(255, 255, 255);
            time_label.Position = new CCPoint(192, 60);
            AddChild(time_label);
            best_time_label = new CCLabel("Best time was: " + CCUserDefault.SharedUserDefault.GetStringForKey("level_" + level, "-"), "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            best_time_label.Color = new CCColor3B(255, 255, 255);
            best_time_label.Position = new CCPoint(192, 30);
            AddChild(best_time_label);

            if (CCUserDefault.SharedUserDefault.GetStringForKey("level_" + level, "-") != "-")
            {
                /*int now_min = Int32.Parse(time.Split(':')[0]);
                int now_sec = Int32.Parse(time.Split(':')[1]);*/

                int best_min = Int32.Parse(CCUserDefault.SharedUserDefault.GetStringForKey("level_" + level, "-").Split(':')[0]);
                int best_sec = Int32.Parse(CCUserDefault.SharedUserDefault.GetStringForKey("level_" + level, "-").Split(':')[1]);

                if (best_min * 60 + best_sec > min * 60 + sec)
                {
                    CCUserDefault.SharedUserDefault.SetStringForKey("level_" + level, min + ":" + sec);
                    CCUserDefault.SharedUserDefault.Flush();
                }
            }
            else
            {
                CCUserDefault.SharedUserDefault.SetStringForKey("level_" + level, min + ":" + sec);
                CCUserDefault.SharedUserDefault.Flush();
            }
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

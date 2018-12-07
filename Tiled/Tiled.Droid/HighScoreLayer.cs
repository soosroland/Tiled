using CocosSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Tiled.Droid.Entities;

namespace Tiled
{
    class HighScoreLayer : CCNode
    {
        MainLayer _mainLayer;
        CCLabel label;
        public HighScoreLayer(MainLayer mainLayer)
        {
            _mainLayer = mainLayer;
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

            // Creates or Overwrites a key called "highscore" with the int value stored in the field "playerScore"
            //CCUserDefault.SharedUserDefault.SetStringForKey("playerName", "Roli");
            // Flush is then required to perform the action of the above line
            //CCUserDefault.SharedUserDefault.Flush();

            // Retrieves the int stored in the database with the key "highscore" and sets it to the field "highScore"
            //String level1 = CCUserDefault.SharedUserDefault.GetStringForKey("level2", "inf");

            Button backGround = new Button("background_1");
            backGround.Position = new CCPoint(192, 120);
            AddChild(backGround);

            int size = 240 / 11;
            label = new CCLabel("HIGH SCORES", "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            label.Scale = 1.2f;
            label.Color = new CCColor3B(0, 0, 0);
            label.Position = new CCPoint(184, 215);
            this.AddChild(label);
            for(int i = 1; i < 10; i++)
            {
                label = new CCLabel("Level " + i + ":               " + CCUserDefault.SharedUserDefault.GetStringForKey("level_"+i, "-"), "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
                label.Scale = 0.5f;
                label.Color = new CCColor3B(0, 0, 0);
                label.Position = new CCPoint(184, 240 - ((i+1) * size) - 15);
                this.AddChild(label);
            }
        }
    }
}

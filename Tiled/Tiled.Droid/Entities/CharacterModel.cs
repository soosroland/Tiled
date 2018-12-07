using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CocosSharp;

namespace Tiled.Droid.Entities
{
    public class CharacterModel : Entities
    {
        public CCSprite sprite;
        public CCSprite sprite_shielded;
        bool shielded = false;
        
        
        public CharacterModel(String str, String part) : base()
        {
            sprite = new CCSprite(str + part + ".png");
            sprite_shielded = new CCSprite("shielded_" + part + ".png");
            this.AddChild(sprite);
        }

        public override void MoveX(int x)
        {
            sprite.PositionX += x;
        }

        public override void MoveY(int y)
        {
            sprite.PositionY += y;
        }
        public void Shield()
        {
            shielded = true;
        }
        public void UnShield()
        {
            shielded = false;
        }
        public void ChangeSprite()
        {
            if (shielded)
            {
                this.RemoveAllChildren();
                this.AddChild(sprite_shielded);
            }
            else
            {
                this.RemoveAllChildren();
                this.AddChild(sprite);
            }
        }
    }
}
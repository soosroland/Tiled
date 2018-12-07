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
    public class Door : Entities
    {
        public CCSprite sprite_closed;
        public CCSprite sprite_opened;
        public int number { get; set; }
        public String type { get; set; }
        public String part { get; set; }
        public bool open { get; set; }

        String opened;
        String closed;

        public Door(String s, String o, String p) : base()
        {
            type = s;
            part = p;
            if (o == "true")
            {
                open = true;
            }
            else
            {
                open = false;
            }
            opened = s + "_open_";
            closed = s + "_closed_";
            opened += p;
            closed += p;
            sprite_opened = new CCSprite(opened +".png");
            sprite_closed = new CCSprite(closed + ".png");
            if (open)
            {
                this.AddChild(sprite_opened);
            }
            else
            {
                this.AddChild(sprite_closed);
            }
        }
        public override void MoveX(int x)
        {
            this.PositionX += x;
        }

        public override void MoveY(int y)
        {
            this.PositionY += y;
        }

        public void Open()
        {
            this.RemoveAllChildren();
            this.AddChild(sprite_opened);
        }
    }
}
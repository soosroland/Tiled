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
    public class Treasure : Entities
    {
        public CCSprite sprite_closed;
        public CCSprite sprite_opened;
        bool opened;

        public Treasure(String str) : base()
        {
            opened = false;
            sprite_closed = new CCSprite(str + "_closed.png");
            sprite_opened = new CCSprite(str + "_opened.png");
            this.AddChild(sprite_closed);
        }

        public bool IsOpen()
        {
            return opened;
        }

        public void Open()
        {
            opened = true;
            this.RemoveChild(sprite_closed);
            this.AddChild(sprite_opened);
        }

        public void Close()
        {
            opened = false;
            this.RemoveChild(sprite_opened);
            this.AddChild(sprite_closed);
        }

        public override void MoveX(int x)
        {
            this.PositionX += x;
            /*sprite_opened.PositionX += x;
            sprite_closed.PositionX += x;*/
            /*sprite2.PositionX += x;
            sprite3.PositionX += x;
            sprite4.PositionX += x;*/
        }

        public override void MoveY(int y)
        {
            this.PositionY += y;
            /*sprite_opened.PositionY += y;
            sprite_closed.PositionY += y;*/
            /*sprite2.PositionY += y;
            sprite3.PositionY += y;
            sprite4.PositionY += y;*/
        }

        public void Interact()
        {
            if (IsOpen())
            {
                Close();
            }
            else
            {
                Open();
            }
        }
    }
}
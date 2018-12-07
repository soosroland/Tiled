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
    public class Wall : Entities
    {
        public CCSprite wall;

        public Wall(String s) : base()
        {
            wall = new CCSprite("wall.png");
            this.AddChild(wall);
        }
        public Wall() : base()
        {
            wall = new CCSprite("wall.png");
            this.AddChild(wall);
        }
        public override void MoveX(int x)
        {
            this.PositionX += x;
        }

        public override void MoveY(int y)
        {
            this.PositionY += y;
        }
    }
}
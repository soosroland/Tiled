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
    public class ImmortalityDrink : Entities
    {
        public CCSprite immortalitydrink;

        public ImmortalityDrink() : base()
        {
            immortalitydrink = new CCSprite("immortalitydrink.png");
            this.AddChild(immortalitydrink);
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
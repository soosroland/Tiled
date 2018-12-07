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
    public class Bullet : Entities
    {
        public CCSprite sprite;

        public float VelocityX
        {
            get;
            set;
        }

        public float VelocityY
        {
            get;
            set;
        }

        public Bullet() : base()
        {
            sprite = new CCSprite("bullet.png");
            // Making the Sprite be centered makes
            // positioning easier.
            sprite.AnchorPoint = CCPoint.AnchorMiddle;
            this.AddChild(sprite);

            this.ContentSize = sprite.ContentSize;

            this.Schedule(ApplyVelocity, 0.041f);
        }

        void ApplyVelocity(float time)
        {
            //if (!((PositionX > 394) || (PositionY > 250) || (PositionX < -10) || (PositionY < -10)))
            //{
                PositionX += VelocityX * time;
                PositionY += VelocityY * time;
            //}
                
        }

        public override void MoveX(int x)
        {
            PositionX += x;
        }

        public override void MoveY(int y)
        {
            PositionY += y;
        }
    }
}
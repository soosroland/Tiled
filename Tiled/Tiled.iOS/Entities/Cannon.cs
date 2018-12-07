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
    public class Cannon : Entities
    {
        CCSprite sprite;
        int _x=0;
        int _y=0;
        public int _rotation { get; set; }

        public int row { get; set; }
        public int column { get; set; }

        public Cannon(int x, int y, String dir, String speed) : base()
        {
            float intervall = 4.0f;
            switch (speed)
            {
                case "slow":
                    intervall = 4.0f;
                    break;
                case "normal":
                    intervall = 3.0f;
                    break;
                case "fast":
                    intervall = 2.0f;
                    break;
                default:
                    break;
            }
            sprite = new CCSprite("cannon.png");
            sprite.AnchorPoint = CCPoint.AnchorMiddle;
            switch (dir)
            {
                case "up":
                    _rotation = 0;
                    break;
                case "right":
                    _rotation = 90;
                    break;
                case "down":
                    _rotation = 180;
                    break;
                case "left":
                    _rotation = 270;
                    break;
                default:
                    _rotation = 0;
                    break;
            }
            sprite.Rotation = _rotation;
            this.AddChild(sprite);
            _x = x;
            _y = y;

            Schedule(FireBullet, interval: intervall);

        }

        public override void MoveX(int x)
        {
            this.PositionX += x;
        }

        public override void MoveY(int y)
        {
            this.PositionY += y;
        }

        void FireBullet(float unusedValue)
        {
            Bullet newBullet = BulletFactory.Self.CreateNew();
            newBullet.Rotation = _rotation;
            newBullet.Position = this.Position;
            switch (_rotation)
            {
                case 0:
                    newBullet.PositionY += 8;
                    break;
                case 90:
                    newBullet.PositionX += 8;
                    break;
                case 180:
                    newBullet.PositionY -= 8;
                    break;
                case 270:
                    newBullet.PositionX -= 8;
                    break;
                default:
                    break;
            }
            newBullet.VelocityX = _x;
            newBullet.VelocityY = _y;
        }
    }
}
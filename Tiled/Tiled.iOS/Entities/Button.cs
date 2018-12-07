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
    public class Button : Entities
    {
        public CCSprite sprite;
        CCDrawNode buttonBox = new CCDrawNode();     //Declare  globally

        //CCEventListenerTouchAllAtOnce touchListener;

        public Button(String s): base()
        {
            sprite = new CCSprite(s + ".png");
            sprite.AnchorPoint = CCPoint.AnchorMiddle;
            this.AddChild(sprite);
            this.ContentSize = sprite.ContentSize;
        }
        public Button(int x, int y, String s) : base()
        {
            sprite = new CCSprite(s+".png");
            sprite.PositionX = x;
            sprite.PositionY = y;
            // Center the Sprite in this entity to simplify
            // centering the Ship on screen
            sprite.AnchorPoint = CCPoint.AnchorMiddle;

            //buttonBox.DrawRect(sprite.BoundingBox, CCColor4B.Transparent, 2f, CCColor4B.Red);
            this.AddChild(sprite);
            this.ContentSize = sprite.ContentSize;
            //this.AddChild(buttonBox);

            /*touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesMoved = HandleInput;
            AddEventListener(touchListener, this);*/

        }

        public override void MoveX(int x)
        {
            //throw new NotImplementedException();
        }

        public override void MoveY(int y)
        {
            //throw new NotImplementedException();
        }

        /*private void HandleInput(System.Collections.Generic.List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                CCTouch firstTouch = touches[0];

                this.PositionX = firstTouch.Location.X;
                this.PositionY = firstTouch.Location.Y;
            }
        }*/
    }
}
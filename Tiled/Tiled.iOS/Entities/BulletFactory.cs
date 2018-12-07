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

namespace Tiled.Droid.Entities
{
    public class BulletFactory
    {
        static Lazy<BulletFactory> self =
            new Lazy<BulletFactory>(() => new BulletFactory());

        // simple singleton implementation
        public static BulletFactory Self
        {
            get
            {
                return self.Value;
            }
        }

        public event Action<Bullet> BulletCreated;

        private BulletFactory()
        {

        }

        public Bullet CreateNew()
        {
            Bullet newBullet = new Bullet();

            if (BulletCreated != null)
            {
                BulletCreated(newBullet);
            }

            return newBullet;
        }
    }
}
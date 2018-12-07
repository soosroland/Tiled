﻿using System;
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
    public abstract class Entities : CCNode
    {
        public abstract void MoveX(int x);

        public abstract void MoveY(int y);
    }
}
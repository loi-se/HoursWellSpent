using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;

namespace HoursWellSpent
{
    static class ModEnumConst
    {
        public const double POSITIVE_INFINITY = 100000.0;
        public const int SCREEN_WIDTH = 1200;
        public const int SCREEN_HEIGHT = 800;
        public const int PLAY_HEIGHT = 400;
        public const int SPEED = 10;
        public static APEngine APEngine1;
        public static int Totalscore = 0;
        public static Graphics g;
        public static ArrayList paintQueue;

        public static int nofcollosions = 0;

        public static Stack _Abstractparticles = new Stack();
        public static int _Xclicked;

        public static int _Yclicked;

    }

}

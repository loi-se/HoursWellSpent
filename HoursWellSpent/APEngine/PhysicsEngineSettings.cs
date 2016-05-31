using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace HoursWellSpent
{
    public static class PhysicsEngineSettings
    {
        //2D Physics engine word cloud:
        public static String objtype = "circle";
        public static double Elasticity = 0.2;
        public static double Friction = 0;

        public static String Colortype = "random";
        public static Color objectcolor = Color.Orange;

        public static String fontname = "Arial";
        public static int fontsize = 9;

        // word cloud:
        public static String Wordcloudfontname = "Showcard Gothic";
        public static int Wordcloudfontsize = 7;

        public static int MaxWordsize = 45;
        public static int MinWordsize = 6;

        public static String WordcloudColortype = "random";
        public static Color Wordcloudobjectcolor = Color.Orange;

    }
}

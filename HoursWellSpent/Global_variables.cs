using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Calendar;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using System.Globalization;

namespace HoursWellSpent
{
    class Global_variables
    {


            public static CalendarItem _GActiveCalendarItem;

            public static Double _Gprice = 0;

            public static Double _Gvariouscosts = 0;

            public static Double _GKil = 0;

            public static Double _GKilprice = 0;

            public static string _Gclient;

            public static string _GItemtext;

            public static string _GItemcolor;

            public static string _GProject;

            public static string _GActivity;

            public static string _Gnote;

            public static List<CalendarItem> _Gitems;

    }
}

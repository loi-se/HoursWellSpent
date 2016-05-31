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

namespace HoursWellSpent
{
    public class Interval
    {
        public double min;
        public double max;
        public Interval(double min, double max)
        {
            this.min = min;
            this.max = max;
        }
        public String toString()
        {
            return (min + " : " + max);
        }
    }
}

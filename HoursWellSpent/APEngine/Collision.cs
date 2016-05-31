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
    public class Collision
    {
        public Vector vn;
        public Vector vt;
        public Collision(Vector vn, Vector vt)
        {
            this.vn = vn;
            this.vt = vt;
        }
    }
}

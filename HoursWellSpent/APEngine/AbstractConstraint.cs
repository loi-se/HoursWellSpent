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
    public abstract class AbstractConstraint
    {
        protected Graphics dc;
        bool _visible;
        double _stiffness;

        public AbstractConstraint(double stiffness)
        {
            _visible = true;
            _stiffness = stiffness;
        }
        public double getStiffness()
        {
            return _stiffness;
        }
        public void setStiffness(double s)
        {
            _stiffness = s;
        }
        public bool getVisible()
        {
            return _visible;
        }
        public void setVisible(bool v)
        {
            _visible = v;
        }

        public virtual void Resolve()
        {
        }

        protected Graphics getDefaultContainer()
        {
            if (APEngine.getDefaultContainer() == null)
            {
                String err = "";
                err += "You must set the defaultContainer property of the APEngine class ";
                err += "if you wish to use the default paint methods of the constraints";
                throw new Error1();
            }
            Graphics parentContainer = APEngine.getDefaultContainer();
            return parentContainer;
        }

    }
}

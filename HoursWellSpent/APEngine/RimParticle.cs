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
    public class RimParticle
    {
        public Vector curr;
        public Vector prev;
        private double wr;
        private double av;
        private double sp;
        private double maxTorque;
        public RimParticle(double r, double mt)
        {
            curr = new Vector(r, 0);
            prev = new Vector(0, 0);
            sp = 0;
            av = 0;
            maxTorque = mt;
            wr = r;
        }
        public double getSpeed()
        {
            return sp;
        }
        public void setSpeed(double s)
        {
            sp = s;
        }
        public double getAngularVelocity()
        {
            return av;
        }
        public void setAngularVelocity(double s)
        {
            av = s;
        }
        public void update(double dt)
        {
            sp = Math.Max(-maxTorque, Math.Min(maxTorque, sp + av));
            double dx = -curr.y;
            double dy = curr.x;
            double len = Math.Sqrt(dx * dx + dy * dy);
            dx = dx / len;
            dy = dy / len;
            curr.x = curr.x + sp * dx;
            curr.y = curr.y + sp * dy;
            double ox = prev.x;
            double oy = prev.y;
            prev.x = curr.x;
            double px = prev.x;
            //= curr.x
            prev.y = curr.y;
            double py = prev.y;
            //= curr.y
            curr.x = curr.x + APEngine.getDamping() * (px - ox);
            curr.y = curr.y + APEngine.getDamping() * (py - oy);
            double clen = Math.Sqrt(curr.x * curr.x + curr.y * curr.y);
            double diff = (clen - wr) / clen;
            curr.x = curr.x - curr.x * diff;
            curr.y = curr.y - curr.y * diff;
        }
    }

}

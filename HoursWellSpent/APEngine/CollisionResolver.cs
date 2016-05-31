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
    public class CollisionResolver
    {

        public static void resolveParticleParticle(AbstractParticle pa, AbstractParticle pb, Vector normal, double depth)
        {
            Vector mtd = normal.mult(depth);
            double te = pa.getElasticity() + pb.getElasticity();
            double tf = 1 - (pa.getFriction() + pb.getFriction());
            if (tf > 1)
            {
                tf = 1;
            }
            else if (tf < 0)
            {
                tf = 0;
            }
            double ma = (pa.getFixed()) ? 100000 : pa.getMass();
            double mb = (pb.getFixed()) ? 100000 : pb.getMass();
            double tm = ma + mb;
            Collision ca = pa.getComponents(normal);
            Collision cb = pb.getComponents(normal);
            if (ca.vn.x > 5)
            {
                int i = 9;
            }
            Vector vnA = (cb.vn.mult((te + 1) * mb).plus(ca.vn.mult(ma - te * mb))).divEquals(tm);
            Vector vnB = (ca.vn.mult((te + 1) * ma).plus(cb.vn.mult(mb - te * ma))).divEquals(tm);
            ca.vt.multEquals(tf);
            cb.vt.multEquals(tf);
            Vector mtdA = mtd.mult(mb / tm);
            Vector mtdB = mtd.mult(-ma / tm);
            if (!pa.getFixed())
            {
                pa.resolveCollision(mtdA, vnA.plusEquals(ca.vt), normal, depth, -1);
                //My.Computer.Audio.Play(System.AppDomain.CurrentDomain.BaseDirectory & "\fall.wav")
                if (vnA.x > 5)
                {
                    int i = 9;
                }
            }
            if (!pb.getFixed())
            {
                pb.resolveCollision(mtdB, vnB.plusEquals(cb.vt), normal, depth, 1);
            }
        }
    }
}

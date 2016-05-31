using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace HoursWellSpent
{
    public class SpringConstraintParticle : RectangleParticle
    {
        private AbstractParticle p1;
        private AbstractParticle p2;
        private Vector avgVelocity;
        public SpringConstraintParticle(AbstractParticle p1, AbstractParticle p2)
            : base(0, 0, 0, 0, 0, false, 1, 0.3, 0, Color.Black,
                "")
        {
            this.p1 = p1;
            this.p2 = p2;
            avgVelocity = new Vector(0, 0);
        }
        public override double getMass()
        {
            return (p1.getMass() + p2.getMass()) / 2;
        }
        public override Vector getVelocity()
        {
            Vector p1v = p1.getVelocity();
            Vector p2v = p2.getVelocity();
            avgVelocity.setTo(((p1v.x + p2v.x) / 2), ((p1v.y + p2v.y) / 2));
            return avgVelocity;
        }
        public override void paint()
        {
            if ((_cornerPositions != null))
            {
                updateCornerPositions();
            }
            base.paint();
        }
        public override void resolveCollision(Vector mtd, Vector vel, Vector n, double d, double o)
        {
            if (!p1.getFixed())
            {
                p1.curr.plusEquals(mtd);
                p1.setVelocity(vel);
            }
            if (!p2.getFixed())
            {
                p2.curr.plusEquals(mtd);
                p2.setVelocity(vel);
            }
        }
    }
}

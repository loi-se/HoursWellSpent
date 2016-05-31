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
using System.Linq;

namespace HoursWellSpent
{
    public class SpringConstraint : AbstractConstraint
    {
        private AbstractParticle p1;
        private AbstractParticle p2;
        private double restLen;
        private Vector delta;
        private double deltaLength;
        private double _collisionRectWidth;
        private double _collisionRectScale;
        private bool _collidable;
        public SpringConstraintParticle collisionRect;
        public SpringConstraint(AbstractParticle p1, AbstractParticle p2, double stiffness)
            : base(stiffness)
        {
            this.p1 = p1;
            this.p2 = p2;
            checkParticlesLocation();
            _collisionRectWidth = 1;
            _collisionRectScale = 1;
            _collidable = false;
            delta = p1.curr.minus(p2.curr);
            deltaLength = p1.curr.distance(p2.curr);
            restLen = deltaLength;
        }
        public double getRotation()
        {
            return Math.Atan2(delta.y, delta.x);
        }
        public Vector getCenter()
        {
            return (p1.curr.plus(p2.curr)).divEquals(2);
        }
        public double getCollisionRectScale()
        {
            return _collisionRectScale;
        }
        public void setCollisionRectScale(double scale)
        {
            _collisionRectScale = scale;
        }
        public double getCollisionRectWidth()
        {
            return _collisionRectWidth;
        }
        public void setCollisionRectWidth(double w)
        {
            _collisionRectWidth = w;
        }
        public double getRestLength()
        {
            return restLen;
        }
        public void setRestLength(double r)
        {
            restLen = r;
        }
        public bool getCollidable()
        {
            return _collidable;
        }
        public void setCollidable(bool b)
        {
            _collidable = b;
            if (_collidable)
            {
                collisionRect = new SpringConstraintParticle(p1, p2);
                orientCollisionRectangle();
            }
            else
            {
                collisionRect = null;
            }
        }
        public bool isConnectedTo(AbstractParticle p)
        {
            return (p.Equals(p1) | p.Equals(p2));
        }
        public void paint()
        {
            if (dc == null)
            {
                dc = getDefaultContainer();
            }
            if (_collidable)
            {
                collisionRect.paint();
            }
            else
            {
                if (!getVisible())
                {
                    return;
                    return;
                }
                double X1 = p1.curr.x;
                double Y1 = p1.curr.y;
                double X2 = p2.curr.x;
                double Y2 = p2.curr.y;
                //Dim line As Line2D = New Line2D.Double(X1, Y1, X2, Y2)
                //dc.draw(line)
                dc.DrawLine(new Pen(Color.Black), new Point(Convert.ToInt32(X1), Convert.ToInt32(Y1)), new Point(Convert.ToInt32(X2), Convert.ToInt32(Y2)));
            }
        }

        public void resolve()
        {
            if (p1.getFixed() & p2.getFixed())
            {
                return;
                return;
            }
            delta = p1.curr.minus(p2.curr);
            deltaLength = p1.curr.distance(p2.curr);
            if (_collidable)
            {
                orientCollisionRectangle();
            }
            double diff = (deltaLength - restLen) / deltaLength;
            Vector dmd = delta.mult(diff * base.getStiffness());
            double invM1 = p1.getInvMass();
            double invM2 = p2.getInvMass();
            double sumInvMass = invM1 + invM2;
            if (!p1.getFixed())
            {
                p1.curr.minusEquals(dmd.mult(invM1 / sumInvMass));
            }
            if (!p2.getFixed())
            {
                p2.curr.plusEquals(dmd.mult(invM2 / sumInvMass));
            }
        }
        public RectangleParticle getCollisionRect()
        {
            return collisionRect;
        }
        private void orientCollisionRectangle()
        {
            Vector c = getCenter();
            double rot = getRotation();
            collisionRect.curr.setTo(c.x, c.y);
            //collisionRect.getExtents

            collisionRect.getExtents()[0] = ((deltaLength / 2) * _collisionRectScale);
            collisionRect.getExtents()[1] = ((_collisionRectWidth / 2));
            collisionRect.setRotation(rot);
        }
        private void checkParticlesLocation()
        {
            if (p1.curr.x == p2.curr.x && p1.curr.y == p2.curr.y)
            {
                throw new Error1();
                //("The two particles specified for a SpringContraint can't be at the same location")
            }
        }
    }
}

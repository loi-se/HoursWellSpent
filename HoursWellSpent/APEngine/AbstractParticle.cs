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
    public abstract class AbstractParticle
    {
        public Vector curr;
        public Vector prev;
        public bool isColliding;
        public Interval interval;
        protected Graphics dc;
        public Vector forces;
        public Vector temp;
        public double _kfr;
        public double _mass;
        public double _invMass;
        public bool _fixed;
        public bool _visible;
        public double _friction;
        public bool _collidable;

        public Collision collision;

        public string idtext = null;

        public AbstractParticle(double x, double y, bool isFixed, double mass, double elasticity, double friction, string _idtext)
        {
            interval = new Interval(0, 0);
            curr = new Vector(x, y);
            prev = new Vector(x, y);
            temp = new Vector(0, 0);
            idtext = _idtext;

            setFixed(isFixed);
            forces = new Vector(0, 0);
            collision = new Collision(new Vector(0, 0), new Vector(0, 0));
            isColliding = false;
            setMass(mass);
            setElasticity(elasticity);
            setFriction(friction);
            setCollidable(true);
            setVisible(true);
        }
        public virtual double getMass()
        {
            return _mass;
        }
        public virtual void setMass(double m)
        {
            if (m <= 0)
            {
                throw new Error1();
                //]("mass may not be set <= 0")
            }
            _mass = m;
            _invMass = 1 / _mass;
        }
        public double getElasticity()
        {
            return _kfr;
        }
        public void setElasticity(double k)
        {
            _kfr = k;
        }
        public bool getVisible()
        {
            return _visible;
        }
        public void setVisible(bool v)
        {
            _visible = v;
        }
        public double getFriction()
        {
            return _friction;
        }
        public void setFriction(double f)
        {
            if (f < 0 | f > 1)
            {
                throw new Error1();
                //]("Legal friction must be >= 0 and <=1")
            }
            _friction = f;
        }
        public bool getFixed()
        {
            return _fixed;
        }
        public void setFixed(bool f)
        {
            _fixed = f;
        }
        public Vector getPosition()
        {
            return new Vector(curr.x, curr.y);
        }
        public void setPosition(Vector p)
        {
            curr.copy(p);
            prev.copy(p);
        }
        public double getpx()
        {
            return curr.x;
        }
        public void setpx(double x)
        {
            curr.x = x;
            prev.x = x;
        }
        public double getpy()
        {
            return curr.y;
        }
        public void setpy(double y)
        {
            curr.y = y;
            prev.y = y;
        }
        public virtual Vector getVelocity()
        {

            if (Math.Abs(curr.minus(prev).x) > 5)
            {
                int i = 9;
            }
            return curr.minus(prev);
        }
        public virtual void setVelocity(Vector v)
        {
            prev = curr.minus(v);
        }
        public bool getCollidable()
        {
            return _collidable;
        }
        public void setCollidable(bool b)
        {
            _collidable = b;
        }
        public void addForce(Vector f)
        {
            forces.plusEquals(f.multEquals(_invMass));
        }
        public void addMasslessForce(Vector f)
        {
            forces.plusEquals(f);
        }
        public virtual void update(double dt2)
        {
            if (_fixed)
            {
                return;
                return;
            }
            addForce(APEngine.force);
            addMasslessForce(APEngine.masslessForce);
            temp.copy(curr);
            Vector nv = getVelocity().plus(forces.multEquals(dt2));
            curr.plusEquals(nv.multEquals(APEngine.getDamping()));
            prev.copy(temp);
            forces.setTo(0, 0);
        }
        public Collision getComponents(Vector collisionNormal)
        {
            Vector vel = getVelocity();
            if (vel.x > 5)
            {
                int i = 9;
            }
            double vdotn = collisionNormal.dot(vel);
            collision.vn = collisionNormal.mult(vdotn);
            collision.vt = vel.minus(collision.vn);


            return collision;
        }
        public virtual void resolveCollision(Vector mtd, Vector vel, Vector n, double d, double o)
        {
            curr.plusEquals(mtd);
            switch (APEngine.getCollisionResponseMode())
            {
                case APEngine.STANDARD:
                    setVelocity(vel);
                    break;
                //   Exit Select
                case APEngine.SELECTIVE:
                    if (!isColliding)
                    {
                        setVelocity(vel);
                    }
                    isColliding = true;
                    break;
                // Exit Select
                case APEngine.SIMPLE:
                    break;
                // Exit Select
            }
        }
        public double getInvMass()
        {
            return _invMass;
        }
        public Graphics getDefaultContainer()
        {
            if (APEngine.getDefaultContainer() == null)
            {
                String err = "";
                err += "You must set the defaultContainer property of the APEngine class ";
                err += "if you wish to use the default paint methods of the particles";
                throw new Error1();
                //](err)
            }
            Graphics parentContainer = APEngine.getDefaultContainer();
            return parentContainer;
        }
        public virtual Interval getProjection(Vector axis)
        {
            return null;
        }
    }
}

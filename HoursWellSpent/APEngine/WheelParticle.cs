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
    public class WheelParticle : CircleParticle
    {
        private RimParticle rp;
        private Vector tan;
        private Vector normSlip;
        private ArrayList _edgePositions = new ArrayList();
        private ArrayList _edgeParticles = new ArrayList();

        private double _traction;
        public System.Drawing.Color myWheeColor = Color.Red;

        public Brush MyWheelbrush;
        public WheelParticle(double x, double y, double radius, bool @fixed, double mass, double elasticity, double friction, double traction, string _itemtext)
            : base(x, y, radius, @fixed, mass, elasticity, friction, Color.Green, _itemtext)
        {
            tan = new Vector(0, 0);
            normSlip = new Vector(0, 0);
            rp = new RimParticle(radius, 2);
            setTraction(traction);
            _edgePositions = getEdgePositions();
            _edgeParticles = getEdgeParticles();
        }
        public double getAngularVelocity()
        {
            return rp.getAngularVelocity();
        }
        public void setAngularVelocity(double a)
        {
            rp.setAngularVelocity(a);
        }
        public double getTraction()
        {
            return 1 - _traction;
        }
        public void setTraction(double t)
        {
            _traction = 1 - t;
        }
        public ArrayList getEdgeParticles()
        {
            if (_edgePositions.Count == 0)
            {
                getEdgePositions();
            }
            if (_edgeParticles.Count == 0)
            {
                CircleParticle cp1 = new CircleParticle(0, 0, 1, false, 1, 0.3, 0, Color.Black, "edgeparticle");
                cp1.setCollidable(false);
                cp1.setVisible(false);
                APEngine.addParticle(cp1);
                CircleParticle cp2 = new CircleParticle(0, 0, 1, false, 1, 0.3, 0, Color.Black, "edgeparticle");
                cp2.setCollidable(false);
                cp2.setVisible(false);
                APEngine.addParticle(cp2);
                CircleParticle cp3 = new CircleParticle(0, 0, 1, false, 1, 0.3, 0, Color.Black, "edgeparticle");
                cp3.setCollidable(false);
                cp3.setVisible(false);
                APEngine.addParticle(cp3);
                CircleParticle cp4 = new CircleParticle(0, 0, 1, false, 1, 0.3, 0, Color.Black, "edgeparticle");
                cp4.setCollidable(false);
                cp4.setVisible(false);
                APEngine.addParticle(cp4);
                _edgeParticles.Add(cp1);
                _edgeParticles.Add(cp2);
                _edgeParticles.Add(cp3);
                _edgeParticles.Add(cp4);
                updateEdgeParticles();
            }
            return _edgeParticles;
        }
        public ArrayList getEdgePositions()
        {
            if (_edgePositions.Count == 0)
            {
                _edgePositions.Add(new Vector(0, 0));
                _edgePositions.Add(new Vector(0, 0));
                _edgePositions.Add(new Vector(0, 0));
                _edgePositions.Add(new Vector(0, 0));
                updateEdgePositions();
            }
            return _edgePositions;
        }
        public override void paint()
        {
            float px = Convert.ToInt64(curr.x);
            float py = Convert.ToInt64(curr.y);
            float rx = Convert.ToInt64(rp.curr.x);
            float ry = Convert.ToInt64(rp.curr.y);
            if (dc == null)
            {
                dc = getDefaultContainer();
            }
            if (!getVisible())
            {
                return;
            }
            //Dim f1 As New GeneralPath()
            //Dim f As path
            // f1.moveTo(px, py)
            // f1.lineTo(rx + px, ry + py)
            // 'f1.moveTo(px, py)
            //f1.lineTo(-rx + px, -ry + py)
            //f1.moveTo(px, py)
            //f1.lineTo(-ry + px, rx + py)
            //f1.moveTo(px, py)
            //f1.lineTo(ry + px, -rx + py)
            //dc.draw(f1)
            dc.DrawLine(new Pen(Color.Black), new Point(Convert.ToInt32(px), Convert.ToInt32(py)), new Point(Convert.ToInt32(rx) + Convert.ToInt32(px), Convert.ToInt32(ry) + Convert.ToInt32(py)));
            dc.DrawLine(new Pen(Color.Black), new Point(Convert.ToInt32(px), Convert.ToInt32(py)), new Point(Convert.ToInt32(-rx) + Convert.ToInt32(px), Convert.ToInt32(-ry) + Convert.ToInt32(py)));
            dc.DrawLine(new Pen(Color.Black), new Point(Convert.ToInt32(px), Convert.ToInt32(py)), new Point(Convert.ToInt32(-ry) + Convert.ToInt32(px), Convert.ToInt32(rx) + Convert.ToInt32(py)));
            dc.DrawLine(new Pen(Color.Black), new Point(Convert.ToInt32(px), Convert.ToInt32(py)), new Point(Convert.ToInt32(ry) + Convert.ToInt32(px), Convert.ToInt32(-rx) + Convert.ToInt32(py)));
            //Dim circle As New Ellipse2D.Double(curr.x - getRadius(), curr.y - getRadius(), DirectCast(getRadius(), Double) * 2, DirectCast(getRadius(), Double) * 2)
            //dc.draw(circle)

            //Mybrush = New SolidBrush(myColor)
            //dc.FillEllipse(Mybrush, CSng(curr.x - getRadius()), CSng(curr.y - getRadius()), CSng(getRadius()) * 2, CSng(getRadius()) * 2)

            dc.DrawEllipse(new Pen(Color.Black), Convert.ToSingle(curr.x - getRadius()), Convert.ToSingle(curr.y - getRadius()), Convert.ToSingle(getRadius()) * 2, Convert.ToSingle(getRadius()) * 2);


        }
        public override void update(double dt)
        {
            base.update(dt);
            rp.update(dt);
            if ((_edgePositions != null))
            {
                updateEdgePositions();
            }
            if ((_edgeParticles != null))
            {
                updateEdgeParticles();
            }
        }
        public override void resolveCollision(Vector mtd, Vector velocity, Vector normal, double depth, double order)
        {
            base.resolveCollision(mtd, velocity, normal, depth, order);
            resolve(normal.mult(sign(depth * order)));
        }
        private void resolve(Vector n)
        {
            tan.setTo(-rp.curr.y, rp.curr.x);
            tan = tan.normalize();
            //velocity of the wheel's surface 
            Vector wheelSurfaceVelocity = tan.mult(rp.getSpeed());
            // the velocity of the wheel's surface relative to the ground
            Vector combinedVelocity = getVelocity().plusEquals(wheelSurfaceVelocity);
            //the wheel's comb velocity projected onto the contact normal
            double cp = combinedVelocity.cross(n);
            //set the wheel's spinspeed to track the ground
            tan.multEquals(cp);
            rp.prev.copy(rp.curr.minus(tan));
            // some of the wheel's torque is removed and converted into linear displacement
            double slipSpeed = (1 - _traction) * rp.getSpeed();
            normSlip.setTo(slipSpeed * n.y, slipSpeed * n.x);
            curr.plusEquals(normSlip);
            rp.setSpeed(rp.getSpeed() * _traction);
        }
        private void updateEdgePositions()
        {
            double px = curr.x;
            double py = curr.y;
            double rx = rp.curr.x;
            double ry = rp.curr.y;
            ((Vector)_edgePositions[0]).setTo(rx + px, ry + py);
            ((Vector)_edgePositions[1]).setTo(-ry + px, rx + py);
            ((Vector)_edgePositions[2]).setTo(-rx + px, -ry + py);
            ((Vector)_edgePositions[3]).setTo(ry + px, -rx + py);
        }
        private void updateEdgeParticles()
        {
            int i = 0;
            for (i = 0; i <= 3; i++)
            {


                AbstractParticle _edgeparticle = (AbstractParticle)_edgeParticles[i];

                _edgeparticle.setpx(((Vector)_edgePositions[i]).x);
                _edgeparticle.setpy(((Vector)_edgePositions[i]).y);
                // System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1)
            }
        }
        private int sign(double val)
        {
            if (val < 0)
            {
                return -1;
            }
            return 1;
        }
    }
}

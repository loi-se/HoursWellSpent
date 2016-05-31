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
    public class CircleParticle : AbstractParticle
    {

        private double _radius;
        public System.Drawing.Color myColor;

        public Brush Mybrush;

        public CircleParticle(double x, double y, double radius, bool @fixed, double mass, double elasticity, double friction, Color CircleColor, string _idtext)
            : base(x, y, @fixed, mass, elasticity, friction, _idtext)
        {
            myColor = CircleColor;
            _radius = radius;
        }
        public double getRadius()
        {
            return _radius;
        }
        public void setRadius(double r)
        {
            _radius = r;
        }
        public virtual void paint()
        {
            if (dc == null)
            {
                dc = getDefaultContainer();
            }

            if (!getVisible())
            {
                return;
                return;
            }

            //Dim circle As New Ellipse2D.Double(curr.x - getRadius(), curr.y - getRadius(), DirectCast(getRadius(), Double) * 2, DirectCast(getRadius(), Double) * 2)
            //dc.DrawEllipse(New Pen(Color.Red), CSng(curr.x - getRadius()), CSng(curr.y - getRadius()), CSng(getRadius()) * 2, CSng(getRadius()) * 2)
            Mybrush = new SolidBrush(myColor);
            dc.FillEllipse(Mybrush, Convert.ToSingle(curr.x - getRadius()), Convert.ToSingle(curr.y - getRadius()), Convert.ToSingle(getRadius()) * 2, Convert.ToSingle(getRadius()) * 2);

            //dc.DrawEllipse
            //Dim mybitMap As New Bitmap(fileName)
            // Create string to draw.

            string drawString = Convert.ToString(this.idtext);
            // Create font and brush.
            Font drawFont = new Font(PhysicsEngineSettings.fontname, PhysicsEngineSettings.fontsize);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            // Create point for upper-left corner of drawing.

            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;

            Point drawPoint = new Point(Convert.ToInt32(curr.x), Convert.ToInt32(curr.y));
            
            // Draw string to screen.
            dc.DrawString(drawString, drawFont, drawBrush, drawPoint, sf);
            //Me.PictureBox1.Image = mybitMap




            // dc.draw(circle)

        }
        public override Interval getProjection(Vector axis)
        {
            double c = curr.dot(axis);
            interval.min = c - _radius;
            interval.max = c + _radius;
            return interval;
        }
        public Interval getIntervalX()
        {
            interval.min = curr.x - _radius;
            interval.max = curr.x + _radius;
            return interval;
        }
        public Interval getIntervalY()
        {
            interval.min = curr.y - _radius;
            interval.max = curr.y + _radius;
            return interval;
        }
    }
}

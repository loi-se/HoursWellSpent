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
using System.Windows.Forms;

namespace HoursWellSpent
{
    public class RectangleParticle : AbstractParticle
    {
        public ArrayList _cornerPositions = new ArrayList();
        private ArrayList _cornerParticles = new ArrayList();
        private ArrayList _extents = new ArrayList();
        private ArrayList _axes = new ArrayList();

        private double _rotation;
        public System.Drawing.Color myColor;
        public Brush Mybrush;

        public Pen MyPen;

        public RectangleParticle(double x, double y, double width, double height, double rotation, bool @fixed, double mass, double elasticity, double friction, Color RectangleColor,
        string _idtext)
            : base(x, y, @fixed, mass, elasticity, friction, _idtext)
        {
            _extents.Add(width / 2);
            _extents.Add(height / 2);
            _axes.Add(new Vector(0, 0));
            _axes.Add(new Vector(0, 0));
            setRotation(rotation);
            _cornerPositions = getCornerPositions();
            _cornerParticles = getCornerParticles();
            myColor = RectangleColor;

        }
        public double getRotation()
        {
            return _rotation;
        }
        public void setRotation(double t)
        {
            _rotation = t;
            setAxes(t);
        }
        public ArrayList getCornerParticles()
        {
            if (_cornerPositions.Count == 0)
            {
                getCornerPositions();
            }
            if (_cornerParticles.Count == 0)
            {
                CircleParticle cp1 = new CircleParticle(0, 0, 1, false, 1, 0.3, 0, Color.Black, "cornerparticle");
                cp1.setCollidable(false);
                cp1.setVisible(false);
                APEngine.addParticle(cp1);
                CircleParticle cp2 = new CircleParticle(0, 0, 1, false, 1, 0.3, 0, Color.Black, "cornerparticle");
                cp2.setCollidable(false);
                cp2.setVisible(false);
                APEngine.addParticle(cp2);
                CircleParticle cp3 = new CircleParticle(0, 0, 1, false, 1, 0.3, 0, Color.Black, "cornerparticle");
                cp3.setCollidable(false);
                cp3.setVisible(false);
                APEngine.addParticle(cp3);
                CircleParticle cp4 = new CircleParticle(0, 0, 1, false, 1, 0.3, 0, Color.Black, "cornerparticle");
                cp4.setCollidable(false);
                cp4.setVisible(false);
                APEngine.addParticle(cp4);
                _cornerParticles.Add(cp1);
                _cornerParticles.Add(cp2);
                _cornerParticles.Add(cp3);
                _cornerParticles.Add(cp4);
                updateCornerParticles();
            }
            return _cornerParticles;
        }
        public ArrayList getCornerPositions()
        {
            if (_cornerPositions.Count == 0)
            {
                _cornerPositions.Add(new Vector(0, 0));
                _cornerPositions.Add(new Vector(0, 0));
                _cornerPositions.Add(new Vector(0, 0));
                _cornerPositions.Add(new Vector(0, 0));
                updateCornerPositions();
            }
            return _cornerPositions;
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
            int j = 0;
            for (j = 0; j <= 3; j++)
            {
                int i = j;
                double X1 = ((Vector)_cornerPositions[i]).x;
                double Y1 = ((Vector)_cornerPositions[i]).y;
                if (j == 3)
                    i = -1;

                float X2 = Convert.ToInt64(((Vector)_cornerPositions[i + 1]).x);
                float Y2 = Convert.ToInt64(((Vector)_cornerPositions[i + 1]).y);
                // Dim line As Line2D = New Line2D.Double(X1, Y1, X2, Y2)
                //dc.draw(line)

                Mybrush = new SolidBrush(myColor);
                MyPen = new Pen(Mybrush);
                MyPen.Width = 3;


                dc.DrawLine(MyPen, new PointF(Convert.ToInt64(X1), Convert.ToInt64(Y1)), new PointF(X2, Y2));

                string drawString = Convert.ToString(this.idtext);
                // Create font and brush.
                Font drawFont = new Font(PhysicsEngineSettings.fontname, PhysicsEngineSettings.fontsize);
                SolidBrush drawBrush = new SolidBrush(Color.Black);
                // Create point for upper-left corner of drawing.

                StringFormat sf = new StringFormat();
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;

                //int rol = curr.x;

                Point drawPoint = new Point(Convert.ToInt32(curr.x), Convert.ToInt32(curr.y));


                // Draw string to screen.
                dc.DrawString(drawString, drawFont, drawBrush, drawPoint, sf);

                //PictureBox picturebox = new PictureBox();

                //int XX1 = Convert.ToInt32(X1);
                //int YY1 = Convert.ToInt32(Y1);

                //int YY2 = Convert.ToInt32(Y2);
                //int XX2 = Convert.ToInt32(X2);

                //dc.FillRectangle(Mybrush, XX1, YY1, YY2, XX2);

                //AddHandler dc.MouseDoubleClick, AddressOf frmDemo.dc_doubleclick
                //Dim XX1 As Integer = CInt(X1)
                //Dim YY1 As Integer = CInt(Y1)

                //Dim XX2 As Integer = CInt(X2)
                //Dim YY2 As Integer = CInt(Y2)

                //dc.FillRectangle(Mybrush, XX1, YY1, XX2, YY2)


                //  System.Math.Max(System.Threading.Interlocked.Increment(j), j - 1)
                // Debug.Print(X1, Y1, X2, Y2)
            }
        }
        public override void update(double dt2)
        {
            base.update(dt2);
            if (_cornerPositions.Count != 0)
            {
                updateCornerPositions();
            }
            if (_cornerParticles.Count != 0)
            {
                updateCornerParticles();
            }
        }
        public ArrayList getAxes()
        {
            return _axes;
        }
        public ArrayList getExtents()
        {
            return _extents;
        }
        public override Interval getProjection(Vector axis)
        {
            double radius = 0;
            radius = ((double)_extents[0]) * Math.Abs(axis.dot((Vector)_axes[0])) + ((double)_extents[1]) * Math.Abs(axis.dot((Vector)_axes[1]));
            double c = curr.dot(axis);
            interval.min = c - radius;
            interval.max = c + radius;
            return interval;
        }
        public void updateCornerPositions()
        {
            double ae0_x = ((Vector)_axes[0]).x * ((double)_extents[0]);
            double ae0_y = ((Vector)_axes[0]).y * ((double)_extents[0]);
            double ae1_x = ((Vector)_axes[1]).x * ((double)_extents[1]);
            double ae1_y = ((Vector)_axes[1]).y * ((double)_extents[1]);
            double emx = ae0_x - ae1_x;
            double emy = ae0_y - ae1_y;
            double epx = ae0_x + ae1_x;
            double epy = ae0_y + ae1_y;
            Vector cornerPosition1 = new Vector(0, 0);
            Vector cornerPosition2 = new Vector(0, 0);
            Vector cornerPosition3 = new Vector(0, 0);
            Vector cornerPosition4 = new Vector(0, 0);
            cornerPosition1.x = curr.x - epx;
            cornerPosition1.y = curr.y - epy;
            _cornerPositions[0] = cornerPosition1;
            cornerPosition2.x = curr.x + emx;
            cornerPosition2.y = curr.y + emy;
            _cornerPositions[1] = cornerPosition2;
            cornerPosition3.x = curr.x + epx;
            cornerPosition3.y = curr.y + epy;
            _cornerPositions[2] = cornerPosition3;
            cornerPosition4.x = curr.x - emx;
            cornerPosition4.y = curr.y - emy;
            _cornerPositions[3] = cornerPosition4;
        }
        private void updateCornerParticles()
        {
            int i = 0;
            for (i = 0; i <= 3; i++)
            {
                ((AbstractParticle)getCornerParticles()[i]).setpx(((Vector)_cornerPositions[i]).x);
                ((AbstractParticle)getCornerParticles()[i]).setpy(((Vector)_cornerPositions[i]).y);
                //System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1)
            }
        }
        private void setAxes(double t)
        {
            double s = Math.Sin(t);
            double c = Math.Cos(t);
            ((Vector)_axes[0]).x = c;
            ((Vector)_axes[0]).y = s;
            ((Vector)_axes[1]).x = -s;
            ((Vector)_axes[1]).y = c;
        }
    }
}

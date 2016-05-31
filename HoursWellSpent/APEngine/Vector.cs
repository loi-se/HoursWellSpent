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
    public class Vector
    {
        public double x;
        public double y;

        public Vector(double px, double py)
        {
            x = px;
            y = py;
        }
        public void setTo(double px, double py)
        {
            x = px;
            y = py;
        }
        public void copy(Vector v)
        {
            x = v.x;
            y = v.y;
        }
        public double dot(Vector v)
        {
            return x * v.x + y * v.y;
        }
        public double cross(Vector v)
        {
            return x * v.y - y * v.x;
        }
        public Vector plus(Vector v)
        {
            return new Vector(x + v.x, y + v.y);
        }
        public Vector plusEquals(Vector v)
        {
            x += v.x;
            y += v.y;
            return this;
        }
        public Vector minus(Vector v)
        {
            return new Vector(x - v.x, y - v.y);
        }
        public Vector minusEquals(Vector v)
        {
            x -= v.x;
            y -= v.y;
            return this;
        }
        public Vector mult(double s)
        {
            return new Vector(x * s, y * s);
        }
        public Vector multEquals(double s)
        {
            x *= s;
            y *= s;
            return this;
        }
        public Vector times(Vector v)
        {
            return new Vector(x * v.x, y * v.y);
        }
        public Vector divEquals(double s)
        {
            if (s == 0)
            {
                s = 0.0001;
            }
            x /= s;
            y /= s;
            return this;
        }
        public double magnitude()
        {
            return Math.Sqrt(x * x + y * y);
        }
        public double distance(Vector v)
        {
            Vector delta = this.minus(v);
            return delta.magnitude();
        }
        public Vector normalize()
        {
            double m = magnitude();
            if (m == 0)
            {
                m = 0.0001;
            }
            return mult(1 / m);
        }

        public string toString()
        {
            return (x + " : " + y);
        }
    }
}

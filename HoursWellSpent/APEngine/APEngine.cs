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
using System.Drawing.Drawing2D;
using System.Linq;


namespace HoursWellSpent
{
    public class APEngine
    {
        public const int STANDARD = 100;
        public const int SELECTIVE = 200;

        public const int SIMPLE = 300;
        public static Vector force;

        public static Vector masslessForce;
        //Public Shared force As New Vector(0, 0)
        //Public Shared masslessForce As New Vector(0, 0)



        private static double timeStep;
        public static ArrayList particles = new ArrayList();

        public static ArrayList constraints = new ArrayList();
        private static double _damping;
        private static Graphics _defaultContainer;
        private static int _collisionResponseMode = STANDARD;

        public static Bitmap bmp = new Bitmap(600, 300);

        public static void init(double dt, int width = 700, int height = 350)
        {
            timeStep = 0;
            timeStep = dt * dt;
            _damping = 1;
            bmp = new Bitmap(width, height);

            //bmp = New Bitmap(My.Resources.earth)
            _defaultContainer = Graphics.FromImage(bmp);
            _defaultContainer.SmoothingMode = SmoothingMode.AntiAlias;

            force = new Vector(0, 0);
            masslessForce = new Vector(0, 0);
        }
        public static double getDamping()
        {
            return _damping;
        }
        public static void setDamping(double d)
        {
            _damping = d;
        }
        public static Graphics getDefaultContainer()
        {
            return _defaultContainer;
        }
        public static void setDefaultContainer(Graphics s)
        {
            _defaultContainer = s;
        }
        public static int getCollisionResponseMode()
        {
            return _collisionResponseMode;
        }
        public static void setCollisionResponseMode(int m)
        {
            _collisionResponseMode = m;
        }
        public static void addForce(Vector v)
        {
            force.plusEquals(v);
        }
        public static void addMasslessForce(Vector v)
        {
            masslessForce.plusEquals(v);
        }
        public static void addParticle(AbstractParticle p)
        {
            particles.Add(p);
        }
        public static void removeParticle(AbstractParticle p)
        {
            int ppos = particles.IndexOf(p);
            if (ppos == -1)
            {
                return;
            }
            //particles.Remove(ppos)
            particles.RemoveAt(ppos);
        }

        public static void addConstraint(AbstractConstraint c)
        {
            constraints.Add(c);
        }

        public static void removeConstraint(AbstractConstraint c)
        {
            int cpos = constraints.IndexOf(c);
            if (cpos == -1)
            {
                return;
            }
            // constraints.Remove(cpos)
            constraints.RemoveAt(cpos);

        }
        public static ArrayList getAll()
        {
            ArrayList a = (ArrayList)particles.Clone();
            a.AddRange(constraints);
            return a;
        }
        public static ArrayList getAllParticles()
        {
            return particles;
        }
        public static ArrayList getCustomParticles()
        {
            ArrayList customParticles = new ArrayList();
            int i = 0;
            for (i = 0; i <= particles.Count - 1; i++)
            {
                AbstractParticle p = (AbstractParticle)particles[i]; 
                if (isCustomParticle(p))
                {
                    customParticles.Add(p);
                }
                // System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1)
            }
            return customParticles;
        }
        public static ArrayList getAPEParticles()
        {
            ArrayList apeParticles = new ArrayList();
            int i = 0;
            for (i = 0; i <= particles.Count - 1; i++)
            {
                AbstractParticle p = (AbstractParticle)particles[i];
                if (!isCustomParticle(p))
                {
                    apeParticles.Add(p);
                }
                //System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1)
            }
            return apeParticles;
        }
        public static ArrayList getAllConstraints()
        {
            return constraints;
        }
        //[step]()
        public static void StepUp()
        {
            _defaultContainer.Clear(Color.White);
            //_defaultContainer.
            integrate();
            satisfyConstraints();
            checkCollisions();
            //frmDemo.BackgroundImage = My.Resources.earth
        }
        private static bool isCustomParticle(AbstractParticle p)
        {
            bool isWP = false;
            bool isCP = false;
            bool isRP = false;
            string className = p.ToString();
            //if (p is WheelParticle)
            //{
            //    isWP = true;
            //}
            if (p is CircleParticle)
            {
                isCP = true;
            }
            if (p is RectangleParticle)
            {
                isRP = true;
            }
            if (!(isWP || isCP || isRP))
            {
                return true;
            }
            return false;
        }
        private static void integrate()
        {
            int i = 0;

            for (i = 0; i <= particles.Count - 1; i++)
            {
                if (particles[i] is RectangleParticle)
                {
                    //DirectCast(particles.Item(i), RectangleParticle).update(timeStep)
                    RectangleParticle objRect = (RectangleParticle)particles[i];

                    objRect.update(timeStep);
                }
                else if (particles[i] is CircleParticle)
                {
                    //DirectCast(particles.Item(i), CircleParticle).update(timeStep)

                    CircleParticle objCir = (CircleParticle)particles[i];

                    objCir.update(timeStep);

                    //particles[i].update(timeStep);
                }
            }

        }
        private static void satisfyConstraints()
        {
            int n = 0;
            //While n < constraints.Count()
            //DirectCast(constraints.Item(n), AbstractConstraint).resolve()
            //System.Math.Max(System.Threading.Interlocked.Increment(n), n - 1)
            for (n = 0; n <= constraints.Count - 1; n++)
            {
                //CType(constraints[n], SpringConstraint).resolve();

                SpringConstraint objSpringConstrain = (SpringConstraint)constraints[n];

                objSpringConstrain.resolve();
            }

            //End While
        }

        private static void checkCollisions()
        {
            for (int j = 0; j <= particles.Count - 1; j++)
            {
                AbstractParticle pa = (AbstractParticle)particles[j];
                //DirectCast(particles.Item(j), AbstractParticle)

                int i = 0;

                for (i = j + 1; i <= particles.Count - 1; i++)
                {
                    AbstractParticle pb = (AbstractParticle)particles[i];
                    //DirectCast(particles.Item(i), AbstractParticle)
                    if ((pa.getCollidable() & pb.getCollidable()))
                    {
                        CollisionDetector.test(pa, pb);
                    }
                }

                for (int n = 0; n <= constraints.Count - 1; n++)
                {
                    if (constraints[n] is AngularConstraint)
                    {
                        //Do nothing
                    }
                    else
                    {
                        SpringConstraint c = (SpringConstraint)constraints[n];
                        if ((pa.getCollidable() & c.getCollidable() & (!c.isConnectedTo(pa))))
                        {
                            CollisionDetector.test(pa, c.getCollisionRect());
                        }
                    }
                }
                pa.isColliding = false;
            }

        }
    }
}

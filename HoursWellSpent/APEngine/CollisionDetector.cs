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
using System.Linq;




namespace HoursWellSpent
{
    
    // ERROR: Not supported in C#: OptionDeclaration
    public class CollisionDetector
    {
        public static void test(AbstractParticle objA, AbstractParticle objB)
        {
            if (objA.getFixed() && objB.getFixed())
            {
                return;
                return;
            }
            //// rectangle to rectangle
            if (objA is RectangleParticle & objB is RectangleParticle)
            {
               RectangleParticle objArect = (RectangleParticle)objA;
               RectangleParticle objBrect = (RectangleParticle)objB;

                testOBBvsOBB(objArect, objBrect);
                //// circle to circle
            }
            else if (objA is CircleParticle & objB is CircleParticle)
            {
                CircleParticle objAcir = (CircleParticle)objA;
                CircleParticle objBcir = (CircleParticle)objB;

                testCirclevsCircle(objAcir, objBcir);
                //// rectangle to circle - two ways
            }
            else if (objA is RectangleParticle & objB is CircleParticle)
            {

                RectangleParticle objArect = (RectangleParticle)objA;
                CircleParticle objBcir = (CircleParticle)objB;

                testOBBvsCircle(objArect, objBcir);
            }
            else if (objA is CircleParticle & objB is RectangleParticle)
            {

                CircleParticle objAcir = (CircleParticle)objA;
                RectangleParticle objBrect = (RectangleParticle)objB;

                testOBBvsCircle(objBrect, objAcir);
            }
            //testOBBvsOBB(DirectCast(objA, RectangleParticle), DirectCast(objB, RectangleParticle))
        }

        private static void testOBBvsOBB(RectangleParticle ra, RectangleParticle rb)
        {
            Vector collisionNormal = new Vector(0, 0);
            double collisionDepth = ModEnumConst.POSITIVE_INFINITY;
            int i = 0;
            for (i = 0; i <= 1; i++)
            {
                Vector axisA = (Vector)ra.getAxes()[i];
                double depthA = testIntervals(ra.getProjection(axisA), rb.getProjection(axisA));
                if (depthA == 0)
                {
                    return;
                    return;
                }
                Vector axisB = (Vector)rb.getAxes()[i];
                double depthB = testIntervals(ra.getProjection(axisB), rb.getProjection(axisB));
                if (depthB == 0)
                {
                    return;
                    return;
                }
                double absA = Math.Abs(depthA);
                double absB = Math.Abs(depthB);
                if (absA < Math.Abs(collisionDepth) || absB < Math.Abs(collisionDepth))
                {
                    bool altb = absA < absB;
                    collisionNormal = altb ? axisA : axisB;
                    collisionDepth = altb ? depthA : depthB;



                    //*************collision
                    ModEnumConst.nofcollosions = ModEnumConst.nofcollosions + 1;
                    //*************


                    //My.Computer.Audio.Play(System.AppDomain.CurrentDomain.BaseDirectory & "\fall.wav")
                }
            }
            //System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1)
            CollisionResolver.resolveParticleParticle(ra, rb, collisionNormal, collisionDepth);
        }
        private static void testOBBvsCircle(RectangleParticle ra, CircleParticle ca)
        {
            Vector collisionNormal = new Vector(0, 0);
            double collisionDepth = ModEnumConst.POSITIVE_INFINITY;
            ArrayList depths = new ArrayList(2);
            int i = 0;
            //While i < 2
            for (i = 0; i <= 1; i++)
            {
                Vector boxAxis = (Vector)ra.getAxes()[i];
                double depth = testIntervals(ra.getProjection(boxAxis), ca.getProjection(boxAxis));
                if (depth == 0)
                {
                    return;
                    return;
                }
                if (Math.Abs(depth) < Math.Abs(collisionDepth))
                {
                    collisionNormal = boxAxis;
                    collisionDepth = depth;
                }
                depths.Insert(i, depth);
                // System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1)
            }
            //End While
            double r = ca.getRadius();


             //SByte objSByte0 = (SByte)depths[0];
             int objSByte0 = Convert.ToInt32(depths[0]);



             //SByte objSByte1 = (SByte)depths[1];
             int objSByte1 = Convert.ToInt32(depths[1]);


             if (Math.Abs(objSByte0) < r && Math.Abs(objSByte1) < r)
            {
 
                Vector vertex = closestVertexOnOBB(ca.curr, ra);
                collisionNormal = vertex.minus(ca.curr);
                double mag = collisionNormal.magnitude();
                collisionDepth = r - mag;
                if (collisionDepth > 0)
                {
                    collisionNormal.divEquals(mag);


                    //*************collision
                    ModEnumConst.nofcollosions = ModEnumConst.nofcollosions + 1;
                    //*************


                    //If ra.myColor = ca.myColor Then

                    //    Totalscore = Totalscore + 1
                    //    'APEngine1.removeParticle(ca)
                    //    'APEngine1.removeParticle(ra)

                    //End If


                }
                else
                {
                    return;
                    return;
                }
            }
            CollisionResolver.resolveParticleParticle(ra, ca, collisionNormal, collisionDepth);
        }
        public static void testCirclevsCircle(CircleParticle ca, CircleParticle cb)
        {
            double depthX = testIntervals(ca.getIntervalX(), cb.getIntervalX());
            if (depthX == 0)
            {
                return;
                return;
            }
            double depthY = testIntervals(ca.getIntervalY(), cb.getIntervalY());
            if (depthY == 0)
            {
                return;
                return;
            }
            Vector collisionNormal = ca.curr.minus(cb.curr);
            double mag = collisionNormal.magnitude();
            double collisionDepth = (ca.getRadius() + cb.getRadius()) - mag;
            if (collisionDepth > 0)
            {
                collisionNormal.divEquals(mag);
                CollisionResolver.resolveParticleParticle(ca, cb, collisionNormal, collisionDepth);

                //*************collision
                ModEnumConst.nofcollosions = ModEnumConst.nofcollosions + 1;
                //*************


                //If cb.myColor = ca.myColor Then
                //    'My.Computer.Audio.Play(System.AppDomain.CurrentDomain.BaseDirectory & "\score.wav")
                //    Totalscore = Totalscore + 1

                //End If


            }
        }
        private static double testIntervals(Interval intervalA, Interval intervalB)
        {
            double functionReturnValue = 0;
            if (intervalA.max < intervalB.min)
            {
                return 0;
                return functionReturnValue;
            }
            if (intervalB.max < intervalA.min)
            {
                return 0;
                return functionReturnValue;
            }
            double lenA = intervalB.max - intervalA.min;
            double lenB = intervalB.min - intervalA.max;
            return (Math.Abs(lenA) < Math.Abs(lenB)) ? lenA : lenB;
            return functionReturnValue;
        }
        private static Vector closestVertexOnOBB(Vector p, RectangleParticle r)
        {
            Vector d = p.minus(r.curr);
            Vector q = new Vector(r.curr.x, r.curr.y);
            int i = 0;
            for (i = 0; i <= 1; i++)
            {
                double dist = d.dot((Vector)r.getAxes()[i]);
                if (dist >= 0)
                {
                    dist = ((double)r.getExtents()[i]);
                }
                else if (dist < 0)
                {
                    dist = -((double)r.getExtents()[i]);
                }
                q.plusEquals(((Vector)r.getAxes()[i]).mult(dist));
                //  System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1)
            }
            return q;
        }
    }
}

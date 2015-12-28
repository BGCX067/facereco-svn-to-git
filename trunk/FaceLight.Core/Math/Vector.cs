#region Header
//
//   Project:           FaceLight - Simple Silverlight Real Time Face Detection.
//
//   Changed by:        $Author$
//   Changed on:        $Date$
//   Changed in:        $Revision$
//   Project:           $URL$
//   Id:                $Id$
//
//
//   Copyright © 2010 Rene Schulte
//
//   This Software is weak copyleft open source. Please read the License.txt for details.
//
#endregion

using System.Windows;
using System;

namespace FaceLight.Core
{
   /// <summary>
   /// Integer vector.
   /// </summary>
   public struct Vector
   {
      public int X;
      public int Y;


      public static Vector Zero { get { return new Vector(0, 0); } }
      public static Vector One { get { return new Vector(1, 1); } }
      public static Vector UnitX { get { return new Vector(1, 0); } }
      public static Vector UnitY { get { return new Vector(0, 1); } }
      
      public int Length { get { return (int)System.Math.Sqrt(X * X + Y * Y); } }
      public int LengthSq { get { return X * X + Y * Y; } }


      public Vector(int x, int y)
      {
         this.X = x;
         this.Y = y;
      }

      public Vector(Point point)
         : this((int)point.X, (int) point.Y)
      {
      }


      public static Vector operator +(Vector v1, Vector v2)
      {
         return new Vector(v1.X + v2.X, v1.Y + v2.Y);
      }

      public static Vector operator -(Vector v1, Vector v2)
      {
         return new Vector(v1.X - v2.X, v1.Y - v2.Y);
      }

      public static Vector operator *(Vector p, int s)
      {
         return new Vector(p.X * s, p.Y * s);
      }

      public static Vector operator *(int s, Vector p)
      {
         return new Vector(p.X * s, p.Y * s);
      }

      public static Vector operator *(Vector p, float s)
      {
         return new Vector((int)(p.X * s), (int)(p.Y * s));
      }

      public static Vector operator *(float s, Vector p)
      {
         return new Vector((int)(p.X * s), (int)(p.Y * s));
      }

      public static bool operator ==(Vector v1, Vector v2)
      {
         return v1.X == v2.X && v1.Y == v2.Y;
      }

      public static bool operator !=(Vector v1, Vector v2)
      {
         return v1.X != v2.X || v1.Y != v2.Y;
      }


      public Vector Interpolate(Vector v2, float amount)
      {
         return new Vector((int)(this.X + ((v2.X - this.X) * amount)), (int)(this.Y + ((v2.Y - this.Y) * amount)));
      }

      public int Dot(Vector v2)
      {
         return this.X * v2.X + this.Y * v2.Y;
      }

      public int AngleDeg(Vector v2)
      {
         // Normalize this
         var len = this.Length;
         double x1 = 0, y1 = 0, x2 = 0, y2 = 0;
         if (len != 0)
         {
            double s1 = 1.0f / len;
            x1 = this.X * s1;
            y1 = this.Y * s1;
         }

         // Normalize v2
         len = v2.Length;
         if (len != 0)
         {
            double s2 = 1.0f / len;
            x2 = v2.X * s2;
            y2 = v2.Y * s2;
         }

         // Acos of the dot product would only return degrees between 0° and 180° without a sign
         var rad = Math.Atan2(y2, x2) - Math.Atan2(y1, x1);
         
         // return the angle in degrees
         return (int)(MathHelper.ToDegrees(rad));
      }


      public override bool Equals(object obj)
      {
         if (obj is Vector)
         {
            return ((Vector)obj) == this;
         }
         return false;
      }

      public override int GetHashCode()
      {
         return X.GetHashCode() ^ Y.GetHashCode();
      }

      public override string ToString()
      {
         return String.Format("({0}, {1})", X, Y);;
      }
   }
}
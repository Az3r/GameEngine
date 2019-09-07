using System;
using System.Drawing;
namespace GameEngine.Math
{
    /// <summary>
    /// Represent a 2D vector and point
    /// </summary>
    public struct Vector2 : IEquatable<Vector2>
    {
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }
        public Vector2(in Vector2 source)
        {
            X = source.X;
            Y = source.Y;
        }
        public Vector2(in Point source)
        {
            X = source.X;
            Y = source.Y;
        }
        public Vector2(in PointF source)
        {
            X = source.X;
            Y = source.Y;
        }
        // Override methods
        public override string ToString() => string.Format($"({X}, {Y})");
        public override int GetHashCode()
        {
            // 269 and 47 are primes
            int hash = 269;
            hash = (hash * 47) + X.GetHashCode();
            hash = (hash * 47) + Y.GetHashCode();
            return hash;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            Vector2 vec = (Vector2)obj;
            return Equals(vec);
        }
        public bool Equals(Vector2 vec) => X == vec.X && Y == vec.Y;
        // Conversions
        public PointF ToPointF() => new PointF(X, Y);
        public Point ToPointInt() => new Point((int)X, (int)Y);


        // Interfaces
        /// <summary>
        /// Calculate the angle between two vectors
        /// </summary>
        /// <param name="vec"></param>
        /// <returns>Angle formed by two vectors, measured in radian, in range of [0, PI]</returns>
        /// 
        // 
        public float GetAngle(in Vector2 vec) => AngleBetween(vec);
        public float Distance() => (float)System.Math.Sqrt(X * X + Y * Y);
        public Vector2 Floor() => new Vector2((float)System.Math.Floor(X), (float)System.Math.Floor(Y));
        public Vector2 Ceil() => new Vector2((float)System.Math.Ceiling(X), (float)System.Math.Ceiling(Y));
        public Vector2 Scale(float multiplier) => new Vector2(X * multiplier, Y * multiplier);
        /// <summary>
        /// Rotates around (0, 0) coordinate by an amount of <paramref name="angle"/> measured in radians
        /// </summary>
        /// <param name="angle">rotates clockwise if <paramref name="angle"/> > 0, otherwise rotates anti-clockwise</param>
        /// <returns>new vector after rotation</returns>
        public Vector2 Rotate(float angle)
        {
            float cosx = (float)System.Math.Cos(angle);
            float sinx = (float)System.Math.Sin(angle);
            return new Vector2(cosx * X - sinx * Y, sinx * X + cosx * Y);
        }
        public Vector2 Normalized()
        {
            float distance = Distance();
            return new Vector2(X / distance, Y / distance);
        }

        // Operators
        public static Vector2 operator +(in Vector2 left, in Vector2 right) => left.Add(right);
        public static Vector2 operator -(in Vector2 vec) => vec.Opposite();
        public static Vector2 operator -(in Vector2 left, in Vector2 right) => left.Add(right.Opposite());
        public static float operator *(in Vector2 left, in Vector2 right) => (float)left.DotProduct(right);
        public static Vector2 operator *(in Vector2 vec, float multiplier) => vec.Scale(multiplier);
        public static Vector2 operator /(in Vector2 vec, float divisor) => vec.Scale(1f / divisor);
        public static bool operator ==(in Vector2 left, in Vector2 right) => left.Equals(right);
        public static bool operator !=(in Vector2 left, in Vector2 right) => !left.Equals(right);

        // Private methods
        private Vector2 Opposite() => new Vector2(-X, -Y);
        private Vector2 Add(in Vector2 right) => new Vector2(X + right.X, Y + right.Y);
        private double DotProduct(in Vector2 right) => X * right.X + Y * right.Y;
        private float AngleBetween(in Vector2 vec)
        {
            double cosAlpha = DotProduct(vec) / (Distance() * vec.Distance());
            return (float)System.Math.Acos(cosAlpha);
        }

        // Properties
        public float X { get; private set; }
        public float Y { get; private set; }

        // Predefined instances
        public static readonly Vector2 Right = new Vector2(1f, 0f);
        public static readonly Vector2 Left = new Vector2(-1f, 0f);
        public static readonly Vector2 Up = new Vector2(0f, -1f);
        public static readonly Vector2 Down = new Vector2(0f, 1f);
    }
    public static class Angle
    {
        public static float FromDegrees(float degrees) => degrees * (float)System.Math.PI / 180f;
        public static float FromRadians(float radians) => radians * 180f / (float)System.Math.PI;
    }
}

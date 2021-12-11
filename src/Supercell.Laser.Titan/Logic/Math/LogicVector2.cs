namespace Supercell.Laser.Titan.Logic.Math
{
    using Supercell.Laser.Titan.DataStream;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class LogicVector2
    {
        public int X;
        public int Y;

        public LogicVector2()
        {
        }

        public static LogicVector2 MoveTowards(LogicVector2 current, LogicVector2 target, float maxDistanceDelta)
        {
            // avoid vector ops because current scripting backends are terrible at inlining
            int toVector_x = target.X - current.X;
            int toVector_y = target.Y - current.Y;

            int sqDist = toVector_x * toVector_x + toVector_y * toVector_y;

            if (sqDist == 0 || (maxDistanceDelta >= 0 && sqDist <= maxDistanceDelta * maxDistanceDelta))
                return target;

            int dist = LogicMath.Sqrt(sqDist);
            Console.WriteLine("Dist: " + dist);
            return new LogicVector2((int)(current.X + toVector_x / dist * maxDistanceDelta),
                (int)(current.Y + toVector_y / dist * maxDistanceDelta));
        }

        public LogicVector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Destruct()
        {
            X = 0;
            Y = 0;
        }

        public void Add(LogicVector2 vector2)
        {
            X += vector2.X;
            Y += vector2.Y;
        }

        public LogicVector2 Clone()
        {
            return new LogicVector2(X, Y);
        }

        public int Dot(LogicVector2 vector2)
        {
            return X * vector2.X + Y * vector2.Y;
        }

        public int GetAngle()
        {
            return LogicMath.GetAngle(X, Y);
        }

        public int GetAngleBetween(int x, int y)
        {
            var angleInDegrees = System.Math.Atan2(y, x) * 180 / System.Math.PI;
            if (angleInDegrees < 0) angleInDegrees += 360;
            return (int)angleInDegrees;
        }

        public int GetAngleBetweenPositions(int posX, int posY)
        {
            int x = posX - X;
            int y = posY - Y;

            var angleInDegrees = System.Math.Atan2(y, x) * 180 / System.Math.PI;
            if (angleInDegrees < 0) angleInDegrees += 360;
            return (int)angleInDegrees;
        }

        public int GetAngleBetweenPositions(LogicVector2 pos)
        {
            int x = pos.X - X;
            int y = pos.Y - Y;

            var angleInDegrees = System.Math.Atan2(y, x) * 180 / System.Math.PI;
            if (angleInDegrees < 0) angleInDegrees += 360;
            return (int)angleInDegrees;
        }

        public int GetDistance(LogicVector2 vector2)
        {
            int x = X - vector2.X;
            int distance = 0x7FFFFFFF;

            if ((uint)(x + 46340) <= 92680)
            {
                int y = Y - vector2.Y;

                if ((uint)(y + 46340) <= 92680)
                {
                    int distanceX = x * x;
                    int distanceY = y * y;

                    if ((uint)distanceY < (distanceX ^ 0x7FFFFFFFu))
                    {
                        distance = distanceX + distanceY;
                    }
                }
            }

            return LogicMath.Sqrt(distance);
        }

        public int GetDistanceSquared(LogicVector2 vector2)
        {
            int x = X - vector2.X;
            int distance = 0x7FFFFFFF;

            if ((uint)(x + 46340) <= 92680)
            {
                int y = Y - vector2.Y;

                if ((uint)(y + 46340) <= 92680)
                {
                    int distanceX = x * x;
                    int distanceY = y * y;

                    if ((uint)distanceY < (distanceX ^ 0x7FFFFFFFu))
                    {
                        distance = distanceX + distanceY;
                    }
                }
            }

            return distance;
        }

        public int GetDistanceSquaredTo(int x, int y)
        {
            int distance = 0x7FFFFFFF;

            x -= X;

            if ((uint)(x + 46340) <= 92680)
            {
                y -= Y;

                if ((uint)(y + 46340) <= 92680)
                {
                    int distanceX = x * x;
                    int distanceY = y * y;

                    if ((uint)distanceY < (distanceX ^ 0x7FFFFFFFu))
                    {
                        distance = distanceX + distanceY;
                    }
                }
            }

            return distance;
        }

        public int GetLength()
        {
            int length = 0x7FFFFFFF;

            if ((uint)(46340 - X) <= 92680)
            {
                if ((uint)(46340 - Y) <= 92680)
                {
                    int lengthX = X * X;
                    int lengthY = Y * Y;

                    if ((uint)lengthY < (lengthX ^ 0x7FFFFFFFu))
                    {
                        length = lengthX + lengthY;
                    }
                }
            }

            return LogicMath.Sqrt(length);
        }

        public int GetLengthSquared()
        {
            int length = 0x7FFFFFFF;

            if ((uint)(46340 - X) <= 92680)
            {
                if ((uint)(46340 - Y) <= 92680)
                {
                    int lengthX = X * X;
                    int lengthY = Y * Y;

                    if ((uint)lengthY < (lengthX ^ 0x7FFFFFFFu))
                    {
                        length = lengthX + lengthY;
                    }
                }
            }

            return length;
        }

        public bool IsEqual(LogicVector2 vector2)
        {
            return X == vector2.X && Y == vector2.Y;
        }

        public bool IsInArea(int minX, int minY, int maxX, int maxY)
        {
            if (X >= minX && Y >= minY)
                return X < minX + maxX && Y < maxY + minY;
            return false;
        }

        public void Multiply(LogicVector2 vector2)
        {
            X *= vector2.X;
            Y *= vector2.Y;
        }

        public int Normalize(int value)
        {
            int length = this.GetLength();

            if (length != 0)
            {
                X = X * value / length;
                Y = Y * value / length;
            }

            return length;
        }

        public void Set(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Substract(LogicVector2 vector2)
        {
            X -= vector2.X;
            Y -= vector2.Y;
        }

        public void Encode(ByteStream stream)
        {
            stream.WriteVInt(X);
            stream.WriteVInt(Y);
        }

        public override string ToString()
        {
            return "LogicVector2(" + X + "," + Y + ")";
        }
    }
}

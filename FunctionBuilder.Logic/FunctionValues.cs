using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace FunctionBuilder.Logic
{
    public static class FunctionValues
    {
        public static List<Point> GetPointsList(RPN rpn, double xStart, double xEnd, double minY, 
            double maxY, double step, Point offset, double zoom = 1)
        {
            if (Double.IsNaN(step)) step = (xEnd - xStart) / 339;

            var output = new List<Point>();

            double x = xStart - offset.X - step;
            bool repeat = true;
            do
            {
                x += step;

                if (x >= xEnd - offset.X)
                {
                    repeat = false;
                    x = xEnd - offset.X;
                }

                double y = rpn.Calculate(x * zoom) / zoom - offset.Y;

                if (y > maxY) y = maxY;
                if (y < minY) y = minY;

                output.Add(new Point(x + offset.X, y));
            } while (repeat);

            for (int i = 1; i < output.Count - 1; i++)
            {
                if (output[i].Y == maxY)
                {
                    if (output[i - 1].Y == maxY && output[i + 1].Y == maxY)
                    {
                        output.RemoveAt(i);
                        i--;
                    }
                }
                else if (output[i].Y == minY)
                {
                    if (output[i - 1].Y == minY && output[i + 1].Y == minY)
                    {
                        output.RemoveAt(i);
                        i--;
                    }
                }
            }

            return output;
        }
    }

    public struct Point
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
        public override string ToString() => X.ToString() + " " + Y.ToString();
    }
}

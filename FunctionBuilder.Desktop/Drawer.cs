using FunctionBuilder.Logic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FunctionBuilder.Desktop
{
    public static class Drawer
    {
        private static double zoom = 100;
        private static RPN rpn = new RPN("0");
        private static System.Windows.Point mouseLastPos;
        private static bool mousePressed = false;

        public static Canvas GraphCanvas { get; set; }
        public static Logic.Point Offset { get; private set; }
        public static MainWindow MainWindow{ get; set; }

        public static void SetFunction(string func) 
        {
            rpn = new RPN(func);
        }

        public static void SetControls()
        {
            GraphCanvas.MouseDown += GraphCanvas_MouseDown;
            GraphCanvas.MouseMove += GraphCanvas_MouseMoveDrag;
            GraphCanvas.MouseUp += GraphCanvas_MouseUp; ;
            GraphCanvas.MouseWheel += GraphCanvas_MouseWheel;
            GraphCanvas.MouseMove += GraphCanvas_MouseMove;

            GraphCanvas.SizeChanged += GraphCanvas_SizeChanged; ;
        }

        private static void GraphCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawField();
        }

        private static void GraphCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            zoom = Math.Min(Math.Max(zoom * (1 - 0.005 * e.Delta), 0.001), 200);
            ((TextBlock)MainWindow.FindName(("tbZoom"))).Text = "zoom: " + Math.Round(zoom, 3).ToString();

            DrawField();
        }

        private static void GraphCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var canvas = (Canvas)sender;
            System.Windows.Point mouseEndPos = e.GetPosition(canvas);
            Offset = new Logic.Point(mouseEndPos.X - mouseLastPos.X + Offset.X, mouseEndPos.Y - mouseLastPos.Y + Offset.Y);
            DrawField();
            mousePressed = false;
        }

        private static void GraphCanvas_MouseMoveDrag(object sender, MouseEventArgs e)
        {
            var canvas = (Canvas)sender;
            System.Windows.Point mousePos = e.GetPosition(canvas);
            if (mousePressed)
            {
                Offset = new Logic.Point(Offset.X + mousePos.X - mouseLastPos.X, Offset.Y + mousePos.Y - mouseLastPos.Y);
                DrawField();
                mouseLastPos = mousePos;
            }
        }

        private static void GraphCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var canvas = (Canvas)sender;
            mouseLastPos = e.GetPosition(canvas);
            mousePressed = true;
        }

        private static void GraphCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            TextBlock tbXCoord = (TextBlock)MainWindow.FindName("tbXCoord");
            TextBlock tbYCoord = (TextBlock)MainWindow.FindName("tbYCoord");
            var canvas = (Canvas)sender;

            double x = (e.GetPosition(canvas).X - GraphCanvas.ActualWidth / 2 - Offset.X) * zoom;
            double y = rpn.Calculate(x);

            tbXCoord.Text = "x: " + Math.Round(x, 2).ToString();
            tbYCoord.Text = "y: " + Math.Round(y, 2).ToString();
        }

        public static void DrawField() 
        {
            GraphCanvas.Children.Clear();

            AddArrow(0, GraphCanvas.ActualHeight / 2 + Offset.Y, GraphCanvas.ActualWidth, GraphCanvas.ActualHeight / 2 + Offset.Y, GraphCanvas);
            AddArrow(GraphCanvas.ActualWidth / 2 + Offset.X, GraphCanvas.ActualHeight, GraphCanvas.ActualWidth / 2 + Offset.X, 0, GraphCanvas);

            double minY = -GraphCanvas.ActualHeight / 2 - 1;
            double maxY = GraphCanvas.ActualHeight / 2 + 1;
            List<Logic.Point> points = FunctionValues.GetPointsList(rpn, -GraphCanvas.ActualWidth / 2, GraphCanvas.ActualWidth / 2, minY, maxY, Double.NaN, Offset, zoom);
            AddLinesOnGraphCanvas(points);
        }

        private static void AddLinesOnGraphCanvas(List<Logic.Point> pointsList)
        {
            for (int i = 1; i < pointsList.Count; i++)
            {
                GraphCanvas.Children.Add(new Line()
                {
                    X1 = pointsList[i - 1].X + GraphCanvas.ActualWidth / 2,
                    Y1 = -pointsList[i - 1].Y + GraphCanvas.ActualHeight / 2,
                    X2 = pointsList[i].X + GraphCanvas.ActualWidth / 2,
                    Y2 = -pointsList[i].Y + GraphCanvas.ActualHeight / 2,
                    StrokeThickness = 1,
                    Stroke = Brushes.DarkSlateBlue
                });
            }
        }

        private static void AddArrow(double x1, double y1, double x2, double y2, Canvas canvas)
        {
            double width = 5;
            double length = 15;

            double d = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));

            double X = x2 - x1;
            double Y = y2 - y1;

            double X3 = x2 - (X / d) * length;
            double Y3 = y2 - (Y / d) * length;

            double Xp = y2 - y1;
            double Yp = x1 - x2;

            double X4 = X3 + (Xp / d) * width;
            double Y4 = Y3 + (Yp / d) * width;
            double X5 = X3 - (Xp / d) * width;
            double Y5 = Y3 - (Yp / d) * width;

            Line line = new Line
            {
                Stroke = Brushes.Black,
                X1 = x1,
                X2 = x2,
                Y1 = y1,
                Y2 = y2
            };

            if (line.Y1> 0)
                canvas.Children.Add(line);

            line = new Line
            {
                Stroke = Brushes.Black,
                X1 = x2 - (X / d),
                X2 = X4,
                Y1 = y2 - (Y / d),
                Y2 = Y4
            };

            if (line.Y1 > 0)
                canvas.Children.Add(line);

            line = new Line
            {
                Stroke = Brushes.Black,
                X1 = x2 - (X / d),
                X2 = X5,
                Y1 = y2 - (Y / d),
                Y2 = Y5
            };

            if (line.Y1 > 0)
                canvas.Children.Add(line);
        }
    }
}

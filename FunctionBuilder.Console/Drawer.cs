namespace FunctionBuilder.Console
{
    using System;
    class Drawer
    {
        public static string AskFunction(string text)
        {
            Console.Write("\r" + new string(' ', Console.WindowWidth - Console.CursorLeft) + "\r" + text);
            string expression = Console.ReadLine();

            while (!RPN.IsExpressionCorrectly(expression))
            {
                Console.SetCursorPosition(0, 0);
                Console.Write("\r" + new string(' ', Console.WindowWidth - Console.CursorLeft) + "\r" + "Ошибка!");
                Console.ReadKey(true);
                Console.Write("\r" + new string(' ', Console.WindowWidth - Console.CursorLeft) + "\r" + text);
                expression = Console.ReadLine();
            }
            return expression;
        }

        public static double AskValue(int line, string text)
        {
            double output;
            string input;
            do{
                Console.SetCursorPosition(0, line);
                Console.Write(new string(' ', Console.WindowWidth - Console.CursorLeft) + "\r" + text);
                input = Console.ReadLine();

            } while (!double.TryParse(input, out output));

            return output;
        }

        public static void GiveTable(string formula, double step, double start, double end) 
        {
            RPN rpn = new RPN(formula);
            int maxSize = GetMaxSizeOfValues(rpn, 2, start, step, end);
            DrawPartOfBox(maxSize, '╔', '═', '╦', '╗', "", "");
            DrawPartOfBox(maxSize, '║', ' ', '║', '║', "X", "Y");
            DrawPartOfBox(maxSize, '╠', '═', '╬', '╣', "", "");

            double x = start;
            do{
                double y = GetY(rpn, x);
                DrawPartOfBox(maxSize, '║', ' ', '║', '║', x.ToString(), y.ToString());
                x = Convert.ToDouble(Convert.ToDecimal(x) + Convert.ToDecimal(step));

            } while ((step > 0 && x <= end) || (step < 0 && x >= end));

            DrawPartOfBox(maxSize, '╚', '═', '╩', '╝', "", "");
        }

        private static void DrawPartOfBox(int maxSize, char beginOfBox, char indent, char center, char endOfBox, string x, string y)
        {
            Console.Write(beginOfBox);
            for (int i = 0; i < 2; i++)
            {
                bool endOfField = i == (2 - 1);
                string str = endOfField ? y.ToString() : x.ToString();
                int spaceNum = maxSize - str.Length;

                Console.Write(str);
                for (int j = 0; j < spaceNum + 1; j++)
                    Console.Write(indent);
                Console.Write(endOfField ? endOfBox : center);
            }
            Console.WriteLine("");
        }

        private static int GetMaxSizeOfValues(RPN rpn, int maxSize, double x, double step, double end)
        {
            do{
                double y = GetY(rpn, x);
                if (GetDigitSize(y) > maxSize) maxSize = GetDigitSize(y);
                if (GetDigitSize(x) > maxSize) maxSize = GetDigitSize(x);
                x += step;

            } while ((step > 0 && x <= end) || (step < 0 && x >= end));

            return maxSize;
        }
        private static double GetY(RPN rpn, double x) => rpn.Calculate(x);
        private static int GetDigitSize(double d) => d.ToString().Length;
    }
}

namespace FunctionBuilder.Console
{
    using System;
    using System.Globalization;
    class Program
    {
        static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            string formula = Drawer.AskFunction("Введите формулу: у= ");
            double step = Drawer.AskDoubleNum(1, "Введите шаг: ");
            double xStart = Drawer.AskDoubleNum(2, "Введите начало: ");
            double xEnd = Drawer.AskDoubleNum(3, "Введите конец: ");

            if (step > 0 == xStart < xEnd)
                Drawer.WriteResult(formula, step, xStart, xEnd);
            else
                Drawer.WriteResult(formula, step, xEnd, xStart);
        }
    }
}

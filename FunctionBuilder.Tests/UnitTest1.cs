using NUnit.Framework;
using System.Collections.Generic;
using System.Globalization;

namespace FunctionBuilder.Tests
{
    public class Tests
    {
        [TestCase]
        public void CalculatorTest()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            var expression = "log(2,4) + x* 5";
            Assert.AreEqual(new RPN("log(2,4) + 3* 5").Calculate(), new RPN(expression).Calculate(3));
        }

        [TestCase("cosx", ExpectedResult = true)]
        [TestCase("3*5 +(2*x -1) + log(2,4)", ExpectedResult = true)]
        public bool FormulaCorrectlyTest(string formula)
        {
            return RPN.IsExpressionCorrectly(formula);
        }

        [TestCaseSource(nameof(TestCases))]
        public void CalculatorTests(double result, string expression)
        {
            Assert.AreEqual(result, new RPN(expression).Calculate());
        }
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData(39, " 14/2 * 5 + 4");
                yield return new TestCaseData(double.PositiveInfinity, "(15 + 45)/0");
            }
        }
    }
}
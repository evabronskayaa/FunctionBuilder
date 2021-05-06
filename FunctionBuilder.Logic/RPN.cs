using System;
using System.Collections.Generic;

namespace FunctionBuilder
{
    public class RPN
    {
        private readonly List<object> tokens;
        public RPN(string expression) 
        {
            expression = expression.Replace(" ", "");
            tokens = FormStringList(ParseToTokens(expression));
        }

        public RPN(List<object> tokens) 
        {
            this.tokens = new List<object>(tokens);
        }

        public List<object> ParseToTokens(string str)
        {
            string element = string.Empty;
            List<object> tokenList = new List<object>();

            for (int i = 0; i < str.Length; i++)
            {
                element += str[i];

                if (char.IsDigit(element[0]))
                {
                    if (!(char.IsDigit(element[element.Length - 1])))
                    {
                        element = element.Remove(element.Length - 1);
                        i--;
                        tokenList.Add(double.Parse(element));
                        element = string.Empty;
                    }
                }
                else if (element == "(" || element == ")")
                {
                    tokenList.Add(new Parenthessis(element));
                    element = string.Empty;
                }
                else if (element == "x")
                {
                    tokenList.Add(new Variable());
                    element = string.Empty;
                }
                else if (TryGetOperator(element, out Operation result))
                {
                    tokenList.Add(result);
                    element = string.Empty;
                }
                else
                {
                    throw new Exception("Выражение некорректно");
                }
            }
            if (element.Length != 0)
            {
                tokenList.Add(double.Parse(element));
            }

            return tokenList;
        }

        public bool TryGetOperator(string oper, out Operation result)
        {
            switch (oper)
            {
                case "+":
                    result = (Operation)(new Plus());
                    return true;
                case "-":
                    result = (Operation)(new Minus());
                    return true;
                case "*":
                    result = (Operation)(new Multiply());
                    return true;
                case "/":
                    result = (Operation)(new Divide());
                    return true;
                case "log":
                    result = (Operation)(new Log());
                    return true;
                case "cos":
                    result = (Operation)(new Cos());
                    return true;
                default:
                    throw new Exception("Использование неопознанного оператора");
            }
        }

        private List<object> FormStringList(List<object> pInput)
        {
            List<object> firstList = new List<object>();
            List<object> secondList = new List<object>();

            for (int i = 0; i < pInput.Count; i++)
            {
                if (pInput[i] is double || pInput[i] is Variable)
                {
                    firstList.Add(pInput[i]);
                }
                else if (pInput[i] is Parenthessis && ((Parenthessis)pInput[i]).IsOpening) 
                {
                    secondList.Add(pInput[i]);
                }
                else if (pInput[i] is Parenthessis && !((Parenthessis)pInput[i]).IsOpening)
                {
                    for (int j = secondList.Count - 1; j >= 0; j--)
                    {
                        if (pInput[i] is Parenthessis && ((Parenthessis)pInput[i]).IsOpening)
                        {
                            secondList.RemoveAt(secondList.Count - 1);
                            break;
                        }
                        else
                        {
                            firstList.Add(secondList[secondList.Count - 1]);
                            secondList.RemoveAt(secondList.Count - 1);
                        }
                    }
                }
                else if (pInput[i] is Operation)
                {
                    if (secondList.Count >= 1)
                    {
                        if (((Operation)secondList[secondList.Count - 1]).Priority >= ((Operation)pInput[i]).Priority)
                        {
                            firstList.Add(secondList[secondList.Count - 1]);
                            secondList.RemoveAt(secondList.Count - 1);
                        }
                    }
                    secondList.Add(pInput[i]);
                }
            }

            for (int i = secondList.Count - 1; i >= 0; i--) 
            {
                firstList.Add(secondList[i]);
            }

            return firstList;
        }

        public double Calculate()
        { 
            for (int i = 0; i < tokens.Count; i++) 
            {
                if (tokens[i] is Operation operation) 
                {
                    List<object> tokenList = new List<object>();
                    int count = operation.OperandCount;

                    for (int j = 1; j <= count; j++)
                    {
                        tokenList.Add(tokens[i - j]);
                        tokens.RemoveAt(i - j);
                    }

                    i -= count;
                    tokens[i] = operation.Evaluate(tokenList);
                }
            }

            return (double)tokens[0];
        }

        public double Calculate(double num) 
        {
            RPN rpn = new RPN(tokens);

            for (int i = 0; i < rpn.tokens.Count; i++)
                if (rpn.tokens[i] is Variable)
                    rpn.tokens[i] = num;

            return rpn.Calculate();
        }

        public static bool IsExpressionCorrectly(string expression) 
        {
            try
            {
                RPN rpn = new RPN(expression);
                rpn.Calculate(1);
                return true;
            }
            catch 
            {
                return false;
            }
        }
    }
}

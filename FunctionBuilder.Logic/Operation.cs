using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionBuilder
{
    public class Parenthessis
    {
        public bool IsOpening { get; }

        public Parenthessis(string parenthesis)
        {
            IsOpening = parenthesis == "(";
        }
    }

    public class Variable
    {
    }

    public abstract class Operation
    {
        public abstract int Priority { get; }
        public abstract string Name { get; }
        public abstract int OperandCount { get; }

        protected void CheckOperandCount(List<object> values)
        {
            if (values.Count != OperandCount)
                throw new ArgumentException("Неверное количество аргументов.");
        }
        
        public abstract object Evaluate(List<object> values);
    }

    public class Plus : Operation
    {
        public override int Priority { get; } = 1;
        public override string Name { get; } = "+";
        public override int OperandCount { get; } = 2;

        public override object Evaluate(List<object> values)
        {
            CheckOperandCount(values);
            return (double)values[0] + (double)values[1];
        }
    }

    public class Minus : Operation
    {
        public override int Priority => 1;
        public override string Name => "-";
        public override int OperandCount => 2;

        public override object Evaluate(List<object> values)
        {
            CheckOperandCount(values);
            return (double)values[0] - (double)values[1];
        }
    }

    public class Multiply : Operation
    {
        public override int Priority => 2;
        public override string Name => "*";
        public override int OperandCount => 2;

        public override object Evaluate(List<object> values)
        {
            CheckOperandCount(values);
            return (double)values[0] * (double)values[1];
        }
    }

    public class Divide : Operation
    {
        public override int Priority => 2;
        public override string Name => "/";
        public override int OperandCount => 2;

        public override object Evaluate(List<object> values)
        {
            CheckOperandCount(values);
            return (double)values[0] / (double)values[1];
        }
    }

    public class Log : Operation
    {
        public override int Priority => 10;
        public override string Name => "log";
        public override int OperandCount => 2;

        public override object Evaluate(List<object> values)
        {
            CheckOperandCount(values);
            return Math.Log((double)values[0], (double)values[1]);
        }
    }

    public class Cos : Operation
    {
        public override int Priority => 10;
        public override string Name => "cos";
        public override int OperandCount => 1;

        public override object Evaluate(List<object> values)
        {
            CheckOperandCount(values);
            return Math.Cos((double)values[0]);
        }
    }
}
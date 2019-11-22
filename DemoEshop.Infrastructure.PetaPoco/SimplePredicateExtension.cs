using System;
using System.Collections.Generic;
using DemoEshop.Infrastructure.Query.Predicates;
using DemoEshop.Infrastructure.Query.Predicates.Operators;

namespace DemoEshop.Infrastructure.PetaPoco
{
    public static class SimplePredicateExtension
    {
        private static readonly IDictionary<ValueComparingOperator, Func<object, string>> BinaryOperations =
            new Dictionary<ValueComparingOperator, Func<object, string>>
            {
                {ValueComparingOperator.Equal, rightOperand => CheckCommaUsage(rightOperand) ? $" = '{rightOperand}'" : $" = {rightOperand}" },
                {ValueComparingOperator.NotEqual, rightOperand => CheckCommaUsage(rightOperand) ? $" != '{rightOperand}'" : $" != {rightOperand}" },
                {ValueComparingOperator.GreaterThan, rightOperand => $" > {rightOperand}" },
                {ValueComparingOperator.GreaterThanOrEqual, rightOperand => $" >= {rightOperand}" },
                {ValueComparingOperator.LessThan, rightOperand => $" < {rightOperand}" },
                {ValueComparingOperator.LessThanOrEqual, rightOperand => $" <= {rightOperand}" },
                {ValueComparingOperator.StringContains, rightOperand => $" LIKE '%{rightOperand}%'"}
            };

        
        public static string GetWhereCondition(this SimplePredicate simplePredicate)
        {
            if (!BinaryOperations.ContainsKey(simplePredicate.ValueComparingOperator))
            {
                throw new InvalidOperationException($"Transformation of value comparing operator: {simplePredicate.ValueComparingOperator} to where condition is not supported!");
            }
            if (simplePredicate.ComparedValue == null)
            {
                return GetNullWhereCondition(simplePredicate);
            }
            return GetEscapedWhereCondition(simplePredicate);
        }
        
        private static string GetNullWhereCondition(SimplePredicate simplePredicate)
        {
            if (simplePredicate.ValueComparingOperator.Equals(ValueComparingOperator.NotEqual))
            {
                return simplePredicate.TargetPropertyName + " IS NOT NULL";
            }
            return simplePredicate.TargetPropertyName + " IS NULL";
        }

        private static bool CheckCommaUsage(object rightOperand)
        {
            return rightOperand != null && (rightOperand is string || rightOperand is Guid || rightOperand is DateTime);
        }

        private static string GetEscapedWhereCondition(SimplePredicate simplePredicate)
        {
            const string atChar = "@";
            if (simplePredicate.ComparedValue is string value && value.Contains(atChar))
            {
                var escapedValue = (object) value.Insert(value.IndexOf(atChar, StringComparison.Ordinal), atChar);
                return simplePredicate.TargetPropertyName + BinaryOperations[simplePredicate.ValueComparingOperator]
                           .Invoke(escapedValue);
            }
            return simplePredicate.TargetPropertyName + BinaryOperations[simplePredicate.ValueComparingOperator]
                       .Invoke(simplePredicate.ComparedValue);
        }
    }
}

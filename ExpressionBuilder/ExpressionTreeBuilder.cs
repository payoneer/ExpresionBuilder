using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpresionBuilder
{
    public class ExpressionTreeBuilder
    {
        private static Type StringType = typeof(string);
        private const int LastInOrder = 6;
        private static Dictionary<Type, int> TypesDictionary = new Dictionary<Type, int>
        {
            [typeof(bool)] = 1,

            [typeof(int)] = 2,
            [typeof(long)] = 2,
            [typeof(uint)] = 2,
            [typeof(ulong)] = 2,

            [typeof(float)] = 3,
            [typeof(double)] = 3,

            [typeof(decimal)] = 4,

            [typeof(string)] = 5,
            [typeof(Guid)] = 5,
        };
        private static readonly List<string> StringOperations = new List<string> { ".Contains(" };

        public static int EnumerateProperty<t>(Expression<Func<t, bool>> expression) where t : class
        {
            Type type;
            if (expression.Body is BinaryExpression body)
            {
                if (!body.ToString().Contains("And"))//if complex expression
                {
                    type = body.Left.Type;
                    if (type.Name.Contains("Nullable"))//is Nullable type
                    {
                        type = Nullable.GetUnderlyingType(type);
                    }
                }
                else
                {
                    type = StringType;
                }
            }
            else// if not BinaryExpression
            {
                var bodyString = expression.Body.ToString();
                type = StringOperations.Any(s => bodyString.Contains(s)) ?
                    typeof(string) :
                    expression.Body.Type;
            }

            return TypesDictionary.ContainsKey(type) ? TypesDictionary[type] : LastInOrder;
        }

        public static Expression<Func<t, bool>> CreateANDQuery<t>(List<Expression<Func<t, bool>>> expressionList, bool sortProperties = true) where t : class
        {
            if (sortProperties)
            {
                expressionList = expressionList.OrderBy(o => EnumerateProperty<t>(o)).ToList();
            }

            var compoundQuery = expressionList[0];

            foreach (var item in expressionList.Skip(1))
            {
                compoundQuery = compoundQuery.And(item);
            }

            return compoundQuery;
        }


        public static Func<t, bool> CreateCompiledANDQuery<t>(List<Expression<Func<t, bool>>> expressionList, bool sortProperties = true) where t : class
        {
            return CreateANDQuery<t>(expressionList, sortProperties).Compile();
        }
        public static Expression<Func<t, bool>> CreateORQuery<t>(List<Expression<Func<t, bool>>> expressionList, bool sortProperties = true) where t : class
        {
            if (sortProperties)
            {
                expressionList = expressionList.OrderBy(o => EnumerateProperty<t>(o)).ToList();
            }

            var compoundQuery = expressionList[0];

            foreach (var item in expressionList.Skip(1))
            {
                compoundQuery = compoundQuery.Or(item);
            }

            return compoundQuery;
        }

        public static Func<t, bool> CreateCompiledORQuery<t>(List<Expression<Func<t, bool>>> expressionList, bool sortProperties = true) where t : class
        {
            return CreateORQuery<t>(expressionList, sortProperties).Compile();
        }
        public static Expression<Func<t, bool>> CreateQuery<t>(List<Expression<Func<t, bool>>> expressionANDList, List<Expression<Func<t, bool>>> expressionORList,
                                                                                                            bool sortProperties = true) where t : class
        {
            if (sortProperties)
            {
                expressionANDList = expressionANDList.OrderBy(o => EnumerateProperty<t>(o)).ToList();
                expressionORList = expressionORList.OrderBy(o => EnumerateProperty<t>(o)).ToList();
            }

            var compoundQuery = expressionANDList[0];

            foreach (var item in expressionANDList.Skip(1))
            {
                compoundQuery = compoundQuery.And(item);
            }

            foreach (var item in expressionORList)
            {
                compoundQuery = compoundQuery.Or(item);
            }

            return compoundQuery;
        }

        public static Func<t, bool> CreateCompiledQuery<t>(List<Expression<Func<t, bool>>> expressionANDList, List<Expression<Func<t, bool>>> expressionORList, bool sortProperties = false) where t : class
        {
            return CreateQuery<t>(expressionANDList, expressionORList, sortProperties).Compile();
        }



        public static IQueryable<t> CreateOrderASCQuery<t>(IQueryable<t> query, List<Expression<Func<t, dynamic>>> list) where t : class
        {
            var compound = query.OrderBy(list[0].Compile());

            foreach (var nextExpression in list.Skip(1))
            {
                compound = compound.ThenBy(nextExpression.Compile());
            }

            return compound.AsQueryable();
        }

        public static IQueryable<t> CreateOrderDescQuery<t>(IQueryable<t> query, List<Expression<Func<t, dynamic>>> list) where t : class
        {
            var compound = query.OrderByDescending(list[0].Compile());

            foreach (var nextExpression in list.Skip(1))
            {
                compound = compound.ThenByDescending(nextExpression.Compile());
            }

            return compound.AsQueryable();
        }

        public static IQueryable<t> CreateOrderQuery<t>(IQueryable<t> query, List<Expression<Func<t, dynamic>>> listAsc, List<Expression<Func<t, dynamic>>> listDesc, bool startAsc) where t : class
        {
            var compound = startAsc ? query.OrderBy(listAsc[0].Compile()) : query.OrderByDescending(listAsc[0].Compile());

            foreach (var nextExpression in listAsc.Skip(1))
            {
                compound = startAsc ? compound.ThenBy(nextExpression.Compile()) : compound.ThenByDescending(nextExpression.Compile());
            }

            foreach (var nextExpression in listDesc)
            {
                compound = startAsc ? compound.ThenByDescending(nextExpression.Compile()) : compound.ThenBy(nextExpression.Compile());
            }

            return compound.AsQueryable();
        }

    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionBuilderCore
{
    public class ExpressionTreeBuilder
    {
        private static readonly Type StringType = typeof(string);
        private const int LastInOrder = 10;
        private static readonly Dictionary<Type, int> TypesDictionary = new()
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

        private static readonly List<string> StringOperations = new() { ".Contains(" };

        public static int SortProperties<T>(Expression<Func<T, bool>> expression) where T : class
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

                if (type.Name.Contains("Nullable"))//is Nullable type
                {
                    type = Nullable.GetUnderlyingType(type);
                }
            }

            return TypesDictionary.ContainsKey(type) ? TypesDictionary[type] : LastInOrder;
        }

        public static Expression<Func<T, bool>> CreateANDQuery<T>(List<Expression<Func<T, bool>>> expressionList, bool sortProperties = true) where T : class
        {
            if (sortProperties)
            {
                expressionList = expressionList.OrderBy(SortProperties<T>).ToList();
            }

            var compoundQuery = expressionList[0];

            foreach (var item in expressionList.Skip(1))
            {
                compoundQuery = compoundQuery.And(item);
            }

            return compoundQuery;
        }


        public static Func<T, bool> CreateCompiledANDQuery<T>(List<Expression<Func<T, bool>>> expressionList, bool sortProperties = true) where T : class
        {
            return CreateANDQuery<T>(expressionList, sortProperties).Compile();
        }
        public static Expression<Func<T, bool>> CreateORQuery<T>(List<Expression<Func<T, bool>>> expressionList, bool sortProperties = true) where T : class
        {
            if (sortProperties)
            {
                expressionList = expressionList.OrderBy(o => SortProperties<T>(o)).ToList();
            }

            var compoundQuery = expressionList[0];

            foreach (var item in expressionList.Skip(1))
            {
                compoundQuery = compoundQuery.Or(item);
            }

            return compoundQuery;
        }

        public static Func<T, bool> CreateCompiledORQuery<T>(List<Expression<Func<T, bool>>> expressionList, bool sortProperties = true) where T : class
        {
            return CreateORQuery<T>(expressionList, sortProperties).Compile();
        }
        public static Expression<Func<T, bool>> CreateQuery<T>(List<Expression<Func<T, bool>>> expressionANDList, List<Expression<Func<T, bool>>> expressionORList,
                                                                                                            bool sortProperties = true) where T : class
        {
            if (sortProperties)
            {
                expressionANDList = expressionANDList.OrderBy(SortProperties<T>).ToList();
                expressionORList = expressionORList.OrderBy(SortProperties<T>).ToList();
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

        public static Func<T, bool> CreateCompiledQuery<T>(List<Expression<Func<T, bool>>> expressionANDList, List<Expression<Func<T, bool>>> expressionORList, bool sortProperties = false) where T : class
        {
            return CreateQuery<T>(expressionANDList, expressionORList, sortProperties).Compile();
        }



        public static IQueryable<T> CreateOrderASCQuery<T>(IQueryable<T> query, List<Expression<Func<T, dynamic>>> list) where T : class
        {
            var compound = query.OrderBy(list[0].Compile());

            foreach (var nextExpression in list.Skip(1))
            {
                compound = compound.ThenBy(nextExpression.Compile());
            }

            return compound.AsQueryable();
        }

        public static IQueryable<T> CreateOrderDescQuery<T>(IQueryable<T> query, List<Expression<Func<T, dynamic>>> list) where T : class
        {
            var compound = query.OrderByDescending(list[0].Compile());

            foreach (var nextExpression in list.Skip(1))
            {
                compound = compound.ThenByDescending(nextExpression.Compile());
            }

            return compound.AsQueryable();
        }

        public static IQueryable<T> CreateOrderQuery<T>(IQueryable<T> query, List<Expression<Func<T, dynamic>>> listAsc, List<Expression<Func<T, dynamic>>> listDesc, bool startAsc) where T : class
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


using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpresionBuilder
{

    public class ExpresionTreeBuilder
    {
        public static int EnumerateProperty<t> (Expression<Func<t,Boolean>> expression) where t : class
        {
            Type type;
            var body = expression.Body as System.Linq.Expressions.BinaryExpression;
            if(body == null)
            {
                type = (expression.Body as System.Linq.Expressions.Expression).Type;
            }
            else
            {
                if(!body.ToString().Contains("And"))
                {
                    type = body.Left.Type;
                }
                else
                {
                    type = typeof(string);
                }

            }


            if(type == typeof(bool))
            {
                return 1;
            }

            if(type == typeof(int) || type == typeof(long) || type == typeof(uint) || type == typeof(ulong))
            {
                return 2;
            }

            if(type == typeof(float) || type == typeof(double))
            {
                return 3;
            }

            if(type == typeof(decimal))
            {
                return 4;
            }

            if(type == typeof(string) || type == typeof(Guid))
            {
                return 5;
            }

            return 6;
        }

        public static Expression<Func<t,bool>> CreateANDQuery<t> (List<Expression<Func<t,bool>>> expressionList,bool sortProperties = false) where t : class
        {
            if(sortProperties)
            {
                expressionList = expressionList.OrderBy(o => EnumerateProperty<t>(o)).ToList();
            }

            var compoundQuery = expressionList[0];

            foreach(var item in expressionList.Skip(1))
            {
                compoundQuery = compoundQuery.And(item);
            }

            return compoundQuery;
        }

        public static Expression<Func<t,bool>> CreateORQuery<t> (List<Expression<Func<t,bool>>> expressionList,bool sortProperties = false) where t : class
        {
            if(sortProperties)
            {
                expressionList = expressionList.OrderBy(o => EnumerateProperty<t>(o)).ToList();
            }

            var compoundQuery = expressionList[0];

            foreach(var item in expressionList.Skip(1))
            {
                compoundQuery = compoundQuery.Or(item);
            }

            return compoundQuery;
        }

        public static Expression<Func<t,bool>> CreateQuery<t> (List<Expression<Func<t,bool>>> expressionANDList,List<Expression<Func<t,bool>>> expressionORList,
                                                                                                            bool sortProperties = false) where t : class
        {
            if(sortProperties)
            {
                expressionANDList = expressionANDList.OrderBy(o => EnumerateProperty<t>(o)).ToList();
                expressionORList = expressionORList.OrderBy(o => EnumerateProperty<t>(o)).ToList();
            }

            var compoundQuery = expressionANDList[0];

            foreach(var item in expressionANDList.Skip(1))
            {
                compoundQuery = compoundQuery.And(item);
            }

            foreach(var item in expressionORList)
            {
                compoundQuery = compoundQuery.Or(item);
            }

            return compoundQuery;
        }

        public static IQueryable<t> CreateOrderASCQuery<t> (IQueryable<t> query,List<Expression<Func<t,dynamic>>> list) where t : class
        {
            var compound = query.OrderBy(list[0].Compile());

            foreach(var nextExpression in list.Skip(1))
            {
                compound = compound.ThenBy(nextExpression.Compile());
            }

            return compound.AsQueryable();
        }

        public static IQueryable<t> CreateOrderDescQuery<t> (IQueryable<t> query,List<Expression<Func<t,dynamic>>> list) where t : class
        {
            var compound = query.OrderByDescending(list[0].Compile());

            foreach(var nextExpression in list.Skip(1))
            {
                compound = compound.ThenByDescending(nextExpression.Compile());
            }

            return compound.AsQueryable();
        }

        public static IQueryable<t> CreateOrderQuery<t> (IQueryable<t> query,List<Expression<Func<t,dynamic>>> listAsc,List<Expression<Func<t,dynamic>>> listDesc,bool startAsc) where t : class
        {
            var compound = startAsc ? query.OrderBy(listAsc[0].Compile()) : query.OrderByDescending(listAsc[0].Compile());

            foreach(var nextExpression in listAsc.Skip(1))
            {
                compound = startAsc ? compound.ThenBy(nextExpression.Compile()) : compound.ThenByDescending(nextExpression.Compile());
            }

            foreach(var nextExpression in listDesc)
            {
                compound = startAsc ? compound.ThenByDescending(nextExpression.Compile()) : compound.ThenBy(nextExpression.Compile());
            }

            return compound.AsQueryable();
        }

    }
    //http://blogs.msdn.com/b/meek/archive/2008/05/02/linq-to-entities-combining-predicates.aspx

    public class ParameterRebinder :ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression,ParameterExpression> map;

        public ParameterRebinder (Dictionary<ParameterExpression,ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression,ParameterExpression>();
        }

        public static Expression ReplaceParameters (Dictionary<ParameterExpression,ParameterExpression> map,Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        protected override Expression VisitParameter (ParameterExpression p)
        {
            ParameterExpression replacement;
            if(map.TryGetValue(p,out replacement))
            {
                p = replacement;
            }
            return base.VisitParameter(p);
        }
    }

    public static class ExpressionUtility
    {
        public static Expression<T> Compose<T> (this Expression<T> first,Expression<T> second,Func<Expression,Expression,Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)
            var map = first.Parameters.Select((f,i) => new { f,s = second.Parameters[i] }).ToDictionary(p => p.s,p => p.f);

            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = ParameterRebinder.ReplaceParameters(map,second.Body);

            // apply composition of lambda expression bodies to parameters from the first expression
            return Expression.Lambda<T>(merge(first.Body,secondBody),first.Parameters);
        }

        public static Expression<Func<T,bool>> And<T> (this Expression<Func<T,bool>> first,Expression<Func<T,bool>> second)
        {
            return first.Compose(second,Expression.AndAlso);
        }

        public static Expression<Func<T,bool>> Or<T> (this Expression<Func<T,bool>> first,Expression<Func<T,bool>> second)
        {
            return first.Compose(second,Expression.OrElse);
        }
    }
}


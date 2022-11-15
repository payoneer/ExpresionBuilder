using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionBuilderCore
{
    //for more info read the blog posts:
    //https://learn.microsoft.com/en-us/archive/blogs/meek/linq-to-entities-combining-predicates
    //https://learn.microsoft.com/en-us/archive/blogs/mattwar/linq-building-an-iqueryable-provider-part-ii

    public class ParameterReplacer : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> map;

        public ParameterReplacer(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ParameterReplacer(map).Visit(exp);
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            if (map.TryGetValue(p, out ParameterExpression replacement))
            {
                p = replacement;
            }
            return base.VisitParameter(p);
        }
    }
}


using System;
using ExpressionBuilder.Tests.Models;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ExpressionBuilderCore;
using Microsoft.EntityFrameworkCore;

namespace ExpressionBuilder.Tests
{
    public class ExpressionSearch
    {
        public List<Blog> Search(SearchModel searchModel)
        {
            var context = new BlogsContext();

            var expressionList = new List<Expression<Func<Blog, bool>>>();

            if (searchModel.Name != null)
            {
                expressionList.Add(b => b.Name == searchModel.Name);
            }

            if (searchModel.Description != null)
            {
                expressionList.Add(b => b.Description.Contains(searchModel.Description));
            }

            if (searchModel.Link != null)
            {
                expressionList.Add(b => b.Link.Contains(searchModel.Link));
            }

            if (searchModel.Author != null)
            {
                expressionList.Add(b => b.Author.Contains(searchModel.Author));
            }

            if (searchModel.Tags != null)
            {
                expressionList.Add(b => b.Tags.Contains(searchModel.Tags));
            }

            if (searchModel.AgeRestriction.HasValue)
            {
                expressionList.Add(b => b.AgeRestriction >= searchModel.AgeRestriction.Value);
            }

            if (searchModel.IsActive.HasValue)
            {
                expressionList.Add(b => b.IsActive == searchModel.IsActive.Value);
            }

            var predicate = ExpressionTreeBuilder.CreateANDQuery<Blog>(expressionList);
            var query = context.Set<Blog>().Where(predicate);
            //print the query to console
            Console.WriteLine(query.ToQueryString());

            return query.ToList();
        }
    }

}

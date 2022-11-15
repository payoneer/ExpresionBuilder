using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ExpressionBuilder.Tests.Models;
using ExpressionBuilderCore;
using Microsoft.EntityFrameworkCore;

namespace ExpressionBuilder.Tests
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            var context = new BlogContext();

            var beneficiaries = await context.Blogs.Select(s => new Blog
            {
                Id = s.Id
            }).ToListAsync();

            var andList = new List<Expression<Func<Blog, bool>>>
            {
                p => p.Rank > 1,
                p=>p.Name.Contains("books"),
                p => p.IsActive,
            };

            var predicate = ExpressionTreeBuilder.CreateANDQuery<Blog>(andList);
            var query = context.Set<Blog>().Where(predicate);
            Console.WriteLine(query.ToQueryString());
            var test = await query.ToListAsync();


            Console.WriteLine("Hello World!");
        }

    }
}

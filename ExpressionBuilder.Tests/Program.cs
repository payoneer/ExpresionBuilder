using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ExpresionBuilder.Tests.Models;
using ExpresionBuilderCore;
using Microsoft.EntityFrameworkCore;

namespace ExpresionBuilder.Tests
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            var context = new PaydayContext();

            var beneficiaries = await context.Beneficiaries.Select(s => new Beneficiary
            {
                Id = s.Id
            }).ToListAsync();

            var andList = new List<Expression<Func<Beneficiary, bool>>>
            {
                p => p.Rank > 1,
                p=>p.Name.Contains("books"),
                p => p.IsActive,
            };

            var predicate = ExpressionTreeBuilder.CreateANDQuery<Beneficiary>(andList);
            var query = context.Set<Beneficiary>().Where(predicate);
            Console.WriteLine(query.ToQueryString());
            var test = await query.ToListAsync();


            Console.WriteLine("Hello World!");
        }

    }
}

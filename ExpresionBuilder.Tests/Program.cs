using ExpresionBuilder.Tests.Models;
using ExpresionBuilderCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ExpresionBuilder.Tests
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            var context = new ExpressionsContext();
             
            var persons = await context.Persons.Select(s => new Person
            {
                PersonId = s.PersonId
            }).ToListAsync();

            var andList = new List<Expression<Func<Person, bool>>>();
            andList.Add(p => p.PersonId ==1);
            andList.Add(p => p.FirstName == "michael");

            var predicate = ExpresionTreeBuilder.CreateANDQuery<Person>(andList);
            var query = context.Set<Person>().Where(predicate);
            Console.WriteLine(query.ToQueryString());
            var test = await query.ToListAsync();


            Console.WriteLine("Hello World!");
        }

    }
}

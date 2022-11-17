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
            var search = new ExpressionSearch();
            var result = search.Search(new SearchModel
            {
                IsActive = true,
                AgeRestriction = 9,
                Name = "myBlog"
            });

            Console.ReadKey();
        }

    }
}

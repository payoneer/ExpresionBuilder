using ExpressionBuilder.Tests.Models;
using System.Collections.Generic;
using System.Linq;

namespace ExpressionBuilder.Tests
{
    public class BasicSearch
    {
        public List<Blog> Search(SearchModel searchModel)
        {
            var context = new BlogContext();

            var blogsList = context.Blogs.AsQueryable();

            if (searchModel.IsActive.HasValue)
            {
                blogsList = blogsList.Where(b => b.IsActive == searchModel.IsActive.Value);
            }

            if (searchModel.Rank.HasValue)
            {
                blogsList = blogsList.Where(b => b.Rank == searchModel.Rank.Value);
            }

            if (searchModel.Name != null)
            {
                blogsList = blogsList.Where(b => b.Name == searchModel.Name);
            }

            if (searchModel.Description != null)
            {
                blogsList = blogsList.Where(b => b.Description.Contains(searchModel.Description));
            }

            if (searchModel.Link != null)
            {
                blogsList = blogsList.Where(b => b.Link.Contains(searchModel.Link));
            }

            return blogsList.ToList();
        }
    }
}

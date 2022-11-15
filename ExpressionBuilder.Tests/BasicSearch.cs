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

            if (searchModel.AgeRestrection.HasValue)
            {
                blogsList = blogsList.Where(b => b.AgeRestrection == searchModel.AgeRestrection.Value);
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

            if (searchModel.Auther != null)
            {
                blogsList = blogsList.Where(b => b.Auther.Contains(searchModel.Auther));
            }

            if (searchModel.Tags != null)
            {
                blogsList = blogsList.Where(b => b.Tags.Contains(searchModel.Tags));
            }

            return blogsList.ToList();
        }
    }
}

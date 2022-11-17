using System;
using System.Collections.Generic;

namespace ExpressionBuilder.Tests.Models;

public partial class Blog
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Author { get; set; }

    public string Tags { get; set; }

    public string Link { get; set; }

    public string ImageUrl { get; set; }

    public bool IsActive { get; set; }

    public int AgeRestriction { get; set; }
}

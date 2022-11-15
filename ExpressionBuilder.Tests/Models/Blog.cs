using System.Diagnostics;

namespace ExpressionBuilder.Tests.Models;

[DebuggerDisplay("{Id} {Name} {Rank} {IsActive}")]
public partial class Blog
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Auther { get; set; }

    public string Tags { get; set; }

    public string Link { get; set; }

    public string ImageUrl { get; set; }

    public bool IsActive { get; set; }
 

    public int AgeRestrection { get; set;}

}

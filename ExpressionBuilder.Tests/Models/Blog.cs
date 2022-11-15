using System.Diagnostics;

namespace ExpressionBuilder.Tests.Models;

[DebuggerDisplay("{Id} {Name} {Rank} {IsActive}")]
public partial class Blog
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string Link { get; set; }

    public string ImageUrl { get; set; }

    public bool IsActive { get; set; }

    public int Rank { get; set; }

}

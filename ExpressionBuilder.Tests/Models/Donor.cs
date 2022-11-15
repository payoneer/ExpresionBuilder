using System.Collections.Generic;

namespace ExpresionBuilder.Tests.Models;

public partial class Donor
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public virtual ICollection<DonorStatus> DonorStatuses { get; } = new List<DonorStatus>();

    public virtual ICollection<Payment> Payments { get; } = new List<Payment>();
}

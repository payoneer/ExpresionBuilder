using System.Collections.Generic;

namespace ExpressionBuilder.Tests.Models;

public partial class StatusSetting
{
    public int Id { get; set; }

    public int BeneficiaryId { get; set; }

    public int Amount { get; set; }

    public string Name { get; set; }

    public virtual Beneficiary Beneficiary { get; set; }

    public virtual ICollection<DonorStatus> DonorStatuses { get; } = new List<DonorStatus>();
}

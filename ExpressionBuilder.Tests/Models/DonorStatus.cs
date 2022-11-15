namespace ExpressionBuilder.Tests.Models;

public partial class DonorStatus
{
    public int Id { get; set; }

    public int DonorId { get; set; }

    public int BeneficiaryId { get; set; }

    public int StatusId { get; set; }

    public virtual Beneficiary Beneficiary { get; set; }

    public virtual Donor Donor { get; set; }

    public virtual StatusSetting Status { get; set; }
}

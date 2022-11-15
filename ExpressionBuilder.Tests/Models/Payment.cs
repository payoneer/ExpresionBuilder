using System;
using System.Collections.Generic;

namespace ExpressionBuilder.Tests.Models;

public partial class Payment
{
    public int Id { get; set; }

    public int BeneficiaryId { get; set; }

    public int DonorId { get; set; }

    public DateTime Date { get; set; }

    public double Amount { get; set; }

    public bool IsRecurring { get; set; }

    public int? RecurringCycle { get; set; }

    public string Currency { get; set; }

    public virtual Beneficiary Beneficiary { get; set; }

    public virtual Donor Donor { get; set; }

    public virtual ICollection<PaymentHistory> PaymentHistories { get; } = new List<PaymentHistory>();
}

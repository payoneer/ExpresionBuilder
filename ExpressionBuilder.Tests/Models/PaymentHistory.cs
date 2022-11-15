using System;

namespace ExpresionBuilder.Tests.Models;

public partial class PaymentHistory
{
    public int Id { get; set; }

    public int PaymentId { get; set; }

    public bool WasPaid { get; set; }

    public string Comment { get; set; }

    public DateTime Date { get; set; }

    public double Amount { get; set; }

    public virtual Payment Payment { get; set; }
}

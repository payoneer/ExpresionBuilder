using Microsoft.EntityFrameworkCore;

namespace ExpresionBuilder.Tests.Models;

public partial class PaydayContext : DbContext
{
    public PaydayContext()
    {
    }

    public PaydayContext(DbContextOptions<PaydayContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Beneficiary> Beneficiaries { get; set; }

    public virtual DbSet<Donor> Donors { get; set; }

    public virtual DbSet<DonorStatus> DonorStatuses { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentHistory> PaymentHistories { get; set; }

    public virtual DbSet<StatusSetting> StatusSettings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Initial Catalog=Payday;Trusted_Connection=true;Integrated Security=True;MultipleActiveResultSets=True;encrypt=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Beneficiary>(entity =>
        {
            entity.ToTable("Beneficiary");

            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.ImageUrl).HasMaxLength(200);
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.Link).HasMaxLength(200);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.Rank).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<Donor>(entity =>
        {
            entity.ToTable("Donor");

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.FirstName).HasMaxLength(200);
            entity.Property(e => e.LastName).HasMaxLength(200);
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(200);
        });

        modelBuilder.Entity<DonorStatus>(entity =>
        {
            entity.ToTable("DonorStatus");

            entity.HasOne(d => d.Beneficiary).WithMany(p => p.DonorStatuses)
                .HasForeignKey(d => d.BeneficiaryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DonorStatus_Beneficiary");

            entity.HasOne(d => d.Donor).WithMany(p => p.DonorStatuses)
                .HasForeignKey(d => d.DonorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DonorStatus_Donor");

            entity.HasOne(d => d.Status).WithMany(p => p.DonorStatuses)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DonorStatus_StatusSetting");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("Payment");

            entity.Property(e => e.Currency)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Date).HasColumnType("date");

            entity.HasOne(d => d.Beneficiary).WithMany(p => p.Payments)
                .HasForeignKey(d => d.BeneficiaryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payment_Beneficiary");

            entity.HasOne(d => d.Donor).WithMany(p => p.Payments)
                .HasForeignKey(d => d.DonorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payment_Donor");
        });

        modelBuilder.Entity<PaymentHistory>(entity =>
        {
            entity.ToTable("PaymentHistory");

            entity.Property(e => e.Date).HasColumnType("datetime");

            entity.HasOne(d => d.Payment).WithMany(p => p.PaymentHistories)
                .HasForeignKey(d => d.PaymentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PaymentHistory_Payment");
        });

        modelBuilder.Entity<StatusSetting>(entity =>
        {
            entity.ToTable("StatusSetting");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.Beneficiary).WithMany(p => p.StatusSettings)
                .HasForeignKey(d => d.BeneficiaryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StatusSetting_Beneficiary");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

global using Microsoft.EntityFrameworkCore;
using Members1stRest;

/// <summary>
/// Sets up an in-memory DB context for EF Core.
/// </summary>
class TransactionDataDB : DbContext
{
    public TransactionDataDB(DbContextOptions<TransactionDataDB> options)
        : base(options) { }

    public DbSet<TransactionData> Transactions => Set<TransactionData>();
}
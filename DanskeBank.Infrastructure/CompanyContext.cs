using DanskeBank.Domain.CompanyAggregate;
using DanskeBank.Domain.SeedWork;
using DanskeBank.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace DanskeBank.Infrastructure;

public class CompanyContext : DbContext, IUnitOfWork
{
    public const string DefaultSchema = "company";
    public const string DbName = "DanskeBankDatabase";

    public DbSet<Company> Companies { get; set; }

    public IDbContextTransaction? GetCurrentTransaction() => _currentTransaction;

    public bool HasActiveTransaction => _currentTransaction != null;

    private IDbContextTransaction? _currentTransaction;

    public CompanyContext(DbContextOptions<CompanyContext> options) : base(options) { }

    public async Task<IDbContextTransaction?> BeginTransactionAsync()
    {
        if (_currentTransaction != null) 
            return null;

        _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        if (transaction == null) 
            throw new ArgumentNullException(nameof(transaction));
        if (transaction != _currentTransaction) 
            throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

        try
        {
            await SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            RollbackTransaction();

            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();

                _currentTransaction = null;
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfiguration(new CompanyEntityConfiguration());
}

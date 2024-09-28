using System.Linq.Expressions;
using Honeywell.DataAccess.Data;
using Honeywell.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Honeywell.DataAccess.Repositories;

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    private readonly ApplicationDbContext _dbContext;

    internal RepositoryBase(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<T> FindAll(bool trackChanges) =>
        !trackChanges
            ? _dbContext.Set<T>()
                .AsNoTracking()
            : _dbContext.Set<T>();

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression,
        bool trackChanges) =>
        !trackChanges
            ? _dbContext.Set<T>()
                .Where(expression)
                .AsNoTracking()
            : _dbContext.Set<T>()
                .Where(expression);

    public void Create(T entity) => _dbContext.Set<T>().Add(entity);

    public void Update(T entity) => _dbContext.Set<T>().Update(entity);

    public void Delete(T entity) => _dbContext.Set<T>().Remove(entity);

    public Task SaveAsync() => _dbContext.SaveChangesAsync();
}
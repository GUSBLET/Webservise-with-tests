namespace DataAccessLayer.Repositories;

public class TestRepository : IBaseRepository<Test>
{
    private readonly ApplicationDbContext _database;

    public TestRepository(ApplicationDbContext database) => _database = database;

    public async Task<bool> Add(Test entity)
    {
        try
        {
            await _database.Tests.AddAsync(entity);
            await _database.SaveChangesAsync();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }

    public async Task<bool> Delete(Test entity)
    {
        try
        {
            _database.Tests.Remove(entity);
            await _database.SaveChangesAsync();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }

    public IQueryable<Test> Select()
    {
        return _database.Tests;
    }

    public async Task<bool> Update(Test entity)
    {
        try
        {
            _database.Tests.Update(entity);
            await _database.SaveChangesAsync();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }
}

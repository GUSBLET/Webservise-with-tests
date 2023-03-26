namespace DataAccessLayer.Repositories;

public class ResultTestRepository : IBaseRepository<ResultTest>
{
    private readonly ApplicationDbContext _database;

    public ResultTestRepository(ApplicationDbContext database) => _database = database;

    public async Task<bool> Add(ResultTest entity)
    {
        try
        {
            await _database.ResultTest.AddAsync(entity);
            await _database.SaveChangesAsync();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }

    public async Task<bool> Delete(ResultTest entity)
    {
        try
        {
            _database.ResultTest.Remove(entity);
            await _database.SaveChangesAsync();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }

    public IQueryable<ResultTest> Select()
    {
        return _database.ResultTest;
    }

    public async Task<bool> Update(ResultTest entity)
    {
        try
        {
            _database.ResultTest.Update(entity);
            await _database.SaveChangesAsync();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }
}

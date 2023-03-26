namespace DataAccessLayer.Repositories;

public class ResultRepository : IBaseRepository<Result>
{
    private readonly ApplicationDbContext _database;

    public ResultRepository(ApplicationDbContext database) => _database = database;

    public async Task<bool> Add(Result entity)
    {
        try
        {
            await _database.Results.AddAsync(entity);
            await _database.SaveChangesAsync();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }

    public async Task<bool> Delete(Result entity)
    {
        try
        {
            _database.Results.Remove(entity);
            await _database.SaveChangesAsync();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }

    public IQueryable<Result> Select()
    {
        return _database.Results;
    }

    public async Task<bool> Update(Result entity)
    {
        try
        {
            _database.Results.Update(entity);
            await _database.SaveChangesAsync();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }
}

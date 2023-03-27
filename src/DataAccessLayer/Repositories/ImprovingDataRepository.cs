namespace DataAccessLayer.Repositories;

public class ImprovingDataRepository : IBaseRepository<ImprovingData>
{
    private readonly ApplicationDbContext _database;

    public ImprovingDataRepository(ApplicationDbContext database) => _database = database;

    public async Task<bool> Add(ImprovingData entity)
    {
        try
        {
            await _database.ImprovingDatas.AddAsync(entity);
            await _database.SaveChangesAsync();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }

    public async Task<bool> Delete(ImprovingData entity)
    {
        try
        {
            _database.ImprovingDatas.Remove(entity);
            await _database.SaveChangesAsync();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }

    public IQueryable<ImprovingData> Select()
    {
        return _database.ImprovingDatas;
    }

    public async Task<bool> Update(ImprovingData entity)
    {
        try
        {
            _database.ImprovingDatas.Update(entity);
            await _database.SaveChangesAsync();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }
}

namespace DataAccessLayer.Repositories;

public class ProfileImprovingDataRepository : IBaseRepository<ProfileImprovingData>
{
    private readonly ApplicationDbContext _database;

    public ProfileImprovingDataRepository(ApplicationDbContext database) => _database = database;

    public async Task<bool> Add(ProfileImprovingData entity)
    {
        try
        {
            await _database.ProfileImprovingDatas.AddAsync(entity);
            await _database.SaveChangesAsync();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }

    public async Task<bool> Delete(ProfileImprovingData entity)
    {
        try
        {
            _database.ProfileImprovingDatas.Remove(entity);
            await _database.SaveChangesAsync();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }

    public IQueryable<ProfileImprovingData> Select()
    {
        return _database.ProfileImprovingDatas;
    }

    public async Task<bool> Update(ProfileImprovingData entity)
    {
        try
        {
            _database.ProfileImprovingDatas.Update(entity);
            await _database.SaveChangesAsync();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }
}

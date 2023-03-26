namespace DataAccessLayer.Repositories;

public class ProfileRepository : IBaseRepository<Profile>
{
    private readonly ApplicationDbContext _database;

    public ProfileRepository(ApplicationDbContext database) => _database = database;

    public async Task<bool> Add(Profile entity)
    {
        try
        {
            await _database.Profiles.AddAsync(entity);
            await _database.SaveChangesAsync();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }

    public async Task<bool> Delete(Profile entity)
    {
        try
        {
            _database.Profiles.Remove(entity);
            await _database.SaveChangesAsync();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }

    public IQueryable<Profile> Select()
    {
        return _database.Profiles;
    }

    public async Task<bool> Update(Profile entity)
    {
        try
        {
            _database.Profiles.Update(entity);
            await _database.SaveChangesAsync();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }
}

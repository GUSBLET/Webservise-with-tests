namespace DataAccessLayer.Repositories;

public class ProfileTestRepository : IBaseRepository<ProfileTest>
{
    private readonly ApplicationDbContext _database;

    public ProfileTestRepository(ApplicationDbContext database) => _database = database;

    public async Task<bool> Add(ProfileTest entity)
    {
        try
        {
            await _database.ProfileTest.AddAsync(entity);
            await _database.SaveChangesAsync();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }

    public async Task<bool> Delete(ProfileTest entity)
    {
        try
        {
            _database.ProfileTest.Remove(entity);
            await _database.SaveChangesAsync();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }

    public IQueryable<ProfileTest> Select()
    {
        return _database.ProfileTest;
    }

    public async Task<bool> Update(ProfileTest entity)
    {
        try
        {
            _database.ProfileTest.Update(entity);
            await _database.SaveChangesAsync();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }
}

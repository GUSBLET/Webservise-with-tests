namespace DataAccessLayer.Repositories;

public class UserRepository : IBaseRepository<User>
{
    private readonly ApplicationDbContext _database;

    public UserRepository(ApplicationDbContext database) => _database = database;

    public async Task<bool> Add(User entity)
    {
        //try
        //{
            await _database.Users.AddAsync(entity);
            await _database.SaveChangesAsync();
        //}
        //catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }

    public async Task<bool> Delete(User entity)
    {
        try
        {
            _database.Users.Remove(entity);
            await _database.SaveChangesAsync();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }

    public IQueryable<User> Select()
    {
        return _database.Users;
    }

    public async Task<bool> Update(User entity)
    {
        try
        {
            _database.Users.Update(entity);
            await _database.SaveChangesAsync();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }
}

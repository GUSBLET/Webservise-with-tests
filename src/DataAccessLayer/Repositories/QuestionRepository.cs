namespace DataAccessLayer.Repositories;

public class QuestionRepository : IBaseRepository<Question>
{
    private readonly ApplicationDbContext _database;

    public QuestionRepository(ApplicationDbContext database) => _database = database;

    public async Task<bool> Add(Question entity)
    {
        try
        {
            await _database.Questions.AddAsync(entity);
            await _database.SaveChangesAsync();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }

    public async Task<bool> Delete(Question entity)
    {
        try
        {
            _database.Questions.Remove(entity);
            await _database.SaveChangesAsync();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }

    public IQueryable<Question> Select()
    {
        return _database.Questions;
    }

    public async Task<bool> Update(Question entity)
    {
        try
        {
            _database.Questions.Update(entity);
            await _database.SaveChangesAsync();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }
}

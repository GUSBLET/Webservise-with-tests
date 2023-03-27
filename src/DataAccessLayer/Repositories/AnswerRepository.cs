namespace DataAccessLayer.Repositories;

public class AnswerRepository : IBaseRepository<Answer>
{
    private readonly ApplicationDbContext _database;

    public AnswerRepository(ApplicationDbContext database) => _database = database;

    public async Task<bool> Add(Answer entity)
    {
        try
        {
            await _database.Answers.AddAsync(entity);
            await _database.SaveChangesAsync();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }

    public async Task<bool> Delete(Answer entity)
    {
        try
        {
            _database.Answers.Remove(entity);
            await _database.SaveChangesAsync();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }

    public IQueryable<Answer> Select()
    {
        return _database.Answers;
    }

    public async Task<bool> Update(Answer entity)
    {
        try
        {
            _database.Answers.Update(entity);
            await _database.SaveChangesAsync();
        }
        catch { return await Task.FromResult(false); }

        return await Task.FromResult(true);
    }
}

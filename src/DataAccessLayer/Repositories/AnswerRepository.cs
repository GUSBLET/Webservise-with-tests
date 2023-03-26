namespace DataAccessLayer.Repositories;

public class AnswerRepository : IBaseRepository<Answer>
{
    private readonly ApplicationDbContext _database;

    public AnswerRepository(ApplicationDbContext database) => _database = database;

    public Task<bool> Add(Answer entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(Answer entity)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Answer> Select()
    {
        throw new NotImplementedException();
    }

    public Task<bool> Update(Answer entity)
    {
        throw new NotImplementedException();
    }
}

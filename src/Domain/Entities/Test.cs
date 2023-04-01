namespace Domain.Entities;

public class Test
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public TimeOnly TimePassing { get; set; }
    public User Author { get; set; }
    public ICollection<ProfileTest> ProfileTests { get; set; }
    public ICollection<ResultTest> ResultTests { get; set; }
    public ICollection<Question> Questions { get; set; }
}

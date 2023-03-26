namespace Domain.Entities;

public class Answer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool PowerOfAnswer { get; set; }
    public ICollection<Question> Questions { get; set; }
}

namespace Domain.Entities;

public class Question
{
    public int QuestionId { get; set; }
    public string QuestionText { get; set; }
    public Test Test { get; set; }
    public ICollection<Answer> Answers { get; set; }
}

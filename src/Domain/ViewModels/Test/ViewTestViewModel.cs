namespace Domain.ViewModels.Test;

 public class ViewTestViewModel
{
    public int TestId { get; set; }
    public List<Question> Questions { get; set; }
    public List<string> AnswersChoise { get; set; }

    public string? Email { get; set; }
    
}

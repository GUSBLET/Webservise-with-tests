namespace Domain.ViewModels.Test;

public class AddNewQuestionViewModel
{
    public string? TestTitle { get; set; }
    public string QuestionText { get; set; }
    public List<string> Answers { get; set; }
    public string AnswersPowerHtml { get; set; }
    public List<bool> AnswersPower { get; set; }
    public ParametersOfQuestionViewModel? Parameters { get; set; } 
}

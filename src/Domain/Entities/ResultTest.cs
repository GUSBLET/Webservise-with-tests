namespace Domain.Entities;

public class ResultTest
{
    public int ResultId { get; set; }
    public Result Result { get; set; }
    public int TestId { get; set; }
    public Test Test { get; set; }
    public short MinimumOfDiapason { get; set; }
    public short MaximumOfDiapason { get; set; }
}

namespace Domain.Entities;

public class Result
{
    public int Id { get; set; }
    public string Name { get; set; }    
    public ICollection<ResultTest> ResultTests { get; set; }
}

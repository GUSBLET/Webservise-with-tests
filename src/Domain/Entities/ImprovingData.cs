namespace Domain.Entities;

public class ImprovingData
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Information { get; set; }
    public User Author { get; set; }
    public ICollection<Profile> Profiles { get; set; }
}

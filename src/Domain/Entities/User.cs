namespace Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; }   
    public string Password { get; set; }
    public Role Role { get; set; }
    public bool EmailConfirmed { get; set; }
    public Guid EmailConfirmedToken { get; set; }
    public int ProfileId { get; set; }
    public virtual Profile Profile { get; set; }
    public ICollection<Test> Tests { get; set; }
    public ICollection<ProfileImprovingData> ProfileImprovingDatas { get; set; }
    public ICollection<ImprovingData> ImprovingDatas { get; set; }
}

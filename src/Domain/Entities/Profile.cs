namespace Domain.Entities;

public class Profile
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public DateOnly Year { get; set; }
    public byte[] Avatar { get; set; }
    public int UserId { get; set; }
    public virtual User User { get; set; }
    public ICollection<ProfileImprovingData> ProfileImprovingDatas { get; set; }
    public ICollection<ProfileTest> ProfileTests { get; set; }
}

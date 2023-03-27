namespace Domain.Entities;

public class ProfileImprovingData
{
    public int ProfileId { get; set; }
    public Profile Profile { get; set; }
    public int ImprovingDataId { get; set; }
    public ImprovingData ImprovingData { get; set; }
}

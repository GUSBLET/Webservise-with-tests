using Microsoft.AspNetCore.Http;

namespace Domain.ViewModels.Account;

public class ProfileViewModel
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public DateOnly Year { get; set; }
    public IFormFile Avatar { get; set; }
    public byte[] AvatarShow { get; set; }
    public User User { get; set; }
    public List<Entities.ImprovingData> ImprovingDatas { get; set; }
    public List<Entities.Test> Test { get; set; }
}
    
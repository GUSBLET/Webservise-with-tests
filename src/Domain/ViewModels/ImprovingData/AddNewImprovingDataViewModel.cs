namespace Domain.ViewModels.ImprovingData;

public class AddNewImprovingDataViewModel
{
    public string? Email { get; set; }

    [Required(ErrorMessage = "Title have to be required")]
    [DataType(DataType.Text)]
    [Display(Name = "Title:")]
    public string Title { get; set; }

    [DataType(DataType.Text)]
    [Display(Name = "Description:")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Information have to be required")]
    [DataType(DataType.Text)]
    [Display(Name = "Information:")]
    public string Information { get; set; }
}

namespace Domain.ViewModels.Account;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email have to be required")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email:")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password have to be required")]
    [DataType(DataType.Password)]
    [Display(Name = "Password:")]
    public string Password { get; set; }
}

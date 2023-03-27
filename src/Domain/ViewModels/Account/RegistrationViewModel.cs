namespace Domain.ViewModels.Account;

public class RegistrationViewModel
{
    [Required(ErrorMessage = "Email have to be required")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email:")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password have to be required")]
    [MinLength(8, ErrorMessage = "Password have to be more 8 chars")]
    [DataType(DataType.Password)]
    [Display(Name = "Password:")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Password reapete have to be required")]
    [Compare("Password", ErrorMessage = "Password dosen't compare")]
    [DataType(DataType.Password)]
    [Display(Name = "Password confirm:")]
    public string PasswordConfirm { get; set; }
}

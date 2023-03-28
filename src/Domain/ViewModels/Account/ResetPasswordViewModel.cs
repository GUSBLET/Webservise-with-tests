namespace Domain.ViewModels.Account;

public class ResetPasswordViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Field password have to be required")]
    [MinLength(8, ErrorMessage = "Password have to be more 8 chars")]
    [DataType(DataType.Password)]
    [Display(Name = "Password:")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Field password confirm have to be required")]
    [Compare("Password", ErrorMessage = "Password dosen't compare")]
    [DataType(DataType.Password)]
    [Display(Name = "Password confirm:")]
    public string PasswordConfirm { get; set; }
}

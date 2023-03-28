namespace Domain.ViewModels.Account;

public class SendResetPasswordRequistViewModel
{
    [Required(ErrorMessage = "Field have to be required")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email:")]
    public string Email { get; set; }

}

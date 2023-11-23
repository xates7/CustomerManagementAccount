
using System;
using System.ComponentModel.DataAnnotations;

public class Account
{
    public int Id { get; set; }
    public bool IsOnline { get; set; }

    [Required(ErrorMessage = "Kullanıcı Adı boş geçilemez.")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "E-posta gereklidir.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Şifre boş geçilemez.")]
    [MaxLength(6, ErrorMessage = "Şifre 6 haneli olmalıdır.")]
    [MinLength(6, ErrorMessage = "Şifre 6 haneli olmalıdır.")]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "Şifre yalnızca rakamlardan oluşmalıdır.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Doğum Tarihi boş geçilemez.")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
    [Display(Name = "Doğum Tarihi")]
    public DateTime? BirthDate { get; set; }
}

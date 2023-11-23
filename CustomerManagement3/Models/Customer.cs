using System;
using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class IdentityAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is string identity)
        {
           
            if (identity.Length != 11)
            {
                return false;
            }

          
            foreach (char character in identity)
            {
                if (!Char.IsDigit(character))
                {
                    return false;
                }
            }

            return true;
        }

        return false;
    }
}

public class Customer
{
    public int Id { get; set; }
    public bool IsOnline { get; set; }

    [Required(ErrorMessage = "Ad boş geçilemez.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Soyad boş geçilemez.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "E-posta gereklidir.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Kimlik  boş geçilemez.")]
    [MaxLength(11, ErrorMessage = "Kimlik 11 haneli olmalıdır.")]
    [MinLength(11, ErrorMessage = "Kimlik 11 haneli olmalıdır.")]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "Kimlik yalnızca rakamlardan oluşmalıdır.")]
    public string Identity { get; set; }

    [Required(ErrorMessage = "Telefon boş geçilemez.")]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "Telefon yalnızca rakamlardan oluşmalıdır.")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "Doğum Tarihi boş geçilemez.")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
    [Display(Name = "Doğum Tarihi")]
    public DateTime? BirthDate { get; set; }
}

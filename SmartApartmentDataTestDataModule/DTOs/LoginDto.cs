using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApartmentDataTestDataModule.DTOs
{
    public class LoginDto : IValidatableObject
    {
        #region Properties

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        #endregion

        #region Object Validation

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Username.Length < 5)
                yield return new ValidationResult("Username field needs to be at least 5 characters long", new[] { nameof(Username) });

            if (Password.Equals(string.Empty) || Password.Length < 6)
                yield return new ValidationResult("Password field needs to be at least 5 characters long", new[] { nameof(Password) });
        }

        #endregion
    }
}

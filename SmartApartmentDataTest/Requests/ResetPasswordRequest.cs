using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartApartmentDataTest.Requests
{
    public class ResetPasswordRequest
    {
        #region Properties

        [Required]
        public string Username { get; set; }

        #endregion

        #region Object Validation

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Username.Length < 5)
                yield return new ValidationResult("You have to provide a username that is at least 5 characters long");
        }

        #endregion
    }
}
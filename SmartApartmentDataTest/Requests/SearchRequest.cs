using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartApartmentDataTest.Requests
{
    public class SearchRequest : IValidatableObject
    {
        #region Properties

        [Required]
        public string Phrase { get; set; }

        public string Market { get; set; }

        public int? Size { get; set; }

        #endregion

        #region Object Validation

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Phrase))
                yield return new ValidationResult("Search phrase can't be null or empty", new[] { nameof(Phrase) });
            if (Size.HasValue && Size.Value < 0)
                yield return new ValidationResult("Size value can't be negative", new[] { nameof(Size) });
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartApartmentDataTest.Requests
{
    public class DeleteIndexRequest : IValidatableObject
    {
        #region Properties

        public string IndexName { get; set; }

        #endregion

        #region Object Validation

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(string.IsNullOrEmpty(IndexName))
                yield return new ValidationResult("Index name can't be null or empty", new []{nameof(IndexName)});
        }

        #endregion
    }
}
using SmartApartmentDataTestDataModule.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApartmentDataTestDataModule.Models
{
    public class ClassModel
    {
        #region Properties

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        #endregion

        #region Public Methods

        public static ClassModel FromDbObject(Entity.@class dbObject) => dbObject != null
            ? new ClassModel
            {
                Id = dbObject.pk_id,
                Name = dbObject.name,
                Description = dbObject.description
            }
            : null;

        #endregion
    }
}

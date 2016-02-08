using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using ClassLibrary;

namespace ClassLibrary.Mapping
{
    public class CategoryMap : EntityTypeConfiguration<mCategory>
    {

        public CategoryMap()
        {
            HasKey(t => t.Id);

            //property  
            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.Name);
            Property(t => t.Description);
            Property(t => t.FCname);
            

            //table  
            ToTable("Categories");  
        }
    }
}

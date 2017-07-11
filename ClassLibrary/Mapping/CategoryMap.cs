using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ClassLibrary.Models;


namespace ClassLibrary.Mapping
{
    public class CategoryMap : EntityTypeConfiguration<CategoryModel>
    {

        //public CategoryMap()
        //{
        //    HasKey(t => t.Id);

        //    //property  
        //    Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        //    Property(t => t.Name);
        //    Property(t => t.Description);
        //    Property(t => t.FCname);
            

        //    //table  
        //    ToTable("Categories");  
        //}
    }
}

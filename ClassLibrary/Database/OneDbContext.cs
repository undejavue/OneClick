using System.Data.Entity;
using System.Data.SqlClient;
using ClassLibrary.Models;

namespace ClassLibrary.Database
{
    public class OneDbContext : DbContext
    {


        public DbSet<CategoryModel> dbCategory { get; set; }
        public DbSet<DataBlockModel> dbDataBlock { get; set; }
        public DbSet<KeyModel> dbKey { get; set; }
        public DbSet<SymbolTableItemModel> dbSymbolTableItem { get; set; }



        public OneDbContext(bool isNew): base(OneConnectionString(""))
        {

            //Database.SetInitializer<OneDbContext>(new DropCreateDatabaseIfModelChanges<OneDbContext>());

            if (isNew)
                System.Data.Entity.Database.SetInitializer<OneDbContext>(new DropCreateDatabaseAlways<OneDbContext>());
                //Database.SetInitializer<OneDbContext>(new ForceDeleteInitializer(new DropCreateDatabaseAlways<OneDbContext>()));
            else
                System.Data.Entity.Database.SetInitializer<OneDbContext>(new DropCreateDatabaseIfModelChanges<OneDbContext>());

            this.Configuration.LazyLoadingEnabled = false; 
            
            
            
        }


        public OneDbContext(string filename) : base(OneConnectionString(filename))
        {

            System.Data.Entity.Database.SetInitializer<OneDbContext>(new DropCreateDatabaseIfModelChanges<OneDbContext>());
            this.Configuration.LazyLoadingEnabled = false;
           
        }


        private static string OneConnectionString(string filepath)
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

            //string  connectionString="Data Source=(LocalDB)\v11.0;AttachDbFilename=D:\proj_VS\OneCLick_Automation\ClassLibrary\OneClickDB.mdf;Integrated Security=True";

            if (filepath.Equals("")) filepath = "D:\\OneDB\\OneDBgenerated.mdf";

            // Set the properties for the data source.
            sqlBuilder.DataSource = "(LocalDB)\\v11.0";
            sqlBuilder.AttachDBFilename = filepath;
            sqlBuilder.IntegratedSecurity = true;
            sqlBuilder.Pooling = false;


            return sqlBuilder.ToString();
        }


        //public static void Clear<T>(this DbSet<T> dbSet) where T : class
        //{
        //    dbSet.RemoveRange(dbSet);
        //}


        // Переопределяем метод OnModelCreating для добавления
        // настроек конфигурации
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // настройка полей с помощью Fluent API
            //modelBuilder.Entity<DataBlockModel>().HasKey(e => e.CategoryId);
            //modelBuilder.Entity<CategoryModel>().HasOptional(c => c.DB).WithRequired(e => e.CategoryModel);
            modelBuilder.Entity<DataBlockModel>().HasOptional<CategoryModel>(db => db.CategoryModel).WithOptionalDependent(cat => cat.Db);//.Map(p => p.MapKey("CategoryId"));

            //modelBuilder.Entity<KeyModel>().HasRequired<CategoryModel>(c => c.CategoryModel).WithMany(c => c.Keys).HasForeignKey(c => c.KeyId);
            modelBuilder.Entity<CategoryModel>().HasMany<KeyModel>(k => k.Keys).WithRequired(k => k.CategoryModel).HasForeignKey(k => k.KeyId);

            modelBuilder.Entity<SymbolTableItemModel>().HasMany<BaseEntityModel>(e => e.PeripheryCode);

            //modelBuilder.Entity<SymbolTableItemModel>().HasRequired<CategoryModel>(c => c.CategoryModel).WithMany(c => c.S7items).HasForeignKey(c => c.itemId);
            modelBuilder.Entity<CategoryModel>().HasMany<SymbolTableItemModel>(i => i.S7Items).WithRequired(i => i.CategoryModel).HasForeignKey(i => i.ItemId);
        }



        public class ForceDeleteInitializer : IDatabaseInitializer<OneDbContext>
        {
            private readonly IDatabaseInitializer<OneDbContext> _initializer;

            public ForceDeleteInitializer(IDatabaseInitializer<OneDbContext> innerInitializer)
            {
                _initializer = innerInitializer;
            }

            public void InitializeDatabase(OneDbContext context)
            {
                //context.Database.SqlQuery("ALTER DATABASE Tocrates SET SINGLE_USER WITH ROLLBACK IMMEDIATE");                
                context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, "ALTER DATABASE [" + context.Database.Connection.Database + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE");                
                _initializer.InitializeDatabase(context);
            }
        }

    }


}

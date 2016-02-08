using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;

using System.Data.Entity;

namespace ClassLibrary.Db
{
    public class OneDbContext : DbContext
    {


        public DbSet<mCategory> dbCategory { get; set; }
        public DbSet<mDataBlock> dbDataBlock { get; set; }
        public DbSet<mKey> dbKey { get; set; }
        public DbSet<mSymbolTableItem> dbSymbolTableItem { get; set; }



        public OneDbContext(bool isNew): base(OneConnectionString(""))
        {

            //Database.SetInitializer<OneDbContext>(new DropCreateDatabaseIfModelChanges<OneDbContext>());

            if (isNew)
                Database.SetInitializer<OneDbContext>(new DropCreateDatabaseAlways<OneDbContext>());
                //Database.SetInitializer<OneDbContext>(new ForceDeleteInitializer(new DropCreateDatabaseAlways<OneDbContext>()));
            else
                Database.SetInitializer<OneDbContext>(new DropCreateDatabaseIfModelChanges<OneDbContext>());

            this.Configuration.LazyLoadingEnabled = false; 
            
            
            
        }


        public OneDbContext(string filename) : base(OneConnectionString(filename))
        {

            Database.SetInitializer<OneDbContext>(new DropCreateDatabaseIfModelChanges<OneDbContext>());
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
            //modelBuilder.Entity<mDataBlock>().HasKey(e => e.CategoryId);
            //modelBuilder.Entity<mCategory>().HasOptional(c => c.DB).WithRequired(e => e.mCategory);
            modelBuilder.Entity<mDataBlock>().HasOptional<mCategory>(db => db.mCategory).WithOptionalDependent(cat => cat.DB);//.Map(p => p.MapKey("CategoryId"));

            //modelBuilder.Entity<mKey>().HasRequired<mCategory>(c => c.mCategory).WithMany(c => c.Keys).HasForeignKey(c => c.KeyId);
            modelBuilder.Entity<mCategory>().HasMany<mKey>(k => k.Keys).WithRequired(k => k.mCategory).HasForeignKey(k => k.KeyId);

            modelBuilder.Entity<mSymbolTableItem>().HasMany<mBaseEntity>(e => e.peripheryCode);

            //modelBuilder.Entity<mSymbolTableItem>().HasRequired<mCategory>(c => c.mCategory).WithMany(c => c.S7items).HasForeignKey(c => c.itemId);
            modelBuilder.Entity<mCategory>().HasMany<mSymbolTableItem>(i => i.S7items).WithRequired(i => i.mCategory).HasForeignKey(i => i.itemId);
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

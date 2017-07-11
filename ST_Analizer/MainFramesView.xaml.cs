using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Windows;
using ClassLibrary.Database;
using ClassLibrary.Models;


namespace OneClickUI
{
    /// <summary>
    /// Логика взаимодействия для pageFrame.xaml
    /// </summary>
    public partial class MainFramesView
    {
        private FrameDbView _windowDbView;
        public ObservableCollection<CategoryModel> CollCategories;

        public OneDbContext Context;
        private readonly bool _isNew;

        public string Rootdir;
       
        public MainFramesView(string mdfFilename)
        {
            InitializeComponent();
            Context = new OneDbContext(mdfFilename);
            
            try
            {
                Context.dbCategory.Include(c => c.Keys).Include(c => c.S7Items).Include(c => c.Db).Load();
                Context.dbSymbolTableItem.Include(i => i.PeripheryCode).Load();
                var n = new SymbolTableItemModel();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database load error: " + ex.Message.ToString());
            }

            _windowDbView = new FrameDbView(Context.dbCategory.Local);
            this.mainFrame.NavigationService.Navigate(_windowDbView);
        }


        public MainFramesView(IEnumerable<CategoryModel> newCollection)
        {
            InitializeComponent();

            //collCategories = new ObservableCollection<CategoryModel>();

            _isNew = true;
            Context = new OneDbContext(_isNew);
             
            Context.dbCategory.AddRange(newCollection);
            Context.SaveChanges();

            Context.dbCategory.Load();

            _windowDbView = new FrameDbView(Context.dbCategory.Local);
            this.mainFrame.NavigationService.Navigate(_windowDbView);

        }

        private void BtnDbCreateClick(object sender, RoutedEventArgs e)
        {
           
           //context.Database.Delete();
           //context.dbCategory.Load();
           //context.dbCategory.Include(c => c.DB);
           //collCategories = new ObservableCollection<CategoryModel>(context.dbCategory.Include(c => c.Keys).Include(c=>c.S7items));
            


            //foreach (CategoryModel mc in context.dbCategory)
            //{
            //    collCategories.Add(mc);
            //}

           Context.dbCategory.Include(c => c.Keys).Include(c => c.S7Items).Load();

           //ObservableCollection<DataBlockModel> dblist = new ObservableCollection<DataBlockModel>(context.dbDataBlock.ToList());

           _windowDbView = new FrameDbView(Context.dbCategory.Local);
           this.mainFrame.NavigationService.Navigate(_windowDbView);
        }

        private void BtnDbAddClick(object sender, RoutedEventArgs e)
        {

           foreach (var c in GenDefaultCategories())
           {
               Context.dbCategory.Add(c);
           }
      
        }

        private void BtnDbSaveClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {             
                MessageBox.Show("Database update error with message: "+ ex.Message.ToString(), "Database Error");
            }
            catch (DbEntityValidationException ex)
            {
                MessageBox.Show("Database update error with message: " + ex.Message.ToString(), "Database Error");
            }
            catch (ObjectDisposedException ex)
            {
                MessageBox.Show("Database update error with message: " + ex.Message.ToString(), "Database Error");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database update error with message: " + ex.Message.ToString(), "Database Error");
            }
            
        }

        private void BtnDbDeleteClick(object sender, RoutedEventArgs e)
        {
            Context.dbCategory.RemoveRange(Context.dbCategory);
            Context.SaveChanges();
        }


        private void BtnDbCloseClick(object sender, RoutedEventArgs e)
        {
            Context.Dispose();
            _windowDbView.Flush();

            this.mainFrame.NavigationService.Refresh();
        }

        private IEnumerable<CategoryModel> GenDefaultCategories()
        {
            var coll = new ObservableCollection<CategoryModel>();
            
            var cat = new CategoryModel();
            cat.Description = "Датчики 4..20";
            cat.Db.UdtNumber = "1";
            cat.Db.Symbol = "A";
            cat.Db.UdtName = "SNS_UDT";
            cat.FCname = "periphery_SNS";
            cat.Db.ArrayName = "SNS";
            cat.Keys.Add(new KeyModel { Name = "IW" });

            cat.S7Items = new ObservableCollection<SymbolTableItemModel>(GenRandomItemsList(cat.Db.Symbol));
            coll.Add(cat);

            cat = new CategoryModel();
            cat.Description = "Датчики уровня";
            cat.Db.UdtNumber = "3";
            cat.Db.Symbol = "D";
            cat.Db.SymbolName = "11D";
            cat.Db.FullName = "DB112";
            cat.Db.ArrayName = "SNL";
            cat.Db.UdtName = "SNL_UDT";
            cat.FCname = "periphery_SNL";
            cat.Keys.Add(new KeyModel { Name = "уровень" });

            cat.S7Items = new ObservableCollection<SymbolTableItemModel>(GenRandomItemsList(cat.Db.Symbol));
            coll.Add(cat);


            cat = new CategoryModel();
            cat.Description = "Дискретные сигналы";
            cat.Db.UdtNumber = "2";
            cat.Db.Symbol = "B";
            cat.Db.ArrayName = "SNB";
            cat.Db.UdtName = "SNB_UDT";
            cat.FCname = "periphery_SNB";
            cat.Keys.Add(new KeyModel { Name = "атчик" });
            cat.Keys.Add(new KeyModel { Name = "фланш-панел" });
            cat.Keys.Add(new KeyModel { Name = "Датчик ФП" });
            cat.Keys.Add(new KeyModel { Name = "калача" });
            cat.Keys.Add(new KeyModel { Name = "Соединение" });

            cat.S7Items = new ObservableCollection<SymbolTableItemModel>(GenRandomItemsList(cat.Db.Symbol));
            coll.Add(cat);
          

            //cat = new cCategory(9, "C", "Счетчики");
            //cat.DB.UDT_Number = "9";
            //cat.DB.Symbol = "SNC";
            //cat.DB.ArrayName = "SNC";
            //cat.DB.UDT_Name = "SNC_UDT";
            //cat.FCname = "periphery_SNC";
            //cat.Keys.Add("Импульсный выход");
            //cat.Keys.Add("Счетчик");
            //categories.Add(cat);

            //cat = new cCategory();
            //cat.Description = "ПИД-регуляторы";
            //cat.DB.UDT_Number = "1";
            //cat.DB.Symbol = "PID";
            //cat.DB.ArrayName = "PID";
            //cat.DB.UDT_Name = "PID";
            //cat.FCname = "periphery_PID";
            //cat.Keys.Add("Регулятор");
            //cat.Keys.Add("Позиционер");
            //categories.Add(cat);
        



            
            //cat = new cCategory(4, "Y", "Клапаны");
            //cat.DB.UDT_Number = "4";
            //cat.DB.Symbol = "Y";
            //cat.DB.ArrayName = "DRV";
            //cat.FCname = "periphery_VLV";
            //cat.DB.UDT_Name = "DRV_UDT";
            //cat.Keys.Add("клапан");
            //cat.Keys.Add("мембр");
            //cat.Keys.Add("невмоцилиндр");
            ////cat.Keys.Add("озатор");
            //categories.Add(cat);

            
            //cat = new cCategory(7, "MIX", "Мешалки СИ");
            //cat.DB.UDT_Number = "7";
            //cat.DB.Symbol = "MIX";
            //cat.DB.ArrayName = "MIX";
            //cat.DB.UDT_Name = "MIXER_CM_UDT";
            //cat.FCname = "periphery_MIXER_CM";
            //cat.Keys.Add("мешалка СИ");
            //cat.Keys.Add("сыроизготовит");
            //cat.Keys.Add("тормоз");
            ////cat.Keys.Add("СИ");
            //categories.Add(cat);

            
            //cat = new cCategory(99, "SC", "Задание скорости");
            //cat.DB.UDT_Number = "1";
            //cat.DB.Symbol = "SC";
            //cat.DB.ArrayName = "SC";
            //cat.DB.UDT_Name = "SC";
            //cat.FCname = "periphery_SC";
            //cat.Keys.Add("Задание скорости");
            //categories.Add(cat);

            //cat = new cCategory(5, "M", "Насосы");
            //cat.DB.UDT_Number = "5";
            //cat.DB.Symbol = "M";
            //cat.DB.ArrayName = "DRV";
            //cat.FCname = "periphery_PMP";
            //cat.DB.UDT_Name = "DRV_UDT";
            //cat.Keys.Add("асос");
            //cat.Keys.Add("пускател");
            //cat.Keys.Add("ПЧ");
            //cat.Keys.Add("озатор");
            //cat.Keys.Add("вибросито");
            //cat.Keys.Add("мешалк");
            //categories.Add(cat);

            //cat = new cCategory(6, "mxr", "Мешалки 2х скоростные");
            //cat.DB.UDT_Number = "5";
            //cat.DB.Symbol = "MXR";
            //cat.DB.ArrayName = "DRV";
            //cat.DB.UDT_Name = "DRV_UDT";
            //cat.FCname = "periphery_MIXER_2S";
            //cat.Keys.Add("мешалк");
            //categories.Add(cat);

            return coll;
        }


        private static List<SymbolTableItemModel> GenRandomItemsList(string s)
        {
            var list = new List<SymbolTableItemModel>();
            
            for (var i = 0; i < 10; i++ )
            {

                var item = new SymbolTableItemModel();

                item.Name = "Name " + s + i.ToString();
                item.SignalName = "SignalName " + s + i.ToString();
                item.Codename = "Codename " + s + i.ToString();
                item.Description = "Description " + s + i.ToString();
                

                list.Add(item);
            }
            return list;
        }


        public ObservableCollection<T> ToObservableCollection<T>(IQueryable<T> enumeration)
        {
            return new ObservableCollection<T>(enumeration);
        }
    }
}

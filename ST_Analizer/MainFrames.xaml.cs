using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.Linq;
using System.Data.Sql;
using System.Data.Entity;
using System.Collections.ObjectModel;

using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

using ClassLibrary;
using ClassLibrary.Db;


namespace OneClick_Analyser
{
    /// <summary>
    /// Логика взаимодействия для pageFrame.xaml
    /// </summary>
    public partial class MainFrames : Window
    {
        private Frame_DB window_DB;

        public ObservableCollection<mCategory> collCategories;

        public OneDbContext context;
        bool isNew;

        public string rootdir;
       
        public MainFrames(string mdf_filename)
        {
            InitializeComponent();
            context = new OneDbContext(mdf_filename);
            

            try
            {
                context.dbCategory.Include(c => c.Keys).Include(c => c.S7items).Include(c => c.DB).Load();
                context.dbSymbolTableItem.Include(i => i.peripheryCode).Load();
                mSymbolTableItem n = new mSymbolTableItem();
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database load error: " + ex.Message.ToString());
            }

            window_DB = new Frame_DB(context.dbCategory.Local);
            this.mainFrame.NavigationService.Navigate(window_DB);
        }


        public MainFrames(ObservableCollection<mCategory> newCollection)
        {
            InitializeComponent();

            //collCategories = new ObservableCollection<mCategory>();

            isNew = true;
            context = new OneDbContext(isNew);
             
            context.dbCategory.AddRange(newCollection);
            context.SaveChanges();

            context.dbCategory.Load();

            window_DB = new Frame_DB(context.dbCategory.Local);
            this.mainFrame.NavigationService.Navigate(window_DB);

        }

        private void btn_DB_Create_Click(object sender, RoutedEventArgs e)
        {
           
           //context.Database.Delete();
           //context.dbCategory.Load();
           //context.dbCategory.Include(c => c.DB);
           //collCategories = new ObservableCollection<mCategory>(context.dbCategory.Include(c => c.Keys).Include(c=>c.S7items));
            


            //foreach (mCategory mc in context.dbCategory)
            //{
            //    collCategories.Add(mc);
            //}

           context.dbCategory.Include(c => c.Keys).Include(c => c.S7items).Load();

           //ObservableCollection<mDataBlock> dblist = new ObservableCollection<mDataBlock>(context.dbDataBlock.ToList());

           window_DB = new Frame_DB(context.dbCategory.Local);
           this.mainFrame.NavigationService.Navigate(window_DB);
        }

        private void btn_DB_Add_Click(object sender, RoutedEventArgs e)
        {

           foreach (mCategory c in genDefaultCategories())
           {
               context.dbCategory.Add(c);
           }
      
        }

        private void btn_DB_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                context.SaveChanges();
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

        private void btn_DB_Delete_Click(object sender, RoutedEventArgs e)
        {
            context.dbCategory.RemoveRange(context.dbCategory);
            context.SaveChanges();
        }


        private void btn_DB_Close_Click(object sender, RoutedEventArgs e)
        {
            context.Dispose();
            window_DB.flush();

            this.mainFrame.NavigationService.Refresh();
        }

        private ObservableCollection<mCategory> genDefaultCategories()
        {
            ObservableCollection<mCategory> coll = new ObservableCollection<mCategory>();
            
            mCategory cat = new mCategory();
            cat.Description = "Датчики 4..20";
            cat.DB.UDT_Number = "1";
            cat.DB.Symbol = "A";
            cat.DB.UDT_Name = "SNS_UDT";
            cat.FCname = "periphery_SNS";
            cat.DB.ArrayName = "SNS";
            cat.Keys.Add(new mKey { Name = "IW" });

            cat.S7items = new ObservableCollection<mSymbolTableItem>(genRandomItemsList(cat.DB.Symbol));
            coll.Add(cat);

            cat = new mCategory();
            cat.Description = "Датчики уровня";
            cat.DB.UDT_Number = "3";
            cat.DB.Symbol = "D";
            cat.DB.SymbolName = "11D";
            cat.DB.FullName = "DB112";
            cat.DB.ArrayName = "SNL";
            cat.DB.UDT_Name = "SNL_UDT";
            cat.FCname = "periphery_SNL";
            cat.Keys.Add(new mKey { Name = "уровень" });

            cat.S7items = new ObservableCollection<mSymbolTableItem>(genRandomItemsList(cat.DB.Symbol));
            coll.Add(cat);


            cat = new mCategory();
            cat.Description = "Дискретные сигналы";
            cat.DB.UDT_Number = "2";
            cat.DB.Symbol = "B";
            cat.DB.ArrayName = "SNB";
            cat.DB.UDT_Name = "SNB_UDT";
            cat.FCname = "periphery_SNB";
            cat.Keys.Add(new mKey { Name = "атчик" });
            cat.Keys.Add(new mKey { Name = "фланш-панел" });
            cat.Keys.Add(new mKey { Name = "Датчик ФП" });
            cat.Keys.Add(new mKey { Name = "калача" });
            cat.Keys.Add(new mKey { Name = "Соединение" });

            cat.S7items = new ObservableCollection<mSymbolTableItem>(genRandomItemsList(cat.DB.Symbol));
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


        private List<mSymbolTableItem> genRandomItemsList(string s)
        {
            List<mSymbolTableItem> list = new List<mSymbolTableItem>();
            

            for (int i = 0; i < 10; i++ )
            {

                mSymbolTableItem item = new mSymbolTableItem();

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ClassLibrary.Models;
using ClassLibrary.Services;
using OneClickUI.Helpers;
using OneClickUI.Views;

namespace OneClickUI.ViewModels
{
    public class CategoriesViewModel : BaseViewModel
    {
        public ObservableRangeCollection<CategoryModel> Categories { get; set; }
        private CategoryModel selectedCategory;
        public CategoryModel SelectedCategory
        {
            get { return selectedCategory; }
            set { SetProperty(ref selectedCategory, value); }
        }

        private KeyModel selectedKey;
        public KeyModel SelectedKey
        {
            get { return selectedKey; }
            set { SetProperty(ref selectedKey, value); }
        }

        public ICommand SetDefaultCategoriesCommand { get; set; }
        public ICommand AddCategoryCommand { get; set; }
        public ICommand RemoveCategoryCommand { get; set; }
        public ICommand AddKeyCommand { get; set; }
        public ICommand RemoveKeyCommand { get; set; }


        public CategoriesViewModel(ObservableRangeCollection<CategoryModel> categories)
        {
            SetDefaultCategoriesCommand = new RelayCommand(async obj => await ExecuteDefaultCategoriesCommand());
            AddCategoryCommand = new RelayCommand(ExecuteAddCategoryCommand);
            RemoveCategoryCommand = new RelayCommand(ExecuteRemoveCategoryCommand);
            AddKeyCommand = new RelayCommand(ExecuteAddKeyCommand);
            RemoveKeyCommand = new RelayCommand(ExecuteRemoveKeyCommand);
            Categories = categories;
        }

        private void ExecuteRemoveCategoryCommand(object obj)
        {

            if (SelectedCategory != null)
               Categories.Remove(SelectedCategory);

        }

        private void ExecuteAddCategoryCommand(object obj)
        {
            var newCategory = new CategoryModel();
            var view = new NewCategoryView(newCategory);
            var showDialog = view.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                Categories.Add(newCategory);
            }
        }

        private void ExecuteAddKeyCommand(object obj)
        {
            var newKey = new KeyModel();
            var view = new NewCategoryKeyView(newKey);
            var showDialog = view.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                if (SelectedCategory != null)
                    SelectedCategory.Keys.Add(newKey);
            }
        }

        private void ExecuteRemoveKeyCommand(object obj)
        {

            if (SelectedKey != null && SelectedCategory != null)
                SelectedCategory.Keys.Remove(SelectedKey);
        }

        private async Task ExecuteDefaultCategoriesCommand()
        {
            var defaultList = await OneService.GenerateDefaultCategoriesAsync();

            if (Categories == null)
                Categories = new ObservableRangeCollection<CategoryModel>(defaultList);
            else
            {
                Categories.ReplaceRange(defaultList);

            }
        }
    }
}

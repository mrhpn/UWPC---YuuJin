using DataAccessLibrary;
using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using YuuJin.Database;

namespace YuuJin.Views
{
    public sealed partial class VocabularyPage : Page, INotifyPropertyChanged
    {
        public VocabularyPage()
        {
            InitializeComponent();

            ComboBox_Level.SelectedIndex = 1;
            ComboBox_Unit.SelectedIndex = 2;

            loadVocabularies("2.3");
        }

        public void loadVocabularies(string unit)
        {
            List<Vocabulary> vocabularies = getVocabularies(unit);
            DataGrid_Vocabulary.ItemsSource = vocabularies;
            TextBlock_TotalRows.Text = vocabularies.Count.ToString();
        }

        public List<Vocabulary> getVocabularies(string unit)
        {
            List <Vocabulary> vocabularies = new VocabularyModel().GetVocabularies(unit);
            return vocabularies;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            string keyword = sender.Text.ToLower().Trim();
            var matchedItems = new List<Vocabulary>();

            var list = new ObservableCollection<Vocabulary>();

            string level = ((ComboBoxItem)ComboBox_Level.SelectedItem).Tag.ToString();
            string unit = ((ComboBoxItem)ComboBox_Unit.SelectedItem).Content.ToString();
            var vocabularies = getVocabularies($"{level}.{unit}");

            foreach (Vocabulary vocabulary in vocabularies)
            {
                if (vocabulary.name.ToLower().Contains(keyword)) matchedItems.Add(vocabulary);
                else if (vocabulary.kanji.ToLower().Contains(keyword)) matchedItems.Add(vocabulary);
                else if (vocabulary.meaning.ToLower().Contains(keyword)) matchedItems.Add(vocabulary);
                else if (vocabulary.meaningEn.ToLower().Contains(keyword)) matchedItems.Add(vocabulary);
            }

            var bindingList = new BindingList<Vocabulary>(matchedItems);
            DataGrid_Vocabulary.ItemsSource = bindingList;
            TextBlock_TotalRows.Text = bindingList.Count.ToString();
        }

        private void Add_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            
        }

        private void SelectionChanged_Level(object sender, SelectionChangedEventArgs e)
        {
            ComboBox_Unit.Items.Clear();

            string selectedId = ((ComboBoxItem)e.AddedItems[0]).Tag.ToString();

            if (selectedId == "2")
            {
                for (int i = 1; i <= 13; i++)
                {
                    ComboBox_Unit.Items.Add(new ComboBoxItem { Tag = i, Content = i.ToString() });
                }
            }
            else if (selectedId == "3")
            {
                for (int i = 1; i <= 12; i++)
                {
                    ComboBox_Unit.Items.Add(new ComboBoxItem { Tag = i, Content = i.ToString() });
                }
            }
            else if (selectedId == "4")
            {
                for (int i = 26; i <= 50; i++)
                {
                    ComboBox_Unit.Items.Add(new ComboBoxItem { Tag = i, Content = i.ToString() });
                }
            }
            else if (selectedId == "5")
            {
                for (int i = 1; i <= 25; i++)
                {
                    ComboBox_Unit.Items.Add(new ComboBoxItem { Tag = i, Content = i.ToString() });
                }
            }
        }

        private void SelectionChanged_Unit(object sender, SelectionChangedEventArgs e)
        {
            string level = ((ComboBoxItem)ComboBox_Level.SelectedItem).Tag.ToString();

            if (((ComboBoxItem)ComboBox_Unit.SelectedItem) != null)
            {
                string unit = ((ComboBoxItem)ComboBox_Unit.SelectedItem).Content.ToString();
                loadVocabularies($"{level}.{unit}");
            }
            else
            {
                ComboBox_Unit.SelectedIndex = 0;
                loadVocabularies($"{level}.1");
            }

            // set 'Mark as Favorite' button to default
            AppBarButton_MarkFavorite.Label = "Mark as Favorite";
            AppBarButton_MarkFavorite.Icon = new SymbolIcon(Symbol.OutlineStar);
        }

        private void Button_MarkFavorite(object sender, RoutedEventArgs e)
        {
            Vocabulary selectedRow = (Vocabulary)DataGrid_Vocabulary.SelectedItem;

            if (selectedRow == null)
            {
                Noti_Info.Show(2000);
            }
            else
            {
                bool isFavorite = selectedRow.isFavorite;
                int vocabularyId = selectedRow.vocabularyId;
                var vocabularyModel = new VocabularyModel();
                int updated = vocabularyModel.ToggleFavorite(!isFavorite, vocabularyId);

                if (updated > 0)
                {
                    AppBarButton_MarkFavorite.Label = "Mark as Favorite";
                    AppBarButton_MarkFavorite.Icon = new SymbolIcon(Symbol.OutlineStar);

                    string level = ((ComboBoxItem)ComboBox_Level.SelectedItem).Tag.ToString();

                    if (((ComboBoxItem)ComboBox_Unit.SelectedItem) != null)
                    {
                        string unit = ((ComboBoxItem)ComboBox_Unit.SelectedItem).Content.ToString();
                        loadVocabularies($"{level}.{unit}");
                    }

                    // TODO: select the updated row programmatically using vocabularyId
                }
                else
                {
                    Noti_Error.Show(2000);
                }
            }
        }

        private void SelectionChanged_DataGrid(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                Vocabulary selectedRow = (Vocabulary)e.AddedItems[0];
                if (selectedRow.isFavorite)
                {
                    AppBarButton_MarkFavorite.Label = "Unmark as Favorite";
                    AppBarButton_MarkFavorite.Icon = new SymbolIcon(Symbol.Favorite);
                }
                else
                {
                    AppBarButton_MarkFavorite.Label = "Mark as Favorite";
                    AppBarButton_MarkFavorite.Icon = new SymbolIcon(Symbol.OutlineStar);
                }
            }
        }

        private async void Button_Edit(object sender, RoutedEventArgs e)
        {
            Vocabulary selectedRow = (Vocabulary)DataGrid_Vocabulary.SelectedItem;

            if (selectedRow == null)
            {
                Noti_Info.Show(2000);
            }
            else
            {
                TextBox_Name.Text = "";
                TextBox_Kanji.Text = "";
                TextBox_Meaning.Text = "";
                TextBox_Meaning_En.Text = "";
                CheckBox_Favorite.IsChecked = false;

                TextBox_Name.Text = selectedRow.name;
                TextBox_Kanji.Text = selectedRow.kanji;
                TextBox_Meaning.Text = selectedRow.meaning;
                TextBox_Meaning_En.Text = selectedRow.meaningEn;
                CheckBox_Favorite.IsChecked = selectedRow.isFavorite;

                // updating vocabulary
                ContentDialogResult result = await ContentDialog_UpdateVocabulary.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    string name = TextBox_Name.Text;
                    string kanji = TextBox_Kanji.Text;
                    string meaning = TextBox_Meaning.Text;
                    string meaningEn = TextBox_Meaning_En.Text;
                    bool isFavorite = (bool) CheckBox_Favorite.IsChecked;
                    Vocabulary updatedVocabulary = new Vocabulary(name, kanji, meaning, meaningEn, isFavorite);

                    var updated = new VocabularyModel().UpdateVocabulary(selectedRow.vocabularyId, updatedVocabulary);

                    if (updated > 0)
                    {
                        Noti_Success.Show(2000);

                        // refresh the datagrid
                        string level = ((ComboBoxItem)ComboBox_Level.SelectedItem).Tag.ToString();
                        if (((ComboBoxItem)ComboBox_Unit.SelectedItem) != null)
                        {
                            string unit = ((ComboBoxItem)ComboBox_Unit.SelectedItem).Content.ToString();
                            loadVocabularies($"{level}.{unit}");
                        }

                        // TODO: select the updated row programmatically using vocabularyId
                    }
                    else
                    {
                        Noti_Error.Show(2000);
                    }
                }
            }
        }

        private async void Button_Delete(object sender, RoutedEventArgs e)
        {
            Vocabulary selectedRow = (Vocabulary)DataGrid_Vocabulary.SelectedItem;

            if (selectedRow == null)
            {
                Noti_Info.Show(2000);
            }
            else
            {
                ContentDialogResult result = await ContentDialog_DeleteVocabulary.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    var deleted = new VocabularyModel().DeleteVocabulary(selectedRow.vocabularyId);

                    if (deleted > 0)
                    {
                        Noti_Success.Show(2000);

                        // refresh the datagrid
                        string level = ((ComboBoxItem)ComboBox_Level.SelectedItem).Tag.ToString();
                        if (((ComboBoxItem)ComboBox_Unit.SelectedItem) != null)
                        {
                            string unit = ((ComboBoxItem)ComboBox_Unit.SelectedItem).Content.ToString();
                            loadVocabularies($"{level}.{unit}");
                        }
                    }
                    else
                    {
                        Noti_Error.Show(2000);
                    }
                }
            }
        }
    }
}

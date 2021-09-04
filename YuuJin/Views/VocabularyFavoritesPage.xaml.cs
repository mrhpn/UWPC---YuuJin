﻿using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using YuuJin.Database;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace YuuJin.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VocabularyFavoritesPage : Page
    {
        public VocabularyFavoritesPage()
        {
            this.InitializeComponent();

            ComboBox_Level.SelectedIndex = 1;
            ComboBox_Unit.SelectedIndex = 2;

            loadVocabularies("2.3");
        }

        public void loadVocabularies(string unit)
        {
            List<Vocabulary> vocabularies = getFavoriteVocabularies(unit);
            DataGrid_Vocabulary.ItemsSource = vocabularies;
            TextBlock_TotalRows.Text = vocabularies.Count.ToString();
        }

        public List<Vocabulary> getFavoriteVocabularies(string unit)
        {
            List<Vocabulary> vocabularies = new VocabularyModel().GetFavoriteVocabularies(unit);
            return vocabularies;
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            string keyword = sender.Text.ToLower().Trim();
            var matchedItems = new List<Vocabulary>();

            var list = new ObservableCollection<Vocabulary>();

            string level = ((ComboBoxItem)ComboBox_Level.SelectedItem).Tag.ToString();
            string unit = ((ComboBoxItem)ComboBox_Unit.SelectedItem).Content.ToString();
            var vocabularies = getFavoriteVocabularies($"{level}.{unit}");

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

        private void SelectionChanged_Level(object sender, SelectionChangedEventArgs e)
        {
            ComboBox_Unit.Items.Clear();

            string selectedId = ((ComboBoxItem)e.AddedItems[0]).Tag.ToString();

            if (selectedId == "2")
            {
                for (int i = 1; i <= 13; i++)
                {
                    ComboBox_Unit.IsEnabled = true;
                    ComboBox_Unit.Items.Add(new ComboBoxItem { Tag = i, Content = i.ToString() });
                }
            }
            else if (selectedId == "3")
            {
                for (int i = 1; i <= 12; i++)
                {
                    ComboBox_Unit.IsEnabled = true;
                    ComboBox_Unit.Items.Add(new ComboBoxItem { Tag = i, Content = i.ToString() });
                }
            }
            else if (selectedId == "4")
            {
                for (int i = 26; i <= 50; i++)
                {
                    ComboBox_Unit.IsEnabled = true;
                    ComboBox_Unit.Items.Add(new ComboBoxItem { Tag = i, Content = i.ToString() });
                }
            }
            else if (selectedId == "5")
            {
                for (int i = 1; i <= 25; i++)
                {
                    ComboBox_Unit.IsEnabled = true;
                    ComboBox_Unit.Items.Add(new ComboBoxItem { Tag = i, Content = i.ToString() });
                }
            }
            else if (selectedId == "6")
            {
                ComboBox_Unit.IsEnabled = false;
                loadVocabularies("0");
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
        }

        private async void Button_Add(object sender, RoutedEventArgs e)
        {
            Add_TextBox_Name.Text = "";
            Add_TextBox_Kanji.Text = "";
            Add_TextBox_Meaning.Text = "";
            Add_TextBox_Meaning_En.Text = "";
            Add_CheckBox_Favorite.IsChecked = true;

            string level = ((ComboBoxItem)ComboBox_Level.SelectedItem).Tag.ToString();
            string unit = "";
            if (level == "6")
            {
                unit = "0";
                Add_TextBox_Description.Text = "You are adding a new favorite vocabulary to Free.";
            }
            else
            {
                if (((ComboBoxItem)ComboBox_Unit.SelectedItem) != null)
                {
                    unit = ((ComboBoxItem)ComboBox_Unit.SelectedItem).Content.ToString();
                    Add_TextBox_Description.Text = $"You are adding a new favorite vocabulary to level {level}, unit {unit}.";
                }
            }

            ContentDialogResult result = await ContentDialog_AddVocabulary.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                string name = Add_TextBox_Name.Text;
                string kanji = Add_TextBox_Kanji.Text;
                string meaning = Add_TextBox_Meaning.Text;
                string meaningEn = Add_TextBox_Meaning_En.Text;
                bool isFavorite = true;

                Vocabulary newVocabulary = new Vocabulary(name, kanji, meaning, meaningEn, isFavorite, $"{level}.{unit}");
                var added = new VocabularyModel().InsertVocabulary(newVocabulary);

                if (added > 0)
                {
                    Noti_Success.Show(2000);

                    // refresh the datagrid
                    loadVocabularies($"{level}.{unit}");

                    // TODO: select the added row programmatically using vocabularyId
                }
                else
                {
                    Noti_Error.Show(2000);
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
                    bool isFavorite = (bool)CheckBox_Favorite.IsChecked;
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

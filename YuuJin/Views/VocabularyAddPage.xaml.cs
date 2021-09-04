using CsvHelper;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using YuuJin.Database;
using YuuJin.Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace YuuJin.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VocabularyAddPage : Page
    {
        public VocabularyAddPage()
        {
            this.InitializeComponent();
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
        }

        private void SelectionChanged_Unit(object sender, SelectionChangedEventArgs e)
        {
            string level = ((ComboBoxItem)ComboBox_Level.SelectedItem).Tag.ToString();

            if (((ComboBoxItem)ComboBox_Unit.SelectedItem) != null)
            {
                string unit = ((ComboBoxItem)ComboBox_Unit.SelectedItem).Content.ToString();
            }
            else
            {
                ComboBox_Unit.SelectedIndex = 0;
            }

            Button__Check.IsEnabled = true;
        }

        private void Button_Check(object sender, RoutedEventArgs e)
        {
            string level = ((ComboBoxItem)ComboBox_Level.SelectedItem).Tag.ToString();
            string unit = ((ComboBoxItem)ComboBox_Unit.SelectedItem).Content.ToString();
            string checkUnit = $"{level}.{unit}";

            int count = new VocabularyModel().getCountOfVocabulary(checkUnit);
            TextBlock_TotalVocabularyCount.Text = $"Total vocabulary count in Level {level}, Unit {unit} is {count}.";
        }

        private async void Button_ImportExcel(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".csv");

            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                try
                {
                    using (var stream = await file.OpenStreamForReadAsync())
                    using (var reader = new StreamReader(stream, Encoding.UTF8))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        var records = csv.GetRecords<VocabularyExcel>();

                        foreach (var row in records)
                        {
                            var vocabularyModel = new VocabularyModel();
                            vocabularyModel.InsertVocabularyExcel(row);
                        }
                    }
                }
                catch (Exception exc)
                {
                    Debug.WriteLine($"Exception {exc.Message}");
                }
            }
        }
    }
}

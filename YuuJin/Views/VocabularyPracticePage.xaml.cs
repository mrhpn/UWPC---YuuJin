using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using YuuJin.Database;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace YuuJin.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VocabularyPracticePage : Page
    {
        public VocabularyPracticePage()
        {
            this.InitializeComponent();

            ComboBox_PracticeType.SelectedIndex = 0;
            ComboBox_Level.SelectedIndex = 0;
            ComboBox_WantToSee.SelectedIndex = 0;
        }

        private void SelectionChanged_DataGrid(object sender, SelectionChangedEventArgs e)
        {
            Vocabulary selectedRow = (Vocabulary)DataGrid_Vocabulary.SelectedItem;

            if (selectedRow != null)
            {
                Panel_AnswerBox.Visibility = Visibility.Visible;
                TextBlock_Vocabulary.Text = selectedRow.name;
                TextBlock_Kanji.Text = selectedRow.kanji;
                TextBlock_Meaning.Text = selectedRow.meaning;
                TextBlock_MeaningEn.Text = selectedRow.meaningEn;
            }
        }

        private void SelectionChanged_UnitFrom(object sender, SelectionChangedEventArgs e)
        {
            string level = ((ComboBoxItem)ComboBox_Level.SelectedItem).Tag.ToString(); // 5
            if (((ComboBoxItem)ComboBox_Unit.SelectedItem) != null)
            {
                string unitFrom = ((ComboBoxItem)e.AddedItems[0]).Tag.ToString();
                int startUnit = Convert.ToInt32(unitFrom);

                if (level == "5")
                {
                    ComboBox_UnitTo.Items.Clear();
                    for (int i = startUnit; i <= 25; i++)
                    {
                        ComboBox_UnitTo.IsEnabled = true;
                        ComboBox_UnitTo.Items.Add(new ComboBoxItem { Tag = i, Content = i.ToString() });
                        Button__Start.IsEnabled = false;
                    }
                }
                else if (level == "2")
                {
                    ComboBox_UnitTo.Items.Clear();
                    for (int i = startUnit; i <= 13; i++)
                    {
                        ComboBox_UnitTo.IsEnabled = true;
                        ComboBox_UnitTo.Items.Add(new ComboBoxItem { Tag = i, Content = i.ToString() });
                        Button__Start.IsEnabled = false;
                    }
                }
                else if (level == "3")
                {
                    ComboBox_UnitTo.Items.Clear();
                    for (int i = startUnit; i <= 12; i++)
                    {
                        ComboBox_UnitTo.IsEnabled = true;
                        ComboBox_UnitTo.Items.Add(new ComboBoxItem { Tag = i, Content = i.ToString() });
                        Button__Start.IsEnabled = false;
                    }
                }
                else if (level == "4")
                {
                    ComboBox_UnitTo.Items.Clear();
                    for (int i = startUnit; i <= 50; i++)
                    {
                        ComboBox_UnitTo.IsEnabled = true;
                        ComboBox_UnitTo.Items.Add(new ComboBoxItem { Tag = i, Content = i.ToString() });
                        Button__Start.IsEnabled = false;
                    }
                }
            }
        }

        private void SelectionChanged_Level(object sender, SelectionChangedEventArgs e)
        {
            ComboBox_Unit.Items.Clear();
            ComboBox_UnitTo.IsEnabled = false;
            Button__Start.IsEnabled = false;

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
                ComboBox_UnitTo.IsEnabled = false;
                // loadVocabularies("0");
            }
        }

        private void Button_Start(object sender, RoutedEventArgs e)
        {
            // only favorites -> is_favorite=1
            // without favorites -> is_favorite=0
            string practiceMode = ((ComboBoxItem)ComboBox_PracticeType.SelectedItem).Tag.ToString(); 
            string level = ((ComboBoxItem)ComboBox_Level.SelectedItem).Tag.ToString(); // 5
            string unitFrom = ((ComboBoxItem)ComboBox_Unit.SelectedItem).Tag.ToString(); // 5
            string unitTo = ((ComboBoxItem)ComboBox_UnitTo.SelectedItem).Tag.ToString(); // 10
            string wantToSee = ((ComboBoxItem)ComboBox_WantToSee.SelectedItem).Tag.ToString(); // 1,2

            // load vocas
            string _unitFrom = $"{level}{unitFrom}";
            string _unitTo = $"{level}{unitTo}";

            List<Vocabulary> vocabularies = null;
            bool isFavorite = false;
            if (practiceMode == "1")
            {
                vocabularies = new VocabularyModel().getPracticeVocabulary(_unitFrom, _unitTo);
            }
            else if (practiceMode == "2")
            {
                vocabularies = new VocabularyModel().getPracticeVocabularyFavorite(_unitFrom, _unitTo, true);
            }
            else if (practiceMode == "3")
            {
                vocabularies = new VocabularyModel().getPracticeVocabularyFavorite(_unitFrom, _unitTo, false);
            }

            DataGrid_Vocabulary.ItemsSource = vocabularies;
            TextBlock_TotalRows.Text = vocabularies.Count.ToString();

            // hide columns
            // 1 -> Meaning, MeaningEn
            // 2 -> Name
            if (wantToSee == "1")
            {
                Col_Meaning.Visibility = Visibility.Collapsed;
                Col_MeaningEn.Visibility = Visibility.Collapsed;
                Col_Name.Visibility = Visibility.Visible;
                Col_Kanji.Visibility = Visibility.Visible;
            }
            else
            {
                Col_Name.Visibility = Visibility.Collapsed;
                Col_Kanji.Visibility = Visibility.Collapsed;
                Col_Meaning.Visibility = Visibility.Visible;
                Col_MeaningEn.Visibility = Visibility.Visible;
            }
        }

        private void SelectionChanged_UnitTo(object sender, SelectionChangedEventArgs e)
        {
            Button__Start.IsEnabled = true;
        }
    }
}

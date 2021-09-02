using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace DataAccessLibrary.Models
{
    public class Vocabulary
    {
        public int displayNo { get; set; }
        public int vocabularyId { get; set; }
        public string name { get; set; }
        public string kanji { get; set; }
        public string meaning { get; set; }
        public string meaningEn { get; set; }
        public bool isFavorite { get; set; }
        public int unitId { get; set; }

        public Visibility FavoriteIconVisibility
        {
            get { return isFavorite == true ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Vocabulary(int displayNo, int vocabularyId, string name, string kanji, string meaning, string meaningEn, bool isFavorite, int unit)
        {
            this.displayNo = displayNo;
            this.vocabularyId = vocabularyId;
            this.name = name;
            this.kanji = kanji;
            this.meaning = meaning;
            this.meaningEn = meaningEn;
            this.isFavorite = isFavorite;
            this.unitId = unitId;
        }

        public Vocabulary(int displayNo, int vocabularyId, string name, string kanji, string meaning, string meaningEn, bool isFavorite)
        {
            this.displayNo = displayNo;
            this.vocabularyId = vocabularyId;
            this.name = name;
            this.kanji = kanji;
            this.meaning = meaning;
            this.meaningEn = meaningEn;
            this.isFavorite = isFavorite;
        }

        public Vocabulary(string name, string kanji, string meaning, string meaningEn, bool isFavorite)
        {
            this.name = name;
            this.kanji = kanji;
            this.meaning = meaning;
            this.meaningEn = meaningEn;
            this.isFavorite = isFavorite;
        }
    }
}

using DataAccessLibrary.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace YuuJin.Database
{
    class VocabularyModel
    {

        public List<Vocabulary> GetVocabularies(string unit)
        {
            var entries = new List<Vocabulary>();

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "yuuJin.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand($"SELECT * FROM vocabularies WHERE unit_id = {unit}", db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                int displayNo = 1;
                while (query.Read())
                {
                    entries.Add(new Vocabulary(displayNo, query.GetInt32(0), query.GetString(1), query.GetString(2), query.GetString(3), query.GetString(4), query.GetBoolean(5)));
                    displayNo++;
                }

                db.Close();
            }

            return entries;
        }

        public void ToggleFavorite(bool isFavorite, int vocabularyId)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "yuuJin.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand($"UPDATE vocabularies SET is_favorite = {isFavorite} WHERE vocabulary_id = {vocabularyId}", db);

                selectCommand.ExecuteNonQuery();

                db.Close();
            }
        }
    }
}

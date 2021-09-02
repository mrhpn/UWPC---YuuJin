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

        public int ToggleFavorite(bool isFavorite, int vocabularyId)
        {
            int updated = 0;

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "yuuJin.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand($"UPDATE vocabularies SET is_favorite = {isFavorite} WHERE vocabulary_id = {vocabularyId}", db);

                updated = selectCommand.ExecuteNonQuery();

                db.Close();
            }

            return updated;
        }

        public int UpdateVocabulary(int vocabularyId, Vocabulary vocabulary)
        {
            int updated = 0;

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "yuuJin.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand($"UPDATE vocabularies SET name = '{vocabulary.name}', kanji = '{vocabulary.kanji}', meaning = '{vocabulary.kanji}', meaning_en = '{vocabulary.kanji}', is_favorite = {vocabulary.isFavorite} WHERE vocabulary_id = {vocabularyId}", db);

                updated = selectCommand.ExecuteNonQuery();

                db.Close();
            }

            return updated;
        }

        public int DeleteVocabulary(int vocabularyId)
        {
            int deleted = 0;

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "yuuJin.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand($"DELETE FROM vocabularies WHERE vocabulary_id = {vocabularyId}", db);

                deleted = selectCommand.ExecuteNonQuery();

                db.Close();
            }

            return deleted;
        }
    }
}

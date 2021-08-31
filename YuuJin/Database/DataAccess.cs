using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Models;
using Microsoft.Data.Sqlite;
using Windows.Storage;

namespace DataAccessLibrary
{
    public static class DataAccess
    {
        public async static void InitializeDatabase()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync("yuuJin.db", CreationCollisionOption.OpenIfExists);
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "yuuJin.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                String cmd = "CREATE TABLE IF NOT EXISTS vocabularies (" +
                    "vocabulary_id INTEGER PRIMARY KEY," +
                    "name TEXT NOT NULL," +
                    "kanji TEXT," +
                    "meaning TEXT NOT NULL," +
                    "meaning_en TEXT DEFAULT '-'," +
                    "is_favorite INTEGER NOT NULL DEFAULT 0," +
                    "unit_id NUMERIC NOT NULL DEFAULT 0)";

                SqliteCommand createTable = new SqliteCommand(cmd, db);

                createTable.ExecuteReader();
            }
        }
    }
}

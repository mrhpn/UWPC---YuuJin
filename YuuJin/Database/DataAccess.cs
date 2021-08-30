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
        public static SqliteConnection InitializeDatabase()
        {
            string dbpath = Path.Combine(Windows.ApplicationModel.Package.Current.InstalledLocation.Path, "Database", "sqliteYuuJin.db");
            return new SqliteConnection($"Filename={dbpath}");
        }

        public static void AddData(string inputText)
        {
            string dbpath = Path.Combine(Windows.ApplicationModel.Package.Current.InstalledLocation.Path, "Database", "sqliteYuuJin.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO MyTable VALUES (NULL, @Entry);";
                insertCommand.Parameters.AddWithValue("@Entry", inputText);

                insertCommand.ExecuteReader();

                db.Close();
            }

        }
    }
}

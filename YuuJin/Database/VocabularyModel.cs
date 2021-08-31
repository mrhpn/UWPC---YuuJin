using DataAccessLibrary.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YuuJin.Database
{
    class VocabularyModel
    {
        private SqliteConnection connection;

        public VocabularyModel()
        {
            this.connection = DataAccessLibrary.DataAccess.InitializeDatabase();
        }

        public List<Vocabulary> GetVocabularies(string unit)
        {
            var entries = new List<Vocabulary>();

            using (this.connection)
            {
                this.connection.Open();

                SqliteCommand selectCommand = new SqliteCommand($"SELECT * FROM vocabularies WHERE unit_id = {unit}", this.connection);

                SqliteDataReader query = selectCommand.ExecuteReader();

                int displayNo = 1;
                while (query.Read())
                {
                    entries.Add(new Vocabulary(displayNo, query.GetInt32(0), query.GetString(1), query.GetString(2), query.GetString(3), query.GetString(4), query.GetBoolean(5)));
                    displayNo++;
                }

                this.connection.Close();
            }

            return entries;
        }
    }
}

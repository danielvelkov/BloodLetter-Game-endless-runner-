using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace GameDemo
{
    public class Database
    {
        private SQLiteConnection db = null;

        protected static Database database;

        static Database()
        {
            database = new Database();
        }

        protected Database()
        {
            db = new SQLiteConnection(Constants.DbFilePath);

            db.CreateTable<Scores>();
        }
        
        public static int SaveScore(Scores score)
        {
            database.db.Insert(score);
            return score.Id;
        }

        public static Scores GetPerson(int id)

        {
            return database.db.Get<Scores>(p => p.Id == id);
        }

        public static List<Scores> getAllScores()
        {
            List<Scores> scores = new List<Scores>();
            scores = database.db.Query<Scores>("Select * from scores Order by score DESC Limit 5 ");
            return scores;
        }
    }
}
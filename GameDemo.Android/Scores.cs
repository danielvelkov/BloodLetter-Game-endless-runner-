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
    /// <summary>
    /// Each player will have a name and a high score to it
    /// </summary>
    [Table("Scores")]
    public class Scores
    {
            [PrimaryKey, AutoIncrement]
            public int Id { get; set; }

            
            public string Name { get; set; }

            public double score { get; set; }
        
    }

}

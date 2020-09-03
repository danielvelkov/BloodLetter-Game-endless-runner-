

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;

namespace GameDemo
{
    public class Constants
    {

        // the database file path
        public static readonly string DbFilePath =
            Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),  "scores.db"  );

    }
}
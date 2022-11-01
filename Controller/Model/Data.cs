using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
namespace Controller.Model
{
    public class Data
    {
        public DateTime Date { get; set; }
        public string Time { get { return Date.ToString(); } }
        public string Log { get; set; }
        public Data(string log)
        {
            Log = log;
            Date = DateTime.Now;
        }

    }
}

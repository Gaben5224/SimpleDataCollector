using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DataCollector
{
    internal class Collect
    {
        private int _startNumber;
        private int _endNumber;
        private int _totalValue;
        private string _comment;
        private TimeSpan _startTime;
        private TimeSpan _endTime;

        /// <summary>
        /// Collect all the error messeges in a List.
        /// </summary>
        public List<string> Exceptions = new List<string>();
        public string WorkType { private get; set; }
        public string EmployeeName { private get; set; }

        /// <summary>
        /// Cast the start number to int and set it.
        /// </summary>
        public string StartNumber
        {
            set
            {
                if (!int.TryParse(value.ToString(), out this._startNumber))
                    Exceptions.Add("A kezdő érték csak szám formátumú lehet. Kérlek ellenőrizd!");
            }
        }

        public string EndNumber
        {
            set
            {
                if (!int.TryParse(value.ToString(), out this._endNumber))
                    Exceptions.Add("A vég érték csak szám formátumú lehet. Kérlek ellenőrizd!");
            }
        }

        public string TotalValue
        {
            set
            {
                if (!int.TryParse(value.ToString(), out this._totalValue))
                    Exceptions.Add("A teljes mennyiség csak szám formátumú lehet. Kérlek ellenőrizd!");
            }
        }

        public string Comment
        {
            set
            {
                this._comment = value.Trim();
            }
        }

        public string StartTime
        {
            set
            {
                if (!TimeSpan.TryParse(value, out this._startTime))
                    this.Exceptions.Add("A kezdés időpontja nem megfelelő formátumú. Kérlek ellenőrizd!");
            }
        }

        public string EndTime
        {
            set
            {
                if (!TimeSpan.TryParse(value, out this._endTime))
                    this.Exceptions.Add("A befejezés időpontja nem megfelelő formátumú. Kérlek ellenőrizd!");
            }
        }

        public void SaveData()
        {
            if (this._startNumber == 0 && this._endNumber == 0 && this._totalValue == 0)
                Exceptions.Add("Nem adtál meg egyetlen értéket sem. Kérlek ellenőrizd!");

            if (this._startNumber > 0 && this._endNumber == 0)
                Exceptions.Add("Megadtad a kezdő értéket, de nem adtál meg vég értéket. Kérlek ellenőrizd!");

            if (String.IsNullOrWhiteSpace(this.WorkType))
                Exceptions.Add("Nem adtad meg a munka típusát. Kérlek ellenőrizd!");

            if (String.IsNullOrWhiteSpace(this.EmployeeName))
                Exceptions.Add("Nem adtad meg a dolgozó nevét. Kérlek ellenőrizd!");

            if (this._endTime < this._startTime)
                Exceptions.Add("A befejezés időpontja korábbi, mint a kezdés időpontja. Kérlek ellenőrizd!");

            if (this._endNumber < this._startNumber)
                Exceptions.Add("A vég érték nagyobb, mint a kezdő érték. Kérlek ellenőrizd!");

            if (this._endNumber > 0)
                this._totalValue = this._endNumber - this._startNumber;


            if (Exceptions.Any())
                throw new Exception("Hiba!");


            this.WriteToCsv();
        }

        private void WriteToCsv()
        {
            var csvPath = "colectedData.csv";
            var content = String.Format("{0}; {1}; {2}; {3}; {4}; {5}; {6}; {7}; {8}" + Environment.NewLine, DateTime.Now.ToShortDateString(), this.EmployeeName, this.WorkType, this._startNumber, this._endNumber, this._totalValue, this._startTime, this._endTime, this._comment);

            if (!File.Exists(csvPath))
            {
                var header = "Dátum; Dolgozó; Munka típusa; Kezdő érték; Vég érték; Teljes mennyiség; Kezdés ideje; Befejezés ideje; Megjegyzés" + Environment.NewLine;
                File.WriteAllText(csvPath, header, Encoding.UTF8);
            }

            File.AppendAllText(csvPath, content, Encoding.UTF8);
        }


    }
}

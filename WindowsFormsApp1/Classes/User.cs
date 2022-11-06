using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector
{
    internal class User
    {
        private bool _isLogged;
        public List<string> Exceptions = new List<string>();

        public bool IsUserLogged 
        { 
            get { return _isLogged; }
            private set
            {
                _isLogged = Properties.Settings.Default.userIsLogged;
            }
        }

        public void LoginForm(string password)
        {
            if (password == "7GJJrbEd+)eXmD@B")
                Properties.Settings.Default.userIsLogged = true;
            else
                this.Exceptions.Add("Hibás jelszót adtál meg!");

            if (this.Exceptions.Any())
                throw new Exception("Hiba!");
        }
    }
}

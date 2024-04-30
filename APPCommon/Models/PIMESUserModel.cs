using System;
using System.ComponentModel;

namespace APPCommon.Models
{
    [Serializable]
    public class PIMESUserModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _loginmode;

        public string loginmode
        {
            get { return _loginmode; }
            set { _loginmode = value; OnPropertyChanged("loginmode"); }
        }

        private string _username;

        public string username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged("username"); }
        }

        private string _password;

        public string password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged("password"); }
        }

        private string _passwordconfidential;

        public string passwordconfidential
        {
            get { return _passwordconfidential; }
            set { _passwordconfidential = value; OnPropertyChanged("passwordconfidential"); }
        }

        private string _programname;

        public string programname
        {
            get { return _programname; }
            set { _programname = value; OnPropertyChanged("programname"); }
        }

        private string _buname;

        public string buname
        {
            get { return _buname; }
            set { _buname = value; OnPropertyChanged("buname"); }
        }

        private string _role;

        public string role
        {
            get { return _role; }
            set { _role = value; OnPropertyChanged("role"); }
        }
    }
}
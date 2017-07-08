using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;

namespace WpfApp1
{
    public class CheckListItem : INotifyPropertyChanged
    {
        private string _text;
        private bool _checked;
        private Visibility _visibility;

        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                NotifyPropertyChanged("Text");
            }
        }

        public bool Checked
        {
            get
            {
                return _checked;
            }
            set
            {
                _checked = value;
                NotifyPropertyChanged("Checked");
            }
        }

        public Visibility Visibility
        {
            get
            {
                return _visibility;
            }
            set
            {
                _visibility = value;
                NotifyPropertyChanged("Visibility");
            }
        }

        public CheckListItem(string text)
        {
            Text = text;
            Checked = false;
            Visibility = Visibility.Visible;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        override public string ToString()
        {
            return Text;
        }
    }   
}

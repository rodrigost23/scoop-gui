using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ScoopGui.Util
{
    public class ObservableObject<T> : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableObject(T value) {
            Value = value;
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private T objValue;

        public T Value
        {
            get => objValue;
            set
            {
                objValue = value;
                NotifyPropertyChanged("Value");
            }
        }

        public static implicit operator T(ObservableObject<T> obj) => obj.Value;
        public static implicit operator ObservableObject<T>(T value) => new ObservableObject<T>(value);
    }
}

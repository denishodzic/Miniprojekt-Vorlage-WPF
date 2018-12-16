using ch.hsr.wpf.gadgeothek.domain;
using System.Runtime.CompilerServices;

namespace Gadgeothek.WinUI.ViewModels
{

    public class CustomerViewModel : BindableBase
    {
        public delegate void CustomerDataChanged(CustomerViewModel sender, string propertyname);

        public event CustomerDataChanged DataChanged;

        protected override void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.RaisePropertyChanged(propertyName);
            DataChanged?.Invoke(this,propertyName);
        }
        
        public Customer Data { get; }
        public CustomerViewModel(Customer data)
        {
            Data = data;
        }

        public string Name
        {
            get
            {
                return Data.Name;
            }
            set
            {
                Data.Name = value;
                RaisePropertyChanged();
            }
        }

        public string Email
        {
            get
            {
                return Data.Email;
            }
            set
            {
                Data.Email = value;
                RaisePropertyChanged();
            }
        }

        public string Password
        {
            get
            {
                return Data.Password;
            }
            set
            {
                Data.Password = value;
                RaisePropertyChanged();
            }
        }

        public string StudentNumber
        {
            get
            {
                return Data.Studentnumber;
            }
        }


    }
}

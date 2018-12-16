using ch.hsr.wpf.gadgeothek.domain;
using System;

namespace Gadgeothek.WinUI.ViewModels
{
    public class LoanViewModel: BindableBase
    {
        public Loan Data { get; }
        public LoanViewModel(Loan data)
        {
            Data = data;
        }

        public string Id
        {
            get { return Data.Id; }
        }

        private CustomerViewModel _customer;
        public CustomerViewModel Customer
        {
            get
            {
                if(_customer == null)
                {
                    _customer = new CustomerViewModel(Data.Customer ?? new Customer() { Name = "Kunde unbekannt" });
                }
                return _customer;
            }
           
        }

        private GadgetViewModel _gadget;
        public GadgetViewModel Gadget
        {
            get
            {
                if(_gadget == null)
                {
                    _gadget = new GadgetViewModel(Data.Gadget?? new Gadget() { Name= "Gadget unbekannt" });
                }

                return _gadget;
            }
         
        }

        public bool IsLent
        {
            get
            {
                return Data.IsLent;
            }
        }

        public bool IsOverdue
        {
            get
            {
                return Data.IsOverdue;
            }
        }

        public DateTime? OverDueDate
        {
            get
            {
                return Data.OverDueDate;
            }
        }

        public DateTime? PickupDate
        {
            get
            {
                return Data.PickupDate;
            }
            set
            {
                Data.PickupDate = value;
                RaisePropertyChanged();
            }
        }

        public DateTime? ReturnDate
        {
            get
            {
                return Data.ReturnDate;
            }
            set
            {
                Data.ReturnDate = value;
                RaisePropertyChanged();
            }
        }

        public bool WasReturned
        {
            get
            {
                return Data.WasReturned;
            }
        }
    }
}

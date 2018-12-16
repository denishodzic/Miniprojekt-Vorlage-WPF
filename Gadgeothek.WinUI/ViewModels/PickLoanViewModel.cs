using ch.hsr.wpf.gadgeothek.domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Gadgeothek.WinUI.ViewModels
{
    public class PickLoanViewModel : BindableBase
    {
        public string Title { get; } = "Neue Ausleihe";

        private MainWindowViewModel _mainWindowViewModel;


        public PickLoanViewModel(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _gadgets = new ObservableCollection<GadgetViewModel>(mainWindowViewModel.Gadgets);
            _selectedGadget = _gadgets.FirstOrDefault();

            _customers = new ObservableCollection<CustomerViewModel>(mainWindowViewModel.Customers);
            _selectedCustomer = _customers.FirstOrDefault();
        }

        private ICommand _closeDialogCommand;
        public ICommand CancelCommand => _closeDialogCommand ??
            (_closeDialogCommand = new RelayCommand<Window>((pickLoanView) =>
            {
                try
                {
                    //do nothing
                }
                finally
                {
                    pickLoanView.Close();
                }
            }));

        private ICommand _saveGadgetCommand;
        public ICommand SaveCommand => _saveGadgetCommand ??
            (_saveGadgetCommand = new RelayCommand<Window>((pickLoanView) =>
            {
                try
                {
                    var newLoan = new Loan()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Customer = SelectedCustomer.Data,
                        Gadget = SelectedGadget.Data,
                        PickupDate = PickupDate,
                        
                    };

                    if (_mainWindowViewModel.dataService.AddLoan(newLoan))
                    {
                        _mainWindowViewModel.Loans.Add(new LoanViewModel(newLoan));
                    }
                    else
                    {
                        MessageBox.Show("Fehler beim Abspeichern der Ausleihe. Bitte versuchen Sie es nochmals.", "Speichern fehlgeschlagen", MessageBoxButton.OK);
                    }
                }
                finally
                {
                    pickLoanView.Close();
                }
            }));


        private DateTime? _pickupDate = DateTime.Now;
        public DateTime? PickupDate
        {
            get
            {
                return _pickupDate;
            }
            set
            {
                _pickupDate = value;
                RaisePropertyChanged();
            }
        }

        DateTime? _dueDate = DateTime.Now+ TimeSpan.FromDays(7);
        public DateTime? DueDate
        {
            get
            {
                return _dueDate;
            }
            set
            {
                _dueDate = value;
                RaisePropertyChanged();
            }
        }


        private ObservableCollection<GadgetViewModel> _gadgets;
        public ObservableCollection<GadgetViewModel> Gadgets
        {
            get { return _gadgets; }
            set
            {
                _gadgets = value;
                RaisePropertyChanged();
            }
        }

        private GadgetViewModel _selectedGadget;
        public GadgetViewModel SelectedGadget
        {
            get
            {
                return _selectedGadget;
            }
            set
            {
                _selectedGadget = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<CustomerViewModel> _customers;
        public ObservableCollection<CustomerViewModel> Customers
        {
            get { return _customers; }
            set
            {
                _customers = value;   
                RaisePropertyChanged();
            }
        }

        private CustomerViewModel _selectedCustomer;
        public CustomerViewModel SelectedCustomer
        {
            get
            {
                return _selectedCustomer;
            }
            set
            {
                _selectedCustomer = value;
                RaisePropertyChanged();
            }
        }
        
    }
}

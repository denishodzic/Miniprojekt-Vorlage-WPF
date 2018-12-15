using ch.hsr.wpf.gadgeothek.domain;
using ch.hsr.wpf.gadgeothek.service;
using Gadgeothek.WinUI.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Gadgeothek.WinUI.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public LibraryAdminService dataService;


        public MainWindowViewModel()
        {
            dataService = new LibraryAdminService(ConfigurationManager.AppSettings.Get("server")?.ToString());

            try
            {
                Gadgets = new ObservableCollection<GadgetViewModel>(dataService.GetAllGadgets()
                    .Select(g => new GadgetViewModel(g)));
                SelectedGadget = Gadgets.FirstOrDefault();

                Customers = new ObservableCollection<CustomerViewModel>(dataService.GetAllCustomers()
                    .Select(c => 
                    {
                        var customerViewModel = new CustomerViewModel(c);
                        customerViewModel.DataChanged += OnCustomerDataChanged;
                        return customerViewModel;
                    }));
                SelectedCustomer = Customers.FirstOrDefault();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Beim Laden ist ein Problem aufgetreten.", ex.Message, MessageBoxButton.OK);
                return;
            }

            var loans = dataService.GetAllLoans();
            if (loans == null)
            {
                MessageBox.Show("Konnte Ausleihen nicht vom Server laden.", "Serverfehler", MessageBoxButton.OK);
                return;
            }
            else if (loans.Count > 0)
            {
                Loans = new ObservableCollection<Loan>(loans);

                SelectedLoan = Loans.First();
            }

            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(5000);
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        loans = dataService.GetAllLoans();
                        if (loans != null)
                        {
                            Loans.Clear();

                            loans.ForEach(Loans.Add);
                        }
                    });
                }

            });
        }

        #region Gadgets

        private ICommand _openAddGadgetCommand;
        public ICommand OpenAddGadgetCommand => _openAddGadgetCommand ?? (_openAddGadgetCommand = new RelayCommand(() => OpenAddGadget()));

        private ICommand _editGadgetCommand;
        public ICommand EditGadgetCommand => _editGadgetCommand ??
            ( _editGadgetCommand = new RelayCommand( () => EditGadget() ) );


        private ICommand _removeGadgetCommand;
        public ICommand RemoveGadgetCommand => _removeGadgetCommand ?? (_removeGadgetCommand = new RelayCommand(() => RemoveGadget()));
        
        internal void UpdateGadget( GadgetViewModel editGadgetViewModel )
        {
            var existingGadget = Gadgets.FirstOrDefault( g => g.InventoryNumber == editGadgetViewModel.InventoryNumber );
            if(existingGadget != null )
            {
                existingGadget.Name = editGadgetViewModel.Name;
                existingGadget.Condition = editGadgetViewModel.Condition;
                existingGadget.Price = editGadgetViewModel.Price;
                existingGadget.Manufacturer = editGadgetViewModel.Manufacturer;

                dataService.UpdateGadget(editGadgetViewModel.Data);
            }
        }

        private ICollectionView _gadgetCollectionView;
        public ICollectionView GadgetsCollectionView
        {
            get
            {
                if(_gadgetCollectionView == null)
                {
                    _gadgetCollectionView = CollectionViewSource.GetDefaultView(Gadgets);
                }

                return _gadgetCollectionView;
            }
        }

        public GadgetViewModel SelectedGadget
        {
            get
            {
                return (GadgetViewModel)GadgetsCollectionView.CurrentItem;
            }
            set
            {
                GadgetsCollectionView.MoveCurrentTo( value );
                RaisePropertyChanged(nameof(IsGadgetSelected));
            }
        }

        public bool IsGadgetSelected
        {
            get { return SelectedGadget != null; }
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

        #endregion

        #region Customers

        private ICommand _createCustomerCommand;
        public ICommand CreateCustomerCommand => _createCustomerCommand ?? 
            (_createCustomerCommand = new RelayCommand(() => 
            {
                var newCustomer = new Customer()
                {
                    Name = "<Name>",
                    Email = "<Email>",
                    Password = "<Password>",
                    Studentnumber = Guid.NewGuid().ToString("N")
                };

                if (!dataService.AddCustomer(newCustomer))
                {
                    MessageBox.Show("CreateCustomer failed", "Serverfehler", MessageBoxButton.OK);
                    return;
                }

                var newCustomerViewModel = new CustomerViewModel(dataService.GetCustomer(newCustomer.Studentnumber));
                newCustomerViewModel.DataChanged += OnCustomerDataChanged;
                Customers.Add(newCustomerViewModel);
            }
            ));
        
        private ICommand _removeCustomerCommand;
        public ICommand RemoveCustomerCommand => _removeCustomerCommand ??
            (_removeCustomerCommand = new RelayCommand(() =>
            {
                if (!dataService.DeleteCustomer(SelectedCustomer.Data))
                {
                    MessageBox.Show("DeleteCustomer failed", "Serverfehler", MessageBoxButton.OK);
                    return;
                }

                SelectedCustomer.DataChanged -= OnCustomerDataChanged;
                Customers.Remove(SelectedCustomer);
            }));

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

        private ICollectionView _customersCollectionView;
        public ICollectionView CustomersCollectionView
        {
            get
            {
                if (_customersCollectionView == null)
                {
                    _customersCollectionView = CollectionViewSource.GetDefaultView(Customers);
                }

                return _customersCollectionView;
            }
        }

        public CustomerViewModel SelectedCustomer
        {
            get
            {
                return (CustomerViewModel)CustomersCollectionView.CurrentItem;
            }
            set
            {
                GadgetsCollectionView.MoveCurrentTo(value);
                RaisePropertyChanged(nameof(IsCustomerSelected));
            }
        }

        public bool IsCustomerSelected
        {
            get { return SelectedCustomer != null; }
        }

        private void OnCustomerDataChanged(CustomerViewModel customer, string propertyname)
        {
            dataService.UpdateCustomer(customer.Data);
        }

        #endregion

        public Loan SelectedLoan { get; set; }
        private ObservableCollection<Loan> _loans;
        public ObservableCollection<Loan> Loans
        {
            get { return _loans; }
            set
            {
                _loans = value;
                RaisePropertyChanged();
            }
        }


        public void OpenAddGadget()
        {
            var addNewGadgetWindow = new ChangeGadgetView(new AddGadgetViewModel(this));
            addNewGadgetWindow.Show();
        }

        public void EditGadget()
        {
            var editGadgetWindow = new ChangeGadgetView( new EditGadgetViewModel( this, SelectedGadget ) );

            editGadgetWindow.Show();
        }

        public void RemoveGadget()
        {
            try
            {
                if (SelectedGadget != null)
                {
                    MessageBoxResult dialogResult = MessageBox.Show($"Sind Sie sicher, dass Sie{Environment.NewLine}{Environment.NewLine}{SelectedGadget.Data.FullDescription()}{Environment.NewLine}{Environment.NewLine}löschen möchten?", "Löschen bestätigen", MessageBoxButton.YesNo);

                    if (dialogResult == MessageBoxResult.Yes)
                    {
                        if (dataService.DeleteGadget(SelectedGadget.Data))
                        {
                            Gadgets.Remove(SelectedGadget);
                        }
                        else
                        {
                            MessageBox.Show("Fehler beim Löschen des Gadgets. Bitte versuchen Sie es nochmals.", "Löschen fehlgeschlagen", MessageBoxButton.OK);
                        }
                    }
                }
            }
            catch (InvalidCastException exception)
            {
                MessageBox.Show("Fehler beim Löschen des Gadgets. Bitte versuchen Sie es nochmals.", "Löschen fehlgeschlagen", MessageBoxButton.OK);
                Debug.Print(exception.ToString());
            }
        }


    }
}

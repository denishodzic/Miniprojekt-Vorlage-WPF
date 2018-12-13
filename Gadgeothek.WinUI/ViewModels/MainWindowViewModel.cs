using ch.hsr.wpf.gadgeothek.domain;
using ch.hsr.wpf.gadgeothek.service;
using Gadgeothek.WinUI.Views;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Gadgeothek.WinUI.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public LibraryAdminService libraryAdminService;

        private ICommand _openAddGadgetCommand;
        public ICommand OpenAddGadgetCommand => _openAddGadgetCommand ?? (_openAddGadgetCommand = new RelayCommand(() => OpenAddGadget()));

        private ICommand _editGadgetCommand;
        public ICommand EditGadgetCommand => _editGadgetCommand ??
            ( _editGadgetCommand = new RelayCommand( () => EditGadget() ) );

        

        private ICommand _removeGadgetCommand;
        public ICommand RemoveGadgetCommand => _removeGadgetCommand ?? (_removeGadgetCommand = new RelayCommand(() => RemoveGadget()));

        public Gadget SelectedGadget { get; set; }

        internal void UpdateGadget( EditGadgetViewModel editGadgetViewModel )
        {
            var existingGadget = Gadgets.FirstOrDefault( g => g.InventoryNumber == editGadgetViewModel.InventoryNumber );
            if(existingGadget != null )
            {
                existingGadget.Name = editGadgetViewModel.Name;
                existingGadget.Manufacturer = editGadgetViewModel.Manufacturer;
                existingGadget.Price = editGadgetViewModel.Price;
                existingGadget.Condition = editGadgetViewModel.Condition;
                libraryAdminService.UpdateGadget( existingGadget );
            }

            
        }

        private ObservableCollection<Gadget> _gadgets;
        public ObservableCollection<Gadget> Gadgets
        {
            get { return _gadgets; }
            set
            {
                _gadgets = value;
                RaisePropertyChanged();
            }
        }

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

        public MainWindowViewModel()
        {
            libraryAdminService = new LibraryAdminService(ConfigurationManager.AppSettings.Get("server")?.ToString());

            var gadgets = libraryAdminService.GetAllGadgets();

            if(gadgets == null)
            {
                MessageBox.Show("Konnte Gadgets nicht vom Server laden.", "Serverfehler", MessageBoxButton.OK);
            } else if (gadgets.Count > 0)
            {
                Gadgets = new ObservableCollection<Gadget>(gadgets);

                SelectedGadget = Gadgets.First();
            }
            else
            {
                MessageBox.Show("Keine Gadgets vorhanden. Bitte fügen Sie zuerst ein Gadget hinzu.", "Keine Gadgets vorhanden", MessageBoxButton.OK);
            }


            var loans = libraryAdminService.GetAllLoans();
            if (loans == null)
            {
                MessageBox.Show("Konnte Ausleihen nicht vom Server laden.", "Serverfehler", MessageBoxButton.OK);
                return;
            } else if (loans.Count > 0)
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
                        loans = libraryAdminService.GetAllLoans();
                        if (loans != null)
                        {
                            Loans.Clear();

                            loans.ForEach(Loans.Add);
                        }
                    });
                }

            });
        }

        public void OpenAddGadget()
        {
            var addNewGadgetWindow = new ChangeGadgetView( this);
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
                    MessageBoxResult dialogResult = MessageBox.Show($"Sind Sie sicher, dass Sie{Environment.NewLine}{Environment.NewLine}{SelectedGadget.FullDescription()}{Environment.NewLine}{Environment.NewLine}löschen möchten?", "Löschen bestätigen", MessageBoxButton.YesNo);

                    if (dialogResult == MessageBoxResult.Yes)
                    {
                        if (libraryAdminService.DeleteGadget(SelectedGadget))
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

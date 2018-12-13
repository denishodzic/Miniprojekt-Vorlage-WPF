using ch.hsr.wpf.gadgeothek.domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Gadgeothek.WinUI.ViewModels
{
    class AddGadgetViewModel : BindableBase
    {
        private ICommand _closeDialogCommand;
        public ICommand CloseDialogCommand => _closeDialogCommand ?? (_closeDialogCommand = new RelayCommand<Window>((x) => CloseDialog(x)));

        private ICommand _saveGadgetCommand;
        public ICommand SaveGadgetCommand => _saveGadgetCommand ?? (_saveGadgetCommand = new RelayCommand<Window>((x) => SaveGadget(x)));

        private bool _isFormValid;
        public bool FormIsValid
        {
            get
            {
                return _isFormValid;
            }
            set
            {
                _isFormValid = value;
                RaisePropertyChanged();
            }
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                RaisePropertyChanged();
                validateForm();
            }
        }

        private string _manufacturer;
        public string Manufacturer {
            get
            {
                return _manufacturer;
            }
            set
            {
                _manufacturer = value;
                RaisePropertyChanged();
                validateForm();
            }
        }

        private double _price;
        public double Price
        {
            get
            {
                return _price;
            }
            set
            {
                _price = value;
                RaisePropertyChanged();
                validateForm();
            }
        }


        private ch.hsr.wpf.gadgeothek.domain.Condition _condition;
        public ch.hsr.wpf.gadgeothek.domain.Condition Condition
        {
            get
            {
                return _condition;
            }
            set
            {
                RaisePropertyChanged();
                validateForm();
            }
        }

        private MainWindowViewModel _mainWindowViewModel;

        public AddGadgetViewModel(MainWindowViewModel mainWindowViewModel)
        {
           _mainWindowViewModel = mainWindowViewModel;
        }

        public void CloseDialog(Window window)
        {
            window.Close();
        }

        private void validateForm()
        {
            if (_name == null || _name.Length <= 0)
            {
                FormIsValid = false;
                return;
            }

            if (_manufacturer == null || _manufacturer.Length <= 0)
            {
                FormIsValid = false;
                return;
            }

            if (_price <= 0.0)
            {
                FormIsValid = false;
                return;
            }

            FormIsValid = true;
            CommandManager.InvalidateRequerySuggested();

            Debug.Print("Form is Valid");
        }

        public void SaveGadget(Window window)
        {
            var newGadget = new Gadget(_name)
            {
                Condition = _condition,
                Manufacturer = _manufacturer,
                Price = _price
            };

            if (_mainWindowViewModel.libraryAdminService.AddGadget(newGadget)) {
                _mainWindowViewModel.Gadgets.Add(newGadget);
                 window.Close();
             }
             else
             {
                 MessageBox.Show("Fehler beim Speichern des Gadgets. Bitte versuchen Sie es nochmals.", "Speichern fehlgeschlagen", MessageBoxButton.OK);
             }

        }
    }
}

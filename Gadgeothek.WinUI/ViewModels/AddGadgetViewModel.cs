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

    class AddGadgetViewModel : BindableBase, IGadgetEditViewModel
    {
        public string Title { get; } = "Gadget hinzufügen";
        private ICommand _closeDialogCommand;
        public ICommand CancelCommand => _closeDialogCommand ??
            ( _closeDialogCommand = new RelayCommand<Window>( ( changeGadgetWindow ) =>
            {
                try
                {
                    //do nothing
                }
                finally
                {
                    changeGadgetWindow.Close();
                }
            } ) );

        private ICommand _saveGadgetCommand;
        public ICommand SaveCommand => _saveGadgetCommand ??
            ( _saveGadgetCommand = new RelayCommand<Window>( ( changeGadgetWindow ) =>
            {
                try
                {
                    var newGadget = new Gadget( _name )
                    {
                        Condition = _condition,
                        Manufacturer = _manufacturer,
                        Price = _price
                    };

                    if ( _mainWindowViewModel.libraryAdminService.AddGadget( newGadget ) )
                    {
                        _mainWindowViewModel.Gadgets.Add( new GadgetViewModel(newGadget) );
                    }
                    else
                    {
                        MessageBox.Show( "Fehler beim Speichern des Gadgets. Bitte versuchen Sie es nochmals.", "Speichern fehlgeschlagen", MessageBoxButton.OK );
                    }
                }
                finally
                {
                    changeGadgetWindow.Close();
                }
            } ) );

        private bool _isFormValid;
        public bool IsFormValid
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
                _condition = value;
                RaisePropertyChanged();
                validateForm();
            }
        }

        private MainWindowViewModel _mainWindowViewModel;

        public AddGadgetViewModel(MainWindowViewModel mainWindowViewModel)
        {
           _mainWindowViewModel = mainWindowViewModel;
        }
        
        private void validateForm()
        {
            if (_name == null || _name.Length <= 0)
            {
                IsFormValid = false;
                return;
            }

            if (_manufacturer == null || _manufacturer.Length <= 0)
            {
                IsFormValid = false;
                return;
            }

            if (_price <= 0.0)
            {
                IsFormValid = false;
                return;
            }

            IsFormValid = true;
            CommandManager.InvalidateRequerySuggested();

            Debug.Print("Form is Valid");
        }
    }
}

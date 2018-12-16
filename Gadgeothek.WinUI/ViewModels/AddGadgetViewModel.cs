using ch.hsr.wpf.gadgeothek.domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Gadgeothek.WinUI.ViewModels
{
    public class AddGadgetViewModel : BindableBase, IGadgetEditViewModel, INotifyDataErrorInfo
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

                    if ( _mainWindowViewModel.dataService.AddGadget( newGadget ) )
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
                RaisePropertyChanged(nameof(HasErrors));
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
                RaisePropertyChanged(nameof(HasErrors));
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
                RaisePropertyChanged(nameof(HasErrors));
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
                RaisePropertyChanged(nameof(HasErrors));
            }
        }

        private MainWindowViewModel _mainWindowViewModel;

        public AddGadgetViewModel(MainWindowViewModel mainWindowViewModel)
        {
           _mainWindowViewModel = mainWindowViewModel;
        }

        public bool HasErrors
        {
            get
            {
                _validationErrors.Clear();

                if (Name == null || Name.Length <= 0)
                {
                    _validationErrors.Add(nameof(Name), "Invalid Name");
                }

                if (Manufacturer == null || Manufacturer.Length <= 0)
                {
                    _validationErrors.Add(nameof(Manufacturer), "Invalid Manufacturer");
                }

                if (Price < 0)
                {
                    _validationErrors.Add(nameof(Price), "Price has to be >= 0");
                }

                return _validationErrors.Any();
            }
        }

        private IDictionary<string, string> _validationErrors = new Dictionary<string, string>();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return _validationErrors[propertyName];
        }
    }
}

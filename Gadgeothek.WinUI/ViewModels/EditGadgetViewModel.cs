using ch.hsr.wpf.gadgeothek.domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Gadgeothek.WinUI.ViewModels
{
    public class EditGadgetViewModel : BindableBase, IGadgetEditViewModel, INotifyDataErrorInfo
    {
        public string Title { get; } = "Gadget editieren";

        private MainWindowViewModel _mainWindowViewModel;
        private GadgetViewModel _gadget;

        public EditGadgetViewModel( MainWindowViewModel mainWindowViewModel , GadgetViewModel selectedGadget)
        {
            _mainWindowViewModel = mainWindowViewModel;

            _gadget = new GadgetViewModel( selectedGadget.Data);
        }

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
                    _mainWindowViewModel.UpdateGadget(_gadget);
                }
                finally
                {
                    changeGadgetWindow.Close();
                }
            } ) );

        public string InventoryNumber { get; }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public string Name
        {
            get
            {
                return _gadget.Name;
            }
            set
            {
                _gadget.Name = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(HasErrors));
            }
        }

        public string Manufacturer
        {
            get
            {
                return _gadget.Manufacturer;
            }
            set
            {
                _gadget.Manufacturer = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(HasErrors));
            }
        }

        public double Price
        {
            get
            {
                return _gadget.Price;
            }
            set
            {
                _gadget.Price = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(HasErrors));
            }
        }


        public ch.hsr.wpf.gadgeothek.domain.Condition Condition
        {
            get
            {
                return _gadget.Condition;
            }
            set
            {
                _gadget.Condition = value;
                RaisePropertyChanged();
            }
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

        public IEnumerable GetErrors(string propertyName)
        {
            return _validationErrors[propertyName];
        }
    }
}

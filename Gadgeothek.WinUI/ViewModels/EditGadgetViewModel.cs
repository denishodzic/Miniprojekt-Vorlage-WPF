using ch.hsr.wpf.gadgeothek.domain;
using System.Windows;
using System.Windows.Input;

namespace Gadgeothek.WinUI.ViewModels
{
    public class EditGadgetViewModel : BindableBase
    {
        public string Title { get; } = "Gadget editieren";

        private MainWindowViewModel _mainWindowViewModel;
        private GadgetViewModel _gadget;

        public EditGadgetViewModel( MainWindowViewModel mainWindowViewModel , GadgetViewModel selectedGadget)
        {
            _mainWindowViewModel = mainWindowViewModel;

            _gadget = selectedGadget;
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
                //validateForm();
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
                //validateForm();
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
               // validateForm();
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
                //validateForm();
            }
        }
    }
}

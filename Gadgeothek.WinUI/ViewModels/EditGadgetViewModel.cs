using ch.hsr.wpf.gadgeothek.domain;
using System.Windows;
using System.Windows.Input;

namespace Gadgeothek.WinUI.ViewModels
{
    public class EditGadgetViewModel : BindableBase
    {
        public string Title { get; } = "Gadget editieren";

        private MainWindowViewModel _mainWindowViewModel;

        public EditGadgetViewModel( MainWindowViewModel mainWindowViewModel , Gadget selectedGadget)
        {
            _mainWindowViewModel = mainWindowViewModel;

            InventoryNumber = selectedGadget.InventoryNumber;
            Name = selectedGadget.Name;
            Manufacturer = selectedGadget.Manufacturer;
            Price = selectedGadget.Price;
            Condition = selectedGadget.Condition;
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
                    _mainWindowViewModel.UpdateGadget( this );
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
                //validateForm();
            }
        }

        private string _manufacturer;
        public string Manufacturer
        {
            get
            {
                return _manufacturer;
            }
            set
            {
                _manufacturer = value;
                RaisePropertyChanged();
                //validateForm();
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
               // validateForm();
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
                //validateForm();
            }
        }
    }
}

using ch.hsr.wpf.gadgeothek.domain;

namespace Gadgeothek.WinUI.ViewModels
{


    public class GadgetViewModel : BindableBase
    {
        public Gadget Data { get; }

        public GadgetViewModel(Gadget data)
        {
            Data = data;
        }
        public string InventoryNumber
        {
            get
            {
                return Data.InventoryNumber;
            }
            set
            {
                Data.InventoryNumber = value;
                RaisePropertyChanged();
            }
        }

        public Condition Condition
        {
            get
            {
                return Data.Condition;
            }
            set
            {
                Data.Condition = value;
                RaisePropertyChanged();
            }
        }

        public double Price
        {
            get
            {
                return Data.Price;
            }
            set
            {
                Data.Price = value;
                RaisePropertyChanged();
            }
        }

        public string Manufacturer
        {
            get
            {
                return Data.Manufacturer;
            }
            set
            {
                Data.Manufacturer = value;
                RaisePropertyChanged();
            }
        }

        public string Name
        {
            get
            {
                return Data.Name;
            }
            set
            {
                Data.Name = value;
                RaisePropertyChanged();
            }
        }
    }
}

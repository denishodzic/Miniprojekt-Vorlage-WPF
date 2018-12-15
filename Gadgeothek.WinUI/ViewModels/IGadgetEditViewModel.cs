using System.Windows.Input;

namespace Gadgeothek.WinUI.ViewModels
{
    public interface IGadgetEditViewModel
    {
        string Title { get; }
        ICommand CancelCommand { get; }

        ICommand SaveCommand { get; }
    }
}

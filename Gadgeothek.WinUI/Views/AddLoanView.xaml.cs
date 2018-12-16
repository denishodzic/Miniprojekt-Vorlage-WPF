using ch.hsr.wpf.gadgeothek.domain;
using ch.hsr.wpf.gadgeothek.service;
using Gadgeothek.WinUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Gadgeothek.WinUI.Views
{
    /// <summary>
    /// Interaktionslogik für ChangeGadgetView.xaml
    /// </summary>
    public partial class AddLoanView : Window
    {
        public AddLoanView(PickLoanViewModel pickLoanViewModel)
        {
            InitializeComponent();

            DataContext = pickLoanViewModel;
        }

    }
}

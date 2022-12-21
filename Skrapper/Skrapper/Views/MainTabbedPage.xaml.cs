using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Skrapper
{
    public partial class MainTabbedPage : TabbedPage
    {
        MainViewModel _mainViewModel;
        public MainTabbedPage()
        {
            InitializeComponent();
            BindingContext= _mainViewModel = new MainViewModel();
        }
    }
}

using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Skrapper
{
    public class SkidViewModel : MainViewModel
    {
        public SkidViewModel()
        {
            Title = "SKID";
        }

        int selectedSkidIndex = Globals.pSkidIdx;
        public int SelectedSkidIndex
        {
            get { return selectedSkidIndex; }
            set 
            {
                SetProperty(ref selectedSkidIndex, value);
                Globals.pSkidIdx = value;
                Console.WriteLine("[SkidViewModel.cs] (SelectedSkidIndex) >> " + value);
            }
        }

        string selectedSkidItem = Globals.pSkidItem;
        public string SelectedSkidItem
        {
            get { return selectedSkidItem; }
            set 
            { 
                SetProperty (ref selectedSkidItem, value);
                Globals.pSkidItem = value;
                Console.WriteLine("[SkidViewModel.cs] (SelectedSkidItem) >> " + value);
            }
        }

        int selectedActionIndex = Globals.pActionIdx;
        public int SelectedActionIndex
        {
            get { return selectedActionIndex; }
            set 
            { 
                SetProperty(ref selectedActionIndex, value);
                Globals.pActionIdx = value;
                Console.WriteLine("[SkidViewModel.cs] (SelectedActionIndex) >> " + value);
            }
        }

        string selectedActionItem = Globals.pActionItem;
        public string SelectedActionItem
        {
            get { return selectedActionItem; }
            set 
            { 
                SetProperty (ref selectedActionItem, value);
                Globals.pActionItem = value;
                Console.WriteLine("[SkidViewModel.cs] (SelectedActionItem) >> " + value);
            }    
        }

        int selectedCarrierIndex = Globals.pCarrierIdx;
        public int SelectedCarrierIndex
        {
            get { return selectedCarrierIndex; }
            set 
            { 
                SetProperty(ref selectedCarrierIndex, value);
                Globals.pCarrierIdx = value;
                Console.WriteLine("[SkidViewModel.cs] (SelectedCarrierIndex) >> " + value);
            }
        }

        string selectedCarrierItem = Globals.pCarrierItem;
        public string SelectedCarrierItem
        {
            get { return selectedCarrierItem; }
            set 
            { 
                SetProperty (ref selectedCarrierItem, value);
                Globals.pCarrierItem = value;
                Console.WriteLine("[SkidViewModel.cs] (SelectedCarrierItem) >> " + value);
            }
        }
    }
}

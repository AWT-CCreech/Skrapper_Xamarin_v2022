using MyWebService;
using Skrapper.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Skrapper
{
    public class SkidViewModel : MainViewModel
    {
        public IMessageService _messageService = new MessageService();

        public Command EnterSkidNumberCommand { private set; get; }
        public Command CreateNewSkidNumberCommand { private set; get; }

        public SkidViewModel()
        {
            Title = "SKID";
            EnterSkidNumberCommand = new Command(OnEnterSkidNumberClicked);
            CreateNewSkidNumberCommand = new Command(
                execute: async () =>
                {
                    await OnCreateNewSkidNumberClicked(Skids.Result);
                    OnPropertyChanged("Skids");
                    SetProperty(ref selectedSkidIndex, 0);
                    OnPropertyChanged("SelectedSkidIndex");
                });
        }

        private async void OnEnterSkidNumberClicked(object obj)
        {
            if (eHelpDeskContext.WebServiceConnected())
            {
                try
                {
                    if (Skids == null) return;
                    bool b = await _messageService.CustomInputDialog(Skids.Result, "Skid # Entry", "Please insert skid number below...", "OK");
                    if (b)
                    {
                        SetProperty(ref selectedSkidIndex, 0);
                        OnPropertyChanged("SelectedSkidIndex");
                    }
                }
                catch(Exception ex)
                {
                    await _messageService.DisplayError("[ERROR: SkidViewModel.cs]", "(OnEnterSkidNumberClicked)\r\n" + ex.Message, "dismiss");
                    Console.WriteLine("[ERROR: SkidViewModel.cs] (OnEnterSkidNumberClicked) >> " + ex.ToString());
                }
            }
        }

        private async Task OnCreateNewSkidNumberClicked(ObservableCollection<string> list)
        {
            Task<string> ts;
            if (eHelpDeskContext.WebServiceConnected())
            {
                try
                {
                    using (ScannerWebServiceSoapClient locWS = eHelpDeskContext.GetWebServiceRef())
                    {
                        ts = Task.Run(() => locWS.Skp_SkidNewNumber(selectedUser));
                        string skidNo = ts.Result;
                        Console.WriteLine("[SkidsViewModel.cs] (OnCreateNewSkidNumberClicked) skidNo >>" + skidNo);
                        list.Insert(0, skidNo);
                        locWS.Close();
                    }
                }
                catch (Exception ex)
                {
                    await _messageService.DisplayError("[ERROR: SkidViewModel.cs]", "(OnCreateNewSkidNumberClicked)\r\n" + ex.Message, "dismiss");
                    Console.WriteLine("[ERROR: SkidViewModel.cs] (OnCreateNewSkidNumberClicked) >> " + ex.ToString());
                }
            }
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

using MyWebService;
using Skrapper.Models;
using Skrapper.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Skrapper
{
    public class SkidViewModel : MainViewModel
    {
        public Command EnterSkidNumberCommand { private set; get; }
        public Command CreateNewSkidNumberCommand { private set; get; }
        public Command RefreshSkidsPickerCommand { private set; get; }

        public SkidViewModel()
        {
            Title = "SKID";

            //Skids = new NotifyTaskCompletion<ObservableCollection<string>>(PickerService.GetSkidNumbers());
            //Carriers = new NotifyTaskCompletion<ObservableCollection<string>>(PickerService.GetCarriers());

            #region ... COMMANDS ...
            EnterSkidNumberCommand = new Command(OnEnterSkidNumberClicked);
            CreateNewSkidNumberCommand = new Command(async () => { await OnCreateNewSkidNumberClicked(Skids.Result);});
            RefreshSkidsPickerCommand = new Command(OnSkidsPickerRefresh);
            #endregion ... COMMANDS ...
        }

        public void OnAppearing()
        {
            //RefreshSkidsPickerCommand.Execute(true);
        }

        private async void OnEnterSkidNumberClicked(object obj)
        {
            if (eHelpDeskContext.WebServiceConnected())
            {
                try
                {
                    if (Skids == null) return;
                    bool b = await _messageService.CustomInputDialog(Skids.Result, "Skid # Entry", "Please insert skid number below...", "OK", selectedSkidItem.ToString());
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
                IsBusy = true;
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
                    IsBusy = false;
                    await _messageService.DisplayError("[ERROR: SkidViewModel.cs]", "(OnCreateNewSkidNumberClicked)\r\n" + ex.Message, "dismiss");
                    Console.WriteLine("[ERROR: SkidViewModel.cs] (OnCreateNewSkidNumberClicked) >> " + ex.ToString());
                }
                finally
                {
                    IsBusy = false;
                }
                OnPropertyChanged("Skids");
                SetProperty(ref selectedSkidIndex, 0);
                OnPropertyChanged("SelectedSkidIndex");
            }
        }

        private async void OnSkidsPickerRefresh()
        {
            IsBusy = true;
            try
            {
                SetProperty(ref selectedSkidIndex, -1);
                OnPropertyChanged("SelectedSkidIndex");
                Skids = new NotifyTaskCompletion<ObservableCollection<string>>(PickerService.GetSkidNumbers());
                OnPropertyChanged("Skids");
            }
            catch (Exception ex)
            {
                IsBusy = false;
                Console.WriteLine("[ERROR: SkidViewModel.cs] (OnSkidsPickerRefresh) ex >> " + ex.ToString());
                await _messageService.DisplayError("[ERROR: SkidViewModel.cs]", "(OnSkidsPickerRefresh)\r\n" + ex.Message, "dismiss");
            }
            finally
            {
                IsBusy = false;
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

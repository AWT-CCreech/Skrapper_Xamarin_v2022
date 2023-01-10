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
        public Command CreateSkidCommand { private set; get; }
        public Command RefreshCommand { private set; get; }
        public Command UpdateCommand { private set; get; }

        public SkidViewModel()
        {
            Title = "SKID";

            //Skids = new NotifyTaskCompletion<ObservableCollection<string>>(PickerService.GetSkidNumbers());
            //Carriers = new NotifyTaskCompletion<ObservableCollection<string>>(PickerService.GetCarriers());

            #region ... COMMANDS ...
            EnterSkidNumberCommand = new Command(OnEnterSkidNumberClicked);
            CreateSkidCommand = new Command(async () => { await OnCreateSkidClicked(Skids.Result);});
            RefreshCommand = new Command(async () => await OnRefreshClicked());
            UpdateCommand = new Command(OnUpdateClicked);
            #endregion ... COMMANDS ...
        }

        public void OnAppearing()
        {
            //RefreshCommand.Execute(true);
            SetProperty(ref skidPrintLabel, Globals.lblSkidPrint);
            OnPropertyChanged("SkidPrintLabel");
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

        private async Task OnCreateSkidClicked(ObservableCollection<string> list)
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
                        Console.WriteLine("[SkidsViewModel.cs] (OnCreateSkidClicked) skidNo >>" + skidNo);
                        list.Insert(0, skidNo);
                        locWS.Close();
                    }
                }
                catch (Exception ex)
                {
                    IsBusy = false;
                    await _messageService.DisplayError("[ERROR: SkidViewModel.cs]", "(OnCreateSkidClicked)\r\n" + ex.Message, "dismiss");
                    Console.WriteLine("[ERROR: SkidViewModel.cs] (OnCreateSkidClicked) >> " + ex.ToString());
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

        public async Task OnRefreshClicked()
        {
            IsBusy = true;
            try
            {
                if (selectedSkidIndex == -1) return;
                await DataGridService.LoadGridFromSkidNum(selectedSkidItem);
                await DataGridService.LoadPartListFromSkidNum(selectedSkidItem);
                await SkidInfoDataLoad(selectedSkidItem);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                Console.WriteLine("[ERROR: SkidViewModel.cs] (OnRefreshClicked) ex >> " + ex.ToString());
                await _messageService.DisplayError("[ERROR: SkidViewModel.cs]", "(OnRefreshClicked)\r\n" + ex.Message, "dismiss");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void OnUpdateClicked()
        {
            if (Globals.pSkidIdx < 0) return;
            Task.Run(()=>SkidInfoDataSave(false));
        }
    }
}

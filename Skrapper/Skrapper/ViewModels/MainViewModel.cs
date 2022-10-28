using Skrapper.Models;
using Skrapper.Services;
using Skrapper.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Android.Webkit.ConsoleMessage;

namespace Skrapper
{
    public class MainViewModel : ViewModelBase
    {
        public string VersionLabel { get; } = Constants.ksVersionStr;
        public string CopyrightLabel { get; } = Constants.ksCopyrightStr;

        public MainViewModel()
        {
            Users = new NotifyTaskCompletion<ObservableCollection<string>>(PickerService.LoadUserPickerList());
            Skids = new NotifyTaskCompletion<ObservableCollection<string>>(PickerService.GetSkidNumbers());
            Carriers = new NotifyTaskCompletion<ObservableCollection<string>>(PickerService.GetCarriers());
        }

        #region [USER]
        public NotifyTaskCompletion<ObservableCollection<string>> Users { get; private set; }

        public int selectedUserIndex = Globals.pUserIdx;
        public int SelectedUserIndex
        {
            set
            {

                Console.WriteLine("[MainViewModel.cs] (SelectedUserIndex) >> " + value);
                SetProperty(ref selectedUserIndex, value);
                Globals.pUserIdx = selectedUserIndex;
                if (messageLabel.Length > 0)
                {
                    SetProperty(ref messageLabel, string.Empty);
                    SetProperty(ref messageLabelColor, Color.Black);
                    OnPropertyChanged("MessageLabel");
                    OnPropertyChanged("MessageLabelColor");
                }
            }
            get { return selectedUserIndex; }

        }

        string selectedUser = Globals.UserName;
        public string SelectedUser
        {
            set
            {
                Console.WriteLine("[MainViewModel.cs] (SelectedUser) >> " + value);

                SetProperty(ref selectedUser, value);
                Globals.UserName = selectedUser;
            }
            get { return selectedUser; }
        }
        #endregion

        #region --: Login Page :--
        Color messageLabelColor = Color.IndianRed;
        public Color MessageLabelColor
        {
            set
            {
                if (value != null)
                {
                    if (value != messageLabelColor)
                        messageLabelColor = value;
                }
                else
                {
                    messageLabelColor = Color.IndianRed;
                }

            }
            get { return messageLabelColor; }
        }

        public string messageLabel = string.Empty;
        public string MessageLabel
        {
            set
            {
                Console.WriteLine("[LoginViewModel.cs] (MessageLabel) >> " + value);

                SetProperty(ref messageLabel, value);
            }
            get { return messageLabel; }
        }
        #endregion

        #region --: Skid Page :-- 
        public NotifyTaskCompletion<ObservableCollection<string>> Skids { get; private set; }
        public NotifyTaskCompletion<ObservableCollection<string>> Carriers { get; private set; }
        #endregion

        #region --: Scan Page :--
        public void SetOrderInProcess(bool inProcess)
        {
            SetProperty(ref orderInProcess, true);
            OnPropertyChanged("OrderInProcess");
        }
        #endregion

        //[SOURCE] frmMainSkrapper.cs
        #region --: SKID Data Submit Functions :--
        #endregion

        //[SOURCE] frmMainSkrapper.cs
        #region --: SKID Related Functions and Events :--
        #endregion
    }
}

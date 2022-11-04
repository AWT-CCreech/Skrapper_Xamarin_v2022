using Skrapper.Models;
using Skrapper.Services;
using Skrapper.Themes;
using Skrapper.Views;
using System;
using System.Collections.Generic;
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


        public Command EnterPartNumberCommand { private set; get; }
        public Command SerialNumberReturnCommand { private set; get; }
        public Command SubmitScanCommand { private set; get; }
        public Command CompleteOrderCommand { private set; get; }
        public Command DeleteLastScanCommand { private set; get; }

        public MainViewModel()
        {
            Theme theme;
            if (!Globals.bTestMode)
                theme = Theme.Live;
            else
                theme = Theme.Test;

            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();

                switch (theme)
                {
                    case Theme.Test:
                        mergedDictionaries.Add(new TestTheme());
                        break;
                    case Theme.Live:
                    default:
                        mergedDictionaries.Add(new LiveTheme());
                        break;
                }
            }

            Users = new NotifyTaskCompletion<ObservableCollection<string>>(PickerService.GetUserPickerList());
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

        public string selectedUser = Globals.UserName;
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
        public NotifyTaskCompletion<ObservableCollection<string>> Skids { get; set; }
        public NotifyTaskCompletion<ObservableCollection<string>> Carriers { get; private set; }
        #endregion

        #region --: Scan Page :--
        public NotifyTaskCompletion<ObservableCollection<string>> PartNumberChoices { get; private set; }

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

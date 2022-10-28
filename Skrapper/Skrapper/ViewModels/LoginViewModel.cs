using Skrapper.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Skrapper
{
    public class LoginViewModel : MainViewModel
    {
        public IMessageService _messageService = new MessageService();

        public Command LoginCommand { get; }
        public Command QuitCommand { get; }

        public LoginViewModel()
        {
            //Users = new NotifyTaskCompletion<ObservableCollection<string>>(PickerService.LoadUserPickerList());
            LoginCommand = new Command(OnLoginClicked);
            QuitCommand = new Command(OnQuitClicked);
        }

        //public NotifyTaskCompletion<ObservableCollection<string>> Users { get; private set; }

        /*
        int selectedUserIndex = Globals.pUserIdx;
        public int SelectedUserIndex
        {
            set
            {

                Console.WriteLine("[MainViewModel.cs] (SelectedUserIndex) >> " + value);
                SetProperty(ref selectedUserIndex, value);
                Globals.pUserIdx = selectedUserIndex;
                if(messageLabel.Length > 0)
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

        string messageLabel = string.Empty;
        public string MessageLabel
        {
            set
            {
                Console.WriteLine("[LoginViewModel.cs] (MessageLabel) >> " + value);

                SetProperty(ref messageLabel, value);
            }
            get { return messageLabel; }
        }
        */
        private async void OnLoginClicked(object obj)
        {
            IsBusy = true;

            bool validated = await IsValidUser();
            if (validated)
            {
                // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
                //await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
                App.IsUserLoggedIn = true;
                await Xamarin.Essentials.SecureStorage.SetAsync("isUserLogged", "1");
                Application.Current.MainPage = new AppShell();
            }
            else
            {
                return;
            }
        }

        private void OnQuitClicked(object obj)
        {
            Globals.gWS = null;
            App.IsUserLoggedIn = false;
            Environment.Exit(0);
        }

        private async Task<bool> IsValidUser()
        {
            bool res = false;
            try
            {
                res = selectedUserIndex > -1;
                if (res)
                    SetProperty(ref messageLabel, string.Empty);
                else
                    SetProperty(ref messageLabel, "[ERROR: Select Username]");
                OnPropertyChanged("MessageLabel");
            }
            catch(Exception ex)
            {
                await _messageService.DisplayError("[ERROR: LoginViewModel.cs]", ex.Message, "dismiss");
            }
            IsBusy = false;
            return res;
        }
    }
}

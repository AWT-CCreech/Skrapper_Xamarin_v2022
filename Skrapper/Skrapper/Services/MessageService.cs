using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Skrapper.Services
{
    public interface IMessageService
    {
        //Task<bool> ShowCompleteSkidWarning(string title, string message, string cancel, string confirm);
        Task<bool> CustomInputDialog(ObservableCollection<string> list, string title, string message, string confirm, string initialValue);
        Task DisplayError(string title, string message, string cancel);
        Task<bool> DisplayCustomPrompt(string title, string message, string confirm, string cancel);
    }
    public class MessageService : IMessageService
    {
        /// <summary>
        /// Display complete order alert and determine user response for completing inProcess order.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="cancel"></param>
        /// <param name="confirm"></param>
        /// <returns>(bool) Globals.showCompleteSkidWarningAnswer</returns>
        //public async Task<bool> ShowCompleteSkidWarning(string title, string message, string cancel, string confirm)
        //{
            //Globals.showCompleteSkidWarningAnswer = await Xamarin.Forms.Application.Current.MainPage.DisplayAlert(title, message, confirm, cancel);
            //Console.WriteLine("[MessageService.cs] (ShowCompleteSkidWarning) Globals.showCompleteSkidWarningAnswer >> " + Globals.showCompleteSkidWarningAnswer);
            //return Globals.showCompleteSkidWarningAnswer;
        //}

        /// <summary>
        /// Allows for display of custom async prompt (i.e. Order # Entry, Part # Entry).
        /// </summary>
        /// <param name="list"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="confirm"></param>
        /// <returns>(bool) r</returns>
        public async Task<bool> CustomInputDialog(ObservableCollection<string> list, string title, string message, string confirm, string initialValue)
        {
            string r;
            if(title.Contains("Skid"))
                r = await Xamarin.Forms.Application.Current.MainPage.DisplayPromptAsync(title, message, confirm, maxLength: 21, keyboard: Xamarin.Forms.Keyboard.Numeric, initialValue: initialValue);
            else
                r = await Xamarin.Forms.Application.Current.MainPage.DisplayPromptAsync(title, message, confirm, maxLength: 21);

            Console.WriteLine("[MessageService.cs] (CustomInputDialog) r >> " + r);
            if (string.IsNullOrEmpty(r))
                return false;

            string cdi = r.ToUpper();
            Console.WriteLine("[MessageService.cs] (CustomInputDialog) cdi >> " + cdi);
            list.Insert(0, cdi);
            return true;
        }

        /// <summary>
        /// Used to display errors on UI via ViewModels.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        public async Task DisplayError(string title, string message, string cancel)
        {
            await Xamarin.Forms.Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        /// <summary>
        /// Used to display a custom alert on UI via ViewModels (i.e. Scan Detail, Delete Scan).
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="confirm"></param>
        /// <param name="cancel"></param>
        /// <returns>(bool) r</returns>
        public async Task<bool> DisplayCustomPrompt(string title, string message, string confirm, string cancel)
        {
            bool r = await Xamarin.Forms.Application.Current.MainPage.DisplayAlert(title, message, confirm, cancel);
            return r;
        }

    }
}

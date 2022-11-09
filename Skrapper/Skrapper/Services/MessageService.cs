using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Skrapper.Services
{
    public interface IMessageService
    {
        //Task<bool> ShowCompleteOrderWarning(string title, string message, string cancel, string confirm);
        Task<bool> CustomInputDialog(ObservableCollection<string> list, string title, string message, string confirm);
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
        /// <returns>(bool) Globals.showCompleteOrderWarningAnswer</returns>
        //public async Task<bool> ShowCompleteOrderWarning(string title, string message, string cancel, string confirm)
        //{
            //Globals.showCompleteOrderWarningAnswer = await Xamarin.Forms.Application.Current.MainPage.DisplayAlert(title, message, confirm, cancel);
            //Console.WriteLine("[MessageService.cs] (ShowCompleteOrderWarning) Globals.showCompleteOrderWarningAnswer >> " + Globals.showCompleteOrderWarningAnswer);
            //return Globals.showCompleteOrderWarningAnswer;
        //}

        /// <summary>
        /// Allows for display of custom async prompt (i.e. Order # Entry, Part # Entry).
        /// </summary>
        /// <param name="list"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="confirm"></param>
        /// <returns>(bool) r</returns>
        public async Task<bool> CustomInputDialog(ObservableCollection<string> list, string title, string message, string confirm)
        {
            string r = await Xamarin.Forms.Application.Current.MainPage.DisplayPromptAsync(title, message, confirm, maxLength: 21);
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

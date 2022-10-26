using System;
using System.Threading.Tasks;

namespace Skrapper
{
    public sealed class NotifyTaskCompletion<TResult> : ViewModelBase
    {
        public NotifyTaskCompletion(Task<TResult> task)
        {
            Task = task;
            if (!task.IsCompleted)
            {
                var _ = WatchTaskAsync(task);
            }
        }

        private async Task WatchTaskAsync(Task task)
        {
            try
            {
                await task;
            }
            catch
            {
            }

            OnPropertyChanged("Status");
            OnPropertyChanged("IsCompleted");
            OnPropertyChanged("IsNotCompleted");
            
            if (task.IsCanceled)
            {
                OnPropertyChanged("IsCanceled");
            }
            else if (task.IsFaulted)
            {
                OnPropertyChanged("IsFaulted");
                OnPropertyChanged("Exception");
                OnPropertyChanged("InnerException");
                OnPropertyChanged("ErrorMessage");
            }
            else
            {
                OnPropertyChanged("IsSuccessfullyCompleted");
                OnPropertyChanged("Result");
            }
        }

        public Task<TResult> Task { get; private set; }
        public TResult Result
        {
            get
            {
                return (Task.Status == TaskStatus.RanToCompletion) ? Task.Result : default;
            }
        }
        public TaskStatus Status { get { return Task.Status; } }
        public bool IsCompleted { get { return Task.IsCompleted; } }
        public bool IsNotCompleted { get { return !Task.IsCompleted; } }
        public bool IsSuccessfullyCompleted
        {
            get
            {
                return Task.Status == TaskStatus.RanToCompletion;
            }
        }
        public bool IsCanceled { get { return Task.IsCanceled; } }
        public bool IsFaulted { get { return Task.IsFaulted; } }
        public AggregateException Exception { get { return Task.Exception; } }
        public Exception InnerException
        {
            get
            {
                return Exception?.InnerException;
            }
        }
        public string ErrorMessage
        {
            get
            {
                return InnerException?.Message;
            }
        }
    }
}

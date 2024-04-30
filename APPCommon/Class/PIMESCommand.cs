using System;
using System.Windows.Input;

namespace APPCommon.Class
{
    public class PIMESCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action DoWork;

        public PIMESCommand(Action work)
        {
            DoWork = work;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            DoWork();
        }
    }
}
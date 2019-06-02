using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using OutlookAddIn.Views;

namespace OutlookAddin.Domain
{
    public interface IUIWindowDialogService
    {
        /// <summary>
        /// Shows a dialog to the user in a tool popup
        /// </summary>
        /// <param name="title">Title of the dialog window</param>
        /// <param name="datacontext">View model to be rendered. To return a value from a button clicked
        /// as a dialog result, a nullable bool property called DialogResult must exist on the view model
        /// and raise the OnPropertyChanged event to fire from the view. Otherwise, the only way to close
        /// the dialog will be the close button in the corner. The view model can also have a Title
        /// property of type string to display a title in the top bar.</param>
        /// <returns>A nullable boolean based on what action the user took if the view model handles it.
        /// (See the datacontext parameter for more info.) If the view model does not handle it. False
        /// will be returned it.</returns>
        bool? ShowDialog(object datacontext);
    }

    class WindowDialogService : IUIWindowDialogService
    {
        /// <summary>
        /// Shows a dialog to the user in a tool popup
        /// </summary>
        /// <param name="title">Title of the dialog window</param>
        /// <param name="datacontext">View model to be rendered. To return a value from a button clicked
        /// as a dialog result, a nullable bool property called DialogResult must exist on the view model
        /// and raise the OnPropertyChanged event to fire from the view. Otherwise, the only way to close
        /// the dialog will be the close button in the corner. The view model can also have a Title
        /// property of type string to display a title in the top bar.</param>
        /// <returns>A nullable boolean based on what action the user took if the view model handles it.
        /// (See the datacontext parameter for more info.) If the view model does not handle it. False
        /// will be returned it.</returns>
        public bool? ShowDialog(object datacontext)
        {
            var win = new EmptyWindow();
            win.DataContext = win.Content = datacontext;  // Set the content to show and the datacontext to bind to

            //Set the owner of the dialog box so that it's always in front
            var currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive);
            if (currentWindow != null)
            {
                win.Owner = currentWindow;
                win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }

            return win.ShowDialog();
        }
    }
}

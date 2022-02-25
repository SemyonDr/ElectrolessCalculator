using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ElectrolessCalculator.View
{
    /// <summary>
    /// Interaction logic for FractionButton.xaml
    /// </summary>
    public partial class FractionButton : UserControl
    {
        public FractionButton()
        {
            InitializeComponent();

            //Subscribing to popup opening/closing
            Popup.Opened += OnPopupOpened;
            Popup.Closed += OnPopupClosed;
        }


        /// <summary>
        /// Handler for opening/closing [%] button clicks.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (!Popup.IsOpen)
                Popup.IsOpen = true;
            else 
                Popup.IsOpen = false;
        }


        /// <summary>
        /// Handles opening of the popup menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPopupOpened(object sender, EventArgs e)
        {
            this.P_Button.Background = Brushes.PowderBlue;

            //Capturing mouse in popup element so it may register clicks outside of it and being closed by clicking away
            bool res = Mouse.Capture(FractionsMenu, CaptureMode.SubTree);

            //Registering event handler for outside clicks
            //Will close popup when clicked outside
            FractionsMenu.AddHandler(Mouse.PreviewMouseDownOutsideCapturedElementEvent, new MouseButtonEventHandler(OnOutsideClick));

            //Event handler for accidental mouse capture release
            //Re-captures the mouse
            //FractionsMenu.AddHandler(Mouse.LostMouseCaptureEvent, new MouseEventHandler(OnLostMouseCapture),true);

            //Event handler for clicks on popup buttons.
            //Will close the popup
            FractionsMenu.ButtonClickedEvent += OnPopupInsideButtonClicked;
        }


        /// <summary>
        /// Handles popup closing. Unsubscribes from popup input events and releases mouse capture.
        /// </summary>
        private void OnPopupClosed(object sender, EventArgs e) {
            this.P_Button.Background = Brushes.White;

            //Releasing mouse capture
            FractionsMenu.ReleaseMouseCapture();

            //Unregistering handlers
            FractionsMenu.RemoveHandler(Mouse.LostMouseCaptureEvent, new MouseEventHandler(OnLostMouseCapture));
            FractionsMenu.RemoveHandler(Mouse.PreviewMouseDownOutsideCapturedElementEvent, new MouseButtonEventHandler(OnOutsideClick));
            FractionsMenu.ButtonClickedEvent -= OnPopupInsideButtonClicked;
        }

        /// <summary>
        /// Prevents accidental losing of the mouse capture if popup is opened
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLostMouseCapture(object sender, MouseEventArgs e) {
            if (Popup.IsOpen)
                Mouse.Capture(FractionsMenu, CaptureMode.SubTree);
        }

        /// <summary>
        /// Handles clicks outside of popup visual tree when popup is opened.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOutsideClick(object sender, MouseButtonEventArgs e) {

            //Closing the popup menu
            if (Popup.IsOpen)
                Popup.IsOpen = false;
            //Safeguard for situation if mouse is captured, but popup is closed.
            //else
                //FractionsMenu.ReleaseMouseCapture();
            
        }

        private void OnPopupInsideButtonClicked(object sender, EventArgs e)
        {
            if (Popup.IsOpen)
                Popup.IsOpen = false;
        }
    }
}

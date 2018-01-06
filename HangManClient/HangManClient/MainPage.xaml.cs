using System.Threading.Tasks;
using Windows.Networking;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HangManClient
{
    public sealed partial class MainPage : Page
    {
        private readonly HD44780Controller lcd;

        private string lastKey;

        private SocketListener socketListener;
        private readonly SocketClient socketClient;

        public MainPage()
        {
            InitializeComponent();

            //Start lcd display
            lcd = new HD44780Controller();
            lcd.Init(20, 5, 16, 17, 4, 27, 22, 26, 19, 13, 6);
            Task.Delay(5); //Short delay necessary for Init

            //Start Listening for keyboardInput
            Window.Current.CoreWindow.CharacterReceived += CoreWindowOnCharacterReceived;

            //Start internet connection
            socketListener = new SocketListener(9000);
            socketClient = new SocketClient(new HostName("10.0.0.10"), 9000);
        }

        #region Private Methods

        private void CoreWindowOnCharacterReceived(CoreWindow sender, CharacterReceivedEventArgs args)
        {
            args.Handled = true;

            if (args.KeyCode == 13) //[ENTER]
            {
                if (lastKey != null)
                    socketClient.SendMessage(lastKey);

                lastKey = null;
            }

            else if (args.KeyCode == 8) //[BACKSPACE]
                lastKey = null;

            else if (char.IsLetter((char) args.KeyCode))
                lastKey = ((char) args.KeyCode).ToString();

            SetLcdText(lastKey);
        }

        private async void SetLcdText(string text)
        {
            lcd.ClearDisplay();
            await Task.Delay(5); //Short delay necessary for ClearDisplay

            if (lastKey != null)
            {
                lcd.SetCursorPosition(0, 7);
                lcd.WriteLine(text);

                lcd.WriteLine("submit: <enter>");
            }
        }

        #endregion
    }
}
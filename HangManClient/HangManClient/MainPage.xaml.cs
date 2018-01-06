using System.Threading.Tasks;
using Windows.Networking;
using Windows.UI.Core;
using Windows.UI.Xaml;
using UDPSockets;

namespace HangManClient
{
    public sealed partial class MainPage
    {
        private readonly HD44780Controller _lcd;

        private string _lastKey;

        private readonly UDPClientSocket _clientSocket;

        public MainPage()
        {
            InitializeComponent();

            //Start lcd display
            _lcd = new HD44780Controller();
            _lcd.Init(20, 5, 16, 17, 4, 27, 22, 26, 19, 13, 6);
            Task.Delay(5); //Short delay necessary for Init

            //Start Listening for keyboardInput
            Window.Current.CoreWindow.CharacterReceived += CoreWindowOnCharacterReceived;

            //Start internet connection
            _clientSocket = new UDPClientSocket(new HostName("10.0.0.26"), 9000);
        }

        #region Private Methods

        private void CoreWindowOnCharacterReceived(CoreWindow sender, CharacterReceivedEventArgs args)
        {
            args.Handled = true;

            if (args.KeyCode == 13) //[ENTER]
            {
                if (_lastKey != null)
                    _clientSocket.SendMessage(_lastKey);

                _lastKey = null;
            }

            else if (args.KeyCode == 8) //[BACKSPACE]
                _lastKey = null;

            else if (char.IsLetter((char) args.KeyCode))
                _lastKey = ((char) args.KeyCode).ToString();

            SetLcdText(_lastKey);
        }

        private async void SetLcdText(string text)
        {
            _lcd.ClearDisplay();
            await Task.Delay(5); //Short delay necessary for ClearDisplay

            if (_lastKey != null)
            {
                _lcd.SetCursorPosition(0, 7);
                _lcd.WriteLine(text);

                _lcd.WriteLine("submit: <enter>");
            }
        }

        #endregion
    }
}
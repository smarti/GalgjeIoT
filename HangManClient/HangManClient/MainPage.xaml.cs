using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace HangManClient
{
    public sealed partial class MainPage : Page
    {
        private HD44780Controller lcd ;

        private string lastKey;

        private SocketListener socketListener;
        private SocketClient socketClient;


        public MainPage()
        {
            this.InitializeComponent();

            //Start lcd display
            lcd = new HD44780Controller();
            lcd.Init(20, 5, 16, 17, 4, 27, 22, 26, 19, 13, 6);
            Task.Delay(5);

            //Listen for keyboardInput
            Window.Current.CoreWindow.CharacterReceived += CoreWindowOnCharacterReceived;

            //Start internetService
            socketListener = new SocketListener(9000);
            socketClient = new SocketClient("10.0.0.24", 9000);
        }

        public string GetLastKey => lastKey;

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
            {
                lastKey = null;
            }
            else if (char.IsLetter((char)args.KeyCode))
            {
                lastKey = ((char)args.KeyCode).ToString();
            }

            SetLcdText(lastKey);
        }

        private async void SetLcdText(string text)
        {
            lcd.ClearDisplay();
            await Task.Delay(5);

            if (lastKey != null)
            {
                lcd.SetCursorPosition(0, 7);
                lcd.WriteLine(text);

                lcd.WriteLine("submit: <enter>");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Gpio;
using System.Diagnostics;
using Windows.Devices.PointOfService;
using Windows.Security.Cryptography.Core;
using Windows.UI.Xaml.Shapes;

namespace HangManServer
{
    public sealed partial class Game : Page
    {
        public string SecretWord;
        public int Level;
        public int HangmanLevelPlay;
        public int HangmanAttempt;
        public List<Rectangle> HangmanDisplayList;
        public List<string> HangmanAnswerList = new List<string>();
        public List<string> HangmanGoodList = new List<string>();
        public List<string> HangmanFalseList = new List<string>();

        private SocketServer _server;

        public Game()
        {
            this.InitializeComponent();

            _server = new SocketServer(9000);
            _server.MessageReceived += CheckInput;

            InitHangmanDisplay();

            SecretWord = MainPage.SecretWord;
            Level = 12 - MainPage.Level;

            InitSecretWord();
        }

        private void InitHangmanDisplay()
        {
            HangmanDisplayList = new List<Rectangle>
            {
                HangmanDisplay1,
                HangmanDisplay2,
                HangmanDisplay3,
                HangmanDisplay4,
                HangmanDisplay5,
                HangmanDisplay6,
                HangmanDisplay7,
                HangmanDisplay8,
                HangmanDisplay9,
                HangmanDisplay10,
                HangmanDisplay11,
                HangmanDisplay12
            };

            for (int i = 0; i < Level; i++)
                HangmanDisplayList[i].Visibility = Visibility.Visible;
        }

        private void InitSecretWord()
        {
            foreach (char character in SecretWord)
            {
                HangmanAnswerList.Add(character.ToString());
            }

            for (int i = 0; i < HangmanAnswerList.Count; i++)
                HangmanGoodList.Add("_");

            AnswerGood.Text = String.Join(" ", HangmanGoodList.ToArray());
        }

        private void ChangeWordField()
        {
            AnswerGood.Text = String.Join(" ", HangmanGoodList.ToArray());
            AnswerFalse.Text = String.Join(" ", HangmanFalseList.ToArray());
        }

        private void ChangeHangman()
        {

            HangmanDisplayList[HangmanLevelPlay].Visibility = Visibility.Visible;
            ++HangmanLevelPlay;
        }

        private void CheckLevel()
        {
            if (HangmanLevelPlay == 12)
            {
                Result.Text = "GAME OVER THE WORD WAS " + SecretWord;

            }

            if (HangmanAnswerList.Count == HangmanAttempt)
            {
                Result.Text = "YOU WON THE WORD WAS " + SecretWord;

            }
        }

        private void CheckInput(string Letter)
        {
            Debug.WriteLine("CheckInput");

            //bool checking = true;
            //while (checking)
            //{
            //    for (int i = 0; i < HangmanAnswerList.Count; i++)
            //    {
            //        if (HangmanAnswerList[i].Contains(Letter))
            //        {
            //            HangmanGoodList[i] = HangmanAnswerList[i];
            //            HangmanAnswerList[i] = "0";
            //            ++HangmanAttempt;
            //        }
            //    }

            //    if (!HangmanAnswerList.Contains(Letter))
            //    {
            //        if (!HangmanFalseList.Contains(Letter) && !HangmanGoodList.Contains(Letter))
            //        {
            //            HangmanFalseList.Add(Letter);
            //            ChangeHangman();
            //        }

            //        checking = false;
            //    }
            //}

            Debug.WriteLine("CheckInput2");

            //ChangeWordField();
            //CheckLevel();

            Debug.WriteLine("CheckInput3");
        }

        private void BtnAgain_onclick(object sender, RoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(MainPage), null);
        }
    }
}

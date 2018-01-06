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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace HangManServer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Game : Page
    {
        public string HangmanAnswer;
        public int HangmanLevelSetup;
        public int HangmanLevelPlay = 0;
        public int HangmanAttempt = 0;
        public bool Play = false;
        public List<Rectangle> HangmanDisplayList = new List<Rectangle>();
        public List<string> HangmanAnswerList = new List<string>();
        public List<string> HangmanGoodList = new List<string>();
        public List<string> HangmanFalseList = new List<string>();

        public Game()
        {
            this.InitializeComponent();
            HangmanDisplayList.Add(HangmanDisplay1);
            HangmanDisplayList.Add(HangmanDisplay2);
            HangmanDisplayList.Add(HangmanDisplay3);
            HangmanDisplayList.Add(HangmanDisplay4);
            HangmanDisplayList.Add(HangmanDisplay5);
            HangmanDisplayList.Add(HangmanDisplay6);
            HangmanDisplayList.Add(HangmanDisplay7);
            HangmanDisplayList.Add(HangmanDisplay8);
            HangmanDisplayList.Add(HangmanDisplay9);
            HangmanDisplayList.Add(HangmanDisplay10);
            HangmanDisplayList.Add(HangmanDisplay11);
            HangmanDisplayList.Add(HangmanDisplay12);

            Setup();
        }


        public void Setup()
        {

            HangmanAnswer = MainPage.Answer;
            HangmanLevelSetup = 12 - MainPage.Level;
            char[] HangmanArray = HangmanAnswer.ToCharArray();
            foreach (char ch in HangmanArray)
            {
                string Variable = ch.ToString();
                HangmanAnswerList.Add(Variable);
            }
            while (HangmanLevelPlay < HangmanLevelSetup)
            {
                HangmanDisplayList[HangmanLevelPlay].Visibility = Visibility.Visible;
                ++HangmanLevelPlay;
            }
            SetupWordField();
            Play = true;
        }

        public void SetupWordField()
        {
            int i = 0;
            while (i < HangmanAnswerList.Count)
            {
                HangmanGoodList.Add("_");
                ++i;
            }

            AnswerGood.Text = String.Join(" ", HangmanGoodList.ToArray());
        }

        public void ChangeWordField()
        {
            AnswerGood.Text = String.Join(" ", HangmanGoodList.ToArray());
            AnswerFalse.Text= String.Join(" ", HangmanFalseList.ToArray());
        }

        public void ChangeHangman()
        {
            
            HangmanDisplayList[HangmanLevelPlay].Visibility = Visibility.Visible;
            ++HangmanLevelPlay;
        }

        public void CheckLevel()
        {
            if (HangmanLevelPlay == 12)
            {
                Result.Text = "GAME OVER THE WORD WAS "+HangmanAnswer;

            }

            if (HangmanAnswerList.Count == HangmanAttempt)
            {
                Result.Text = "YOU WON THE WORD WAS "+ HangmanAnswer;

            }

            Play = true;
        }

      

        public void CheckInput(string Letter)
        {
            Play = false;
            bool checking = true;
            while (checking == true)
            {
                for (int i = 0; i < HangmanAnswerList.Count; i++)
                {
                    if (HangmanAnswerList[i].Contains(Letter))
                    {
                        HangmanGoodList[i] = HangmanAnswerList[i];
                        HangmanAnswerList[i] = "0";
                        ++HangmanAttempt;
                    }
                        
                }


                if (!HangmanAnswerList.Contains(Letter))
                {
                    if (!HangmanFalseList.Contains(Letter) && !HangmanGoodList.Contains(Letter))
                    {
                        HangmanFalseList.Add(Letter);
                        ChangeHangman();
                    }

                    checking = false;
                    
                }
            }
  
        ChangeWordField(); 
        CheckLevel();    
        }

        private void BtnAgain_onclick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof (MainPage), null);
        }
    }
}

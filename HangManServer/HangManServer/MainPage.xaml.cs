using System;
using Windows.UI.Xaml;

namespace HangManServer
{
    public partial class MainPage
    {
        public static string SecretWord;
        public static int Level;
        
        
        public MainPage()
        {
            InitializeComponent();

            Level = 12;

            SetLevel();
        }

        private void SetLevel()
        {
            string levelDisplay = Convert.ToString(Level);
            HangmanLevel.Text = levelDisplay;
        }

        private void BtnStart_Clicked(object sender, RoutedEventArgs e)
        {
            SecretWord = AnswerTextBox.Text;
            
            Frame.Navigate(typeof(Game));
        }

        private void BtnUp_Clicked(object sender, RoutedEventArgs e)
        {
            if (Level < 12)
                Level++;

            SetLevel();
        }

        private void BtnDown_Clicked(object sender, RoutedEventArgs e)
        {
            if (Level > 0)
                Level--;
            
            SetLevel();
        }
    }
}


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

        #region Private Methods

        private void ButtonDown_Clicked(object sender, RoutedEventArgs e)
        {
            if (Level > 0)
                Level--;

            SetLevel();
        }

        private void ButtonStart_Clicked(object sender, RoutedEventArgs e)
        {
            SecretWord = AnswerTextBox.Text;

            Frame.Navigate(typeof(Game));
        }

        private void ButtonUp_Clicked(object sender, RoutedEventArgs e)
        {
            if (Level < 12)
                Level++;

            SetLevel();
        }

        private void SetLevel()
        {
            string levelDisplay = Convert.ToString(Level);
            HangmanLevel.Text = levelDisplay;
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;

namespace HangManServer
{
    public sealed partial class Game
    {
        private readonly string _secretWord;
        private readonly int _level;

        private int _hangmanLevelPlay;
        private int _hangmanAttempt;

        private List<Rectangle> _hangmanDisplayList;

        private readonly List<string> _hangmanAnswerList = new List<string>();
        private readonly List<string> _hangmanGoodList = new List<string>();
        private readonly List<string> _hangmanFalseList = new List<string>();

        private readonly Server _server;

        public Game()
        {
            InitializeComponent();

            _server = new Server(9000);
            _server.MessageReceived += CheckInputAsync;

            InitHangmanDisplay();

            _secretWord = MainPage.SecretWord;
            _level = 12 - MainPage.Level;

            InitSecretWord();
        }

        #region Private Methods

        private void ChangeHangman()
        {
            _hangmanDisplayList[_hangmanLevelPlay].Visibility = Visibility.Visible;
            ++_hangmanLevelPlay;
        }

        private void ChangeWordField()
        {
            AnswerGood.Text = string.Join(" ", _hangmanGoodList.ToArray());
            AnswerFalse.Text = string.Join(" ", _hangmanFalseList.ToArray());
        }

        private void CheckInput(string letter)
        {
            bool checking = true;
            while (checking)
            {
                for (int i = 0; i < _hangmanAnswerList.Count; i++)
                    if (_hangmanAnswerList[i].Contains(letter))
                    {
                        _hangmanGoodList[i] = _hangmanAnswerList[i];
                        _hangmanAnswerList[i] = "0";
                        ++_hangmanAttempt;
                    }

                if (!_hangmanAnswerList.Contains(letter))
                {
                    if (!_hangmanFalseList.Contains(letter) && !_hangmanGoodList.Contains(letter))
                    {
                        _hangmanFalseList.Add(letter);
                        ChangeHangman();
                    }

                    checking = false;
                }
            }

            ChangeWordField();
            CheckLevel();
        }

        private void CheckLevel()
        {
            if (_hangmanLevelPlay == 12)
                Result.Text = "GAME OVER THE WORD WAS " + _secretWord;

            if (_hangmanAnswerList.Count == _hangmanAttempt)
                Result.Text = "YOU WON THE WORD WAS " + _secretWord;
        }

        private void InitHangmanDisplay()
        {
            _hangmanDisplayList = new List<Rectangle>
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

            for (int i = 0; i < _level; i++)
                _hangmanDisplayList[i].Visibility = Visibility.Visible;
        }

        private void InitSecretWord()
        {
            foreach (char character in _secretWord)
                _hangmanAnswerList.Add(character.ToString());

            for (int i = 0; i < _hangmanAnswerList.Count; i++)
                _hangmanGoodList.Add("_");

            AnswerGood.Text = string.Join(" ", _hangmanGoodList.ToArray());
        }

        private async void CheckInputAsync(string letter)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { CheckInput(letter); });
        }

        #endregion
    }
}
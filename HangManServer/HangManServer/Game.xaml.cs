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
using Windows.Security.Cryptography.Core;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace HangManServer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Game : Page
    {
       


        public Game()
        {
            this.InitializeComponent();
            
            Setup();
        }

        public void Setup_Value(string answer)
        {
           
        }

        public void Setup()
        {

            AnswerBox.Text = MainPage.Answer;

        }


        public void Check(string letter)
        {
            if (letter == "whuttt")
            {
                //Doe Whuttttt
            }

            if (letter == "Fuck")
            {
                Debug.Write("fuckjkkkkk");
            }
        }
    }
}

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
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HangManServer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Instream server;

        

        public MainPage()
        {
            this.InitializeComponent();
            //Creer een nieuwe server die luistert naar poort 9000
            //Poortnummer mag anders zijn, maar de clients moeten naar hetzelfde nummer luisteren
            server = new Instream(9000);

            //Koppel OnDataOntvangen aan de methode die uitgevoerd worden:
            server.OnDataOntvangen += server.Server_OnDataOntvangen;
            
        }
    }
}


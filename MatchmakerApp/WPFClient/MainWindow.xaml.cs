﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFClient.SVC;
using System.ServiceModel;



namespace WPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        SVC.Client localClient = null;
        private void buttonConnect_Click(object sender, RoutedEventArgs e)
        {
            this.localClient = new SVC.Client();
            this.localClient.Name = loginTxtBoxUserName.Text.ToString();
            
        }

        private void chatButtonSend_Click(object sender, RoutedEventArgs e)
        {
            SVC.Message msg = new WPFClient.SVC.Message();
            msg.Sender = this.localClient.Name;
            msg.Content = chatTxtBoxWriteMsg.Text.ToString();
        }

        //private void chatButtonDisconnect_Click(object sender, RoutedEventArgs e)
        //{

        //}
    }
}
    
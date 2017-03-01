using System;
using System.Collections.Generic;
using System.IO.Ports;
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
using System.Windows.Threading;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // Serial
        SerialPort serial = new SerialPort();
        string recieved_data;

        //Richtextbox
        FlowDocument mcFlowDoc = new FlowDocument();
        Paragraph para = new Paragraph();


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (serial.IsOpen == false)
            {
                serial.PortName = "COM3";
                serial.BaudRate = Convert.ToInt32("9600");
                serial.Handshake = System.IO.Ports.Handshake.None;
                serial.Parity = Parity.None;
                serial.DataBits = 8;
                serial.StopBits = StopBits.One;
                serial.ReadTimeout = 200;
                serial.WriteTimeout = 50;
                serial.Open();
                serial.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(Recieve);
            }
            serial.WriteLine("Sended by WPF application ...");
        }


        private delegate void UpdateUiTextDelegate(string text);

        private void WriteData(string text)
        {
            // Assign the value of the recieved_data to the RichTextBox.
            para.Inlines.Add(text);
            mcFlowDoc.Blocks.Add(para);
            Commdata.Document = mcFlowDoc;
        }

        private void Recieve(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            // Collecting the characters received to our 'buffer' (string).
            recieved_data = serial.ReadExisting();
            Dispatcher.Invoke(DispatcherPriority.Send, new UpdateUiTextDelegate(WriteData), recieved_data);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            serial.DtrEnable = true;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            serial.DtrEnable = false;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            serial.RtsEnable = true;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            serial.RtsEnable = false;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            OperatorWindow OW = new OperatorWindow();
            OW.ShowDialog();
            Dispatcher.Invoke(DispatcherPriority.Send, new UpdateUiTextDelegate(WriteData), "OW.ShowDialog()\r\n");
        }
    }
}

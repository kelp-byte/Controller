using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Controller.Model;

namespace Controller
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Monitor : ContentPage
    {
        private List<Data> inputData;
        private List<Data> outputData;
        public Monitor(List<Data> inputData , List<Data> outputData)
        {


            InitializeComponent();
            this.inputData = inputData;
            this.outputData = outputData;
            OutputDataLstViw.ItemsSource = this.outputData;
            InputDataLstViw.ItemsSource = this.inputData;
        }
    }
}
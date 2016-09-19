using System;
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
using System.Data.SqlClient;
using EstimationsAndPlanning;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        cEAPWorkspace myWorkSpace;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (GoButton == sender)
            {
                if (rbStartNewExampleWS.IsChecked == true)
                {
                    //MessageBox.Show("Creating a Example Workspace");
                    myWorkSpace = new cEAPWorkspace();
                    myWorkSpace.createNewStdDatabase("eap_example_db");
                    myWorkSpace.populateDatabaseWithExamples("eap_example_db");


                }
                else if (rbStartNewStdWS.IsChecked == true)
                {
                    //MessageBox.Show("Creating a STD Workspace");
                    myWorkSpace = new cEAPWorkspace();
                    myWorkSpace.createNewStdDatabase("eap_std_db");


                }
                else if (rbStartNewEmptyWS.IsChecked == true)
                {
                    //MessageBox.Show("Creating an empty Workspace");
                    myWorkSpace = new cEAPWorkspace();
                    myWorkSpace.createNewEmptyDatabase("eap_db");

                }
                else if (rbContinueWithLastUsedWS.IsChecked == true)
                {
                    //MessageBox.Show("Creating an empty Workspace");
                    myWorkSpace = new cEAPWorkspace();
                    myWorkSpace.openExistingDatabase("eap_example_db");
                }
                else
                {
                    MessageBox.Show("You need to select something");

                }
                MessageBox.Show("Database created succesfully!");
            }
            else if (EndButton == sender)
            {
                 //MessageBox.Show("Closing the Workspace");
                myWorkSpace.closeDatabase();

            }

        }
    }
}

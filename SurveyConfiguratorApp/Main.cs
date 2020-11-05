using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;

namespace SurveyConfiguratorApp
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //ConfigurationManager.ConnectionStrings["surveyConnection"].ConnectionString;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }
    }
}

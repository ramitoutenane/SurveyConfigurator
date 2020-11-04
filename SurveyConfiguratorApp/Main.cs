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
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["surveyConnection"].ConnectionString);
                con.Open();
                MessageBox.Show("open");
                con.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

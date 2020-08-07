using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Exporter
{
    public partial class Setup : Form
    {
        public static Boolean connected = false;
        public static StringBuilder sb = new StringBuilder();
        public static string hostname;
        public static string user;
        public static string password;
        public static List<string> databaseList;
        public static string database;

        public Setup()
        {
            InitializeComponent();
            textBox1.Text = "192.168.1.108,1433";
            textBox2.Text = "sa";
            textBox4.Text = "Kakadu6664";
            button1.Click += new EventHandler(button1_Click);
            button3.Click += new EventHandler(button3_Click);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            database = comboBox1.SelectedItem.ToString();

            if (!String.IsNullOrEmpty(database))
            {
                Form frm2 = new Query(sb);
                Hide();
                frm2.Show();
            }
            else
            {
                MessageBox.Show("Connection details incomplete");
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            hostname = textBox1.Text.ToString();
            user = textBox2.Text.ToString();
            password = textBox4.Text.ToString();
            var canConnect = DBTest();

            if (canConnect)
            {
                getdatabaseNames();
                MessageBox.Show("Connected. Select a database");
            }
            else
            {
                MessageBox.Show("Failed to connect");
            }
        }

        public Boolean DBTest()
        {
            sb.AppendFormat("Data Source={0};", textBox1.Text.ToString());
            sb.AppendFormat("User ID={0};", textBox2.Text.ToString());
            sb.AppendFormat("Password={0}", textBox4.Text.ToString());

            using (SqlConnection connection = new SqlConnection(sb.ToString()))
            {
                connection.Open();
                connection.Close();
                return true;
            }
        }

        public void getdatabaseNames()
        {
            List<String> databaseListNames = new List<String>();

            using (SqlConnection connection = new SqlConnection(sb.ToString()))
            {
                connection.Open();
                string query = "select * from sys.databases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb');";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            databaseListNames.Add(reader.GetString(0));
                        }
                    }
                }
            }

            foreach(var db in databaseListNames)
            {
                comboBox1.Items.Add(db.ToString());
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Exporter
{
    public partial class Query : Form
    {
        public static string dbConnectionString;
        public static string finalQuery;
        public Query(StringBuilder sb)
        {
            dbConnectionString = sb.ToString();
            InitializeComponent();
        }

        private void QueryNext_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(QueryString.Text.ToString()))
                MessageBox.Show("Connection details incomplete");

            finalQuery = SymmKey.Text.ToString() + QueryString.Text.ToString();

            if(validateQuery(finalQuery))
                runQuery(finalQuery);
        }

        public void runQuery(string finalQuery)
        {
            using (SqlConnection connection = new SqlConnection(dbConnectionString))
            {
                connection.Open();
                string query = finalQuery.ToString();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //databaseListNames.Add(reader.GetString(0));
                        }
                    }
                }
            }
        }
        
        public bool validateQuery(string query)
        {
            if (query.Contains("update"))
                return false;

            if (query.Contains("Insert"))
                return false;
            
            return true;
        }
    }
}

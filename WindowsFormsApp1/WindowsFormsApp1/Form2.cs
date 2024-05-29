using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\Jovan\\source\\repos\\WindowsFormsApp1\\Restoran.accdb";

        public Form2()
        {
            InitializeComponent();
        }



        private void button1_Click(object sender, EventArgs e)
        {


            string naziv = textBox1.Text;
            double cena = Convert.ToDouble(textBox2.Text);
            dodaj_Jelo(naziv, cena);
            LoadData();


        }




        private void dodaj_Jelo(string naziv,double cena)
        {

            string query = $"INSERT INTO Jelo (naziv, cena) VALUES ('{naziv}', {cena})";
            ExecuteQuery(query);
            LoadData();
            reset();

        }



        private void ExecuteQuery(string query)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

        }

            private void LoadData()
        {


            string query = "SELECT * FROM Jelo";
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                }
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedRowIndex = dataGridView1.SelectedRows[0].Index;
                int id_jelo = Convert.ToInt32(dataGridView1.Rows[selectedRowIndex].Cells["id_jelo"].Value);
                string query = $"DELETE FROM Jelo WHERE id_jelo= {id_jelo}";
                ExecuteQuery(query);
                LoadData();
                MessageBox.Show("Jelo  successfully delete!");
            }

            reset();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
            int Id = Convert.ToInt32(dataGridView1.Rows[selectedRowIndex].Cells["id_jelo"].Value);
            string name = textBox1.Text;
            double cena = Convert.ToDouble(textBox2.Text);
            string query = $"UPDATE Jelo SET naziv = '{name}', cena = '{cena}' WHERE id_jelo = {Id}";
            ExecuteQuery(query);
            LoadData();
            MessageBox.Show("Jelo updated successfully!");
            reset();
        }




        private void Form2_Load(object sender, EventArgs e)
        {

            LoadData();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
              
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            foreach(DataGridViewRow row in dataGridView1.SelectedRows)
            {

              textBox1.Text = row.Cells[1].Value.ToString();
              textBox2.Text = row.Cells[2].Value.ToString();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            reset();
        }


        private void reset()
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }
    }
}

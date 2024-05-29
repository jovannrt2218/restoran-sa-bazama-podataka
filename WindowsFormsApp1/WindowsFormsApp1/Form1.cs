using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

       
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\Jovan\\source\\repos\\WindowsFormsApp1\\Restoran.accdb";

        Point tacka;
        Size velicina;

        public Form1()

        {

        
            InitializeComponent();
            velicina = new Size(100, 100);
            tacka = new Point(1, 1);
    
            Task t = new Task(Horizontala);
            t.Start();
            Task t1 = new Task(Vertikala);
            t1.Start();

        }

        int total=0;
        private void Form1_Load(object sender, EventArgs e)
        {


            LoadData();

            dataGridView1.ClearSelection();
            dataGridView2.ClearSelection();
            button3.Enabled = false;

            button5.Enabled = false;



           




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

        private void button1_Click(object sender, EventArgs e)
        {

            string filterText = textBox1.Text;
            string query = $"SELECT * FROM Jelo WHERE naziv LIKE '%{filterText}%'";
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
            Form2 f = new Form2();
            f.Show();
           
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            int id=0; 
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {

                                id = Convert.ToInt32(row.Cells[0].Value);
   
            }

            if (id != 0)
            {



                string query = $"SELECT pr.id_prilog,pr.naziv,pr.cena FROM Prilog pr inner join  Pripadnost p on p.id_prilog=pr.id_prilog  where id_jelo={id}";
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dataGridView2.DataSource = dataTable;
                    }
                }


            }
            dataGridView2.ClearSelection();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int IdP = 0;
            int lastID = 0;
            string query;
            int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
            int Id = Convert.ToInt32(dataGridView1.Rows[selectedRowIndex].Cells["id_jelo"].Value);

            if (dataGridView2.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView2.SelectedRows[0];

                IdP = Convert.ToInt32(dataGridView2.Rows[selectedRowIndex].Cells["id_prilog"].Value);

            }

            query = "select MAX(id_racun) from racun";


            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {

                connection.Open();

                OleDbCommand command = new OleDbCommand(query, connection);

                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    lastID = Convert.ToInt32(result);
                }

            }

            query = $"select cena from Jelo where id_jelo={Id}";
            int jelocena = 0;
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {

                connection.Open();

                OleDbCommand command = new OleDbCommand(query, connection);

                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    jelocena = Convert.ToInt32(result);
                }

            }

            query = $"select naziv  from jelo where id_jelo={Id}";

            string nazivJelo = "";
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {

                connection.Open();

                OleDbCommand command = new OleDbCommand(query, connection);

                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    nazivJelo = result.ToString();
                }

            }

            if (IdP > 0)
            {
                


                query = $"select cena from Prilog where id_prilog={IdP}";
                int prilogcena = 0;

                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {

                    connection.Open();

                    OleDbCommand command = new OleDbCommand(query, connection);

                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        prilogcena = Convert.ToInt32(result);
                    }

                }
               

            
          

                query = $"select naziv  from prilog where id_prilog={IdP}";

                string nazivPrilog = "";
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {

                    connection.Open();

                    OleDbCommand command = new OleDbCommand(query, connection);

                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        nazivPrilog = result.ToString();
                    }

                }
                query = $"select count(*) from stavka_racuna where id_prilog={IdP} and id_racun={lastID} and id_jelo={Id}";
                int broj=0;
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {

                    connection.Open();

                    OleDbCommand command = new OleDbCommand(query, connection);

                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        broj = Convert.ToInt32(result);
                    }

                }

                if (broj > 0)
                {

                    query = $"select kolicina from stavka_racuna where id_prilog={IdP} and id_racun={lastID} and id_jelo={Id}";
                    int broj2 = 0;
                    using (OleDbConnection connection = new OleDbConnection(connectionString))
                    {

                        connection.Open();

                        OleDbCommand command = new OleDbCommand(query, connection);

                        object result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            broj2 = Convert.ToInt32(result);
                        }


                    
                    }

                    query = $"select id from stavka_racuna where id_prilog={IdP} and id_racun={lastID} and id_jelo={Id}";
                    int ids = 0;
                    using (OleDbConnection connection = new OleDbConnection(connectionString))
                    {

                        connection.Open();

                        OleDbCommand command = new OleDbCommand(query, connection);

                        object result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            ids = Convert.ToInt32(result);
                        }


                    }

                    listBox1.Items.Remove(ids.ToString() + "- Jelo: " + nazivJelo + " Prilog: " + nazivPrilog + "Cena: " + (prilogcena + jelocena).ToString() + "Kolicina: " + broj2);
                    listBox1.Items.Add(ids.ToString() + "- Jelo: " + nazivJelo + " Prilog: " + nazivPrilog + "Cena: " + (prilogcena + jelocena).ToString() + "Kolicina: " + (broj2 + 1));
                    query = $"UPDATE stavka_racuna  SET kolicina={broj2 + 1}  WHERE id_jelo = {Id} and id_prilog={IdP} and id_racun={lastID}";
                    ExecuteQuery(query);

                    total = total + (prilogcena + jelocena) * (broj2);

                }
                else
                {

                    query = $"select id from stavka_racuna where id_prilog={IdP} and id_racun={lastID} and id_jelo={Id}";
                    int ids = 0;
                    using (OleDbConnection connection = new OleDbConnection(connectionString))
                    {

                        connection.Open();

                        OleDbCommand command = new OleDbCommand(query, connection);

                        object result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            ids = Convert.ToInt32(result);
                        }


                    }
                    query = $"INSERT INTO Stavka_racuna(id_racun,id_jelo,id_prilog,cenaJelo,cenaPrilog,kolicina) values({lastID},{Id},{IdP},{jelocena},{prilogcena},1)";
                    ExecuteQuery(query);
                    listBox1.Items.Add(ids.ToString() + "- Jelo: " + nazivJelo + " Prilog: " + nazivPrilog + "Cena: " + (prilogcena + jelocena).ToString() + "Kolicina: 1");
                }
                total = total + (prilogcena + jelocena);
                MessageBox.Show("Dodata je stavka na racun!!!");

            }
            else
            {

                query = $"select count(*) from stavka_racuna where id_prilog=12 and id_racun={lastID} and id_jelo={Id}";
                int broj = 0;
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {

                    connection.Open();

                    OleDbCommand command = new OleDbCommand(query, connection);

                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        broj = Convert.ToInt32(result);
                    }

                }


                if (broj > 0)
                {

                    query = $"select kolicina from stavka_racuna where id_prilog=12 and id_racun={lastID} and id_jelo={Id}";
                    int broj2 = 0;
                    using (OleDbConnection connection = new OleDbConnection(connectionString))
                    {

                        connection.Open();

                        OleDbCommand command = new OleDbCommand(query, connection);

                        object result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            broj2 = Convert.ToInt32(result);
                        }

                    
                    }


                    query = $"select id from stavka_racuna where id_prilog=12 and id_racun={lastID} and id_jelo={Id}";
                    int ids = 0;
                    using (OleDbConnection connection = new OleDbConnection(connectionString))
                    {

                        connection.Open();

                        OleDbCommand command = new OleDbCommand(query, connection);

                        object result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            ids = Convert.ToInt32(result);
                        }


                    }
                    

                    listBox1.Items.Remove(ids.ToString()+"- Jelo: " + nazivJelo + " Prilog: nema " + "Cena: " + (jelocena).ToString() + " Kolicina: " + broj2);
                    listBox1.Items.Add(ids.ToString() + "- Jelo: " + nazivJelo + " Prilog: nema " + "Cena: " + (jelocena).ToString() + " Kolicina: " + (broj2 + 1));
                    query = $"UPDATE stavka_racuna  SET kolicina={broj2 + 1}  WHERE id_jelo = {Id} and id_prilog=12 and id_racun={lastID}";
                    ExecuteQuery(query);

                    total = total + (jelocena) * (broj2);
                }
                else
                {
                    query = $"INSERT INTO Stavka_racuna(id_racun,id_jelo,id_prilog,cenaJelo,cenaPrilog,kolicina) values({lastID},{Id},12,{jelocena},0,1)";
                    ExecuteQuery(query);
                    total = total + ( jelocena);

                    query = $"select id from stavka_racuna where id_prilog=12 and id_racun={lastID} and id_jelo={Id}";
                    int ids = 0;
                    using (OleDbConnection connection = new OleDbConnection(connectionString))
                    {

                        connection.Open();

                        OleDbCommand command = new OleDbCommand(query, connection);

                        object result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            ids = Convert.ToInt32(result);
                        }


                    }

                    listBox1.Items.Add(ids.ToString() + "- Jelo: " + nazivJelo + " Prilog: nema " + "Cena: " + (jelocena).ToString()+" Kolicina: 1");

                }
                MessageBox.Show("Dodata je stavka na racun!!!");

            }

            label5.Text = total.ToString() + " DIN";

            }

        
        int lastID = 0;
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
           
                string query = $"INSERT INTO Racun(ukupna_cena,datum) values(0,Now())";
                ExecuteQuery(query);

                query = "select MAX(id_racun) from racun";
            

                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {

                    connection.Open();

                    OleDbCommand command = new OleDbCommand(query, connection);

                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        lastID = Convert.ToInt32(result);
                    }
                    listBox1.Items.Add("Racun broj: " + lastID.ToString());
                }

                button4.Enabled = false;
                button3.Enabled = true;

                button5.Enabled = true; ;

            }
            catch (Exception ex)
            {

                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            button4.Enabled = true;
            button3.Enabled=false;
            MessageBox.Show(lastID.ToString(),total.ToString());
            string query= $"UPDATE Racun SET ukupna_cena={total},datum=Now() where id_racun={lastID}";
            ExecuteQuery(query);

            MessageBox.Show("Racun je sacuvan!");

            button5.Enabled = false;
            dataGridView1.ClearSelection();
            dataGridView2.ClearSelection();

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            foreach(var selectedItem in listBox1.SelectedItems)
            {
                string itemText = selectedItem.ToString();
              

                string p = "";

                for(int i = 0; i < itemText.Length; i++)

                {
                    char g = itemText[i];
                    if (g == '-')
                    {
                        break;
                    }

                    p = p + itemText[i];
                    
              
                }

                int b = Convert.ToInt32(p);

                listBox1.Items.Remove(selectedItem);



               string query = $"select (cenaJelo+cenaPrilog)*kolicina from stavka_racuna where id={b}";
                int c = 0;
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {

                    connection.Open();

                    OleDbCommand command = new OleDbCommand(query, connection);

                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        c = Convert.ToInt32(result);
                    }


                }

                total = total - c;
                label5.Text = total.ToString() + " DIN";


                query = $"delete from stavka_racuna where id={b}";
                ExecuteQuery(query);

                MessageBox.Show("Stavka je uklonjena");

                break;

            }


        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form3 f = new Form3();
            f.Show();
         
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form4 f = new Form4();

            f.Show();
        }

        public void Horizontala()
        {
            int korak = 1;
            while (tacka.X > 0)
            {
                if (tacka.X > ClientSize.Width - velicina.Width)
                    korak = -korak;

                tacka.X += korak;
                Thread.Sleep(1);
                Invalidate();
            }
        }

        public void Vertikala()
        {
            int korak = 1;
            while (tacka.Y > 0)
            {

                if (tacka.Y > ClientSize.Height - velicina.Height)
                    korak = -korak; 

                tacka.Y += korak;
                Thread.Sleep(1);
                Invalidate();
            }
        }





        private void Animate()
        {
            {

                string query = $"select first(j.naziv) from Jelo j inner join Stavka_racuna s on j.id_jelo=s.id_jelo  where s.id_jelo in (select first(id_jelo) from (select id_jelo,count(*) as jeloB from stavka_racuna group by id_jelo order by 2 desc))";
                string jeloo = "";
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {

                    connection.Open();

                    OleDbCommand command = new OleDbCommand(query, connection);

                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        jeloo = result.ToString();
                    }
                    string currenth = jeloo;

                    label3.Text = "Najprodavanije jelo je: " + currenth;

                }
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillEllipse(Brushes.Blue, new Rectangle(tacka, velicina));
            Animate();


        }




    }
    }


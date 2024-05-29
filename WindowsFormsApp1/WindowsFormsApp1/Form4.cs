using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using System;
using System.Windows.Forms.DataVisualization.Charting;
using System.Data.OleDb;

namespace WindowsFormsApp1
{


    public partial class Form4 : Form
    {

        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\Jovan\\source\\repos\\WindowsFormsApp1\\Restoran.accdb";

        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {


            string query = "select id_jelo from Jelo";
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection))
                {
                    DataTable dataTable = new DataTable();
          
             
                    adapter.Fill(dataTable);
                    comboBox1.DisplayMember = "id_jelo";
                    comboBox1.ValueMember = "id_jelo";
                    comboBox1.DataSource = dataTable;

                }
            }



        }

        private void button1_Click(object sender, EventArgs e)
        {
           int idjela = Convert.ToInt32(comboBox1.SelectedValue.ToString());

           DateTime dt  = dateTimePicker1.Value;

            DateTime d2 = dateTimePicker2.Value;

            string query;


            if (comboBox1.SelectedIndex > 0)
            {

                int ukupno = 1;
                query = $"select sum(cenaJelo) from stavka_racuna where id_Racun in( SELECT id_racun FROM Racun where datum>=#{dt.ToShortDateString()}# and datum<=#{d2.ToShortDateString()}#)";
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {

                    connection.Open();

                    OleDbCommand command = new OleDbCommand(query, connection);

                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        ukupno = Convert.ToInt32(result);
                    }

                }




                query = $"select s.id_jelo as IdJela,j.naziv as Jelo, sum(s.cenaJelo) as Brojcano, round((sum(s.cenaJelo)/{ukupno})*100,2) as Procenat from stavka_racuna s inner join Jelo j on j.id_jelo=s.id_jelo where s.id_Racun in( SELECT id_racun FROM Racun where datum>=#{dt.ToShortDateString()}# and datum<=#{d2.ToShortDateString()}#) and s.id_jelo={idjela} group by s.id_jelo,j.naziv";
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dataGridView1.DataSource = dataTable;


                        int[] jela = dataTable.AsEnumerable()
                                                      .Select(row => row.Field<int>("IdJela"))
                                                      .ToArray();

                        double[] values = dataTable.AsEnumerable()
                                                   .Select(row => row.Field<double>("Procenat"))
                                                   .ToArray();
                        Chart pieChart = new Chart();
                        ChartArea chartArea = new ChartArea();
                        pieChart.ChartAreas.Add(chartArea);

                        Series series = new Series("Series1");
                        series.ChartType = SeriesChartType.Pie;

                        for (int i = 0; i < jela.Length; i++)
                        {
                            DataPoint dataPoint = new DataPoint();
                            dataPoint.SetValueXY(jela[i], values[i]);
                            dataPoint.IsVisibleInLegend = true;
                            dataPoint.Label = "#VALX: #VALY";
                            series.Points.Add(dataPoint);
                        }
                        pieChart.Series.Add(series);

                        Legend legend = new Legend();
                        pieChart.Legends.Add(legend);

                        Form5 chartForm = new Form5();
                        pieChart.Dock = DockStyle.Fill;
                        chartForm.Controls.Add(pieChart);


                        chartForm.ShowDialog();
                    }
                }

            }
            else
            {

                int ukupno = 1;
                query = $"select sum(cenaJelo) from stavka_racuna where id_Racun in( SELECT id_racun FROM Racun where datum>=#{dt.ToShortDateString()}# and datum<=#{d2.ToShortDateString()}#)";
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {

                    connection.Open();

                    OleDbCommand command = new OleDbCommand(query, connection);

                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        ukupno = Convert.ToInt32(result);
                    }

                }




                query = $"select s.id_jelo as IdJela,j.naziv as Jelo, sum(s.cenaJelo) as Brojcano, round((sum(s.cenaJelo)/{ukupno})*100,2) as Procenat from stavka_racuna s inner join Jelo j on j.id_jelo=s.id_jelo where s.id_Racun in( SELECT id_racun FROM Racun where datum>=#{dt.ToShortDateString()}# and datum<=#{d2.ToShortDateString()}#) group by s.id_jelo,j.naziv";
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dataGridView1.DataSource = dataTable;


                        int[] jela = dataTable.AsEnumerable()
                                                      .Select(row => row.Field<int>("IdJela"))
                                                      .ToArray();

                        double[] values = dataTable.AsEnumerable()
                                                   .Select(row => row.Field<double>("Procenat"))
                                                   .ToArray();
                        Chart pieChart = new Chart();
                        ChartArea chartArea = new ChartArea();
                        pieChart.ChartAreas.Add(chartArea);

                        Series series = new Series("Series1");
                        series.ChartType = SeriesChartType.Pie;

                        for (int i = 0; i < jela.Length; i++)
                        {
                            DataPoint dataPoint = new DataPoint();
                            dataPoint.SetValueXY(jela[i], values[i]);
                            dataPoint.IsVisibleInLegend = true;
                            dataPoint.Label = "#VALX: #VALY";
                            series.Points.Add(dataPoint);
                        }
                        pieChart.Series.Add(series);

                        Legend legend = new Legend();
                        pieChart.Legends.Add(legend);

                        Form5 chartForm = new Form5();
                        pieChart.Dock = DockStyle.Fill;
                        chartForm.Controls.Add(pieChart);


                        chartForm.ShowDialog();
                    }
                }



            }

        }
    }
}

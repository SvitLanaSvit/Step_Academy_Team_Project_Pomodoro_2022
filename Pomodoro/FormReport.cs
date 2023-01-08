using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http.Headers;
using Pomodoro.DataBase.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using Pomodoro.DataBase.ViewClasses;
using Pomodoro.DataBase.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using LiveCharts.Wpf.Charts.Base;
using LiveCharts.WinForms;
using LiveCharts.Definitions.Charts;

namespace Pomodoro
{
    public partial class FormReport : Form
    {

        DbContextOptions<MyPomodoroProjectContext> options; //diagramm
        string connStrDiagramm = "workstation id=PomodoroWPV122.mssql.somee.com;packet size=4096;user id=ccorbit_SQLLogin_1;pwd=awxyde1ikc;data source=PomodoroWPV122.mssql.somee.com;persist security info=False;initial catalog=PomodoroWPV122";

        public FormReport(string connstr)
        {
            InitializeComponent();
            ConnStr = connstr;
            listViewUserTime.Columns.Add("User", listViewUserTime.Width/2-2, HorizontalAlignment.Left);
            listViewUserTime.Columns.Add("WorkTime", listViewUserTime.Width/2-2, HorizontalAlignment.Left);
            listViewUserTime.View = View.Details;

        }
        string ConnStr;
        private void btnDay_Click(object sender, EventArgs e)
        {
            cartesianChart1.Series.Clear();
            cartesianChart1.AxisX.Clear();
            cartesianChart1.AxisY.Clear();

            //diagramm
            string path = "userForDiagramm.txt";
            using FileStream fs = File.OpenRead(path);
            using StreamReader sr = new StreamReader(fs);

            string line;
            string user = "";
            while ((line = sr.ReadLine()!) != null)
            {
                user += line;
            }

            using (SqlConnection connection = new SqlConnection(connStrDiagramm))
            {
                var dateNow = DateTime.Now;

                string queryId = $"SELECT Id FROM Users Where Login = '{user}'";
                int userId = connection.ExecuteScalar<int>(queryId);

                string queryCount = $"SELECT WorkTime FROM Tasks Where UserId = {userId} and TaskState = 'Finished' and DateOfFinish = '{dateNow.ToString("dd/MM/yyyy")}'";
                int[] arrayOfCount = connection.Query<int>(queryCount).ToArray();
                
                string queryData = $"SELECT DateOfFinish FROM Tasks Where UserId = {userId} and TaskState = 'Finished' and DateOfFinish = '{dateNow.ToString("dd/MM/yyyy")}'";
                string[] arrayOfDate = connection.Query<string>(queryData).ToArray();

                ChartValues<double> chartValues = new ChartValues<double>();
                for (int i = 0; i < arrayOfCount.Length; i++)
                {
                    chartValues.Add(arrayOfCount[i]);
                }
                string[] arr = new string[arrayOfCount.Length];
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = (i + 1).ToString();
                }
                
                DiagrammDay(chartValues, arr);
            }
        }

        private void btnWeek_Click(object sender, EventArgs e)
        {
            cartesianChart1.Series.Clear();
            cartesianChart1.AxisX.Clear();
            cartesianChart1.AxisY.Clear();
            //diagramm
            string path = "userForDiagramm.txt";
            using FileStream fs = File.OpenRead(path);
            using StreamReader sr = new StreamReader(fs);

            string line;
            string user = "";
            while ((line = sr.ReadLine()!) != null)
            {
                user += line;
            }

            using (SqlConnection connection = new SqlConnection(connStrDiagramm))
            {
                var dateNow = DateTime.Today.AddDays(-7);

                string queryId = $"SELECT Id FROM Users Where Login = '{user}'";
                int userId = connection.ExecuteScalar<int>(queryId);

                string queryCount = $"SELECT WorkTime FROM Tasks Where UserId = {userId} and TaskState = 'Finished' and DateOfFinish >= '{dateNow.ToString("dd/MM/yyyy")}'";
                int[] arrayOfCount = connection.Query<int>(queryCount).ToArray();

                string queryData = $"SELECT DateOfFinish FROM Tasks Where UserId = {userId} and TaskState = 'Finished' and DateOfFinish >= '{dateNow.ToString("dd/MM/yyyy")}'";
                string[] arrayOfDate = connection.Query<string>(queryData).ToArray();

                ChartValues<double> chartValues = new ChartValues<double>();
                for (int i = 0; i < arrayOfCount.Length; i++)
                {
                    chartValues.Add(arrayOfCount[i]);
                }
                string[] arr = new string[arrayOfCount.Length];
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = (i + 1).ToString();
                }

                DiagrammWeek(chartValues, arr);
            }
        }

        private void btnMonth_Click(object sender, EventArgs e)
        {
            cartesianChart1.Series.Clear();
            cartesianChart1.AxisX.Clear();
            cartesianChart1.AxisY.Clear();
            //diagramm
            string path = "userForDiagramm.txt";
            using FileStream fs = File.OpenRead(path);
            using StreamReader sr = new StreamReader(fs);

            string line;
            string user = "";
            while ((line = sr.ReadLine()!) != null)
            {
                user += line;
            }

            using (SqlConnection connection = new SqlConnection(connStrDiagramm))
            {
                var dateNow = DateTime.Today.AddDays(-30);

                string queryId = $"SELECT Id FROM Users Where Login = '{user}'";
                int userId = connection.ExecuteScalar<int>(queryId);

                string queryCount = $"SELECT WorkTime FROM Tasks Where UserId = {userId} and TaskState = 'Finished' and DateOfFinish >= '{dateNow.ToString("dd/MM/yyyy")}'";
                int[] arrayOfCount = connection.Query<int>(queryCount).ToArray();

                string queryData = $"SELECT DateOfFinish FROM Tasks Where UserId = {userId} and TaskState = 'Finished' and DateOfFinish >= '{dateNow.ToString("dd/MM/yyyy")}'";
                string[] arrayOfDate = connection.Query<string>(queryData).ToArray();

                ChartValues<double> chartValues = new ChartValues<double>();
                for (int i = 0; i < arrayOfCount.Length; i++)
                {
                    chartValues.Add(arrayOfCount[i]);
                }
                string[] arr = new string[arrayOfCount.Length];
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = (i + 1).ToString();
                }

                DiagrammMonth(chartValues, arr);
            }
        }

        private void FormReport_Load(object sender, EventArgs e)
        {
            ChartValues<double> chartValues = new ChartValues<double> { 5, 3, 2, 4, 5 };
            string[] arr = new string[] { "1", "2", "3", "4", "5" };
            Diagramm(chartValues, arr);
        }

        private void Diagramm(ChartValues<double> chartValues, string[] arr)
        {
            cartesianChart1.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = chartValues
                }
            };

            cartesianChart1.AxisX.Add(new Axis
            {
                Title = "",
                Labels = arr
            });

            cartesianChart1.LegendLocation = LegendLocation.Right;
            cartesianChart1.DataClick += CartesianChart1_DataClick;
        }

        private void DiagrammDay(ChartValues<double> chartValues, string[] arr)
        {
            var date = DateTime.Now;
            cartesianChart1.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = chartValues
                }
            };

            cartesianChart1.AxisX.Add(new Axis
            {
                Title = $"{date.ToString("dd/MM/yyyy")}",
                Labels = arr
            });

            cartesianChart1.LegendLocation = LegendLocation.Right;
            cartesianChart1.DataClick += CartesianChart1_DataClick;
        }

        private void DiagrammWeek(ChartValues<double> chartValues, string[] arr)
        {
            var date = DateTime.Today.AddDays(-7);
            var dateNow = DateTime.Now;
            cartesianChart1.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = chartValues
                }
            };

            cartesianChart1.AxisX.Add(new Axis
            {
                Title = $"{date.ToString("dd/MM/yyyy")} - {dateNow.ToString("dd/MM/yyyy")}",
                Labels = arr
            });

            cartesianChart1.LegendLocation = LegendLocation.Right;
            cartesianChart1.DataClick += CartesianChart1_DataClick;
        }

        private void DiagrammMonth(ChartValues<double> chartValues, string[] arr)
        {
            var date = DateTime.Today.AddDays(-30);
            var dateNow = DateTime.Now;
            cartesianChart1.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = chartValues
                }
            };

            cartesianChart1.AxisX.Add(new Axis
            {
                Title = $"{date.ToString("dd/MM/yyyy")} - {dateNow.ToString("dd/MM/yyyy")}",
                Labels = arr
            });

            cartesianChart1.LegendLocation = LegendLocation.Right;
            cartesianChart1.DataClick += CartesianChart1_DataClick;
        }

        private void CartesianChart1_DataClick(object sender, ChartPoint chartPoint)
        {
            MessageBox.Show("You clicked (" + chartPoint.X + "," + chartPoint.Y + "minutes )");
        }

        private async void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabControl1.SelectedIndex==2)
            {
                listViewUserTime.Items.Clear();
               

                string query = "Select top 10 U.Login as 'User' ,Sum(T.WorkTime) as WorkTime from Users U join Tasks T on T.UserId = U.Id group by U.Login order by Sum(T.WorkTime) desc";
                using (var connection = new SqlConnection(ConnStr))
                {
                    var buf = await connection.QueryAsync<RankClass>(query);
                    foreach (var item in buf)
                    {
                        ListViewItem it = listViewUserTime.Items.Add(item.User);
                        it.SubItems.Add($"{item.WorkTime}");  
                    }
                }
            }
        }

        private void listViewUserTime_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        
    }
}

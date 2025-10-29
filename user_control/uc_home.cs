using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using BLL;

namespace EcoProject.user_control
{
    public partial class uc_home : UserControl
    {
        DateTime current_date = DateTime.Today;
        Home_BLL home;

        public uc_home()
        {
            home = new Home_BLL();
            InitializeComponent();
            InitializeDataGridView();
            dateTimePicker1.Value = new DateTime(DateTime.Now.Year, 1, 1);
            dateTimePicker2.Value = current_date;

            LoadKhongKhiData();
            LoadNuocMatData();
            LoadKhiThaiData();
            dateTimePicker1.ValueChanged += new EventHandler(dateTimePicker_ValueChanged);
            dateTimePicker2.ValueChanged += new EventHandler(dateTimePicker_ValueChanged);

            PredictFutureTrends();



        }

        private void uc_home_Load(object sender, EventArgs e)
        {

        }




        private void guna2HtmlLabel4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            // Clear the data grid and reload the data
            guna2DataGridView1.Rows.Clear();

            // Get the new date range from the DateTimePickers
            DateTime selectedStartDate = dateTimePicker1.Value;
            DateTime selectedEndDate = dateTimePicker2.Value;

            // Ensure start date is before end date
            while (selectedStartDate > selectedEndDate)
            {
                check_valid_date.Text = "Ngày bắt đầu không thể nằm sau ngày kết thúc.";
                check_valid_date.Visible = true;
                //MessageBox.Show("Ngày bắt đầu không thể nằm sau ngày kết thúc.");
                chartKhongKhi.Visible = false;
                chartKhiThai.Visible = false;
                chartNuocMat.Visible = false;
                panel_hold_list.Visible = false;
                lbl_trend.Text = $"Dự đoán: Không có thành phần có thể vượt quá ngưỡng cho phép thường xuyên.";
                return;

            }

            check_valid_date.Visible = false;
            chartKhongKhi.Visible = true;
            chartKhiThai.Visible = true;
            chartNuocMat.Visible = true;
            // Reload all charts with the updated range
            reload_chart_measure();
            panel_hold_list.Visible = true;

            // Optionally log the date change for debugging

            //MessageBox.Show($"Date range updated: {selectedStartDate} to {selectedEndDate}");
        }
        private void LoadKhongKhiData()
        {

            DataTable khongkhi_matching = home.lay_du_lieu_KhongKhi();


            // Initialize pollutants
            string[] pollutants = { "CO", "SO2", "O3", "PM10", "PM2dot5", "NhietDo", "NO2" };

            try
            {
                // Prepare the chart
                chartKhongKhi.Series.Clear();
                chartKhongKhi.ChartAreas[0].AxisX.Title = "Không Khí";
                chartKhongKhi.ChartAreas[0].AxisY.Title = "Độ trùng khớp (%)";

                // Configure X-axis label style and interval
                chartKhongKhi.ChartAreas[0].AxisX.LabelStyle.Angle = -45; // Rotate labels for better visibility
                chartKhongKhi.ChartAreas[0].AxisX.Interval = 1; // Ensure all labels are displayed
                chartKhongKhi.ChartAreas[0].AxisX.LabelStyle.IsStaggered = false; // Optional: Avoid staggered labels

                // Create a new series for the chart
                Series matchSeries = new Series("Độ trùng khớp")
                {
                    ChartType = SeriesChartType.Column,
                    XValueType = ChartValueType.String
                };
                chartKhongKhi.Series.Add(matchSeries);

                if (khongkhi_matching.Rows.Count > 0)
                {
                    DataRow madh_mc = khongkhi_matching.Rows[0]; // Match the first row
                    DataRow vitri_mc = khongkhi_matching.Rows[0]; // Match the first row



                    DataTable khongKhiData = home.khong_khi_data(dateTimePicker2.Value, dateTimePicker1.Value, khongkhi_matching);
                    DataTable chiTieuData = home.chi_tieu_khong_khi(khongkhi_matching);

                    if (khongKhiData.Rows.Count > 0 && chiTieuData.Rows.Count > 0)
                    {
                        DataRow measuredRow = khongKhiData.Rows[0];
                        DataRow standardRow = chiTieuData.Rows[0];

                        // Calculate percentage match for each pollutant
                        load_list_chart(pollutants, "Không Khí", measuredRow, standardRow, matchSeries);
                    }
                    else
                    {
                        // No matching data, initialize chart with 0% for all pollutants
                        foreach (string pollutant in pollutants)
                        {
                            matchSeries.Points.AddXY(pollutant, 0);
                        }
                    }
                }
                else
                {
                    // No matching data, initialize chart with 0% for all pollutants
                    foreach (string pollutant in pollutants)
                    {
                        matchSeries.Points.AddXY(pollutant, 0);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }



        private void LoadNuocMatData()
        {


            // Retrieve matching data for ViTriLayMau and MaDH
            DataTable nuocmat_matching = home.lay_du_lieu_NuocMat();

            // Initialize pollutants
            string[] pollutants = { "TDS", "NhietDo", "pH", "PO4", "NH4", "tongP", "tongN", "TOC", "TSS", "COD", "DO", "NO3" };

            try
            {
                // Prepare the chart
                chartNuocMat.Series.Clear();
                chartNuocMat.ChartAreas[0].AxisX.Title = "Nước Mặt";
                chartNuocMat.ChartAreas[0].AxisY.Title = "Độ trùng khớp (%)";

                // Configure X-axis label style and interval
                chartNuocMat.ChartAreas[0].AxisX.LabelStyle.Angle = -45; // Rotate labels for better visibility
                chartNuocMat.ChartAreas[0].AxisX.Interval = 1; // Ensure all labels are displayed
                chartNuocMat.ChartAreas[0].AxisX.LabelStyle.IsStaggered = false; // Optional: Avoid staggered labels

                // Create a new series for the chart
                Series matchSeries = new Series("Độ trùng khớp")
                {
                    ChartType = SeriesChartType.Column,
                    XValueType = ChartValueType.String
                };
                chartNuocMat.Series.Add(matchSeries);

                if (nuocmat_matching.Rows.Count > 0)
                {
                    DataRow madh_mc = nuocmat_matching.Rows[0]; // Match the first row
                    DataRow vitri_mc = nuocmat_matching.Rows[0]; // Match the first row



                    DataTable nuocMatData = home.nuoc_mat_data(dateTimePicker2.Value, dateTimePicker1.Value, nuocmat_matching);
                    DataTable chiTieuData = home.chi_tieu_nuoc_mat(nuocmat_matching);

                    if (nuocMatData.Rows.Count > 0 && chiTieuData.Rows.Count > 0)
                    {
                        DataRow measuredRow = nuocMatData.Rows[0];
                        DataRow standardRow = chiTieuData.Rows[0];

                        // Calculate percentage match for each pollutant
                        load_list_chart(pollutants, "Nước Mặt", measuredRow, standardRow, matchSeries);
                    }
                    else
                    {
                        // No matching data, initialize chart with 0% for all pollutants
                        foreach (string pollutant in pollutants)
                        {
                            matchSeries.Points.AddXY(pollutant, 0);
                        }
                    }
                }
                else
                {
                    // No matching data, initialize chart with 0% for all pollutants
                    foreach (string pollutant in pollutants)
                    {
                        matchSeries.Points.AddXY(pollutant, 0);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        private void LoadKhiThaiData()
        {
            string[] pollutants = { "CO", "NhietDo", "NO2", "O2", "Hg", "PM", "NH3", "N_O", "ApSuat", "SO2", "H2S" };
            DataTable khithai_matching = home.lay_du_lieu_KhiThai();


            try
            {
                // Prepare the chart
                chartKhiThai.Series.Clear();
                chartKhiThai.ChartAreas[0].AxisX.Title = "Khí Thải";
                chartKhiThai.ChartAreas[0].AxisY.Title = "Độ trùng khớp (%)";

                // Configure X-axis label style and interval
                chartKhiThai.ChartAreas[0].AxisX.LabelStyle.Angle = -45; // Rotate labels for better visibility
                chartKhiThai.ChartAreas[0].AxisX.Interval = 1; // Ensure all labels are displayed
                chartKhiThai.ChartAreas[0].AxisX.LabelStyle.IsStaggered = false; // Optional: Avoid staggered labels

                // Create a new series for the chart
                Series matchSeries = new Series("Độ trùng khớp")
                {
                    ChartType = SeriesChartType.Column,
                    XValueType = ChartValueType.String
                };
                chartKhiThai.Series.Add(matchSeries);

                if (khithai_matching.Rows.Count > 0)
                {
                    DataTable khiThaiData = home.khi_thai_data(dateTimePicker2.Value, dateTimePicker1.Value, khithai_matching);
                    DataTable chiTieuData = home.chi_tieu_khi_thai(khithai_matching);

                    if (khiThaiData.Rows.Count > 0 && chiTieuData.Rows.Count > 0)
                    {
                        DataRow measuredRow = khiThaiData.Rows[0];
                        DataRow standardRow = chiTieuData.Rows[0];

                        // Calculate percentage match for each pollutant
                        load_list_chart(pollutants, "Khí thải", measuredRow, standardRow, matchSeries);
                    }
                    else
                    {
                        // No matching data, initialize chart with 0% for all pollutants
                        foreach (string pollutant in pollutants)
                        {
                            matchSeries.Points.AddXY(pollutant, 0);
                        }
                    }
                }
                else
                {
                    // No matching data, initialize chart with 0% for all pollutants
                    foreach (string pollutant in pollutants)
                    {
                        matchSeries.Points.AddXY(pollutant, 0);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(dateTimePicker2.Value.ToString());
                MessageBox.Show(e.Message);
            }
        }



        private void InitializeDataGridView()
        {

            guna2DataGridView1.Rows.Clear();

            guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            guna2DataGridView1.AllowUserToAddRows = false;
            guna2DataGridView1.ReadOnly = true;
        }

        private void load_list_chart(string[] pollutants, string loai_khi_thai, DataRow measuredRow, DataRow standardRow, Series matchSeries)
        {
            foreach (string pollutant in pollutants)
            {
                double measuredValue = Convert.ToDouble(measuredRow[pollutant]);
                double standardValue = Convert.ToDouble(standardRow[pollutant]);
                double matchPercentage = (measuredValue / standardValue) * 100;

                int pointIndex = matchSeries.Points.AddXY(pollutant, matchPercentage);

                if (matchPercentage > 100 || matchPercentage <= 80)
                {
                    matchSeries.Points[pointIndex].Color = Color.Red;

                    double exceedPercent = Math.Abs(matchPercentage - 100);
                    guna2DataGridView1.Rows.Add(
                        loai_khi_thai,                  // Chart type
                        pollutant,                      // Pollutant
                        matchPercentage.ToString("F2") + "%",  // Match percentage
                        exceedPercent.ToString("F2") + "%"     // Exceed percentage
                    );


                }
            }
            SummarizeCriticalData();
            PredictFutureTrends();
            //reload_chart_measure();
        }

        private void reload_chart_measure()
        {
            SummarizeCriticalData();
            PredictFutureTrends();
            LoadKhongKhiData();
            LoadNuocMatData();
            LoadKhiThaiData();
        }

        private void SummarizeCriticalData()
        {
            int totalCritical = guna2DataGridView1.Rows.Count;
            double totalDeviation = 0;

            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                string deviationString = row.Cells["SaiSo"].Value.ToString().Replace("%", "");
                totalDeviation += Convert.ToDouble(deviationString);
            }

            label_sum_pollutant.Text = $"Tổng thành phần không đạt chỉ tiêu: {totalCritical}";
            label_trung_binh.Text = $"Trung bình sai số: {(totalDeviation / totalCritical):F2}%";
            label_sum_pollutant.Visible = true;
            label_trung_binh.Visible = true;
            lbl_trend.Visible = true;
        }




        private void PredictFutureTrends()
        {
            Dictionary<string, int> criticalCounts = new Dictionary<string, int>();

            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                string pollutant = row.Cells["ThanhPhan"].Value.ToString();

                if (!criticalCounts.ContainsKey(pollutant))
                    criticalCounts[pollutant] = 0;

                criticalCounts[pollutant]++;
            }

            if (criticalCounts.Count > 0)
            {
                string mostCritical = criticalCounts.OrderByDescending(x => x.Value).First().Key;
                lbl_trend.Text = $"Dự đoán: {mostCritical} có thể vượt quá ngưỡng cho phép thường xuyên.";
                lbl_trend.Visible = true;
            }
        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void label_sum_pollutant_Click(object sender, EventArgs e)
        {

        }
    }
}
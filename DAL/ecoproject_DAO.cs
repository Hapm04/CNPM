using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace EcoProject.DAO
{
    public class DataProvider
    {
        string relativePath = @"Database\QuanTracMoiTruong.mdf";
        string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
        private string fullPath;
        private string connectString;

        public DataProvider()
        {
            fullPath = Path.Combine(projectDirectory, relativePath);
            connectString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={fullPath};Integrated Security=True;";
        }
        public DataTable ExecuteQuery(string query, object[] parameter = null)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(query, conn);

                if (parameter != null && parameter.Length > 0)
                {
                    // Danh sách để theo dõi các tham số đã được thêm vào
                    HashSet<string> addedParameters = new HashSet<string>();

                    string[] listPara = query.Split(' ');
                    int i = 0;

                    foreach (string item in listPara)
                    {
                        // Kiểm tra nếu từ này là tham số (bắt đầu bằng @)
                        if (item.StartsWith("@"))
                        {
                            // Đảm bảo chỉ thêm tham số vào khi chưa được thêm vào cmd
                            if (!addedParameters.Contains(item) && i < parameter.Length)
                            {
                                cmd.Parameters.AddWithValue(item, parameter[i]);
                                addedParameters.Add(item);  // Đánh dấu tham số này là đã được thêm vào
                                i++;
                            }
                        }
                    }
                }

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);

                conn.Close();
            }
            return dt;
        }



        public int ExecuteNonQuery(string query, object[] parameter = null)
        {
            int data = 0;

            using (SqlConnection conn = new SqlConnection(connectString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(query, conn);

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }
                data = cmd.ExecuteNonQuery();
                conn.Close();
            }
            return data;
        }

        public object ExecuteScalar(string query, object[] parameter = null)
        {
            object data = 0;

            using (SqlConnection conn = new SqlConnection(connectString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(query, conn);

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }

                data = cmd.ExecuteScalar();

                conn.Close();
            }

            return data;
        }
    }
}
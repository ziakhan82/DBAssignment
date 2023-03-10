// See https://aka.ms/new-console-template for more information

using Microsoft.Data.SqlClient;
using System.Data;
class Program
{
    static string connectionString = "Data Source=LAPTOP-8U7GLB0P;Initial Catalog=Company;Integrated Security=True;Encrypt=False;TrustServerCertificate=False;Trusted_connection=True;";
    static void Main(string[] args)
    {
        


        //  Test usp_CreateDepartment
        //Console.WriteLine("Creating a new department...");
        //CreateDepartment("IT", "666884444");


        //Test usp_UpdateDepartmentName
        //Console.WriteLine("Updating department name...");
        //UpdateDepartmentName(6, "New Department Name");


        //Test usp_UpdateDepartmentManager
        //UpdateDepartmentManager(6, "999887777");

        // Test usp_DeleteDepartment(DNumber)
        //DeleteDepartment(6);




        //int dNumber = 7;
        //try
        //{
        //    DataTable department = GetDepartment(connectionString, dNumber);
        //    Console.WriteLine("Department information:");
        //    Console.WriteLine("DNumber: {0}", department.Rows[0]["DNumber"]);
        //    Console.WriteLine("DName: {0}", department.Rows[0]["DName"]);
        //    Console.WriteLine("MgrSSN: {0}", department.Rows[0]["MgrSSN"]);
        //    Console.WriteLine("MgrStartDate: {0}", department.Rows[0]["MgrStartDate"]);
        //    Console.WriteLine("EmpCount: {0}", department.Rows[0]["EmpCount"]);
        //}
        //catch (SqlException ex)
        //{
        //    Console.WriteLine("Error getting department information: {0}", ex.Message);
        //}





        //Test "EXEC usp_GetAllDepartments"
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            SqlDataReader reader = GetAllDepartments(connection);

            Console.WriteLine("{0,-10} {1,-20} {2,-15}", "DNumber", "DName", "EmpCount");
            Console.WriteLine("----------------------------------------------------------");

            while (reader.Read())
            {
                int dNumber = (int)reader["DNumber"];
                string dName = (string)reader["DName"];
                int empCount = (int)reader["EmpCount"];

                Console.WriteLine("{0,-10} {1,-20} {2,-15}", dNumber, dName, empCount);
            }

            reader.Close();
        }
    }



    static int CreateDepartment(string DName, string MgrSSN)
        {
            int DNumber = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("usp_CreateDepartment", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@DName", DName);
                command.Parameters.AddWithValue("@MgrSSN", MgrSSN);

                connection.Open();
                try
                {
                    DNumber = (int)command.ExecuteScalar();
                    Console.WriteLine("New department created");
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
                connection.Close();
            }

            return DNumber;
        }

        static void UpdateDepartmentName(int DNumber, string DName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand command = new SqlCommand("usp_UpdateDepartmentName", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@DNumber", DNumber);
                command.Parameters.AddWithValue("@DName", DName);

                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Department name updated");
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
                connection.Close();
            }
        }

        static void UpdateDepartmentManager(int dNumber, string newMgrSSN)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("usp_UpdateDepartmentManager", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@DNumber", dNumber));
                    command.Parameters.Add(new SqlParameter("@MgrSSN", newMgrSSN));

                    try
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Department manager updated successfully!");
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    connection.Close();

                }
            }

        }
        static void DeleteDepartment(int dNumber)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("usp_DeleteDepartment", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@DNumber", dNumber));

                    try
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine($"Department deleted successfully! with Id {dNumber}");
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("Error deleting department: {0}", ex.Message);
                    }
                }
            }

        }


        static DataTable GetDepartment(string connectionString, int dNumber)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("usp_GetDepartment", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@DNumber", dNumber));

                    try
                    {
                        DataTable result = new DataTable();
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(result);
                        return result;
                    }
                    catch (SqlException ex)
                    {
                        throw;
                    }
                }
                Console.ReadKey();
            }
        }

        static SqlDataReader GetAllDepartments(SqlConnection connection)
        {
        string sql = "EXEC usp_GetAllDepartments";
        SqlCommand command = new SqlCommand(sql, connection);
        return command.ExecuteReader();
        
        }
    }








        
    

    


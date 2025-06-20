using System.Data.SqlClient;
using ChatroomDesktop.Models;

namespace ChatServer;

public class SQLOperations
{
    public static bool SendSQLSignup(string sql, SignupMessage? message)
    {
        var name = message.Username;
        var password = message.Password;
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;";
        int rowsAffected;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            Console.WriteLine($"Sending New User to Database");
            try
            {
                sqlConnection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine($"Connected to Database");
            using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
            {
                Console.WriteLine("New Command");
                sqlCommand.Parameters.Add("@username", System.Data.SqlDbType.VarChar).Value = name;
                sqlCommand.Parameters.Add("@password", System.Data.SqlDbType.VarChar).Value = password;
                Console.WriteLine("Executing Command");
                rowsAffected = sqlCommand.ExecuteNonQuery();
            }
        }
        Console.WriteLine($"rows affected: {rowsAffected}");
        if (rowsAffected <= 0)
        {
            Console.WriteLine("No rows affected");
            return false;
        }
        else
        {
            
            return true;
        }
    }

    public static (string, bool) SendSQLLogin(string sql, LoginMessage? message)
    {
        var name = message.Username;
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;";
        object result;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            Console.WriteLine($"Sending New User to Database");
            try
            {
                sqlConnection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine($"Connected to Database");
            using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
            {
                Console.WriteLine("New Command");
                sqlCommand.Parameters.Add("@username", System.Data.SqlDbType.VarChar).Value = name;
                Console.WriteLine("Executing Command");
                result = sqlCommand.ExecuteScalar();
            }
        }
        if (result == null)
        {
            Console.WriteLine("No rows affected");
            return ("", false);
        }
        else
        {
            string retrievedPassword = (string)result;
            Console.WriteLine(retrievedPassword);
            return (retrievedPassword, true);
        }
    }
}
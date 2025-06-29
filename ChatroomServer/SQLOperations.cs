using Microsoft.Data.SqlClient;
using ChatroomDesktop.Models;
using SqlDataReader = Microsoft.Data.SqlClient.SqlDataReader;

namespace ChatServer;

public class SQLOperations
{
    
    private string connectionString =@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;";
    private object SQLLock = new object();

    public SQLOperations()
    {
        
    }
    public async Task<(ConnectMessage,bool)> SendSQLSignup(string sql, SignupMessage? message)
    {
        var name = message.Username;
        var password = message.Password;
        bool success = true;
        SqlDataReader result;
        ConnectMessage connectMessage = new ConnectMessage();
        using (SqlConnection sqlConnection = new SqlConnection(this.connectionString))
        {
            lock (SQLLock)
            {
                Console.WriteLine($"Sending New User to Database");
                try
                {
                    sqlConnection.Open();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    success = false;
                }

                Console.WriteLine($"Connected to Database");
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    Console.WriteLine("New Command");
                    sqlCommand.Parameters.Add("@username", System.Data.SqlDbType.VarChar).Value = name;
                    sqlCommand.Parameters.Add("@password", System.Data.SqlDbType.VarChar).Value = password;
                    Console.WriteLine("Executing Command");
                    result = sqlCommand.ExecuteReader();
                }
            }
            if (success == true)
            {
                connectMessage.Username = name;
                while (await result.ReadAsync())
                {
                    if (connectMessage.Userid == 0)
                    {
                        connectMessage.Userid = result.GetInt32(0);
                    }
                
                }
            }
        }
        
        return (connectMessage, success);
    }

    public async Task<(ConnectMessage, string, bool)> SendSQLLogin(string sql, LoginRequestMessage? message)
    {
        var name = message.Username;
        string password = null;
        bool success = true;
        SqlDataReader result;
        ConnectMessage connectMessage = new ConnectMessage();
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            lock (SQLLock)
            {
                Console.WriteLine($"Checking Login Request to Server");
                try
                {
                    sqlConnection.Open();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    success = false;
                }
                finally
                {
                    Console.WriteLine($"Connected to Database");
                    using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                    {
                        Console.WriteLine("New Command");
                        sqlCommand.Parameters.Add("@username", System.Data.SqlDbType.VarChar).Value = name;
                        Console.WriteLine("Executing Command");
                        result = sqlCommand.ExecuteReader();
                    }
                }
            }

            if (success)
            {
                int userId = 0;
                List<GroupModel> groups = new List<GroupModel>();
                while (await result.ReadAsync())
                {
                    if (userId == 0)
                    {
                        userId = result.GetInt32(0);
                    }

                    if (password == null)
                    {
                        password = result.GetString(1);
                    }

                    if (!result.IsDBNull(2))
                    {
                        var group = new GroupModel();
                        group.GroupName = result.GetString(2);
                        group.GroupId = result.GetString(3);
                        groups.Add(group);
                    }
                }

                connectMessage.Username = name;
                connectMessage.Userid = userId;
                connectMessage.GroupList = groups;
            }

        }

        return (connectMessage, password, success);

    }

    public bool SendNewGroup(String sql, string groupName, string groupCode)
    {
        object result;
        lock (SQLLock)
        {
            try
            {

                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    Console.WriteLine($"Sending New Group to Database");
                    try
                    {
                        sqlConnection.Open();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw new Exception("Failed to send New Group to Database");
                    }
                    finally
                    {
                        Console.WriteLine($"Connected to Database");
                        using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                        {
                            Console.WriteLine("New Command");
                            sqlCommand.Parameters.Add("@Name", System.Data.SqlDbType.VarChar).Value = groupName;
                            sqlCommand.Parameters.Add("@Code", System.Data.SqlDbType.VarChar).Value = groupCode;
                            Console.WriteLine("Executing Command");
                            result = sqlCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send New Group to Database");
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        if (result == null)
        {
            Console.WriteLine("No rows affected");
            return false;
        }
        
        return true;
    }
}
using Microsoft.Data.SqlClient;
using ChatroomDesktop.Models;
using SqlDataReader = Microsoft.Data.SqlClient.SqlDataReader;

namespace ChatServer;

public class SQLOperations
{
    
    private readonly string connectionString =@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;";
    private readonly object _sqlLock = new object();

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
        try
        {
            using (SqlConnection sqlConnection = new SqlConnection(this.connectionString))
            {
                lock (_sqlLock)
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
                    List<GroupModel> groups = new List<GroupModel>();
                    connectMessage.GroupList = groups;
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
        }
        catch (SqlException ex)
        {
            if (ex.Number == 2627 || ex.Number == 2601)
            {
                Console.WriteLine("Signup failed: Username already exists.");
                success = false;
            }
            else
            {
                Console.WriteLine("Database error: " + ex.Message);
                success = false;
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
            lock (_sqlLock)
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
        lock (_sqlLock)
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

    public async Task<(ConfirmGroupJoinMessage,bool)> SendJoinGroup(string sql, int userId, string groupCode)
    {
        SqlDataReader result;
        bool success = true;
        string groupName = "";
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            lock (_sqlLock)
            {
                Console.WriteLine($"Sending Join Group to Database");
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
                    sqlCommand.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = userId;
                    sqlCommand.Parameters.Add("@GroupCode", System.Data.SqlDbType.VarChar).Value = groupCode;
                    Console.WriteLine("Executing Command");
                    result = sqlCommand.ExecuteReader();
                }
            }
            if (success == true)
            {
                while (await result.ReadAsync())
                {
                    if (groupName.Length == 0)
                    {
                        groupName = result.GetString(0);
                    }
                }
            }
        }
        ConfirmGroupJoinMessage confirmGroupJoinMessage = new ConfirmGroupJoinMessage();
        confirmGroupJoinMessage.UserId = userId;
        confirmGroupJoinMessage.GroupCode = groupCode;
        confirmGroupJoinMessage.GroupName = groupName;

        return (confirmGroupJoinMessage, success);
    }
}
using System.Collections;
using ChatroomServer.Models;

namespace ChatServer;

public class ActiveGroups
{
    private object _lock;
    private Hashtable _groups;

    public ActiveGroups()
    {
        _lock = new object();
        _groups = new Hashtable();
    }


    public bool AddEntry(string groupId, ClientModel clientModel)
    {
        bool success = false;
        lock (_lock)
        {
            if (_groups.ContainsKey(groupId))
            {
                var userList = _groups[groupId] as List<ClientModel>;
                userList?.Add(clientModel);
                _groups[groupId] = userList;
                success = true;
            }
            else
            {
                var userList = new List<ClientModel>();
                userList.Add(clientModel);
                _groups.Add(groupId, userList);
                success = true;
            }

        }

        return success;
    }

    public bool RemoveEntry(string groupId)
    {
        return false;
    }

    public List<UserDetails>? GetUserList(string groupId)
    {
        List<UserDetails> userList = null;
        lock (_lock)
        {
            if (_groups.ContainsKey(groupId))
            {
                var clientList = _groups[groupId] as List<ClientModel>;
                userList = new List<UserDetails>();
                foreach (var client in clientList)
                {
                    userList.Add(client.Details);
                }

                return userList;
            }
        }

        return userList;
    }
}
using System.Collections;

namespace ChatServer;

public class ActiveGroups
{
    private Hashtable _groups;

    ActiveGroups()
    {
        _groups = new Hashtable();
    }


    public bool AddEntry(string groupID, Client client)
    {
        if (_groups.ContainsKey(groupID))
        {
            var userList = _groups[groupID] as List<Client>;
            userList.Add(client);
            _groups[groupID] = userList;

            return true;
        }
        else
        {
            var userList = new List<Client>();
            userList.Add(client);
            _groups.Add(groupID, userList);
            return true;
        }

        return false;
    }
}
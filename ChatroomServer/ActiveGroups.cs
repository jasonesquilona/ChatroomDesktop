using System.Collections;

namespace ChatServer;

public class ActiveGroups
{
    private Hashtable _groups;

    ActiveGroups()
    {
        _groups = new Hashtable();
    }


    public bool AddEntry(string groupID, ClientModel clientModel)
    {
        if (_groups.ContainsKey(groupID))
        {
            var userList = _groups[groupID] as List<ClientModel>;
            userList.Add(clientModel);
            _groups[groupID] = userList;

            return true;
        }
        else
        {
            var userList = new List<ClientModel>();
            userList.Add(clientModel);
            _groups.Add(groupID, userList);
            return true;
        }

        return false;
    }
}
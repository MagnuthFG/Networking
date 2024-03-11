using System;
using System.Collections.Generic;


[Serializable]
public struct SavedClientInformation
{
    public ulong networkID;
    public string userAuthId;
    public UserData userData;
}

public static class SavedClientInformationManager
{

    private static List<SavedClientInformation> savedClientInformation = new List<SavedClientInformation>();

    // Method to add a client 
    public static void AddClient(UserData userData)
    {
        SavedClientInformation savedClientInfo = new()
        {
            networkID = userData.networkID,
            userAuthId = userData.userAuthId,
            userData = userData
        };
        savedClientInformation.Add(savedClientInfo);
    }


    public static UserData GetUserData(ulong networkID)
    {
        foreach (var client in savedClientInformation)
        {
            if (networkID == client.networkID)
            {
                return client.userData;
            }
        }

        return null;
    }

    public static UserData GetUserData(string userAuthId)
    {
        foreach (var client in savedClientInformation)
        {
            if (userAuthId == client.userAuthId)
            {
                return client.userData;
            }
        }

        return null;
    }


    public static List<SavedClientInformation> GetAllClient()
    {
        return savedClientInformation;
    }

    public static void RemoveClient(ulong networkID)
    {
        foreach (var client in savedClientInformation)
        {
            if (networkID == client.networkID)
            {
                savedClientInformation.Remove(client);
            }

        }

    }
}

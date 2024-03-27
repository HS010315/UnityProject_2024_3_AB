using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

[System.Serializable]

public class PlayerData
{
    public string playerName;
    public int playerLevel;
    public List<string> items = new List<string>();
}

public class ExXMLDataManager : MonoBehaviour
{
    string filePath;

    void Start()
    {
        filePath = Application.persistentDataPath + "/playerData.xml";
        Debug.Log(filePath);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            PlayerData playerData = new PlayerData();
            playerData.playerName = "플레이어 1";
            playerData.playerLevel = 1;
            playerData.items.Add("돌 1");
            playerData.items.Add("바위 1");
            SaveData(playerData);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            PlayerData playerData = new PlayerData();

            playerData = LoadData();

            Debug.Log(playerData.playerName);
            Debug.Log(playerData.playerLevel);
            for(int i = 0; i < playerData.items.Count; i++)
            {
                Debug.Log(playerData.items[i]);
            }
        }
    }
    void SaveData(PlayerData data)
    {
        XmlSerializer serialzer = new XmlSerializer(typeof(PlayerData));
        FileStream stream = new FileStream(filePath, FileMode.Create);
        serialzer.Serialize(stream, data);
        stream.Close();
    }

    PlayerData LoadData()
    {
        if (File.Exists(filePath))
        {
            XmlSerializer serialzer = new XmlSerializer(typeof(PlayerData));
            FileStream stream = new FileStream(filePath, FileMode.Open);
            PlayerData data = (PlayerData)serialzer.Deserialize(stream);
            stream.Close();
            return data;
        }
        else
        {
            return null;
        }
    }


}

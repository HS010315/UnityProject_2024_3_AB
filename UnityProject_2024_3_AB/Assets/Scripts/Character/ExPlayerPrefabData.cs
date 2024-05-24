using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExPlayerPrefabData : MonoBehaviour
{
    public int Score;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            SaveData(Score);
        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log(LoadData());
        }
    }

    void SaveData(int score)
    {
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.Save();
    }

    int LoadData()
    {
        return PlayerPrefs.GetInt("Score");
    }
}

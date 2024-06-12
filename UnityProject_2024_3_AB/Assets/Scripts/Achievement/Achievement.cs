using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
 public class Achievement


{
    public string name;
    public string description;
    public bool isUnlocked;
    public int currentProgress;
    public int goal;

    public Achievement(string name, string description, int goal)       //생성자에서 초기화
    {
        this.name = name;
        this.description = description;
        this.isUnlocked = false;
        this.currentProgress = 0;
        this.goal = goal;
    }

    public void AddProgress(int amount)
    {
        if(!isUnlocked)
        {
            currentProgress += amount;
            if(currentProgress >= goal)
            {
                isUnlocked = true;
                OnAchievementUnlocked();
            }
        }
    }
    protected virtual void OnAchievementUnlocked()          //UI로 표출 시 override로 수정
    {
        Debug.Log($"업적 달성 : {name} ");
    }
}

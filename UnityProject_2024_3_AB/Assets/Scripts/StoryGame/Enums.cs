using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STORYGAME
{
    public class Enums
    {
        public enum StoryType //���丮Ÿ��
        {
            MAIN,
            SUB,
            SERIAL
        }

        public enum EvenType //�̺�Ʈ �߻��� üũ
        {
            NONE,
            GoToBattle = 100,
            CheckSTR = 1000,

        }
        public enum ResultType //�̺�Ʈ ��� ����
        {
            AddExperience,
            GoToNextStory,
            GoToRandomStory,
        }
    }
}

[System.Serializable]
public class Stats
{
    //ü�°� ���
    public int hpPoint;
    public int spPoint;

    public int currentHpPoint;
    public int currentSpPoint;
    public int currentXpPoint;

    //�⺻ ���� ����
    public int strength;        //STR
    public int dexterity;       //DEX
    public int consitution;    //CON
    public int Intelligence;    //INT
    public int wisdom;          //WIS
    public int charisma;        //CHA
}
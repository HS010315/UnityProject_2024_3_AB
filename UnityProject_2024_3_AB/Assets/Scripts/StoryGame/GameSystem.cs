using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;


#if UNITY_EDITOR                                //전처리기 유니티 에디터에서만 동작
[CustomEditor(typeof(GameSystem))]
public class GameSysteEditor : Editor       //에디터를 상속받는 클래스 생성
{
    public override void OnInspectorGUI()           //유니티의 인스펙터 함수를 재정의
    {
        base.OnInspectorGUI();                      //유니티 인스펙터 함수 동작을 같이 한다. (Base)
        GameSystem gameSystem = (GameSystem)target;

        //Reset Story Models 버튼 생성
        if (GUILayout.Button("Reset Story Models"))
        {
            gameSystem.ResetStroyModles();
        }
    }
}
#endif
public class GameSystem : MonoBehaviour
{
    public static GameSystem instance;              //간단한 싱글톤 화

    private void Awake()
    {
        instance = this;
    }

    public enum GAMESTATE
    {
        STORYSHOW,
        WAITSELECT,
        STORYEND
    }

    public Stats stats;
    public GAMESTATE currentSTATE;
    public int currentStoryIndex = 1;
    public StoryModel[] storyModels;

    public void Start()
    {
        ChangeState(GAMESTATE.STORYSHOW);
    }


#if UNITY_EDITOR
    [ContextMenu("Reset Story Models")]
    public void ResetStroyModles()
    {
        storyModels = Resources.LoadAll<StoryModel>(""); //Resources 폴더 아래 모든 StoryModel을 불러 오기 
    }
#endif

    public void StoryShow(int number)
    {
        StoryModel tempStoryModels = FindStoryModel(number);

        StorySystem.instance.currentStoryModel = tempStoryModels;
        StorySystem.instance.CoShowText();
    }

    StoryModel FindStoryModel(int number)
    {
        StoryModel tempStoryModels = null;
        for(int i = 0; i <storyModels.Length; i++)  
        {
            if(storyModels[i].storyNumber == number)
            {
                tempStoryModels = storyModels[i];
                break;
            }
        }
        return tempStoryModels;
    }

    StoryModel RandomStory()
    {
        StoryModel tempStoryModels = null;

        List<StoryModel> storyModelList = new List<StoryModel>();
        for (int i = 0; i < storyModels.Length; i++)
        {
            if (storyModels[i].storyType == StoryModel.STORYTYPE.MAIN)          //for문으로 storymodel을 검색하여 main인 경우만 추출
            {
                storyModelList.Add(storyModels[i]);
            }
        }

        tempStoryModels = storyModelList[Random.Range(0, storyModelList.Count)]; //리스트에서 랜덤으로 선택
        currentStoryIndex = tempStoryModels.storyNumber;
        return tempStoryModels;
    }

    public void ChangeState(GAMESTATE temp)
    {
        currentSTATE = temp;
        if(currentSTATE == GAMESTATE.STORYSHOW)
        {
            StoryShow(currentStoryIndex);
        }
    }

    public void ChangeStats(StoryModel.Result result)
    {
        if (result.stats.hpPoint > 0) stats.hpPoint += result.stats.hpPoint;
        if (result.stats.spPoint > 0) stats.spPoint += result.stats.spPoint;

        if (result.stats.currentHpPoint > 0) stats.currentHpPoint += result.stats.currentHpPoint;
        if (result.stats.currentSpPoint > 0) stats.currentSpPoint += result.stats.currentSpPoint;
        if (result.stats.currentXpPoint > 0) stats.currentXpPoint += result.stats.currentXpPoint;

        if (result.stats.strength > 0) stats.strength += result.stats.strength;
        if (result.stats.dexterity > 0) stats.dexterity += result.stats.dexterity;
        if (result.stats.consitution > 0) stats.consitution += result.stats.consitution;
        if (result.stats.wisdom > 0) stats.wisdom += result.stats.wisdom;
        if (result.stats.Intelligence > 0) stats.Intelligence += result.stats.Intelligence;
        if (result.stats.charisma > 0) stats.charisma += result.stats.charisma;
    }

    public void ApplyChoice(StoryModel.Result result)
    {
        switch(result.resultType)
        {
            case StoryModel.Result.ResultType.ChangeHP:
                stats.currentHpPoint += result.value;
                ChangeStats(result);
                break;

            case StoryModel.Result.ResultType.AddExperience:
                stats.currentXpPoint += result.value;
                ChangeStats(result);
                break;

            case StoryModel.Result.ResultType.GoToNextStory:
                currentStoryIndex = result.value;
                ChangeState(GAMESTATE.STORYSHOW);
                ChangeStats(result);
                break;

            case StoryModel.Result.ResultType.GoToRandomStory:
                RandomStory();
                ChangeState(GAMESTATE.STORYSHOW);
                ChangeStats(result);
                break;
            default:
                Debug.LogError("Unknow type");
                break;
        }
    }
}
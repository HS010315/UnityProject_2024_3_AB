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

        //StorySystem.Instance.currentStoryModel = tempStoryModels;
        //StorySystem.Instance.CoShowText();
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
}
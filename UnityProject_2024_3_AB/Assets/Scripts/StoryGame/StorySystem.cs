using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StorySystem : MonoBehaviour
{
    public static StorySystem instance;

    public StoryModel currentStoryModel;

    public enum TEXTSYSTEM
    { 
        DOING,
        SELECT,
        DONE
    }

    public float delay = 0.1f;
    public string fullText;
    private string currentText = "";

    public Text textComponent;
    public Text storyIndex;

    public Image imageComponent;

    public Button[] buttonWay = new Button[3];
    public Text[] buttonWayText = new Text[3];

    private void Awake()
    {
        instance = this;
    }

    public void OnWayClick(int index)
    {
        bool CheckEventTypeNone = false;
        StoryModel playStoryModel = currentStoryModel;
        Debug.Log(index);

        if(playStoryModel.options[index].eventCheck.eventType == StoryModel.EventCheck.EventType.NONE)
        {
            for (int i = 0; i < playStoryModel.options[index].eventCheck.sucessResult.Length; i++)
            {
                GameSystem.instance.ApplyChoice(currentStoryModel.options[index].eventCheck.sucessResult[i]);
                CheckEventTypeNone = true;
            }
        }

        bool CheckValue = false;
    }
    void Start()
    {
        for(int i = 0; i < buttonWay.Length; i++)
        {
            int wayIndex = 1;       //클로저 문제 해결
            // 클로저 문제 = 람다식 또는 익명 함수가 외부 변수를 캡쳐할 때 발생하는 문제
            buttonWay[i].onClick.AddListener(() => OnWayClick(wayIndex));      //OnWayClick(i) 로 썼을 때는 2값만 계속 돌아감 
        }

        CoShowText();
    }

    public void CoShowText()
    {
        StoryModelInit();
        ResetShow();
        StartCoroutine(ShowText());
    }

    public void StoryModelInit()
    {
        fullText = currentStoryModel.storyText;

        storyIndex.text = currentStoryModel.storyNumber.ToString();

        for(int i = 0; i < currentStoryModel.options.Length; i++)
        {
            buttonWayText[i].text = currentStoryModel.options[i].buttonText;
        }
    }

    public void ResetShow()
    {
        textComponent.text = "";

        for(int i = 0; i <buttonWay.Length; i++)
        {
            buttonWay[i].gameObject.SetActive(false);
        }
    }

    IEnumerator ShowText()
    {
        if(currentStoryModel.mainImage != null)
        {
            Rect rect = new Rect(0, 0, currentStoryModel.mainImage.width, currentStoryModel.mainImage.height);
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            Sprite sprite = Sprite.Create(currentStoryModel.mainImage, rect, pivot);

            imageComponent.sprite = sprite;
        }

        else
        {
            Debug.LogError("텍스쳐 이상");
        }

        for(int i = 0; i < fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            textComponent.text = currentText;
            yield return new WaitForSeconds(delay);
        }

        for(int i = 0; i < currentStoryModel.options.Length; i++)
        {
            buttonWay[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(delay);
        }

        yield return new WaitForSeconds(delay); 
    }
}

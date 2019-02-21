﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollRectControl : ScrollRect
{
    [Header("Custom Settings")]
    public bool unlockAllLevel = false;
    public float effectScale = 1.0f;

    private UnityEngine.UI.Extensions.ScrollSnap scrollSnap;
    private List<Transform> contentList;
    private float contentDistance;

    private void Update()
    {
    }

    private void ScaleEffect(int page0, int page1, float value)
    {
        float mid = (page0 * contentDistance + page1 * contentDistance) * 0.5f;
        float scale = Mathf.Max(0.0f, Mathf.Min(1.0f, Mathf.Abs(value - mid) / (contentDistance / 2)));
        float minScale = effectScale;
        float newScale = minScale + (1 - minScale) * scale;

        if (page0 < contentList.Count)
        {
            contentList[page0].localScale = new Vector3(newScale, newScale, newScale);
        }

        if (page1 < contentList.Count)
        {
            contentList[page1].localScale = new Vector3(newScale, newScale, newScale);
        }
    }

    public void RecordContentPosition()
    {
        DataSet.recentContentPos = content.anchoredPosition;
    }

    public void ResumeContentPosition()
    {
        StartCoroutine(RefreshResumedContentPosition());
    }

    IEnumerator RefreshResumedContentPosition()
    {
        yield return null;
        yield return null;
        content.anchoredPosition = DataSet.recentContentPos;
    }

    public void OnValueChanged(Vector2 scroll)
    {
        float value = scroll.x;
        int page0 = 0;
        int page1 = 1;
        while (page1 < contentList.Count)
        {
            if (value >= page0 * contentDistance && value < page1 * contentDistance)
            {
                break;
            }

            ++page0;
            ++page1;
        }

        ScaleEffect(page0, page1, value);
    }

    public void Init()
    {
        scrollSnap = GetComponent<UnityEngine.UI.Extensions.ScrollSnap>();

        contentList = new List<Transform>();
        foreach (Transform t in content)
        {
            contentList.Add(t);
            t.gameObject.SetActive(false);
        }
        contentDistance = 1.0f / (contentList.Count - 1);

        for (int i = 0; i <= DataSet.unlockedLevelIdx; ++i)
        {
            contentList[i].gameObject.SetActive(true);
        }
        ResumeContentPosition();

        if (unlockAllLevel)
        {
            DataSet.recentCompletedLevelIdx = contentList.Count;
            CheckUnlockedLevel();
        }
        else
        {
            CheckNextLevel();
        }
    }

    public void NextScreen()
    {
        scrollSnap.NextScreen();
    }

    public void UnlockNextLevel()
    {
        DataSet.recentCompletedLevelIdx = DataSet.unlockedLevelIdx;
        CheckNextLevel();
    }

    public void CheckNextLevel()
    {
        if (DataSet.unlockedLevelIdx == DataSet.recentCompletedLevelIdx)
        {
            DataSet.unlockedLevelIdx = Mathf.Min(DataSet.recentCompletedLevelIdx + 1, contentList.Count - 1);
            if (DataSet.unlockedLevelIdx >= contentList.Count)
            {
                return;
            }

            contentList[DataSet.unlockedLevelIdx].gameObject.SetActive(true);

            Services.taskManager
                .Do(new Wait(1))
                .Then(new ActionTask(NextScreen));
        }
    }

    public void CheckUnlockedLevel()
    {
        if (DataSet.unlockedLevelIdx <= DataSet.recentCompletedLevelIdx)
        {
            for (int i = DataSet.unlockedLevelIdx + 1; i <= DataSet.recentCompletedLevelIdx + 1; ++i)
            {
                if (i >= contentList.Count)
                {
                    break;
                }

                contentList[i].gameObject.SetActive(true);
            }
            DataSet.unlockedLevelIdx = Mathf.Min(DataSet.recentCompletedLevelIdx + 1, contentList.Count - 1);
        }
    }

    public void MoveToLevelSelect()
    {
        if (DataSet.unlockedLevelIdx <= 0)
        {
            UnlockNextLevel();
        }
        else
        {
            NextScreen();
        }
    }
}
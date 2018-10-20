﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class HUDControl : MonoBehaviour
{
    public GameObject canvas;
    public List<UnityEvent> levelUIEvents;
    private int curId = 0;

    [SerializeField]
    private List<GameObject> anchors = new List<GameObject>();
    [SerializeField]
    private GameObject anchorsObj;    
    [HideInInspector]
    [SerializeField]
    private int anchorCounter = 0;

    [Header("VFX")]
    public GameObject goodClickPrefab;

    void Start()
    {
        //PlayNextUIEvent();
    }

    private void Update()
    { 
    }

    private void OnDrawGizmos()
    {
        foreach (GameObject anchor in anchors.ToArray())
        {
            if (anchor == null)
            {
                anchors.Remove(anchor);
            }
            else
            {
                //Gizmos.DrawGUITexture(new Rect(anchor.transform.position.x, anchor.transform.position.y, 1, 1), gizmoTexture);
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(anchor.transform.position, 0.02f);
            }
        }
    }

    public void PlayNextUIEvent()
    {
        if (curId < levelUIEvents.Count)
        {
            levelUIEvents[curId].Invoke();
            ++curId;
        }
    }

    public void DisplayImage(string anchorImage)
    {
        string[] info = anchorImage.Split(',');
        foreach (GameObject anch in anchors){
            if(anch.name == info[0])
            {
                GameObject image = new GameObject(info[1], typeof(RectTransform));
                image.transform.position = anch.transform.position;
                image.transform.SetParent(canvas.transform);
                image.AddComponent<Image>();
                Sprite outsideImage = Resources.Load<Sprite>("Sprites/" + info[1]);
                image.GetComponent<Image>().sprite = outsideImage;
                image.transform.localScale = Vector3.zero;
                image.transform.DOScale(Vector3.one, float.Parse(info[2]));
                break;
            }
        }
    }

    public void CloseImage(string targetImage)
    {
        string[] info = targetImage.Split(',');
        GameObject image = GameObject.Find(info[0]);
        image.transform.DOScale(Vector3.zero, float.Parse(info[1])).OnComplete(() => { Destroy(image); });       
    }

    public void AddAnchor()
    {
        if (anchorsObj == null)
        {
            anchorsObj = new GameObject("Anchors", typeof(RectTransform));
            anchorsObj.transform.SetParent(canvas.transform);
            anchorsObj.transform.localPosition = Vector3.zero;
        }

        GameObject anchor = new GameObject("Anchor" + anchorCounter, typeof(RectTransform));
        anchor.transform.SetParent(anchorsObj.transform);
        anchor.transform.localPosition = Vector3.zero;
        ++anchorCounter;

        anchors.Add(anchor);
    }

    public void GoodClick(Vector3 posWorld)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(gameObject.GetComponent<Canvas>().transform as RectTransform,
            Camera.main.WorldToScreenPoint(posWorld), gameObject.GetComponent<Canvas>().worldCamera, out pos);
        
        //GameObject particle = Instantiate(goodClickPrefab, pos, Quaternion.identity) as GameObject;
        GameObject particle = Instantiate(goodClickPrefab, transform) as GameObject;
        RectTransform rect = particle.transform as RectTransform;
        rect.anchoredPosition = pos;
        //Debug.Log(pos);
    }
}

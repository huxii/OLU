﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControl : MonoBehaviour
{
    public float doubleClickTolerence = 0.1f;

    private GameObject mouseClickObject = null;
    private Vector3 mouseClickPos;
    private Vector3 mouseDragPos;
    private bool oneClick = false;
    private float doubleClickTime = 0;
    private float singleClickTime = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DetectMouse()
    {
        if (Time.time - doubleClickTime > doubleClickTolerence)
        {
            // clicked one time but timed out, treated as a single click
            if (oneClick)
            {
                MouseSingleClick();
                oneClick = false;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            mouseDragPos = Input.mousePosition;
            singleClickTime = Time.time;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f))
            {
                mouseClickObject = hit.collider.gameObject;
                mouseClickPos = hit.point;
            }
            else
            {
                mouseClickObject = null;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            // get a second click before it's timed out, treated as a double click
            if (oneClick)
            {
                MouseDoubleClick();
                oneClick = false;
            }
            else
            {
                // when it's not a drag
                if (Time.time - singleClickTime < doubleClickTolerence)
                {
                    oneClick = true;
                }

                doubleClickTime = Time.time;
            }
        }

        if (Input.GetMouseButton(0))
        {
            MouseDrag();
        }

        if (Input.GetMouseButtonUp(0))
        {
            // mouse up
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            Services.cameraController.Zoom(Input.GetAxis("Mouse ScrollWheel"));
        }
    }

    private void MouseSingleClick()
    {
        Debug.Log("Single click " + mouseClickObject);

        Services.gameController.ClickOn(mouseClickObject, mouseClickPos);
    }

    private void MouseDoubleClick()
    {
        Debug.Log("Double click " + mouseClickObject);

        Services.gameController.DoubleClickOn(mouseClickObject);
    }

    private void MouseDrag()
    {
        if (mouseClickObject == null)
        {
            RotateViewport();
        }
        else
        {
            // drag object
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f))
            {
                Vector3 newMouseClickPos = hit.point;
                Vector3 mouseDelta = newMouseClickPos - mouseClickPos;

                if (mouseClickObject.CompareTag("Object"))
                {
                    Services.gameController.DragOn(mouseClickObject, mouseDelta);
                }
                else
                {
                    RotateViewport();
                }

                mouseClickPos = newMouseClickPos;
            }
            //else
            //{
            //    RotateViewport();
            //}
        }
    }

    public void RotateViewport()
    {            
        // rotate viewport
        Vector3 mouseDelta = (Input.mousePosition - mouseDragPos) * Time.time;
        //Debug.Log(mouseDelta);
        Services.cameraController.Orbit(mouseDelta.x, mouseDelta.y);
        mouseDragPos = Input.mousePosition;
    }
}
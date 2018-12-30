﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PropOneTimeDragControl : PropOneTimeControl
{
    public enum DragAxis
    {
        X,
        Y
    };

    [Header("Drag Settings")]
    public DragAxis dragAxis;
    public float dragOffset;
    public UnityEvent onDragging;
    public UnityEvent onReachEnd;

    protected float deltaOffset;
    protected Vector3 origPos;
    protected float minDelta;
    protected float maxDelta;
    protected float speed = 5f;
    protected float hintTimer;
    protected float hintCD = 3f;

    protected bool isDragging = false;

    // on dragging for sound effect
    // Use this for initialization
    void Start ()
    {
        RegisterEvents();

        origPos = transform.position;
        deltaOffset = 0;

        minDelta = 0;
        maxDelta = dragOffset;
        if (minDelta > maxDelta)
        {
            minDelta = maxDelta;
            maxDelta = 0;
        }

        hintTimer = 1f;
    }

    private void OnApplicationQuit()
    {
        UnregisterEvents();
    }

    // Update is called once per frame
    void Update ()
    {
        if (!IsLocked())
        {
            Vector3 targetPos = origPos;
            targetPos[(int)dragAxis] += deltaOffset;
            Vector3 newPos = transform.position;
            if (Vector3.Distance(targetPos, newPos) > 0.001f)
            {
                newPos = Vector3.Lerp(newPos, targetPos, Time.deltaTime * speed);
                transform.position = newPos;

                if (Vector3.Distance(targetPos, newPos) > 0.05f)
                {
                    onDragging.Invoke();
                }
            }
            else
            {
                Vector3 finalPos = origPos;
                finalPos[(int)dragAxis] += dragOffset;
                if (Vector3.Distance(newPos, finalPos) < 0.001f)
                {
                    onReachEnd.Invoke();
                    FreeAllMen();
                    Lock();
                }
                else
                {
                    if (IsActivated())
                    {
                        if (hintTimer <= 0)
                        {
                            hintTimer = hintCD;
                            Services.dotweenEvents.Spring(gameObject.name + " x -0.2 0.25 2");
                            CoolDown();
                        }
                        else
                        {
                            hintTimer -= Time.deltaTime;
                        }
                    }
                }
            }
        }
	}

    protected void RegisterEvents()
    {
        Services.eventManager.Register<ReleaseEvent>(OnRelease);
    }

    protected void UnregisterEvents()
    {
        Services.eventManager.Unregister<ReleaseEvent>(OnRelease);
    }

    protected void OnRelease(Crowd.Event e)
    {
        if (isDragging)
        {
            isDragging = false;
            Services.dotweenEvents.ScaleTo(GetComponent<PropFeedbackBehavior>().targetObj.name + " 1, 1, 1, 0.5");
            if (Mathf.Abs(deltaOffset) < Mathf.Abs(dragOffset))
            {
                deltaOffset = 0;
            }
        }
    }

    public override void Drag(Vector3 d)
    {
        if (!IsCoolingDown() && IsActivated() && !IsLocked())
        {
            base.Drag(d);

            if (!isDragging)
            {
                isDragging = true;
                Services.dotweenEvents.ScaleTo(GetComponent<PropFeedbackBehavior>().targetObj.name + " 1.1, 1.1, 1.1, 0.5");
            }        

            deltaOffset = Mathf.Min(Mathf.Max(minDelta, deltaOffset + d[(int)dragAxis]), maxDelta);
        }
    }
}

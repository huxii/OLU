﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropAutoTimedDeactivateControl : PropAutoControl
{
    public float timeToAutoDeactivate = 1f;

    private float timer = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        TimeUpdate();
    }

    protected void TimeUpdate()
    {
        if (IsActivated())
        {
            if (timer < timeToAutoDeactivate)
            {
                timer += Time.deltaTime;
                if (timer >= timeToAutoDeactivate)
                {
                    Deactivate();
                }
            }
        }
        else
        {
            timer = 0;
        }
    }

    public override void Deactivate()
    {
        FreeAllMen();

        base.Deactivate();
    }
}

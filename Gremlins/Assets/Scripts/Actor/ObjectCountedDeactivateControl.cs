﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCountedDeactivateControl : ObjectControl
{
    public int activatedCount = 1;

    private int count = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override bool CanActivateMultipleTimes()
    {
        return true;
    }

    public override void Activate()
    {
        base.Activate();

        ++count;
        if (count >= activatedCount)
        {
            count = 0;
            Deactivate();
        }
    }
}

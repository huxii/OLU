﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockBehavior : MonoBehaviour
{
    int tick = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Tick()
    {
        ++tick;
        Services.gameController.GetComponent<DotweenEvents>().Rotate("SecondHand, z, 6, 0.1");

        if (tick == 60)
        {
            tick = 0;
            Services.gameController.GetComponent<DotweenEvents>().Rotate("MinuteHand, z, 6, 0.1");
        }
    }
}

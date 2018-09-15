﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPrimaryControl : ObjectControl
{
    [System.Serializable]
    protected class SlotAttr
    {
        public SlotState state;
        public GameObject obj;
        public GameObject man;

        public SlotAttr(GameObject o, GameObject m = null)
        {
            state = SlotState.EMPTY;
            obj = o;
            man = m;
        }
    };

    protected enum SlotState
    {
        EMPTY,
        PLANNED,
        READY,
    }

    public bool needDelayToDeactivate = false;

    [SerializeField]
    public float slotGizmoSize = 1f;

    [SerializeField]
    protected List<SlotAttr> slots = new List<SlotAttr>();
    [HideInInspector]
    [SerializeField]
    private float namingCounter = 0;
    private Vector3 localSpawnPos = new Vector3(0, 0, 0);

    // current slots occupied by the crowd
    protected int currentSlots = 0;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnDrawGizmos()
    {
        foreach (SlotAttr slot in slots.ToArray())
        {
            if (slot.obj == null)
            {
                slots.Remove(slot);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(slot.obj.transform.position, slotGizmoSize);
            }
        }
    }

    public override bool IsActivated()
    {
        return currentSlots >= slots.Count;
    }

    public GameObject AddSlot()
    {
        GameObject newSlot = new GameObject();
        newSlot.name = "Slot" + namingCounter;
        newSlot.transform.SetParent(transform);
        newSlot.transform.localPosition = localSpawnPos;
        ++namingCounter;

        slots.Add(new SlotAttr(newSlot));
        return newSlot;
    }

    public void FreeSlot(int id)
    {
        if (slots[id].state == SlotState.READY)
        {
            if (currentSlots == slots.Count)
            {
                if (needDelayToDeactivate)
                {
                    DelayToDeactivate();
                }
                else
                {
                    Deactivate();
                }
            }
            --currentSlots;
        }
        slots[id].man = null;
        slots[id].state = SlotState.EMPTY;
    }

    public void PlanSlot(int id)
    {
        slots[id].state = SlotState.PLANNED;
    }

    public void ReadySlot(int id, GameObject man)
    {
        if (slots[id].state != SlotState.READY)
        {
            ++currentSlots;
            if (currentSlots == slots.Count)
            {
                Activate();
            }
            slots[id].state = SlotState.READY;
            slots[id].man = man;
        }
    }

    public Vector3 GetSlotPos(int id)
    {
        return slots[id].obj.transform.position;
    }

    //public int GetSlotId(GameObject man)
    //{
    //    for (int i = 0; i < slots.Count; ++i)
    //    {
    //        if (slots[i].man == man)
    //        {
    //            return i;
    //        }
    //    }

    //    return -1;
    //}

    public virtual int FindEmptySlot()
    {
        if (IsLocked())
        {
            return -1;
        }

        for (int i = 0; i < slots.Count; ++i)
        {
            SlotAttr slot = slots[i];
            if (slot.state == SlotState.EMPTY)
            {
                return i;
            }
        }

        return -1;
    }
}

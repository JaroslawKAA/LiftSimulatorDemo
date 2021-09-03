using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDoor : LiftDoor
{
    public int floor;

    protected override void OnLiftArrived(int floor)
    {
        if (floor == this.floor)
        {
            Open();
        }
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        floor = Convert.ToInt32(transform.parent.name.Split('_')[1]);
        _detector = transform.parent.GetComponentInChildren<MovementDetector>();
    }

    private void Update()
    {
        if (_detector.someoneStayInDoor)
        {
            openingTimer = 0f;
            return;
        }
        else
        {
            if (Opened)
            {
                openingTimer += Time.deltaTime;
            }

            if (openingTimer > 5f)
            {
                Opened = false;
                StartCoroutine(CloseDoor());
            }
        }
    }
}
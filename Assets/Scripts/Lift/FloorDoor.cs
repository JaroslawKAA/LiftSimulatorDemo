using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDoor : LiftDoor
{
    public int floor;

    private AudioSource _audioSource;

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
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Close door after 5 second and if player is not in door.
        if (_detector.someoneStayInDoor)
        {
            openingTimer = 0f;
            return;
        }
        else
        {
            if (Opened)
                openingTimer += Time.deltaTime;

            if (openingTimer > 5f)
            {
                Opened = false;
                StartCoroutine(CloseDoor());
            }
        }
    }

    protected override void OnDoorChangeState()
    {
        base.OnDoorChangeState();
        _audioSource.Play();
    }
}
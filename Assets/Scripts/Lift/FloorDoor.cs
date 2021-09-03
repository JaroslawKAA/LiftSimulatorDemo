using System;
using UnityEngine;

public class FloorDoor : LiftDoor
{
    public int Floor { get; private set; }

    private AudioSource _audioSource;

    protected override void OnLiftArrived(int floor)
    {
        if (floor == this.Floor)
        {
            Open();
        }
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        Floor = Convert.ToInt32(transform.parent.name.Split('_')[1]);
        _detector = transform.parent.GetComponentInChildren<MovementDetector>();
        _audioSource = GetComponent<AudioSource>();
    }

    protected override void OnDoorChangeState()
    {
        base.OnDoorChangeState();
        _audioSource.Play();
    }
}
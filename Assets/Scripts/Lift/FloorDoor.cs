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

    // private void Update()
    // {
    //     // Close door after 3 second and if player is not in door.
    //     if (_detector != null && _detector.someoneStayInDoor)
    //     {
    //         openingTimer = 0f;
    //         return;
    //     }
    //
    //     if (Opened)
    //         openingTimer += Time.deltaTime;
    //
    //     if (openingTimer > 3f)
    //     {
    //         Opened = false;
    //         StartCoroutine(CloseDoor());
    //     }
    // }

    protected override void OnDoorChangeState()
    {
        base.OnDoorChangeState();
        _audioSource.Play();
    }
}
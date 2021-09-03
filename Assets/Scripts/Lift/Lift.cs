using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lift : MonoBehaviour
{
    public float movingSpeed = .5f;
    public Transform[] floors;
    public LiftDoor door;
    [HideInInspector] public LiftButton[] liftButtons;

    public event Action<int> onLiftArrived;


    [Header("Defined dynamically")] [SerializeField]
    private int _floor;

    private FloorDoor[] _floorDoors;
    private LiftSounds _sounds;
    private AudioSource _audioSource;
    [HideInInspector] public MovementDetector[] detectors;

    public int Floor
    {
        get => _floor;
        set => _floor = value;
    }

    private Vector3 TargetPosition => new Vector3(
        transform.position.x,
        floors[Floor].position.y,
        transform.position.z);

    private void Awake()
    {
        liftButtons = transform.parent.GetComponentsInChildren<LiftButton>();
        _floorDoors = transform.parent.GetComponentsInChildren<FloorDoor>();
        _audioSource = GetComponent<AudioSource>();
        _sounds = GetComponent<LiftSounds>();
        detectors = transform.parent.GetComponentsInChildren<MovementDetector>();
        foreach (LiftButton button in liftButtons)
        {
            button.onButtonPressed += OnCalledLift;
        }
    }


    private void OnCalledLift(LiftButton button)
    {
        FloorDoor currentFloorDoor = _floorDoors
            .First(d => d.floor == Floor);
        if (currentFloorDoor.Opened)
        {
            currentFloorDoor.Close();
            door.Close(() => { CallLift(button.floor); });
        }
        else
        {
            CallLift(button.floor);
        }
    }

    private void CallLift(int floor)
    {
        Floor = floor;
        StartCoroutine(MoveToSelectedFloor());
    }

    private IEnumerator MoveToSelectedFloor()
    {
        if (_sounds.moving != null)
        {
            _audioSource.clip = _sounds.moving;
            _audioSource.Play();
        }

        while (transform.position.y != TargetPosition.y)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position,
                TargetPosition, movingSpeed * Time.deltaTime);
            transform.position = newPosition;

            yield return null;
        }

        _audioSource.Stop();
        if (_sounds.bell != null)
        {
            _audioSource.clip = _sounds.bell;
            _audioSource.Play();
        }

        onLiftArrived?.Invoke(Floor);
        print($"Lift arrived to {_floor} floor.");
    }
}
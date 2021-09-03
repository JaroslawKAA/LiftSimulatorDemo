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
    
    public event Action<int> onLiftArrived;
    
    [Header("Defined dynamically")]
    [SerializeField]
    private int _floor;

    private LiftButton[] _liftButtons;
    private FloorDoor[] _floorDoors;
    [HideInInspector]
    public MovementDetector[] detectors;
    
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
        _liftButtons = transform.parent.GetComponentsInChildren<LiftButton>();
        _floorDoors = transform.parent.GetComponentsInChildren<FloorDoor>();
        detectors = transform.parent.GetComponentsInChildren<MovementDetector>();
        foreach (LiftButton button in _liftButtons)
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
            door.Close(() =>
            {
                CallLift(button.floor);
            });
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
        while (transform.position.y != TargetPosition.y)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, 
                TargetPosition, movingSpeed * Time.deltaTime);
            transform.position = newPosition;
            
            yield return null;
        }
        onLiftArrived?.Invoke(Floor);
        print($"Lift arrived to {_floor} floor.");
    }
}

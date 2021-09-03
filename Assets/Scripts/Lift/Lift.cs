using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class Lift : MonoBehaviour
{
    [Header("Settings")] public float movingSpeed = .5f;
    public Transform[] floors;
    public LiftDoor door;
    [HideInInspector] public LiftButton[] liftButtons;

    public event Action<int> onLiftArrived;
    public event Action onLiftStartmoving;

    # region Private

    private FloorDoor[] _floorDoors;
    private LiftSounds _sounds;
    private AudioSource _audioSource;
    [HideInInspector] public MovementDetector[] detectors;

    #endregion

    /// <summary>
    /// Current index of floor
    /// </summary>
    public int Floor { get; set; }

    /// <summary>
    /// Position of lift destination.
    /// </summary>
    private Vector3 TargetPosition => new Vector3(
        transform.position.x,
        floors[Floor].position.y,
        transform.position.z);

    private void Awake()
    {
        _floorDoors = transform.parent.GetComponentsInChildren<FloorDoor>();
        _audioSource = GetComponent<AudioSource>();
        _sounds = GetComponent<LiftSounds>();

        liftButtons = transform.parent.GetComponentsInChildren<LiftButton>();
        detectors = transform.parent.GetComponentsInChildren<MovementDetector>();

        foreach (LiftButton button in liftButtons)
        {
            button.onButtonPressed += OnCalledLift;
        }

        onLiftArrived += PlaySound_OnLiftArrived;
        onLiftStartmoving += PlaySound_OnLiftStartMoving;

        Assert.AreNotEqual(0, floors.Length,
            "Add floors Game Object to the list \"Floors\"");
        Assert.IsNotNull(door);
    }


    private void OnCalledLift(LiftButton button)
    {
        // Close door before moving to the target floor
        FloorDoor currentFloorDoor = _floorDoors
            .First(d => d.Floor == Floor);
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
        onLiftStartmoving?.Invoke();
        
        while (transform.position.y != TargetPosition.y)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position,
                TargetPosition, movingSpeed * Time.deltaTime);
            transform.position = newPosition;

            yield return null;
        }
        
        onLiftArrived?.Invoke(Floor);
        print($"Lift arrived to {Floor} floor.");
    }

    private void PlaySound_OnLiftArrived(int floor)
    {
        _audioSource.Stop();
        if (_sounds.bell != null)
        {
            _audioSource.clip = _sounds.bell;
            _audioSource.Play();
        }
    }

    private void PlaySound_OnLiftStartMoving()
    {
        if (_sounds.moving != null)
        {
            _audioSource.clip = _sounds.moving;
            _audioSource.Play();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = null;
        }
    }
}
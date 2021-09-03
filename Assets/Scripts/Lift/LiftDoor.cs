using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class LiftDoor : MonoBehaviour
{
    [Header("Settings - LiftDoor")] public float movingDistance = .45f;
    public float openingSpeed = .5f;
    public Lift lift;

    [Header("Updated dynamically")] [SerializeField]
    protected MovementDetector _detector;
    
    [SerializeField] private bool _opened;
    private Animator _animator;
    
    public bool Opened
    {
        get => _opened;
        set
        {
            _opened = value;
            if (value == false)
                openingTimer = 0f;
        }
    }

    private DoorWing[] _wings = new DoorWing[2];
    protected float openingTimer;
    private static readonly int OpenedAnimatorProperty = Animator.StringToHash("Opened");

    private void Awake()
    {
        int index = 0;
        foreach (Transform child in transform)
        {
            _wings[index] = child.gameObject.GetComponent<DoorWing>();
            index++;
        }
        _animator = GetComponent<Animator>();

        Assert.IsNotNull(lift);
        lift.onLiftArrived += OnLiftArrived;
        OnAwake();
    }
    
    
    private void Update()
    {
        if (_detector != null && _detector.someoneStayInDoor)
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

    protected virtual void OnLiftArrived(int floor)
    {
        Open();
        // Get movement detector from current floor
        _detector = lift.detectors.First(d => d.floor == lift.Floor);
    }


    public delegate void Callback();

    public void Open(Callback callback = null)
    {
        StartCoroutine(OpenDoor(callback));
    }

    public void Close(Callback callback = null)
    {
        StartCoroutine(CloseDoor(callback));
    }

    private IEnumerator OpenDoor(Callback callback = null)
    {
        OnDoorChangeState();
        _animator.SetBool(OpenedAnimatorProperty, true);
        while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("OpenPose"))
        {
            yield return null;
        }

        callback?.Invoke();
        Opened = true;
    }

    protected IEnumerator CloseDoor(Callback callback = null)
    {
        OnDoorChangeState();
        _animator.SetBool(OpenedAnimatorProperty, false);
        while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("ClosePose"))
        {
            yield return null;
        }

        callback?.Invoke();
        Opened = false;
        
    }

    protected virtual void OnAwake() {}
    
    protected virtual void OnDoorChangeState(){}
}
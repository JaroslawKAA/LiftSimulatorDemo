using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class LiftDoor : MonoBehaviour
{
    [Header("Settings - LiftDoor")] public float movingDistance = .45f;
    public Lift lift;

    public delegate void Callback();

    /// <summary>
    /// Check if someone is in door scope.
    /// </summary>
    protected MovementDetector _detector;

    protected float openingTimer;

    private Animator _animator;
    [SerializeField] private bool _opened;
    private static readonly int OpenedAnimatorProperty = Animator.StringToHash("Opened");
    private static readonly int IsSomethingInDoorScope = Animator.StringToHash("IsSomethingInDoorScope");

    public bool Opened
    {
        get => _opened;
        set
        {
            _opened = value;
            openingTimer = 0f;
        }
    }

    public void Open(Callback callback = null)
    {
        StartCoroutine(OpenDoor(callback));
    }

    public void Close(Callback callback = null)
    {
        StartCoroutine(CloseDoor(callback));
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        Assert.IsNotNull(lift);
        lift.onLiftArrived += OnLiftArrived;
        OnAwake();
    }


    private void Update()
    {
        // Close door after 3 second and if player is not in door scope.
        if (_detector != null && _detector.someoneStayInDoor)
        {
            openingTimer = 0f;
            return;
        }

        if (Opened)
            openingTimer += Time.deltaTime;

        if (openingTimer > 3f)
        {
            Opened = false;
            StartCoroutine(CloseDoor());
        }
    }

    protected virtual void OnLiftArrived(int floor)
    {
        Open();
        // Get movement detector from current floor
        _detector = lift.detectors.First(d => d.floor == lift.Floor);
    }

    private IEnumerator OpenDoor(Callback callback = null)
    {
        OnDoorChangeState();
        _animator.SetBool(OpenedAnimatorProperty, true);
        while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("OpenPose"))
        {
            yield return null;
        }

        Opened = true;
        callback?.Invoke();
    }

    protected IEnumerator CloseDoor(Callback callback = null)
    {
        OnDoorChangeState();
        _animator.SetBool(OpenedAnimatorProperty, false);
        _animator.SetBool(IsSomethingInDoorScope, false);

        while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("ClosePose"))
        {
            // If someone is in door scope cancel closing.
            if (_detector.someoneStayInDoor)
            {
                _animator.SetBool(IsSomethingInDoorScope, true);
                Opened = true;
                _animator.SetBool(OpenedAnimatorProperty, true);
                yield break;
            }

            yield return null;
        }

        Opened = false;
        callback?.Invoke();
    }

    protected virtual void OnAwake()
    {
    }

    protected virtual void OnDoorChangeState()
    {
    }
}
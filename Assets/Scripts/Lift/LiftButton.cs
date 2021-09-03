using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class LiftButton : MonoBehaviour
{
    [Header("Settings")]
    public Lift lift;
    public Color pressedColor;
    public Color idleColor;
    public bool pressed;
    /// <summary>
    /// If true get floor index from parent name.
    /// </summary>
    public bool setFloorAutomatic = true;
    public int floor;

    public event Action<LiftButton> onButtonPressed;

    private Renderer _renderer;

    public void CallLift()
    {
        if (pressed || OtherLiftButtonIsPressed())
            return;

        pressed = true;
        _renderer.material.SetColor("_Color", pressedColor);
        onButtonPressed?.Invoke(this);
    }

    private bool OtherLiftButtonIsPressed()
    {
        return lift.liftButtons.Any(b => b.pressed);
    }

    private void ResetButton()
    {
        pressed = false;
        _renderer.material.SetColor("_Color", idleColor);
    }

    private void LiftArrived(int floor)
    {
        if (floor == this.floor)
        {
            ResetButton();
        }
    }

    private void Awake()
    {
        _renderer = transform.GetChild(0).GetComponent<Renderer>();
        if (setFloorAutomatic)
            floor = Convert.ToInt32(transform.parent.name.Split('_')[1]);
        lift.onLiftArrived += LiftArrived;
        
        Assert.IsNotNull(lift, "Connect button to lift.");
    }
}
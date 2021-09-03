using UnityEngine;
using UnityEngine.Assertions;

public class LiftButtonTrigger : Interactable
{
    public GameObject highLightObject;

    private LiftButton _liftButton;

    protected override void OnAwake()
    {
        _liftButton = transform.parent.GetComponent<LiftButton>();
        Assert.IsNotNull(highLightObject);
    }

    public override void Interact()
    {
        base.Interact();
        _liftButton.CallLift();
    }

    protected override void Highlight()
    {
        base.Highlight();
        highLightObject.SetActive(true);
    }

    protected override void OffHighlight()
    {
        base.OffHighlight();
        highLightObject.SetActive(false);
    }
}
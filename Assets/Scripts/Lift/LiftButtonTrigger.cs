
public class LiftButtonTrigger : Interactable
{
    private LiftButton _liftButton;

    protected override void OnAwake()
    {
        _liftButton = transform.parent.GetComponent<LiftButton>();
    }

    public override void Interact()
    {
        _liftButton.CallLift();
    }
}

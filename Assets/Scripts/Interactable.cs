using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    private void Awake()
    {
        gameObject.tag = "Interactable";
        OnAwake();
    }

    public abstract void Interact();

    protected virtual void OnAwake()
    {
    }
}

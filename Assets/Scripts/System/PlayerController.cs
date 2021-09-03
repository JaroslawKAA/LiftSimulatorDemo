using StarterAssets;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private StarterAssetsInputs _input;
    
    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_input.interact)
        {
            Interact();
        }
    }

    private void Interact()
    {
        Transform camera = Camera.main.transform;
        if (Physics.Raycast(camera.position, camera.forward, out RaycastHit hit, 1))
        {
            if (hit.transform.CompareTag("Interactable"))
            {
                hit.transform.SendMessage("Interact");
            }
        }
        print("Interact button pressed");
        _input.interact = false;
    }
}
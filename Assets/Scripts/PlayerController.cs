using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private StarterAssetsInputs _input;

    private void Awake()
    {
    }

    // Start is called before the first frame update
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
        if (Physics.Raycast(camera.position, camera.forward, out RaycastHit hit, 4))
        {
            if (hit.transform.CompareTag("Interactable"))
            {
                hit.transform.GetComponent<Interactable>().Interact();
            }
        }
        print("Interact button pressed");
        _input.interact = false;
    }
}
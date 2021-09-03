using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightSelectedInteractable : MonoBehaviour
{
    // Update is called once per frame
    void LateUpdate()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 1))
        {
            if (hit.transform.CompareTag("Interactable"))
            {
                Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 1, Color.green);
                hit.transform.GetComponent<Interactable>().isMouseOver = true;
            }
            else
            {
                Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 1, Color.red);
            }
        }
    }
}
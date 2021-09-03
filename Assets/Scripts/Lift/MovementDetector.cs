using System;
using UnityEngine;

public class MovementDetector : MonoBehaviour
{
    [Header("Settings")] public float detectionDistance = 1;

    [HideInInspector] public int floor;
    [HideInInspector] public bool someoneStayInDoor;

    private void Awake()
    {
        // Get number of floor from parent floor object name
        floor = Convert.ToInt32(transform.parent.name.Split('_')[1]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, detectionDistance))
        {
            Debug.DrawRay(transform.position, Vector3.forward * detectionDistance, Color.red);
            if (!hit.transform.CompareTag("Lift"))
            {
                someoneStayInDoor = true;
            }
        }
        else
        {
            someoneStayInDoor = false;
            Debug.DrawRay(transform.position, Vector3.forward * detectionDistance, Color.green);
        }
    }
}
using System;
using UnityEngine;

public class MovementDetector : MonoBehaviour
{
    [Header("Settings")] public float detectionDistance = 1;

    [HideInInspector] public int floor;

    [Header("Defined dynamicaly.")] public bool someoneStayInDoor;

    private void Awake()
    {
        floor = Convert.ToInt32(transform.parent.name.Split('_')[1]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, detectionDistance))
        {
            someoneStayInDoor = true;
            Debug.DrawRay(transform.position, Vector3.forward * detectionDistance, Color.red);
        }
        else
        {
            someoneStayInDoor = false;
            Debug.DrawRay(transform.position, Vector3.forward * detectionDistance, Color.green);
        }
    }
}
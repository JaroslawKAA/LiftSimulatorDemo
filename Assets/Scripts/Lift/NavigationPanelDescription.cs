using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NavigationPanelDescription : MonoBehaviour
{
    private TextMeshPro[] _floorNumbers;
    private int _currentFloor;
    private Lift _lift;

    private void Awake()
    {
        _floorNumbers = new TextMeshPro[transform.childCount];
        int i = 0;
        foreach (Transform child in transform)
        {
            _floorNumbers[i] = child.GetComponentInChildren<TextMeshPro>();
            i++;
        }

        _lift = transform.parent.GetComponent<Lift>();
    }

    // Update is called once per frame
    void Update()
    {
        ResetHighlight();
        HighlightFloorNumber();
    }

    private void ResetHighlight()
    {
        foreach (var text in _floorNumbers)
        {
            text.color = Color.white;
        }
    }

    private int GetCurrentFloor()
    {
        float currentHeight = _lift.transform.position.y;
        float closestFloorDistance = Mathf.Abs(_lift.floors[0].transform.position.y - currentHeight);
        Transform closestFloor = _lift.floors[0];
        foreach (Transform floor in _lift.floors)
        {
            float currentDistance = Mathf.Abs(floor.position.y - currentHeight);
            if (currentDistance < closestFloorDistance)
            {
                closestFloorDistance = currentDistance;
                closestFloor = floor;
            }
        }

        return Convert.ToInt32(closestFloor.name.Split('_')[1]);
    }

    private void HighlightFloorNumber()
    {
        _currentFloor = GetCurrentFloor();
        _floorNumbers[_currentFloor].color = Color.green;
    }
}

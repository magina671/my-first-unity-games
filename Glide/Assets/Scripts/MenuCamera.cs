using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{

    private Vector3 desiredPosition;
    private Quaternion desiredRotation;

    private Vector3 startPosition;
    private Quaternion startRotation;

    public Transform shopWaypoint;
    public Transform levelWaypoint;

    void Start()
    {
        startPosition = desiredPosition = transform.localPosition;    
        startRotation = desiredRotation = transform.localRotation;    
    }

    private void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, desiredPosition, 0.1f);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, desiredRotation, 0.1f);
    }

    public void BackToMainMenu()
    {
        desiredPosition = startPosition;
        desiredRotation = startRotation;
    }

    public void MoveToShop()
    {
        desiredPosition = shopWaypoint.localPosition;
        desiredRotation = shopWaypoint.localRotation;
    }

    public void MoveToLevel()
    {
        desiredPosition = levelWaypoint.localPosition;
        desiredRotation = levelWaypoint.localRotation;
    }
}

﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// This camera smoothes out rotation around the y-axis and height.
// Horizontal Distance to the target is always fixed.
// For every one of those smoothed values, calculate the wanted value and the current value.
// Smooth it using the Lerp function and apply the smoothed values to the transform's position.
public class SmoothFollow : MonoBehaviour
{
    public Transform target;
    public GameObject Manager;
    public float distance = 3.0f;
    public float height = 1.50f;
    public float heightDamping = 2.0f;
    public float positionDamping = 2.0f;
    public float rotationDamping = 2.0f;
    public float rotation = 0;
    public int type = 0;

    private void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Early out if we don't have a target
        if (!target)
        {
            if (type == 0)
                target = GetRandomTransform(Manager.GetComponent<ExerciseManager>().pathFollowers);
            else if(type == 1)
                target = GetRandomTransform(Manager.GetComponent<ExerciseManager>().flowFieldFollowers);
            else if(type == 2)
                target = GetRandomTransform(Manager.GetComponent<ExerciseManager>().flockers);
        }
        float wantedHeight = target.position.y + height;
        float currentHeight = transform.position.y;
        // Damp the height
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);
        // Set the position of the camera
        Vector3 wantedPosition = target.position - target.forward * distance;
        transform.position = Vector3.Lerp(transform.position, wantedPosition,
        Time.deltaTime * positionDamping);
        // Adjust the height of the camera
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        // Set the forward to rotate with time
        transform.forward = Vector3.Lerp(transform.forward, Quaternion.Euler(0, rotation, 0) * target.forward,
        Time.deltaTime * rotationDamping);

    }

    /// <summary>
    /// Return a transform from a list of objects
    /// </summary>
    /// <param name="objectList">The list of objects</param>
    /// <returns></returns>
    Transform GetRandomTransform(List<GameObject> objectList)
    {
        int r = Random.Range(0, objectList.Count);

        if (type == 0)
            return objectList[r].transform.GetChild(0).transform;
        else
            return objectList[r].transform;
    }
}
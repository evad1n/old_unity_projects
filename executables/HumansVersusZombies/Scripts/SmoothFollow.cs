﻿using UnityEngine;
using System.Collections;
// This camera smoothes out rotation around the y-axis and height.
// Horizontal Distance to the target is always fixed.
// For every one of those smoothed values, calculate the wanted value and the current value.
// Smooth it using the Lerp function and apply the smoothed values to the transform's position.
public class SmoothFollow : MonoBehaviour
{
    public Transform target;
    public Transform defaultPos;
    public GameObject Manager;
    public float distance = 3.0f;
    public float height = 1.50f;
    public float heightDamping = 2.0f;
    public float positionDamping = 2.0f;
    public float rotationDamping = 2.0f;
    public bool follow = false;

    private void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {

        if (follow)
        {
            // Early out if we don't have a target
            if (!target) return;
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
            transform.forward = Vector3.Lerp(transform.forward, target.forward,
            Time.deltaTime * rotationDamping);
        }
        else
        {
            transform.position = defaultPos.position;
            transform.rotation = defaultPos.rotation;
        }

    }

    private void OnGUI()
    {

        if (GUI.Button(new Rect(275, 25, 150, 50), "Main Camera View"))
        {
            follow = false;
        }

        if (GUI.Button(new Rect(450, 25, 150, 50), "Follow Human"))
        {
            if(Manager.GetComponent<ExerciseManager>().humans.Count != 0)
            {
                follow = true;
                int r = Random.Range(0, Manager.GetComponent<ExerciseManager>().humans.Count);
                target = Manager.GetComponent<ExerciseManager>().humans[r].transform;
            }
        }

        if (GUI.Button(new Rect(625, 25, 150, 50), "Follow Zombie"))
        {
            if (Manager.GetComponent<ExerciseManager>().zombies.Count != 0)
            {
                follow = true;
                int r = Random.Range(0, Manager.GetComponent<ExerciseManager>().zombies.Count);
                target = Manager.GetComponent<ExerciseManager>().zombies[r].transform;
            }
        }

    }
}

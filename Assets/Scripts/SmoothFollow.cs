﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform target;
    public float distance = 8.0f;
    public float height = 1.5f;
    public float heightOffset = 1.0f;
    public float heightDamping = 4.0f;
    public float rotationDamping = 2.0f;

    int FP = -1;
   
    void Start()
    {
        if (PlayerPrefs.HasKey("FP"))
            FP = PlayerPrefs.GetInt("FP");

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target == null)
            return;
        
        if (FP == 1)
        {
            transform.position = target.position - target.forward * 0.4f + target.up;
            transform.LookAt(target.position + target.forward * 3);
        }
        else
        {
            float wantedRotationAngle = target.eulerAngles.y;
            float wantedHeight = target.position.y + height;

            float currentRotationAngle = transform.eulerAngles.y;
            float currentHeight = transform.position.y;

            currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
            currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

            Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

            transform.position = target.position;
            transform.position -= currentRotation * Vector3.forward * distance;

            transform.position = new Vector3(transform.position.x,
                                    currentHeight + heightOffset,
                                    transform.position.z);

            transform.LookAt(target);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            FP = FP * -1;
            PlayerPrefs.SetInt("FP", FP);
        }
    }
}

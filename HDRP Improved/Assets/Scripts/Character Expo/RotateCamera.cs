using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    [SerializeField] Vector3 rotSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotSpeed);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_Character : MonoBehaviour
{
    [field: SerializeField] Transform cam;
    void FixedUpdate()
    {
        Rotate();
    }
    void Rotate()
    {
        Quaternion rot = cam.rotation;
        Debug.Log(rot);
        Quaternion new_rot = Quaternion.Euler(0, rot.y * 360, 0);
        transform.rotation = new_rot;
    }
}
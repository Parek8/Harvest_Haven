using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Character_Stats))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class Player_Movement : MonoBehaviour
{
    [field: SerializeField] Transform cam;
    [field: SerializeField] float turn_smooth_speed;

    Character_Stats stats;
    CharacterController controller;
    Animator animator;

    private float turn_smooth_velocity;
    private void Start()
    {
        stats = GetComponent<Character_Stats>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        animator.SetFloat("Speed", stats.movement_speed);

        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        float x = Input_Manager.GetCustomAxisRaw("Horizontal");
        float y = Input_Manager.GetCustomAxisRaw("Vertical");
        Vector3 direction = new Vector3(x, 0, y).normalized;

        if (direction.magnitude >= 0.1f)
        {
            Move(direction);
            Animate("Idle", false);
            Animate("Moving", true);
        }
        else
        {
            Animate("Idle", true);
            Animate("Moving", false);
        }
        Debug.DrawRay(transform.position + Vector3.up, transform.forward*3, Color.red);
    }
    private void Move(Vector3 direction)
    {
        float target_angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target_angle, ref turn_smooth_velocity, turn_smooth_speed);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        Vector3 move_dir = Quaternion.Euler(0, target_angle, 0) * Vector3.forward;
        controller.Move(move_dir.normalized * stats.movement_speed * Time.deltaTime);
        //Debug.Log("Moved");
    }

    private void Animate(string variable, bool animate)
    {
        animator.SetBool(variable, animate);
    }

    public float Get_Distance(Transform dis)
    {
        return (Vector3.Distance(transform.position, dis.position));
    }

    public Vector3 Get_Position()
    {
        return (Vector3)transform.position;
    }
}
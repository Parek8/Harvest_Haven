using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Character_Stats))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
internal class Player_Movement : MonoBehaviour
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

        GameManager.game_manager.Cursor_Needed(CursorLockMode.Locked);
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
        //if (Input_Manager.GetCustomAxisRaw("Attack") != 0)
        //    Rotate(direction);
    }
    private void Move(Vector3 direction)
    {
        Vector3 move_dir = Quaternion.Euler(0, Rotate(direction), 0) * Vector3.forward;
        controller.Move(move_dir.normalized * stats.movement_speed * Time.deltaTime);
    }

    private float Rotate(Vector3 direction)
    {
        float target_angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target_angle, ref turn_smooth_velocity, turn_smooth_speed);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        return target_angle;
    }

    private void Animate(string variable, bool animate)
    {
        animator.SetBool(variable, animate);
    }

    internal float Get_Distance(Transform dis)
    {
        return (Vector3.Distance(transform.position, dis.position));
    }

    internal Vector3 Get_Position()
    {
        return (Vector3)transform.position;
    }
}
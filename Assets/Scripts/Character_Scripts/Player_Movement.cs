using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Character_Stats))]
[RequireComponent(typeof(Rotate_Character))]
public class Player_Movement : MonoBehaviour
{
    Character_Stats stats;
    Vector3 movement_direction = new();
    Rigidbody rb;

    void Start()
    {
        stats = GetComponent<Character_Stats>();
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        float x_move = InputManager.GetCustomAxisRaw("Horizontal");
        float y_move = InputManager.GetCustomAxisRaw("Vertical");
        movement_direction = new(x_move, 0, y_move);
    }

    void FixedUpdate()
    {   
        Move();
    }
    private void Move()
    {
        rb.velocity = movement_direction * stats.movement_speed;
    }
}

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Character_Stats))]
[RequireComponent(typeof(Rotate_Character))]
public class Player_Movement : MonoBehaviour
{
    [field: SerializeField] CinemachineFreeLook cam;
    [field: SerializeField] Camera main;

    Character_Stats stats;
    Vector3 movement_direction = new();
    Rigidbody rb;
    private void Start()
    {
        stats = GetComponent<Character_Stats>();
    }
    void Update()
    {
        float cam_y = cam.transform.rotation.y;
        Vector3 cameraForward = cam.transform.forward;
        cameraForward.y = 0f;

        float horizontalInput = InputManager.GetCustomAxisRaw("Horizontal");
        float verticalInput = InputManager.GetCustomAxisRaw("Vertical");

        // Vytvoøení vektoru pro pohyb postavy relativnì ke smìru kamery
        Vector3 moveDirection = main.transform.forward * -verticalInput + main.transform.right * horizontalInput;

        // Pohyb postavy
        transform.Translate(moveDirection * stats.movement_speed * Time.deltaTime);

        // Rotace postavy ve smìru kamery
        //if (moveDirection != Vector3.zero)
            Rotate(cam_y);
    }
    private void Rotate(float cam_y)
    {
        transform.rotation = Quaternion.Euler(0, cam_y, 0);
    }
}

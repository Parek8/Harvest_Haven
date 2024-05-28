using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Character_Stats))]
[RequireComponent(typeof(CharacterController))]
internal class Player_Movement : MonoBehaviour
{
    [field: SerializeField] Transform cam;
    [field: SerializeField] float turn_smooth_speed;
    [field: SerializeField] float GravityForce;
    [field: SerializeField] List<AudioSource> FootSteps;

    Character_Stats stats;
    CharacterController controller;
    [field: SerializeField] Animator animator;

    private float turn_smooth_velocity;
    private float verticalVelocity;
    private Vector3 lastPosition;
    private void Start()
    {
        stats = GetComponent<Character_Stats>();
        controller = GetComponent<CharacterController>();
        //animator.SetFloat("Speed", stats.MovementSpeed);

        GameManager.game_manager.Cursor_Needed(CursorLockMode.Locked);
        GameManager.game_manager.ResumeGame();
        StartCoroutine("WalkSound");
    }
    void Update()
    {
        lastPosition = transform.position;

        float x = Input_Manager.GetCustomAxisRaw("Horizontal");
        float y = Input_Manager.GetCustomAxisRaw("Vertical");
        Vector3 direction = new Vector3(x, 0, y).normalized;

        if (direction.magnitude >= 0.1f)
        {
            Move(direction);
            //Animate("Idle", false);
            Animate("Running", true);
        }
        else
        {
            //Animate("Idle", true);
            Animate("Running", false);
        }

        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = 0f;

        verticalVelocity -= GravityForce * Time.deltaTime;

        controller.Move(new Vector3(0, -1, 0) * verticalVelocity * Time.deltaTime);
        //if (Input_Manager.GetCustomAxisRaw("Attack") != 0)
        //    Rotate(direction);
    }

    IEnumerator WalkSound()
    {
        AudioManager _audioManager = GameManager.game_manager.AudioManagerInstance;
        while (true)
        {
            if (controller.isGrounded && lastPosition != transform.position && this.enabled)
            {
                if (FootSteps.Count > 0)
                    _audioManager.PlaySound(FootSteps[Random.Range(0, FootSteps.Count)]);
                yield return new WaitForSeconds(.3f);
            }
            else
                yield return new WaitForEndOfFrame();
        }
    }
    private void Move(Vector3 direction)
    {
        Vector3 move_dir = Quaternion.Euler(0, Rotate(direction), 0) * Vector3.forward;
        controller.Move(move_dir.normalized * stats.MovementSpeed * Time.deltaTime);
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

    internal void StopAllAnimations()
    {
        //Animate("Idle", false);
        Animate("Running", false);
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
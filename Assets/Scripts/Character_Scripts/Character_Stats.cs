using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Stats : MonoBehaviour
{
    [field: Header("Player Movement Variables: ")]
    [field: SerializeField] public float movement_speed { get; private set; } = 15f; 
    [field: SerializeField] public float jump_force { get; private set; } = 100f;
    [field: SerializeField] public float pick_up_distance { get; private set; } = 1.2f;
}
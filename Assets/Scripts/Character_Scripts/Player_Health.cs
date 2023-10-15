using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character_Stats))]
public class Player_Health : MonoBehaviour
{
    Character_Stats stats;
    private void Start()
    {
        stats = GetComponent<Character_Stats>();    
    }
    private void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    [SerializeField] float hp = 20;

    public void Damage(float damage)
    {
        this.hp -= damage;
        if (this.hp <= 0 )
        {
            Debug.Log($"Destroyed {name}");
            Destroy(this);
        }
    }
}

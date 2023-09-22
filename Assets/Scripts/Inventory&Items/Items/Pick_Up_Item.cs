using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick_Up_Item : MonoBehaviour
{
    Item item;
    Transform player;
    float needed_distance;
    void Start()
    {
        player = GameManager.game_manager.player_transform;
        needed_distance = player.GetComponent<Character_Stats>().pick_up_distance;
    }

    void Update()
    {
        float current_distance = Vector3.Distance(transform.position, player.position);

        if (current_distance <= needed_distance)
            Move_Towards_Player();
        if (current_distance <= 0.2f)
        {
            Debug.Log("You picked up an Item");
            Destroy(gameObject);
        }
    }

    private void Move_Towards_Player()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * 5);
    }    
}

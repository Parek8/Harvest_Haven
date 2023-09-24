using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick_Up_Item : MonoBehaviour
{
    [field: SerializeField] Item item;
    Transform player;
    float needed_distance;
    Inventory player_inventory;
    void Start()
    {
        player = GameManager.game_manager.player_transform;
        player_inventory = GameManager.game_manager.player_inventory;
        needed_distance = player.GetComponent<Character_Stats>().pick_up_distance;
    }

    void Update()
    {
        float current_distance = Vector3.Distance(transform.position, player.position);

        if (current_distance <= needed_distance)
            Move_Towards_Player();
        if (current_distance <= 0.2f)
        {
            player_inventory.Add(this.item);
            Destroy(gameObject);
        }
    }

    private void Move_Towards_Player()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * 5);
    }    
}

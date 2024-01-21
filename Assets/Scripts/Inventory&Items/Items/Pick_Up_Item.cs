using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
internal class Pick_Up_Item : MonoBehaviour
{
    [field: SerializeField] Item item;
    Transform player;
    float needed_distance;
    Inventory player_inventory;
    Rigidbody rb;
    void Start()
    {
        player = GameManager.game_manager.player_transform;
        player_inventory = GameManager.game_manager.player_inventory;
        needed_distance = player.GetComponent<Character_Stats>().pick_up_distance;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float current_distance = Vector3.Distance(transform.position, player.position);

        if (current_distance <= needed_distance)
            Move_Towards_Player();
        Debug.Log(current_distance);
        if (current_distance <= 2f)
        {
            player_inventory.Add(this.item);
            Destroy(gameObject);
        }
    }

    private void Move_Towards_Player()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * 5);
    }

    internal void Push_Item_Upwards()
    {
        float jump_force = Random.Range(1.0f, 3.0f);

        rb.AddForce(Vector3.up * jump_force, ForceMode.Impulse);    
    }
}
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
internal class Pick_Up_Item : MonoBehaviour
{
    [field: SerializeField] Item item;
    Transform player;
    float needed_distance;
    Inventory PlayerInventory;
    Rigidbody rb;
    void Start()
    {
        player = GameManager.GameManagerInstance.PlayerTransform;
        PlayerInventory = GameManager.GameManagerInstance.PlayerInventory;
        needed_distance = player.GetComponent<PlayerStats>().PickUpDistance;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float current_distance = Vector3.Distance(transform.position, player.position);

        if (current_distance <= needed_distance)
            Move_Towards_Player();

        if (current_distance <= 5f)
        {
            PlayerInventory.Add(this.item);
            Destroy(gameObject);
        }
    }

    private void Move_Towards_Player()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * 5);
    }

    internal void Push_Item_Upwards()
    {
        float JumpForce = Random.Range(1.0f, 3.0f);

        rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);    
    }
}
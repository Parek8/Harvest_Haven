using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
internal class PickUpItem : MonoBehaviour
{
    [field: SerializeField] Item Item;
    Transform _player;
    float _neededDistance;
    Inventory _playerInventory;
    Rigidbody _rb;
    void Start()
    {
        _player = GameManager.game_manager.player_transform;
        _playerInventory = GameManager.game_manager.player_inventory;
        _neededDistance = _player.GetComponent<PlayerStats>().pick_up_distance;
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float current_distance = Vector3.Distance(transform.position, _player.position);

        if (current_distance <= _neededDistance)
            MoveTowardsPlayer();

        if (current_distance <= 5f)
        {
            _playerInventory.Add(this.Item);
            Destroy(gameObject);
        }
    }

    private void MoveTowardsPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, _player.position, Time.deltaTime * 5);
    }

    internal void PushItemUpwards()
    {
        float jump_force = Random.Range(1.0f, 3.0f);

        _rb.AddForce(Vector3.up * jump_force, ForceMode.Impulse);    
    }
}
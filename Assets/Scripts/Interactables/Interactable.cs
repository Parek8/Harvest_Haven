using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(MeshFilter))]
internal class Interactable : MonoBehaviour
{
    protected float _distance = 2.5f;
    protected Transform _player;
    protected Character_Behaviour _behaviour;
    protected MeshRenderer _renderer;
    [field: SerializeField] UI_Behaviour _highlighter;

    protected void Start()
    {
        _distance = GameManager.game_manager.player_transform.GetComponent<Character_Stats>().pick_up_distance;
        _behaviour = GameManager.game_manager.player_transform.GetComponent<Character_Behaviour>();
        _player = GameManager.game_manager.player_transform;
        _renderer = _player.GetComponent<MeshRenderer>();
    }

    internal virtual void Interact()
    {
        throw new System.NotImplementedException();
    }
}
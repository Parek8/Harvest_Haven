using UnityEngine;

//[RequireComponent(typeof(MeshRenderer))]
//[RequireComponent(typeof(Collider))]
//[RequireComponent(typeof(MeshFilter))]
internal class Interactable : MonoBehaviour
{
    protected float _distance = 2.5f;
    protected Transform _player;
    protected Character_Behaviour _behaviour;
    protected MeshRenderer _renderer;
    [field: SerializeField] UI_Behaviour _highlighter;

    protected void Start()
    {
        _distance = GameManager.GameManagerInstance.PlayerTransform.GetComponent<PlayerStats>().PickUpDistance;
        _behaviour = GameManager.GameManagerInstance.PlayerTransform.GetComponent<Character_Behaviour>();
        _player = GameManager.GameManagerInstance.PlayerTransform;
        _renderer = _player.GetComponent<MeshRenderer>();
    }

    internal virtual void Interact()
    {
        throw new System.NotImplementedException();
    }
}
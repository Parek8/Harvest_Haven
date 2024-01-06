using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshRenderer))]
internal class Destroyable : MonoBehaviour
{
    [SerializeField] float max_hp = 20;
    [SerializeField] List<Item> dropped_items;
    [SerializeField] List<Item.ToolTypes> types;

    [SerializeField] Gradient health_colors;
    [SerializeField] Image filled_health_bar;
    [SerializeField] UI_Behaviour obj_canvas;
    [field: SerializeField] GameObject CanvasPrefab;

    float hp = 20;
    Player_Movement pl;
    Transform environment_parent;

    static GameObject _canvasPrefab;
    static Gradient _gradient;
    static Image _barPrefab;
    private void Awake()
    {
        if (_canvasPrefab == null && CanvasPrefab != null)
            _canvasPrefab = CanvasPrefab;

        if (obj_canvas == null)
        {
            obj_canvas = ((GameObject)Instantiate(_canvasPrefab, transform)).GetComponent<UI_Behaviour>();
            obj_canvas.GetComponent<Canvas>().worldCamera = Camera.main;
        }

        if (_gradient == null && health_colors != null)
            _gradient = health_colors;

        if (_barPrefab == null && filled_health_bar != null)
            _barPrefab = filled_health_bar;
    }
    private void Start()
    {
        hp = max_hp;
        pl = GameManager.game_manager.player_transform.GetComponent<Player_Movement>();
        StartCoroutine("Cycle");        
        environment_parent = GameManager.game_manager.environment_parent;
    }

    internal void Damage(float damage)
    {
        this.hp -= damage;
        if (this.hp <= 0 )
        {
            Destroy(gameObject);
            Drop_Items();
        }
    }
    private IEnumerator Cycle()
    {
        while(true)
        {
            float dis = pl.Get_Distance(transform);
            if (dis <= 5)
                if (hp < max_hp)
                    if (!obj_canvas.is_visible)
                        obj_canvas.Show();
                    else if (hp >= max_hp)
                        obj_canvas.Hide();
            if (dis > 5)
                obj_canvas.Hide();

            Set_Health_Bar();
            if (obj_canvas.is_visible)
                Rotate_Canvas();
            yield return new WaitForSeconds(0.001f);
        }
    }
    private void Set_Health_Bar()
    {
        float fill_ammount = hp / max_hp;
        filled_health_bar.fillAmount = fill_ammount;

        filled_health_bar.color = Get_Color(fill_ammount);
    }
    private Color Get_Color(float percentage)
    {
        return _gradient.Evaluate(percentage);
    }

    private void Drop_Items()
    {
        foreach(Item it in dropped_items)
        {
            if (Random.Range(0.0f, 1.0f) <= it.SpawnRate)
            {
                Pick_Up_Item drop_rb = Instantiate(it.ItemPrefab, Get_Drop_Range(), Quaternion.identity,environment_parent).GetComponent<Pick_Up_Item>();
                //drop_rb.Push_Item_Upwards();
            }
        }
    }
    private Vector3 Get_Drop_Range()
    {
        float x_offset = Random.Range(-1.0f, 1.0f);
        float y_offset = Random.Range(-1.0f, 1.0f);
        Vector3 pos = transform.position;

        Vector3 spawn_position = new Vector3(pos.x + x_offset, pos.y + y_offset, pos.z);
        return spawn_position;
    }

    private void Rotate_Canvas()
    {
        Quaternion rot = Quaternion.LookRotation(transform.position-pl.Get_Position());
        rot.z = 0;
        rot.x = 0;
        obj_canvas.transform.rotation = rot;
    }

    internal bool Compare_Tag(Item.ToolTypes type)
    {
        return types.Contains(type);
    }

    internal void SetupObject(List<Item> dropped_items, List<Item.ToolTypes> types, float max_hp = 20)
    {
        this.dropped_items = dropped_items;
        this.types = types;
        this.max_hp = max_hp;
        this.hp = max_hp;

        filled_health_bar = FindFilled(transform, "Filled").GetComponent<Image>();
    }
    Transform FindFilled(Transform parent, string name)
    {
        Transform result = parent.Find(name);

        if (result != null)
            return result;

        foreach (Transform child in parent)
        {
            result = FindFilled(child, name);
            if (result != null)
                return result;
        }

        return null;
    }
}
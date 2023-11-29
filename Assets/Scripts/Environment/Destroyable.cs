using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Destroyable : MonoBehaviour
{

    [SerializeField] float max_hp = 20;
    [SerializeField] Gradient health_colors;
    [SerializeField] Image filled_health_bar;
    [SerializeField] UI_Behaviour obj_canvas;
    [SerializeField] List<Item> dropped_items;
    [SerializeField] List<Tool_Type> types;

    float hp = 20;
    Player_Movement pl;
    Transform environment_parent;
    Transform canvas_t;
    private void Start()
    {
        hp = max_hp;
        pl = GameManager.game_manager.player_transform.GetComponent<Player_Movement>();
        StartCoroutine("Cycle");
        environment_parent = GameManager.game_manager.environment_parent;
        canvas_t = obj_canvas.transform;
    }

    public void Damage(float damage)
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
        return health_colors.Evaluate(percentage);
    }

    private void Drop_Items()
    {
        foreach(Item it in dropped_items)
        {
            if (Random.Range(0.0f, 1.0f) <= it.spawn_rate)
            {
                Pick_Up_Item drop_rb = Instantiate(it.item_prefab, Get_Drop_Range(), Quaternion.identity,environment_parent).GetComponent<Pick_Up_Item>();
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
        canvas_t.rotation = rot;
    }

    public bool Compare_Tag(Tool_Type type)
    {
        return types.Contains(type);
    }
}
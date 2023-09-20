using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Custom_Update_Subscriber : MonoBehaviour
{
    public abstract void Custom_Update();
    public void OnUnsubscribe()
    {
        My_Update.instance.Unsubscribe(this);
    }
    public void OnDestroy()
    {
        this.OnUnsubscribe();
    }
}
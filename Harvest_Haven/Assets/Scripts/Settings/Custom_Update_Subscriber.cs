using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal abstract class Custom_Update_Subscriber : MonoBehaviour
{
    internal abstract void Custom_Update();
    internal void OnUnsubscribe()
    {
        My_Update.instance.Unsubscribe(this);
    }
    internal void OnDestroy()
    {
        this.OnUnsubscribe();
    }
}
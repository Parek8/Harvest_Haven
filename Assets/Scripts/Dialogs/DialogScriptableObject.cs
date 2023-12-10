using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "/Dialog/Dialog")]
public class DialogScriptableObject : ScriptableObject
{
    [field: SerializeField] List<DialogLine> lines;

    private int index = 0;
}
[CreateAssetMenu(menuName = "/Dialog/Line")]
public class DialogLine : ScriptableObject
{
    [field: SerializeField] List<string> lines;
    [field: SerializeField] List<string> buttons;
}

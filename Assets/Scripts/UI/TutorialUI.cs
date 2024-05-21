using System.Collections;
using UnityEngine;

internal class TutorialUI : UI_Behaviour
{
    protected override IEnumerator __Hide()
    {
        transform.LeanMoveLocalX(500, TimeToPopup);
        yield return new WaitForSeconds(TimeToPopup);
        _Hide();
    }
    protected override IEnumerator __Show()
    {
        _Show();
        transform.LeanMoveLocalX(0, TimeToPopup);
        yield return new WaitForSeconds(TimeToPopup);
    }
}
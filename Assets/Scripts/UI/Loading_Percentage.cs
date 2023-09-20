using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Loading_Percentage : MonoBehaviour
{
    [SerializeField] Image fill;
    [SerializeField] TMP_Text percentage;
    [SerializeField] Animator anim;
        
    private void Start()
    {
        anim.SetBool("Play", true);   
    }
    public void Fill(float amount)
    {
        if (amount is >= 0 and <= 1)
        {
            fill.fillAmount = amount;
            int percent = Mathf.RoundToInt(amount*100);
            percentage.text = $"{percent}%";
            //Debug.Log(percent);
        }
        else
            Debug.LogWarning("Fill amount is somehow in-correct!");
    }
}

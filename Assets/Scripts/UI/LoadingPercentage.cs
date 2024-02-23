using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal class Loading_Percentage : MonoBehaviour
{
    [SerializeField] Image FillImage;
    [SerializeField] TMP_Text Percentage;
    [SerializeField] Animator Anim;
        
    private void Start()
    {
        Anim.SetBool("Play", true);   
    }
    internal void Fill(float amount)
    {
        if (amount is >= 0 and <= 1)
        {
            FillImage.fillAmount = amount;
            int _percent = Mathf.RoundToInt(amount*100);
            Percentage.text = $"{_percent}%";
            //Debug.Log(percent);
        }
        else
            Debug.LogWarning("Fill amount is somehow in-correct!");
    }
}

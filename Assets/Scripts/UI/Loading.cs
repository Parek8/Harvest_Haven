using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

internal class Loading : MonoBehaviour
{
    [field: SerializeField] TMP_Text loading_text;
    [field: SerializeField] float update_text_delay = 0.5f;

    int cycle = 0;
    private void Start()
    {
        StartCoroutine(Cycle());
    }
    private IEnumerator Cycle()
    {
        while (true)
        {
            string new_text = "Loading ";

            for (int i = 0; i < cycle; i++)
            {
                new_text += ". ";
            }

            loading_text.text = new_text;
            cycle = (cycle + 1) % 4;
            //Debug.Log(cycle);
            yield return new WaitForSecondsRealtime(update_text_delay);
        }
    }
}
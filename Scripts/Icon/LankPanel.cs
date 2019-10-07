using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LankPanel : MonoBehaviour {

    public GameObject[] icons;

    // スコアに応じてスプライトを出し分ける
    public void UpdateLank(int score)
    {
        // Sランク表示
        if (score > 10000)
        {
            icons[0].SetActive(true);
            icons[1].SetActive(false);
            icons[2].SetActive(false);
            icons[3].SetActive(false);
        }
        // Aランク表示
        else if (score > 5000)
        {
            icons[0].SetActive(false);
            icons[1].SetActive(true);
            icons[2].SetActive(false);
            icons[3].SetActive(false);
        }
        // Bランク表示
        else if (score > 0)
        {
            icons[0].SetActive(false);
            icons[1].SetActive(false);
            icons[2].SetActive(true);
            icons[3].SetActive(false);
        }
        // Cランク表示
        else
        {
            icons[0].SetActive(false);
            icons[1].SetActive(false);
            icons[2].SetActive(false);
            icons[3].SetActive(true);
        }
    }
}

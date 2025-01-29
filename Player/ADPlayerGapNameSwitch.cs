using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ADPlayerGapNameSwitch : MonoBehaviour
{

    public TextMeshProUGUI lbNickname;
    // public TextMeshProUGUI lbHave;
    public TextMeshProUGUI lbGab;

    private bool isRoutining = false;

    [SerializeField]
    public bool isName = true;

    public void OnPointerDown()
    {
        //lbNickname.gameObject.SetActive(true);
        //lbGab.gameObject.SetActive(false);
        lbNickname.SetAlpha(1f);
        lbGab.SetAlpha(0f);
        if (isName == false)
        {
            isName = true;
        }
    }

    public void OnPointerUp()
    {
        //lbNickname.gameObject.SetActive(false);
        //lbGab.gameObject.SetActive(true);
        lbNickname.SetAlpha(0f);
        lbGab.SetAlpha(1f);
        if (isName == true)
        {
            isName = false;
        }
    }
}

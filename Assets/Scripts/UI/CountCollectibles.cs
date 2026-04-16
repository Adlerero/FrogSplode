using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//Canvas script
public class CountCollectibles : MonoBehaviour
{
    public TextMeshProUGUI CountText;
    public TextMeshProUGUI CountTextKeys;
    string nb;
    string nk;
    // Update is called once per frame
    void Update()
    {
        nb = GameManager.Instance.BananasTot.ToString();
        CountText.text = nb + 'x';

        nk = GameManager.Instance.KeysTot.ToString();
        CountTextKeys.text   = nk + 'x';
    }
}

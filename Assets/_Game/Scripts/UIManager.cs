using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    /*public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }*/
    // instance bằng đối tượng instance nằm trong
    private void Awake()
    {
        instance = this;
    }

    [SerializeField] Text coinText;

    // Gán coinText.text bằng giá trị của coin
    public void SetCoin(int coin)
    {
        coinText.text = coin.ToString();
    }
}

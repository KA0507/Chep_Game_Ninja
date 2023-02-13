using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatText : MonoBehaviour
{
    [SerializeField] Text hpText;
    // Thay đổi kiểu dữ liệu float sang text
    public void OnInit(float damage)
    {
        hpText.text = damage.ToString();
        Invoke(nameof(OnDespawn), 1f);
    }
    // Xóa CombatText
    public void OnDespawn()
    {
        Destroy(gameObject);
    }
}

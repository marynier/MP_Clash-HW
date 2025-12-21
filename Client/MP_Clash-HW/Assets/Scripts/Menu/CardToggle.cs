using UnityEngine;

public class CardToggle : MonoBehaviour
{
    [SerializeField] private CardSelector _selector;
    [SerializeField] private int _index;
    public void Click(bool value)
    {
        if (value == false) return;
        _selector.SetSelectToggleIndex(_index);        
    }
}

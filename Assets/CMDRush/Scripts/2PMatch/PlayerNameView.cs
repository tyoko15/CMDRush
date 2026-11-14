using TMPro;
using UnityEngine;

public class PlayerNameView : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _playerName = default;
    private void Awake()
    {
        if(_playerName != null)
        {
            _playerName.SetText(string.Empty);
        }
        enabled = false;
    }


    public void SetTextSafe(string name)
    {
        if(_playerName != null)
        {
            _playerName.SetText(name);
        }
    }


}

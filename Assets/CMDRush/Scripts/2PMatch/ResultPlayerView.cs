using TMPro;
using UnityEngine;

public class ResultPlayerView : MonoBehaviour
{
    [SerializeField]
    TMP_Text _roundText = default;

}

public struct ResultData
{
    public ushort _round;
    public uint _collect;

    public ResultData(ushort round, uint collect)
    {
        _round = round; _collect = collect;
    }
}


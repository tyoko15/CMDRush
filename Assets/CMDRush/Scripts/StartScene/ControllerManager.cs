using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        Connection();
    }

    void Connection()
    {
        // コントローラの接続がない場合
        if (Gamepad.all.Count == 0)
        {
            // 1PはキーボードのWASD操作
            // 2PはキーボードのArrows操作

            StartManager.Instance.playerInputData[0].name = $"keyboard";            
            StartManager.Instance.playerInputData[0].kind = InputKind.Keyboard;
            StartManager.Instance.playerInputData[1].name = $"keyboard";
            StartManager.Instance.playerInputData[1].kind = InputKind.Keyboard;
        }
        // コントローラの接続が1つある場合
        else if (Gamepad.all.Count == 1)
        {
            // 1Pはコントローラ設定になる。
            // 2Pはキーボード設定になる。
            StartManager.Instance.playerInputData[0].name = $"{Gamepad.all[0].device.name}";
            StartManager.Instance.playerInputData[0].kind = InputKind.Controller;
            StartManager.Instance.playerInputData[1].name = $"keyboard";
            StartManager.Instance.playerInputData[1].kind = InputKind.Keyboard;
        }
        // コントローラの接続が2つある場合
        else
        {
            StartManager.Instance.playerInputData[0].name = $"{Gamepad.all[0].device.name}";
            StartManager.Instance.playerInputData[0].kind = InputKind.Controller;
            StartManager.Instance.playerInputData[1].name = $"{Gamepad.all[1].device.name}";
            StartManager.Instance.playerInputData[1].kind = InputKind.Controller;
        }


        for (int i = 0; i < 2; i++)
        {
            if (StartManager.Instance.playerInputData[i].kind == InputKind.Controller)
            {

            }
        }
        
    }
}

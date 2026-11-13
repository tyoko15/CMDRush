using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Switch;
using UnityEngine.InputSystem.XInput;
using UnityEngine.SceneManagement;
using System;
using System.Xml.Serialization;

public enum InputKind
{
    None,
    Keyboard,
    Controller
}

public enum ControllerKind
{
    None,
    Xbox,
    Nintendo,
    PlayStation,
}

[System.Serializable]
public class InputData
{
    public string name;
    public InputKind kind;
    public ControllerKind controllerKind;
}

public class StartManager : MonoBehaviour
{
    public static StartManager Instance;

    [SerializeField] Sprite[] controllerSprites;
    [SerializeField] Sprite[] upButtonSprites;
    // ì¸óÕê›íË
     public InputData[] playerInputData = new InputData[2];
    Gamepad[] gamepads = new Gamepad[2];

    [SerializeField] GameObject playerUI;
    Image[] controllerImages = new Image[2];
    Image[] readyImages = new Image[2];
    TextMeshProUGUI[] readyTexts = new TextMeshProUGUI[2];

    public bool[] readyFlags = new bool[2];
    bool readyFlag;

    Action move;
    void Start()
    {
        Instance = this;

        Set();
        Init();

        move += SceneMove;
    }

    void Update()
    {
        UpdateUI();

        if (readyFlags[0] && readyFlags[1] && !readyFlag)
        {
            FadeManager.Instance.fade.StartFadeIn(1.0f, move);
            readyFlag = true;
        }
    }

    void Set()
    {
        if (Gamepad.all.Count == 2)
        {
            gamepads[0] = Gamepad.all[0];
            gamepads[1] = Gamepad.all[1];

            for (int i = 0; i < 2; i++)
            {
                switch (gamepads[i])
                {
                    case XInputController:
                        playerInputData[i].controllerKind = ControllerKind.Xbox;
                        break;
                    case SwitchProControllerHID:
                        playerInputData[i].controllerKind = ControllerKind.Nintendo;
                        break;
                    case DualSenseGamepadHID:
                    case DualShockGamepad:
                        playerInputData[i].controllerKind = ControllerKind.PlayStation;
                        break;
                }
            }
        }
        else if (Gamepad.all.Count == 1)
        {
            gamepads[0] = Gamepad.all[0];
            gamepads[1] = null;
            switch (gamepads[0])
            {
                case XInputController:
                    playerInputData[0].controllerKind = ControllerKind.Xbox;
                    break;
                case SwitchProControllerHID:
                    playerInputData[0].controllerKind = ControllerKind.Nintendo;
                    break;
                case DualSenseGamepadHID:
                case DualShockGamepad:
                    playerInputData[0].controllerKind = ControllerKind.PlayStation;
                    break;
            }
            playerInputData[1].controllerKind = ControllerKind.None;
        }
    }
    
    void Init()
    {
        GameObject[] player = new GameObject[2];
        player[0] = playerUI.transform.GetChild(0).gameObject;
        player[1] = playerUI.transform.GetChild(1).gameObject;

        controllerImages[0] = player[0].transform.GetChild(1).GetChild(5).GetComponent<Image>();
        controllerImages[1] = player[1].transform.GetChild(1).GetChild(5).GetComponent<Image>();
        readyImages[0] = player[0].transform.GetChild(1).GetChild(6).GetComponent<Image>();
        readyImages[1] = player[1].transform.GetChild(1).GetChild(6).GetComponent<Image>();
        readyTexts[0] = player[0].transform.GetChild(1).GetChild(7).GetComponent<TextMeshProUGUI>();
        readyTexts[1] = player[1].transform.GetChild(1).GetChild(7).GetComponent<TextMeshProUGUI>();
    }

    void UpdateUI()
    {
        for (int i = 0; i < 2; i++) 
        {
            switch (playerInputData[i].controllerKind)
            {
                case ControllerKind.None:
                    if (i == 0)
                    {
                        readyImages[i].sprite = upButtonSprites[0];
                        controllerImages[i].sprite = controllerSprites[0];
                    }
                    else
                    {
                        readyImages[i].sprite = upButtonSprites[4];
                        controllerImages[i].sprite = controllerSprites[4];
                    }
                    break;
                case ControllerKind.Xbox:
                    readyImages[i].sprite = upButtonSprites[1];
                    controllerImages[i].sprite = controllerSprites[1];
                    break;
                case ControllerKind.Nintendo:
                    readyImages[i].sprite = upButtonSprites[2];
                    controllerImages[i].sprite = controllerSprites[2]; 
                    break;
                case ControllerKind.PlayStation:
                    readyImages[i].sprite = upButtonSprites[3];
                    controllerImages[i].sprite = controllerSprites[3]; 
                    break;
            }
        }
    }


    void SceneMove()
    {
        SceneManager.LoadScene("2PMatchScene");
    }

    public void ButtonX(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (Gamepad.all.Count == 0)
            {
                if (context.control.path == "/Keyboard/w") readyFlags[0] = true;
                else if (context.control.path == "/Keyboard/upArrow") readyFlags[1] = true;
            }
            else if (Gamepad.all.Count == 1)
            {
                var inputDevice = context.control.device;
                if (gamepads[0].deviceId == inputDevice.deviceId) readyFlags[0] = true;
                else if (context.control.path == "/Keyboard/upArrow") readyFlags[1] = true;
            }
            else
            {
                var inputDevice = context.control.device;
                if (gamepads[0].deviceId == inputDevice.deviceId) readyFlags[0] = true;
                else if (gamepads[1].deviceId == inputDevice.deviceId) readyFlags[1] = true;
            }
            for (int i = 0; i < 2; i++)
            {
                if (readyFlags[i])
                {
                    readyTexts[i].color = Color.green;
                    readyImages[i].color = Color.green;
                    if (gamepads[i] != null) gamepads[i].SetMotorSpeeds(0.5f, 0.5f);
                }
            }
        }
    }
    private void OnDisable()
    {
        for (int i = 0; i < 2; i++)
        {
            if (gamepads[i] != null) gamepads[i].SetMotorSpeeds(0f, 0f);
        }
    }
}

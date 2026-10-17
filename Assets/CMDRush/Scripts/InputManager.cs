using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] NotesManager[] notesManagers;
    Gamepad[] gamepads = new Gamepad[2];

    void Start()
    {

    }

    void Update()
    {
        Set();
    }

    void Set()
    {
        if (Gamepad.all.Count >= 2)
        {
            gamepads[0] = Gamepad.all[0];
            notesManagers[0].gamepad = gamepads[0];
            gamepads[1] = Gamepad.all[1];
            notesManagers[1].gamepad = gamepads[1];
        }
    }
    public void ButtonA(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (Gamepad.all.Count == 0)
            {
                if (context.control.path == "/Keyboard/d" && notesManagers[0].inputAllowedFlag) notesManagers[0].SetSelectCommand(0);
                if (context.control.path == "/Keyboard/rightArrow" && notesManagers[1].inputAllowedFlag) notesManagers[1].SetSelectCommand(0);
            }
            else
            {
                var inputDevice = context.control.device;
                if (gamepads[0].deviceId == inputDevice.deviceId && notesManagers[0].inputAllowedFlag) notesManagers[0].SetSelectCommand(0);
                else if (gamepads[1].deviceId == inputDevice.deviceId && notesManagers[1].inputAllowedFlag) notesManagers[1].SetSelectCommand(0);
            }
        }
    }
    public void ButtonB(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (Gamepad.all.Count == 0)
            {
                if (context.control.path == "/Keyboard/s" && notesManagers[0].inputAllowedFlag) notesManagers[0].SetSelectCommand(3);
                if (context.control.path == "/Keyboard/downArrow" && notesManagers[1].inputAllowedFlag) notesManagers[1].SetSelectCommand(3);
            }
            else
            {
                var inputDevice = context.control.device;
                if (gamepads[0].deviceId == inputDevice.deviceId && notesManagers[0].inputAllowedFlag) notesManagers[0].SetSelectCommand(3);
                else if (gamepads[1].deviceId == inputDevice.deviceId && notesManagers[1].inputAllowedFlag) notesManagers[1].SetSelectCommand(3);
            }
        }
    }
    public void ButtonX(InputAction.CallbackContext context)
    {
        if (!GameManager.Instance.startFlag && !GameManager.Instance.gameFlag)
        {
            var inputDevice = context.control.device;

            if (context.started)
            {
                if (Gamepad.all.Count == 0)
                {
                    if (context.control.path == "/Keyboard/w") GameManager.Instance.readyFlags[0] = true;
                    if (context.control.path == "/Keyboard/upArrow") GameManager.Instance.readyFlags[1] = true;
                }
                else
                {
                    if (gamepads[0].deviceId == inputDevice.deviceId) GameManager.Instance.readyFlags[0] = true;
                    if (gamepads[1].deviceId == inputDevice.deviceId) GameManager.Instance.readyFlags[1] = true;
                }
            }
        }
        else
        {
            if (context.started)
            {
                if (Gamepad.all.Count == 0)
                {
                    if (context.control.path == "/Keyboard/w" && notesManagers[0].inputAllowedFlag) notesManagers[0].SetSelectCommand(2);
                    if (context.control.path == "/Keyboard/upArrow" && notesManagers[1].inputAllowedFlag) notesManagers[1].SetSelectCommand(2);
                }
                else
                {
                    var inputDevice = context.control.device;
                    if (gamepads[0].deviceId == inputDevice.deviceId && notesManagers[0].inputAllowedFlag) notesManagers[0].SetSelectCommand(2);
                    else if (gamepads[1].deviceId == inputDevice.deviceId && notesManagers[1].inputAllowedFlag) notesManagers[1].SetSelectCommand(2);
                }
            }
        }
    }
    public void ButtonY(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (Gamepad.all.Count == 0)
            {
                if (context.control.path == "/Keyboard/a" && notesManagers[0].inputAllowedFlag) notesManagers[0].SetSelectCommand(1);
                if (context.control.path == "/Keyboard/leftArrow" && notesManagers[1].inputAllowedFlag) notesManagers[1].SetSelectCommand(1);
            }
            else
            {
                var inputDevice = context.control.device;
                if (gamepads[0].deviceId == inputDevice.deviceId && notesManagers[0].inputAllowedFlag) notesManagers[0].SetSelectCommand(1);
                else if (gamepads[1].deviceId == inputDevice.deviceId && notesManagers[1].inputAllowedFlag) notesManagers[1].SetSelectCommand(1);
            }
        }
    }
}

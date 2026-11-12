using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Switch;
using UnityEngine.InputSystem.XInput;

public class InputManager : MonoBehaviour
{
    [SerializeField] NotesManager[] notesManagers;
    Gamepad[] gamepads = new Gamepad[2];

    void Start()
    {
        Set();
    }

    void Update()
    {
        
    }

    void Set()
    {
        if (Gamepad.all.Count == 2)
        {
            gamepads[0] = Gamepad.all[0];
            notesManagers[0].gamepad = gamepads[0];
            gamepads[1] = Gamepad.all[1];
            notesManagers[1].gamepad = gamepads[1];

            for (int i = 0; i < 2; i++)
            {
                switch (gamepads[i])
                {
                    case XInputController:
                        notesManagers[i].controllerKind = ControllerKind.Xbox;
                        break;
                    case SwitchProControllerHID:
                        notesManagers[i].controllerKind = ControllerKind.Nintendo;
                        break;
                    case DualSenseGamepadHID:
                    case DualShockGamepad:
                        notesManagers[i].controllerKind = ControllerKind.PlayStation;
                        break;
                }
            }
        }
        else if (Gamepad.all.Count == 1)
        {
            gamepads[0] = Gamepad.all[0];
            notesManagers[0].gamepad = gamepads[0];
            gamepads[1] = null;
            notesManagers[1].gamepad = gamepads[1];
            switch (gamepads[0])
            {
                case XInputController:
                    notesManagers[0].controllerKind = ControllerKind.Xbox;
                    break;
                case SwitchProControllerHID:
                    notesManagers[0].controllerKind = ControllerKind.Nintendo;
                    break;
                case DualSenseGamepadHID:
                case DualShockGamepad:
                    notesManagers[0].controllerKind = ControllerKind.PlayStation;
                    break;
            }
            notesManagers[1].controllerKind = ControllerKind.None;
        }
    }
    public void ButtonA(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (TwoPMatchGameManager.Instance.setUpPracticeNotesFlag)
            {
                if (Gamepad.all.Count == 0)
                {
                    if (context.control.path == "/Keyboard/d" && notesManagers[0].setUpPracticeNotesFlag) notesManagers[0].PracticeSelectCommand(0);
                    else if (context.control.path == "/Keyboard/rightArrow" && notesManagers[1].setUpPracticeNotesFlag) notesManagers[1].PracticeSelectCommand(0);
                }
                else if (Gamepad.all.Count == 1)
                {
                    var inputDevice = context.control.device;
                    if (gamepads[0].deviceId == inputDevice.deviceId && notesManagers[0].setUpPracticeNotesFlag) notesManagers[0].PracticeSelectCommand(0);
                    else if (context.control.path == "/Keyboard/rightArrow" && notesManagers[1].setUpPracticeNotesFlag) notesManagers[1].PracticeSelectCommand(0);
                }
                else
                {
                    var inputDevice = context.control.device;
                    if (gamepads[0].deviceId == inputDevice.deviceId && notesManagers[0].setUpPracticeNotesFlag) notesManagers[0].PracticeSelectCommand(0);
                    else if (gamepads[1].deviceId == inputDevice.deviceId && notesManagers[1].setUpPracticeNotesFlag) notesManagers[1].PracticeSelectCommand(0);
                }
            }
            else
            {
                if (Gamepad.all.Count == 0)
                {
                    if (context.control.path == "/Keyboard/d" && notesManagers[0].inputAllowedFlag) notesManagers[0].SetSelectCommand(0);
                    else if (context.control.path == "/Keyboard/rightArrow" && notesManagers[1].inputAllowedFlag) notesManagers[1].SetSelectCommand(0);
                }
                else if (Gamepad.all.Count == 1)
                {
                    var inputDevice = context.control.device;
                    if (gamepads[0].deviceId == inputDevice.deviceId && notesManagers[0].inputAllowedFlag) notesManagers[0].SetSelectCommand(0);
                    else if (context.control.path == "/Keyboard/rightArrow" && notesManagers[1].inputAllowedFlag) notesManagers[1].SetSelectCommand(0);
                }
                else
                {
                    var inputDevice = context.control.device;
                    if (gamepads[0].deviceId == inputDevice.deviceId && notesManagers[0].inputAllowedFlag) notesManagers[0].SetSelectCommand(0);
                    else if (gamepads[1].deviceId == inputDevice.deviceId && notesManagers[1].inputAllowedFlag) notesManagers[1].SetSelectCommand(0);
                }
            }
        }
    }
    public void ButtonB(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (TwoPMatchGameManager.Instance.setUpPracticeNotesFlag)
            {
                if (Gamepad.all.Count == 0)
                {
                    if (context.control.path == "/Keyboard/s" && notesManagers[0].setUpPracticeNotesFlag) notesManagers[0].PracticeSelectCommand(3);
                    else if (context.control.path == "/Keyboard/downArrow" && notesManagers[1].setUpPracticeNotesFlag) notesManagers[1].PracticeSelectCommand(3);
                }
                else if (Gamepad.all.Count == 1)
                {
                    var inputDevice = context.control.device;
                    if (gamepads[0].deviceId == inputDevice.deviceId && notesManagers[0].setUpPracticeNotesFlag) notesManagers[0].PracticeSelectCommand(3);
                    else if (context.control.path == "/Keyboard/downArrow" && notesManagers[1].setUpPracticeNotesFlag) notesManagers[1].PracticeSelectCommand(3);
                }
                else
                {
                    var inputDevice = context.control.device;
                    if (gamepads[0].deviceId == inputDevice.deviceId && notesManagers[0].setUpPracticeNotesFlag) notesManagers[0].PracticeSelectCommand(3);
                    else if (gamepads[1].deviceId == inputDevice.deviceId && notesManagers[1].setUpPracticeNotesFlag) notesManagers[1].PracticeSelectCommand(3);
                }
            }
            else
            {
                if (Gamepad.all.Count == 0)
                {
                    if (context.control.path == "/Keyboard/s" && notesManagers[0].inputAllowedFlag) notesManagers[0].SetSelectCommand(3);
                    else if (context.control.path == "/Keyboard/downArrow" && notesManagers[1].inputAllowedFlag) notesManagers[1].SetSelectCommand(3);
                }
                else if (Gamepad.all.Count == 1)
                {
                    var inputDevice = context.control.device;
                    if (gamepads[0].deviceId == inputDevice.deviceId && notesManagers[0].inputAllowedFlag) notesManagers[0].SetSelectCommand(3);
                    else if (context.control.path == "/Keyboard/downArrow" && notesManagers[1].inputAllowedFlag) notesManagers[1].SetSelectCommand(3);
                }
                else
                {
                    var inputDevice = context.control.device;
                    if (gamepads[0].deviceId == inputDevice.deviceId && notesManagers[0].inputAllowedFlag) notesManagers[0].SetSelectCommand(3);
                    else if (gamepads[1].deviceId == inputDevice.deviceId && notesManagers[1].inputAllowedFlag) notesManagers[1].SetSelectCommand(3);
                }
            }
        }
    }
    public void ButtonX(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (TwoPMatchGameManager.Instance.setUpPracticeNotesFlag)
            {
                if (Gamepad.all.Count == 0)
                {
                    if (context.control.path == "/Keyboard/w" && notesManagers[0].setUpPracticeNotesFlag) notesManagers[0].PracticeSelectCommand(2);
                    else if (context.control.path == "/Keyboard/upArrow" && notesManagers[1].setUpPracticeNotesFlag) notesManagers[1].PracticeSelectCommand(2);
                }
                else if (Gamepad.all.Count == 1)
                {
                    var inputDevice = context.control.device;
                    if (gamepads[0].deviceId == inputDevice.deviceId && notesManagers[0].setUpPracticeNotesFlag) notesManagers[0].PracticeSelectCommand(2);
                    else if (context.control.path == "/Keyboard/upArrow" && notesManagers[1].setUpPracticeNotesFlag) notesManagers[1].PracticeSelectCommand(2);
                }
                else
                {
                    var inputDevice = context.control.device;
                    if (gamepads[0].deviceId == inputDevice.deviceId && notesManagers[0].setUpPracticeNotesFlag) notesManagers[0].PracticeSelectCommand(2);
                    else if (gamepads[1].deviceId == inputDevice.deviceId && notesManagers[1].setUpPracticeNotesFlag) notesManagers[1].PracticeSelectCommand(2);
                }
            }
            else
            {
                if (Gamepad.all.Count == 0)
                {
                    if (context.control.path == "/Keyboard/w" && notesManagers[0].inputAllowedFlag) notesManagers[0].SetSelectCommand(2);
                    else if (context.control.path == "/Keyboard/upArrow" && notesManagers[1].inputAllowedFlag) notesManagers[1].SetSelectCommand(2);
                }
                else if (Gamepad.all.Count == 1)
                {
                    var inputDevice = context.control.device;
                    if (gamepads[0].deviceId == inputDevice.deviceId && notesManagers[0].inputAllowedFlag) notesManagers[0].SetSelectCommand(2);
                    else if (context.control.path == "/Keyboard/upArrow" && notesManagers[1].inputAllowedFlag) notesManagers[1].SetSelectCommand(2);
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
            if (TwoPMatchGameManager.Instance.setUpPracticeNotesFlag)
            {
                if (Gamepad.all.Count == 0)
                {
                    if (context.control.path == "/Keyboard/a" && notesManagers[0].setUpPracticeNotesFlag) notesManagers[0].PracticeSelectCommand(1);
                    else if (context.control.path == "/Keyboard/leftArrow" && notesManagers[1].setUpPracticeNotesFlag) notesManagers[1].PracticeSelectCommand(1);
                }
                else if (Gamepad.all.Count == 1)
                {
                    var inputDevice = context.control.device;
                    if (gamepads[0].deviceId == inputDevice.deviceId && notesManagers[0].setUpPracticeNotesFlag) notesManagers[0].PracticeSelectCommand(1);
                    else if (context.control.path == "/Keyboard/leftArrow" && notesManagers[1].setUpPracticeNotesFlag) notesManagers[1].PracticeSelectCommand(1);
                }
                else
                {
                    var inputDevice = context.control.device;
                    if (gamepads[0].deviceId == inputDevice.deviceId && notesManagers[0].setUpPracticeNotesFlag) notesManagers[0].PracticeSelectCommand(1);
                    else if (gamepads[1].deviceId == inputDevice.deviceId && notesManagers[1].setUpPracticeNotesFlag) notesManagers[1].PracticeSelectCommand(1);
                }
            }
            else
            {
                if (Gamepad.all.Count == 0)
                {
                    if (context.control.path == "/Keyboard/a" && notesManagers[0].inputAllowedFlag) notesManagers[0].SetSelectCommand(1);
                    else if (context.control.path == "/Keyboard/leftArrow" && notesManagers[1].inputAllowedFlag) notesManagers[1].SetSelectCommand(1);
                }
                else if (Gamepad.all.Count == 1)
                {
                    var inputDevice = context.control.device;
                    if (gamepads[0].deviceId == inputDevice.deviceId && notesManagers[0].inputAllowedFlag) notesManagers[0].SetSelectCommand(1);
                    else if (context.control.path == "/Keyboard/leftArrow" && notesManagers[1].inputAllowedFlag) notesManagers[1].SetSelectCommand(1);
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

    public void ButtonChange(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (Gamepad.all.Count == 0)
            {
                if (context.control.path == "/Keyboard/e") notesManagers[0].SetNotesDesign();
                else if (context.control.path == "/Keyboard/rightShift") notesManagers[1].SetNotesDesign();
            }
            else if (Gamepad.all.Count == 1)
            {
                var inputDevice = context.control.device;
                if (gamepads[0].deviceId == inputDevice.deviceId) notesManagers[0].SetNotesDesign();
                else if (context.control.path == "/Keyboard/rightShift") notesManagers[1].SetNotesDesign();
            }
            else
            {
                var inputDevice = context.control.device;
                if (gamepads[0].deviceId == inputDevice.deviceId) notesManagers[0].SetNotesDesign();
                else if (gamepads[1].deviceId == inputDevice.deviceId) notesManagers[1].SetNotesDesign();
            }
        }
    }
}

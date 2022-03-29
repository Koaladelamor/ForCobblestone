using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    enum ACTIONS { USE, UP, DOWN, LEFT, RIGHT, MOVE_PARTY, PAUSE, NORMAL, FAST, CAMERA, INVENTORY, MENU, NUMBER_OF_ACTIONS };

    static InputManager mInstance;

    static public InputManager Instance
    {
        get { return mInstance; }
        private set { }
    }

    KeyCode useButton = KeyCode.E;
    KeyCode upButton = KeyCode.W;
    KeyCode downButton = KeyCode.S;
    KeyCode leftButton = KeyCode.A;
    KeyCode rightButton = KeyCode.D;
    int rightClick = 1;
    KeyCode cameraButton = KeyCode.V;
    KeyCode pauseButton = KeyCode.Space;
    KeyCode pauseButton2 = KeyCode.Alpha1;
    KeyCode x1SpeedButton = KeyCode.Alpha2;
    KeyCode x2SpeedButton = KeyCode.Alpha3;
    KeyCode inventoryButton = KeyCode.I;
    KeyCode menuButton = KeyCode.Escape;

    bool[] buttonsPressed = new bool[(int)ACTIONS.NUMBER_OF_ACTIONS];
    bool[] buttonsHold = new bool[(int)ACTIONS.NUMBER_OF_ACTIONS];
    bool[] buttonsReleased = new bool[(int)ACTIONS.NUMBER_OF_ACTIONS];

    private void Awake()
    {
        if (mInstance == null) { mInstance = this; }
        else { Destroy(this.gameObject); }

        for (int i = 0; i < (int)ACTIONS.NUMBER_OF_ACTIONS; i++)
        {
            buttonsPressed[i] = false;
            buttonsHold[i] = false;
            buttonsReleased[i] = false;
        }
    }

    void Update()
    {
        for (int i = 0; i < (int)ACTIONS.NUMBER_OF_ACTIONS; i++)
        {
            buttonsPressed[i] = false;
            buttonsHold[i] = false;
            buttonsReleased[i] = false;
        }

        if (Input.GetKeyDown(useButton)) { buttonsPressed[(int)ACTIONS.USE] = true; }
        if (Input.GetKey(useButton)) { buttonsHold[(int)ACTIONS.USE] = true; }
        if (Input.GetKeyUp(useButton)) { buttonsReleased[(int)ACTIONS.USE] = true; }

        if (Input.GetKeyDown(upButton)) { buttonsPressed[(int)ACTIONS.UP] = true; }
        if (Input.GetKey(upButton)) { buttonsHold[(int)ACTIONS.UP] = true; }
        if (Input.GetKeyUp(upButton)) { buttonsReleased[(int)ACTIONS.UP] = true; }

        if (Input.GetKeyDown(downButton)) { buttonsPressed[(int)ACTIONS.DOWN] = true; }
        if (Input.GetKey(downButton)) { buttonsHold[(int)ACTIONS.DOWN] = true; }
        if (Input.GetKeyUp(downButton)) { buttonsReleased[(int)ACTIONS.DOWN] = true; }

        if (Input.GetKeyDown(leftButton)) { buttonsPressed[(int)ACTIONS.LEFT] = true; }
        if (Input.GetKey(leftButton)) { buttonsHold[(int)ACTIONS.LEFT] = true; }
        if (Input.GetKeyUp(leftButton)) { buttonsReleased[(int)ACTIONS.LEFT] = true; }

        if (Input.GetKeyDown(rightButton)) { buttonsPressed[(int)ACTIONS.RIGHT] = true; }
        if (Input.GetKey(rightButton)) { buttonsHold[(int)ACTIONS.RIGHT] = true; }
        if (Input.GetKeyUp(rightButton)) { buttonsReleased[(int)ACTIONS.RIGHT] = true; }

        if (Input.GetMouseButtonDown(rightClick)) { buttonsPressed[(int)ACTIONS.MOVE_PARTY] = true; }
        if (Input.GetMouseButton(rightClick)) { buttonsHold[(int)ACTIONS.MOVE_PARTY] = true; }
        if (Input.GetMouseButtonUp(rightClick)) { buttonsReleased[(int)ACTIONS.MOVE_PARTY] = true; }

        if (Input.GetKeyDown(cameraButton)) { buttonsPressed[(int)ACTIONS.CAMERA] = true; }
        if (Input.GetKey(cameraButton)) { buttonsHold[(int)ACTIONS.CAMERA] = true; }
        if (Input.GetKeyUp(cameraButton)) { buttonsReleased[(int)ACTIONS.CAMERA] = true; }

        if (Input.GetKeyDown(pauseButton) || Input.GetKeyDown(pauseButton2)) { buttonsPressed[(int)ACTIONS.PAUSE] = true; }
        if (Input.GetKey(pauseButton) || Input.GetKeyDown(pauseButton2)) { buttonsHold[(int)ACTIONS.PAUSE] = true; }
        if (Input.GetKeyUp(pauseButton) || Input.GetKeyDown(pauseButton2)) { buttonsReleased[(int)ACTIONS.PAUSE] = true; }

        if (Input.GetKeyDown(x1SpeedButton)) { buttonsPressed[(int)ACTIONS.NORMAL] = true; }
        if (Input.GetKey(x1SpeedButton)) { buttonsHold[(int)ACTIONS.NORMAL] = true; }
        if (Input.GetKeyUp(x1SpeedButton)) { buttonsReleased[(int)ACTIONS.NORMAL] = true; }

        if (Input.GetKeyDown(x2SpeedButton)) { buttonsPressed[(int)ACTIONS.FAST] = true; }
        if (Input.GetKey(x2SpeedButton)) { buttonsHold[(int)ACTIONS.FAST] = true; }
        if (Input.GetKeyUp(x2SpeedButton)) { buttonsReleased[(int)ACTIONS.FAST] = true; }

        if (Input.GetKeyDown(inventoryButton)) { buttonsPressed[(int)ACTIONS.INVENTORY] = true; }
        if (Input.GetKey(inventoryButton)) { buttonsHold[(int)ACTIONS.INVENTORY] = true; }
        if (Input.GetKeyUp(inventoryButton)) { buttonsReleased[(int)ACTIONS.INVENTORY] = true; }

        if (Input.GetKeyDown(menuButton)) { buttonsPressed[(int)ACTIONS.MENU] = true; }
        if (Input.GetKey(menuButton)) { buttonsHold[(int)ACTIONS.MENU] = true; }
        if (Input.GetKeyUp(menuButton)) { buttonsReleased[(int)ACTIONS.MENU] = true; }

    }


    public bool UseButtonPressed { get { return buttonsPressed[(int)ACTIONS.USE]; } }
    public bool UseButtonHold { get { return buttonsHold[(int)ACTIONS.USE]; } }
    public bool UseButtonReleased { get { return buttonsReleased[(int)ACTIONS.USE]; } }

    public bool UpButtonPressed { get { return buttonsPressed[(int)ACTIONS.UP]; } }
    public bool UpButtonHold { get { return buttonsHold[(int)ACTIONS.UP]; } }
    public bool UpButtonReleased { get { return buttonsReleased[(int)ACTIONS.UP]; } }

    public bool DownButtonPressed { get { return buttonsPressed[(int)ACTIONS.DOWN]; } }
    public bool DownButtonHold { get { return buttonsHold[(int)ACTIONS.DOWN]; } }
    public bool DownButtonReleased { get { return buttonsReleased[(int)ACTIONS.DOWN]; } }

    public bool LeftButtonPressed { get { return buttonsPressed[(int)ACTIONS.LEFT]; } }
    public bool LeftButtonHold { get { return buttonsHold[(int)ACTIONS.LEFT]; } }
    public bool LeftButtonReleased { get { return buttonsReleased[(int)ACTIONS.LEFT]; } }

    public bool RightButtonPressed { get { return buttonsPressed[(int)ACTIONS.RIGHT]; } }
    public bool RightButtonHold { get { return buttonsHold[(int)ACTIONS.RIGHT]; } }
    public bool RightButtonReleased { get { return buttonsReleased[(int)ACTIONS.RIGHT]; } }

    public bool RightClickButtonPressed { get { return buttonsPressed[(int)ACTIONS.MOVE_PARTY]; } }
    public bool RightClickButtonHold { get { return buttonsHold[(int)ACTIONS.MOVE_PARTY]; } }
    public bool RightClickButtonReleased { get { return buttonsReleased[(int)ACTIONS.MOVE_PARTY]; } }

    public bool CameraButtonPressed { get { return buttonsPressed[(int)ACTIONS.CAMERA]; } }
    public bool CameraButtonHold { get { return buttonsHold[(int)ACTIONS.CAMERA]; } }
    public bool CameraButtonReleased { get { return buttonsReleased[(int)ACTIONS.CAMERA]; } }

    public bool PauseButtonPressed { get { return buttonsPressed[(int)ACTIONS.PAUSE]; } }
    public bool PauseButtonHold { get { return buttonsHold[(int)ACTIONS.PAUSE]; } }
    public bool PauseButtonReleased { get { return buttonsReleased[(int)ACTIONS.PAUSE]; } }

    public bool NormalSpeedButtonPressed { get { return buttonsPressed[(int)ACTIONS.NORMAL]; } }
    public bool NormalSpeedButtonHold { get { return buttonsHold[(int)ACTIONS.NORMAL]; } }
    public bool NormalSpeedButtonReleased { get { return buttonsReleased[(int)ACTIONS.NORMAL]; } }

    public bool FastSpeedButtonPressed { get { return buttonsPressed[(int)ACTIONS.FAST]; } }
    public bool FastSpeedButtonHold { get { return buttonsHold[(int)ACTIONS.FAST]; } }
    public bool FastSpeedButtonReleased { get { return buttonsReleased[(int)ACTIONS.FAST]; } }

    public bool InventoryButtonPressed { get { return buttonsPressed[(int)ACTIONS.INVENTORY]; } }
    public bool InventoryButtonHold { get { return buttonsHold[(int)ACTIONS.INVENTORY]; } }
    public bool InventoryButtonReleased { get { return buttonsReleased[(int)ACTIONS.INVENTORY]; } }

    public bool MenuButtonPressed { get { return buttonsPressed[(int)ACTIONS.MENU]; } }
    public bool MenuButtonHold { get { return buttonsHold[(int)ACTIONS.MENU]; } }
    public bool MenuButtonReleased { get { return buttonsReleased[(int)ACTIONS.MENU]; } }

}
using UnityEngine;
using UnityEngine.InputSystem;

public class UniversalInputHandler : MonoBehaviour
{
    public static UniversalInputHandler instance;
    
    [SerializeField] private GameObject inputReciever;

    private PlayerInput playerInput;

    private InputAction upAction;
    private InputAction rightAction;
    private InputAction downAction;
    private InputAction leftAction;

    private InputAction aAction;
    private InputAction bAction;

    private InputAction selectAction;
    private InputAction startAction;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        upAction = playerInput.actions.FindAction("Up");
        rightAction = playerInput.actions.FindAction("Right");
        downAction = playerInput.actions.FindAction("Down");
        leftAction = playerInput.actions.FindAction("Left");

        aAction = playerInput.actions.FindAction("A");
        bAction = playerInput.actions.FindAction("B");

        selectAction = playerInput.actions.FindAction("Select");
        startAction = playerInput.actions.FindAction("Start");

        if (inputReciever == null)
        {
            ChangeInputReciever(GameObject.FindGameObjectWithTag("StartingInput"));
        }
    }

    private void Update()
    {
        if (inputReciever != null)
        {
            if (upAction.triggered)
            {
                inputReciever.GetComponent<InputReciever>().SendMessage("UpInput", SendMessageOptions.DontRequireReceiver);
            }

            if (rightAction.triggered)
            {
                inputReciever.GetComponent<InputReciever>().SendMessage("RightInput", SendMessageOptions.DontRequireReceiver);
            }

            if (downAction.triggered)
            {
                inputReciever.GetComponent<InputReciever>().SendMessage("DownInput", SendMessageOptions.DontRequireReceiver);
            }

            if (leftAction.triggered)
            {
                inputReciever.GetComponent<InputReciever>().SendMessage("LeftInput", SendMessageOptions.DontRequireReceiver);
            }

            if (aAction.triggered)
            {
                inputReciever.GetComponent<InputReciever>().SendMessage("AInput", SendMessageOptions.DontRequireReceiver);
            }

            if (bAction.triggered)
            {
                inputReciever.GetComponent<InputReciever>().SendMessage("BInput", SendMessageOptions.DontRequireReceiver);
            }

            if (selectAction.triggered)
            {
                inputReciever.GetComponent<InputReciever>().SendMessage("SelectInput", SendMessageOptions.DontRequireReceiver);
            }   

            if (startAction.triggered)
            {
                inputReciever.GetComponent<InputReciever>().SendMessage("StartInput", SendMessageOptions.DontRequireReceiver);
            }
        }
        
    }

    public void ChangeInputReciever(GameObject newInputReciever)
    {
        inputReciever = newInputReciever;
    }
}

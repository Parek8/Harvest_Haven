using System;
using System.Collections;
using TMPro;
using UnityEngine;

public sealed class Tutorial : MonoBehaviour
{
    public static Tutorial TutorialInstance
    {
        get
        {
            if (_instance == null)
                _instance = new Tutorial();
            return _instance;
        }
    }

    private static Tutorial _instance;

    private TutorialState _tutorialState = TutorialState.Movement;

    // Movement stage
    [field: Header("MOVEMENT")]
    [field: SerializeField] UI_Behaviour MoveDialog;
    [field: SerializeField] UI_Behaviour WCheck;
    [field: SerializeField] UI_Behaviour ACheck;
    [field: SerializeField] UI_Behaviour SCheck;
    [field: SerializeField] UI_Behaviour DCheck;
    bool _forward = false;
    bool _backward = false;
    bool _left = false;
    bool _right = false;

    [field: Header("INVENTORY")]
    [field: SerializeField] UI_Behaviour InvDialog;
    [field: SerializeField] UI_Behaviour TABCheck;
    [field: SerializeField] Character_Stats PlayerStats;
    [field: SerializeField] Character_Behaviour PlayerBehaviour;
    [field: SerializeField] Player_Health PlayerHealth;
    [field: SerializeField] Inventory PlayerInventory;
    bool _tab = false;

    [field: Header("INTERACT")]
    [field: SerializeField] UI_Behaviour IntDialog;
    [field: SerializeField] UI_Behaviour ECheck;

    bool _interacted = false;


    private void Start()
    {
        InitMovement();
    }
    private void FixedUpdate()
    {
        switch (_tutorialState)
        {
            case TutorialState.Movement:
                MovementTutorial();
                break;
            case TutorialState.Inventory:
                InventoryTutorial();
                break;
            case TutorialState.Interact:
                InteractTutorial();
                break;
            default:
                Debug.Log("Tak a je to v pièi!");
                break;
        }
    }
    #region Init
    private void InitMovement()
    {
        MoveDialog.Show();
    }
    private IEnumerator InitInventory()
    {
        yield return new WaitForSeconds(1);

        PlayerStats.enabled = true;
        PlayerBehaviour.enabled = true;
        PlayerHealth.enabled = true;
        PlayerInventory.enabled = true;
        InvDialog.Show();
        MoveDialog.Hide();
    }
    private IEnumerator InitInteract()
    {
        yield return new WaitForSeconds(1);

        InvDialog.Hide();
        IntDialog.Show();
    }

    internal void Interacted()
    {
        if (_tutorialState == TutorialState.Interact)
            _interacted = true;
    }

    private IEnumerator InitDestroy()
    {
        yield return new WaitForSeconds(1);

        IntDialog.Hide();
        //IntDialog.Show();
    }

    #endregion Init
    #region Tutorials
    private void MovementTutorial()
    {
        if (!_forward || !_backward || !_left || !_right)
        {
            if (Input_Manager.GetCustomKeyDown(KeybindNames.forward))
            {
                WCheck.Show();
                _forward = true;
            }
            if (Input_Manager.GetCustomKeyDown(KeybindNames.backward))
            {
                SCheck.Show();
                _backward = true;
            }
            if (Input_Manager.GetCustomKeyDown(KeybindNames.left_strafe))
            {
                ACheck.Show();
                _left = true;
            }
            if (Input_Manager.GetCustomKeyDown(KeybindNames.right_strafe))
            {
                DCheck.Show();
                _right = true;
            }
        }
        else
        {
            _tutorialState = TutorialState.Inventory;
            StartCoroutine(InitInventory());
        }
    }
    private void InventoryTutorial()
    {
        if (!_tab)
        {
            if (Input_Manager.GetCustomKeyDown(KeybindNames.inventory))
            {
                TABCheck.Show();
                _tab = true;
            }
        }
        else
        {
            _tutorialState = TutorialState.Interact;
            StartCoroutine(InitInteract());
        }
    }
    private void InteractTutorial()
    {
        if (_interacted)
        {
            ECheck.Show();
            _tutorialState = TutorialState.Destroy;
            StartCoroutine(InitDestroy());
        }
    }

    #endregion Tutorials
    private enum TutorialState
    {
        Movement,
        Inventory,
        Interact,
        Destroy,
        Craft,
        Smelt,
        Shopping
    }
}
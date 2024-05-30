using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public sealed class Tutorial : MonoBehaviour
{
    public static Tutorial TutorialInstance
    {
        get
        {
            if (_instance == null)
                return null;

            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }
    private static Tutorial _instance;

    private TutorialState _tutorialState = TutorialState.Movement;
    [field: SerializeField] float TransitionDelay = 1.5f;
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
    [field: SerializeField] UI_Behaviour ShopHighlighter;
    bool _interacted = false;

    [field: Header("SWAP SLOTS")]
    [field: SerializeField] UI_Behaviour SwapDialog;
    Item _assignedItem;
    bool _swappedSlots = false;

    [field: Header("DESTROY OBJECTS")]
    [field: SerializeField] UI_Behaviour DesDialog;
    [field: SerializeField] Item AxeItem;
    bool _destroyedObject = false;

    [field: Header("CRAFT")]
    [field: SerializeField] UI_Behaviour CraftDialog;
    [field: SerializeField] Item Wood;
    [field: SerializeField] Item Rock;
    bool _crafted = false;

    [field: Header("SEEDING")]
    [field: SerializeField] UI_Behaviour SeedDialog;
    [field: SerializeField] List<Item> Seeds;
    bool _seeded = false;

    [field: Header("HARVESTING")]
    [field: SerializeField] UI_Behaviour HarvestDialog;
    bool _harvested = false;

    /* -----------------------------------------------
       |                                             |
       |             NOT IN USE!!!                   |
       |             NOT IN USE!!!                   |
       |             NOT IN USE!!!                   |
       |                                             |
       ----------------------------------------------- */

    [field: Header("SHOPPING BUY")]
    [field: SerializeField] UI_Behaviour BuyDialog;
    bool _boughtItem = false;

    [field: Header("SHOPPING SELL")]
    [field: SerializeField] UI_Behaviour SelDialog;
    [field: SerializeField] Item PotatoItem;
    bool _soldItem = false;

    private void Start()
    {
        //MoveDialog.Hide();
        InvDialog.Hide();
        IntDialog.Hide();
        SwapDialog.Hide();
        DesDialog.Hide();
        BuyDialog.Hide();
        SelDialog.Hide();
        CraftDialog.Hide();
        SeedDialog.Hide();
        HarvestDialog.Hide();

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

            case TutorialState.DragItems:
                DragItemsTutorial();
                break;

            case TutorialState.Interact:
                InteractTutorial();
                break;

            case TutorialState.ShoppingBuy:
                BuyTutorial();
                break;

            case TutorialState.ShoppingSell:
                SellTutorial();
                break;

            case TutorialState.Destroy:
                DestroyTutorial();
                break;

            case TutorialState.Craft:
                CraftTutorial();
                break;

            case TutorialState.Smelt:
                SmeltTutorial();
                break;

            case TutorialState.Seeding:
                SeedingTutorial();
                break;

            case TutorialState.Harvesting:
                HarvestingTutorial();
                break;

            default:
                Debug.Log("Tak a je to v pièi! :)");
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
        yield return new WaitForSeconds(TransitionDelay);

        PlayerStats.enabled = true;
        PlayerBehaviour.enabled = true;
        PlayerHealth.enabled = true;
        PlayerInventory.enabled = true;
        InvDialog.Show();
        MoveDialog.Hide();
    }
    private IEnumerator InitInteract()
    {
        yield return new WaitForSeconds(TransitionDelay);

        DesDialog.Hide();
        //ShopHighlighter.Show();
        IntDialog.Show();
    }

    internal void Interacted()
    {
        if (_tutorialState == TutorialState.Interact)
            _interacted = true;
    }
    internal void SwappedSlots()
    {
        if (_tutorialState == TutorialState.DragItems)
            _swappedSlots = true;
    }
    internal void DestroyedObject()
    {
        if (_tutorialState == TutorialState.Destroy)
            _destroyedObject = true;
    }
    private IEnumerator InitDestroy()
    {
        yield return new WaitForSeconds(TransitionDelay);

        SwapDialog.Hide();
        DesDialog.Show();

        GameManager.game_manager.player_inventory.Add(AxeItem);
    }

    private IEnumerator InitDragSlots()
    {
        yield return new WaitForSeconds(TransitionDelay);

        _assignedItem = GameManager.game_manager.all_items[UnityEngine.Random.Range(1, GameManager.game_manager.all_items.Count - 1)];
        GameManager.game_manager.player_inventory.Add(_assignedItem);

        InvDialog.Hide();
        SwapDialog.Show();
    }

    private IEnumerator InitBuy()
    {
        yield return new WaitForSeconds(TransitionDelay);

        IntDialog.Hide();
        BuyDialog.Show();
    }
    private IEnumerator InitSell()
    {
        yield return new WaitForSeconds(TransitionDelay);

        BuyDialog.Hide();
        SelDialog.Show();
    }

    private IEnumerator InitCraft()
    {
        yield return new WaitForSeconds(TransitionDelay);

        IntDialog.Hide();
        CraftDialog.Show();
    }

    internal void Crafted()
    {
        _crafted = true;
    }
   
    private IEnumerator InitSeeding()
    {
        yield return new WaitForSeconds(TransitionDelay);

        CraftDialog.Hide();
        SeedDialog.Show();

        GameManager.game_manager.player_inventory.Add(Seeds[Random.Range(0, Seeds.Count)]);
        GameManager.game_manager.player_inventory.Add(Seeds[Random.Range(0, Seeds.Count)]);
        GameManager.game_manager.player_inventory.Add(Seeds[Random.Range(0, Seeds.Count)]);
    }
    internal void Seeded()
    {
        _seeded = true;
    }

    private IEnumerator InitHarvesting()
    {
        yield return new WaitForSeconds(TransitionDelay);

        SeedDialog.Hide();
        HarvestDialog.Show();
    }

    internal void Harvested()
    {
        _harvested = true;
    }
    internal void BoughtItem()
    {
        if (_tutorialState == TutorialState.ShoppingBuy)
            _boughtItem = true;
    }

    internal void SoldItem()
    {
        if (_tutorialState == TutorialState.ShoppingSell)
            _soldItem = true;
    }
    #endregion Init
    #region Tutorials
    private void MovementTutorial()
    {
        if (!_forward || !_backward || !_left || !_right)
        {
            if (Input_Manager.GetCustomKeyDown(KeybindNames.forward))
            {
                //WCheck.Show();
                _forward = true;
            }
            if (Input_Manager.GetCustomKeyDown(KeybindNames.backward))
            {
                //SCheck.Show();
                _backward = true;
            }
            if (Input_Manager.GetCustomKeyDown(KeybindNames.left_strafe))
            {
                //ACheck.Show();
                _left = true;
            }
            if (Input_Manager.GetCustomKeyDown(KeybindNames.right_strafe))
            {
                //DCheck.Show();
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
                //TABCheck.Show();
                _tab = true;
            }
        }
        else
        {
            _tutorialState = TutorialState.DragItems;
            StartCoroutine(InitDragSlots());
        }
    }

    private void DragItemsTutorial()
    {
        if (_swappedSlots)
        {
            _tutorialState = TutorialState.Destroy;
            StartCoroutine(InitDestroy());
        }
    }
    private void DestroyTutorial()
    {
        if (_destroyedObject)
        {
            _tutorialState = TutorialState.Interact;
            StartCoroutine(InitInteract());
        }
    }
    private void InteractTutorial()
    {
        if (_interacted)
        {
            //ECheck.Show();
            _tutorialState = TutorialState.Craft;
            StartCoroutine(InitCraft());
        }
    }

    private void BuyTutorial()
    {
        if (_boughtItem)
        {
            _tutorialState = TutorialState.ShoppingSell;
            StartCoroutine(InitSell());
        }
    }
    private void SellTutorial()
    {
        if (_soldItem)
        {
            _tutorialState = TutorialState.Destroy;
            StartCoroutine(InitDestroy());
        }
    }
    private void CraftTutorial()
    {
        if (_crafted)
        {
            _tutorialState = TutorialState.Seeding;
            StartCoroutine(InitSeeding());
        }
    }
    private void SmeltTutorial()
    {

    }

    private void SeedingTutorial()
    {
        if (_seeded)
        {
            _tutorialState = TutorialState.Harvesting;
            StartCoroutine(InitHarvesting());
        }
    }
    private void HarvestingTutorial()
    {
        if (_harvested)
        {

        }
    }
    #endregion Tutorials
    private enum TutorialState
    {
        Movement,
        Inventory,
        DragItems,
        Interact,
        Destroy,
        Craft,
        Smelt,
        Seeding,
        Harvesting,
        // NOT IN USE
        ShoppingBuy,
        ShoppingSell,
    }
}
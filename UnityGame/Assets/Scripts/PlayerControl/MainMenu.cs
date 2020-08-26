using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;
public class MainMenu : MonoBehaviour
{
    // public class Menu
    // {
    //     public string menuName;
    //     public GameObject menu;
    //     public GameObject firstButton;
    //     public GameObject backButton;
    // }
    private const string MAIN_MENU_NODE_STRING = "Main Menu Node";
    private const string PLAY_MENU_NODE_STRING = "Play Menu Node";
    private const string OPTIONS_MENU_NODE_STRING = "Options Menu Node";
    private const string ARENA_MENU_NODE_STRING = "Arena Menu Node";
    private const string CAMPAIGN_MENU_NODE_STRING = "Campaign Menu Node";

    private MenuNode activeMenuNode;
    private GameObject selectedButton;

    //main menu
    [Space]
    [Header("MainMenu And Buttons:")]
    public GameObject mainMenu;
    public GameObject firstButton;
    public GameObject playClosedButton;
    public GameObject optionsClosedButton;
    // mainMenu sub Menus
    [Space]
    [Header("PLayMenu And Buttons:")]
    public GameObject playMenu;
    public GameObject playFirstButton;
    public GameObject arenaClosedButton;
    public GameObject campaignClosedButton;
    [Space]
    [Header("OPtionsMenu And Buttons:")]
    public GameObject FullscreenSelectText;
    public GameObject optionsMenu;
    public GameObject optionsFirstButton;

    // playMenu sub Menus
    [Space]
    [Header("ArenaMenu And Buttons:")]
    public GameObject arenaMenu;
    public GameObject arenaFirstButton;
    public GameObject gameTypeText;
    private GameTypes gameTypeSelected = (GameTypes)1;
    public GameObject increaseGameTypeBtn;
    public GameObject decreaseGameTypeBtn;

    public GameObject mapSelectText;
    private Maps mapSelected = (Maps)1;
    public GameObject increaseMapBtn;
    public GameObject decreaseMapBtn;

    [Space]
    [Header("CampaignMenu And Buttons:")]
    public GameObject campaignMenu;
    public GameObject campaignFirstButton;
    private Levels levelSelected = (Levels)1;
    public GameObject increaseLevelBtn;
    public GameObject decreaseLevelBtn;

    public GameObject levelSelectedText;



    public class MenuNode
    {
        //constructors
        public MenuNode(string n, GameObject m)
        {
            this.menuName = n;
            this.menu = m;
            childrenMenu = new List<MenuNode>();
        }
        public MenuNode(string n, GameObject m, MenuNode p)
        {
            this.menuName = n;
            this.parentNode = p;
            this.menu = m;
            childrenMenu = new List<MenuNode>();

        }
        public MenuNode(string n, GameObject m, GameObject fb)
        {
            this.menuName = n;
            this.menu = m;
            this.firstSelectedButton = fb;
            childrenMenu = new List<MenuNode>();

        }

        public MenuNode(string n, MenuNode p, GameObject m, GameObject fb, GameObject bb)
        {
            this.menuName = n;
            this.parentNode = p;
            this.menu = m;
            this.goBackSelectedButton = bb;
            this.firstSelectedButton = fb;
            childrenMenu = new List<MenuNode>();

        }

        //members
        public string menuName;
        public GameObject menu;
        public GameObject firstSelectedButton; // this is the one that should be first selected
        public GameObject goBackSelectedButton; // this is the button to be used when going backwards 
        public MenuNode parentNode;
        public List<MenuNode> childrenMenu;

        // methods
        public void printNode()
        {

            string s = "Name: " + menuName;
            if (parentNode != null)
            {
                s += "Parent: " + parentNode.menuName;
            }
            //string d = "Name: "+menuName +"Parent: "+ parentNode.menuName;

            for (int i = 0; i < childrenMenu.Count; i++)
            {
                s += "     Child " + i + ": " + childrenMenu[i].menuName;
            }
            Debug.Log(s);
        }
        public void SetActive(bool active)
        {
            this.menu.SetActive(active);
        }
        public void printName()
        {
            Debug.Log(menuName);
        }
        public void addChildMenuNode(MenuNode c)
        {
            childrenMenu.Add(c);
        }
    }



    void Awake()
    {

        MenuNode mainMenuNode = new MenuNode(MAIN_MENU_NODE_STRING, mainMenu, firstButton);

        MenuNode playMenuNode = new MenuNode(PLAY_MENU_NODE_STRING, mainMenuNode, playMenu, playFirstButton, playClosedButton);
        MenuNode optionsMenuNode = new MenuNode(OPTIONS_MENU_NODE_STRING, mainMenuNode, optionsMenu, optionsFirstButton, optionsClosedButton);

        MenuNode campaignMenuNode = new MenuNode(CAMPAIGN_MENU_NODE_STRING, playMenuNode, campaignMenu, campaignFirstButton, campaignClosedButton);
        MenuNode arenaMenuNode = new MenuNode(ARENA_MENU_NODE_STRING, playMenuNode, arenaMenu, arenaFirstButton, arenaClosedButton);



        mainMenuNode.addChildMenuNode(playMenuNode);
        mainMenuNode.addChildMenuNode(optionsMenuNode);

        playMenuNode.addChildMenuNode(campaignMenuNode);
        playMenuNode.addChildMenuNode(arenaMenuNode);

        // mainMenuNode.printNode();

        // playMenuNode.printNode();
        // optionsMenuNode.printNode();
        // campaignMenuNode.printNode();
        // arenaMenuNode.printNode();


        activeMenuNode = mainMenuNode;
        mainMenuNode.SetActive(false);
        playMenuNode.SetActive(false);
        optionsMenuNode.SetActive(false);
        campaignMenuNode.SetActive(false);
        arenaMenuNode.SetActive(false);

    }


    void Start()
    {

        // set default selected object

        activeMenuNode.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(activeMenuNode.firstSelectedButton);
        // mainMenu.SetActive(true);
        //EventSystem.current.SetSelectedGameObject(firstButton);
    }
    void Update()
    {
        

        if (activeMenuNode.menuName != MAIN_MENU_NODE_STRING)
        {

            if (Gamepad.current != null)
            {
                if ((Gamepad.current.bButton.wasPressedThisFrame) || (Keyboard.current.backspaceKey.wasPressedThisFrame))
                {

                    GoBack();
                }
            }
            else
            {
                if (Keyboard.current.backspaceKey.wasPressedThisFrame)
                {
                    GoBack();
                }

            }
        }
        // if (optionsMenu.activeSelf == true)
        // {
        //     CloseAndGoBack(optionsMenu, mainMenu, optionsClosedButton);
        //     //CloseOptionsMenu();
        // }
        // else if (playMenu.activeSelf == true)
        // {
        //     CloseAndGoBack(playMenu, mainMenu, playClosedButton);
        //     //ClosePlayMenu();
        // }
        // else if (arenaMenu.activeSelf == true)
        // {
        //     CloseAndGoBack(arenaMenu, playMenu, arenaClosedButton);
        //     //CloseArenaMenu();
        // }
        // else if (campaignMenu.activeSelf == true)
        // {
        //     CloseAndGoBack(campaignMenu, playMenu, campaignClosedButton);
        //     //CloseCampaignMenu(); 
        // }
    }



    public void StartGameSetUp()
    {
        SceneManager.LoadScene("PlayerSetup");
    }

    public void setGameTypeText(int x)
    {
        gameTypeSelected += x; // increase or decrease

        if (gameTypeSelected >= GameTypes.end)
        {
            gameTypeSelected -= GameTypes.end - 1;
        }
        else if (gameTypeSelected <= GameTypes.start)
        {

            gameTypeSelected = gameTypeSelected + (int)GameTypes.end - 1;
        }


        gameTypeText.GetComponent<TextMeshProUGUI>().text = gameTypeSelected.ToString();
    }

    public void setLevelSelectText(int x)
    {
        levelSelected += x;
        if (levelSelected >= Levels.end)
        {
            levelSelected -= Levels.end - 1;
        }
        else if (levelSelected <= Levels.start)
        {
            levelSelected = levelSelected + (int)Levels.end - 1;
        }
        levelSelectedText.GetComponent<TextMeshProUGUI>().text = levelSelected.ToString();
    }

    public void GoButtonOnClick(int x)
    {
        if (x == 0)
        {
            //arena
            GameObject g = new GameObject();
            ArenaGameDetails a = g.AddComponent<ArenaGameDetails>() as ArenaGameDetails;

            //Removed the type cast. 
            //a.gameType = (ArenaGameDetails.GameTypes) gameTypeSelected;
            //a.mapName = (ArenaGameDetails.Maps) mapSelected; 
            a.gameType = gameTypeSelected;
            a.mapName = mapSelected;
            g.tag = "ArenaGameDetailsObject";
            g.name = "GameDetails";
            DontDestroyOnLoadManager.DontDestroyOnLoad(g);

            // load the next scene
            SceneManager.LoadScene("PlayerSetup");

        }
        else
        {
            //campaign
            GameObject g = new GameObject();
            //ArenaGameDetails a = g.AddComponent<ArenaGameDetails>() as ArenaGameDetails;
            CampaignGameDetails a = g.AddComponent<CampaignGameDetails>() as CampaignGameDetails;


            a.level = levelSelected;
            //a.setEnemyCount(); 
            //Debug.Log(a.level);
            g.tag = "CampaignGameDetailsObject";
            g.name = "GameDetails";
            DontDestroyOnLoadManager.DontDestroyOnLoad(g);

            // load the next scene
            SceneManager.LoadScene("PlayerSetup");
        }

    }
    public void setMapText(int x)
    {
        mapSelected += x;
        if (mapSelected >= Maps.end)
        {
            mapSelected -= Maps.end - 1;
        }
        else if (mapSelected <= Maps.start)
        {

            mapSelected = mapSelected + (int)Maps.end - 1;
        }
        mapSelectText.GetComponent<TextMeshProUGUI>().text = mapSelected.ToString();
    }
    /*
    Main Menu Button Methods
    */
    public void OpenOptionsMenu()
    {
        OpenMenu(optionsMenu, mainMenu, optionsFirstButton);
        // mainMenu.SetActive(false);
        // optionsMenu.SetActive(true);
        // // deselect selected object from previous menu
        // EventSystem.current.SetSelectedGameObject(null);
        // // set default selected object
        // EventSystem.current.SetSelectedGameObject(optionsFirstButton);
    }
    public void OpenPlayMenu()
    {
        Debug.Log("Openning Play");
        OpenMenu(playMenu, mainMenu, playFirstButton);
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }


    /*
    Play Menu Button Methods
    */


    /*
    OptionsMenuButton Methods
    */
    public void setFullScreen()
    {
        // if (!fullscreenEnabled)
        // {
        //     FullscreenSelectText.GetComponent<TextMeshProUGUI>().text = "On";
        //     fullscreenEnabled = true;
        // }
        // else
        // {
        //     FullscreenSelectText.GetComponent<TextMeshProUGUI>().text = "Off";
        //     fullscreenEnabled = false;
        // }
    }

    public void CloseOptionsMenu()
    {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
        // deselect selected object from previous menu
        EventSystem.current.SetSelectedGameObject(null);
        // set default selected object
        EventSystem.current.SetSelectedGameObject(optionsClosedButton);
    }

    /*
    Helper Methods
    */
    public void OpenMenu(GameObject openThisMenu, GameObject closeThisMenu, GameObject selectThisButton)
    {
        closeThisMenu.SetActive(false);
        openThisMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        // set default selected object
        EventSystem.current.SetSelectedGameObject(selectThisButton);

    }
    //called from this script
    public void CloseAndGoBack(GameObject closeThisMenu, GameObject openThisMenu, GameObject selectThisButton)
    {
        closeThisMenu.SetActive(false);
        openThisMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selectThisButton);

    }


    public void GoBack()
    {
        // 
        activeMenuNode.SetActive(false);
        activeMenuNode.parentNode.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(activeMenuNode.goBackSelectedButton); // get my go back button
                                                                                        // change the active menu
        activeMenuNode = activeMenuNode.parentNode;
    }

    public void GoForward(int ChildNum)
    {
        activeMenuNode.SetActive(false);
        activeMenuNode = activeMenuNode.childrenMenu[ChildNum];
        activeMenuNode.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(activeMenuNode.firstSelectedButton);

        if (activeMenuNode.menuName == ARENA_MENU_NODE_STRING)
        {
            mapSelectText.GetComponent<TextMeshProUGUI>().text = mapSelected.ToString();
            gameTypeText.GetComponent<TextMeshProUGUI>().text = gameTypeSelected.ToString();
        }
        else if (activeMenuNode.menuName == CAMPAIGN_MENU_NODE_STRING)
        {
            levelSelectedText.GetComponent<TextMeshProUGUI>().text = levelSelected.ToString();
        }
    }
    public enum GameTypes
    {
        start,
        freeForAll,
        spaceMarbles,
        end
    }
    public enum Maps
    {
        start,
        arena1,
        Fissure,
        Colosseum,
        end
    }

    public enum Levels
    {
        start,
        tutorial,
        level1,
        level2,
        end
    }

}
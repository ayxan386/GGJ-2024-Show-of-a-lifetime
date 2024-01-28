using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuNavigator : MonoBehaviour
{
    [Header("Panels")]
    public GameObject MainMenuPanel;
    public GameObject SettingsPanel;
    public GameObject CreditsPanel;
    public GameObject NameInputPanel;
    public GameObject LeaderBoardPanel;

    [Header("Buttons")]
    public GameObject menuFirstSelection, SettingsFirstSelection, CreditsfirstSelection,NamePanelFirsSelection,LeaderBoardPanelFirsSelection;

    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(menuFirstSelection);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void OpenSettings()
    {
        MainMenuPanel.SetActive(false);
        SettingsPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(SettingsFirstSelection);
    }

    public void CloseSettings()
    {
        MainMenuPanel.SetActive(true);
        SettingsPanel.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(menuFirstSelection);
    }
    public void OpenCredits()
    {
        MainMenuPanel.SetActive(false);
        CreditsPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(CreditsfirstSelection);
    }

    public void CloseCredits( )
    {
        MainMenuPanel.SetActive(true);
        CreditsPanel.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(menuFirstSelection);
    }

    public void OpenNameInputPanel()
    {
        NameInputPanel.SetActive(true );

        LeaderBoardPanel.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(NamePanelFirsSelection);
    } 
    public void OpenLeaderBoardPanel()
    {
        LeaderBoardPanel.SetActive(true );
        NameInputPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(LeaderBoardPanelFirsSelection);
    }



    public void QuitGame()
    {
        Application.Quit();
    }
}

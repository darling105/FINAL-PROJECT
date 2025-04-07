using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class TitleScreenManager : MonoBehaviour
{
    public static TitleScreenManager instance;
    [Header("Menus")]
    [SerializeField] GameObject titleScreenMainMenu;
    [SerializeField] GameObject titleScreenLoadGameMenu;

    [Header("Button")]
    [SerializeField] Button loadMenuReturnButton;
    [SerializeField] Button mainMenuNewGameButton;
    [SerializeField] Button mainMenuLoadGameButton;
    [SerializeField] Button deletaCharacterPopUpConfirmButton;

    [Header("Pop Ups")]
    [SerializeField] GameObject noCharacterSlotsPopUp;
    [SerializeField] Button noCharacterSlotsOkayButton;
    [SerializeField] GameObject deleteCharacterSlotPopUp;



    [Header("Character Slots")]
    public CharacterSlot currentSelectedSlot = CharacterSlot.NO_SLOT;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        titleScreenMainMenu.SetActive(true);
    }

    public void StartNewGame()
    {
        WorldSaveGameManager.instance.AttempToCreateNewGame();
        //SceneManager.LoadScene("GameScene");
    }

    public void OpenLoadGameMenu()
    {
        titleScreenMainMenu.SetActive(false);

        titleScreenLoadGameMenu.SetActive(true);

        loadMenuReturnButton.Select();
    }

    public void CloseLoadGameMenu()
    {
        titleScreenMainMenu.SetActive(true);

        titleScreenLoadGameMenu.SetActive(false);

        mainMenuLoadGameButton.Select();
    }

    public void DisplayNoFreeCharacterSlotsPopUp()
    {
        noCharacterSlotsPopUp.SetActive(true);
        noCharacterSlotsOkayButton.Select();
    }

    public void CloseNoFreeCharacterSlotsPopUp()
    {
        noCharacterSlotsPopUp.SetActive(false);
        mainMenuNewGameButton.Select();
    }

    public void SelectCharacterSlot(CharacterSlot characterSlot)
    {
        currentSelectedSlot = characterSlot;
    }

    public void SelectNoSlot()
    {
        currentSelectedSlot = CharacterSlot.NO_SLOT;
    }

    public void AttempToDeleteCharacterSlot()
    {
        if (currentSelectedSlot != CharacterSlot.NO_SLOT)
        {
            deleteCharacterSlotPopUp.SetActive(true);
            deletaCharacterPopUpConfirmButton.Select();
        }
    }

    public void DeleteCharacterSlot()
    {
        deleteCharacterSlotPopUp.SetActive(false);
        WorldSaveGameManager.instance.DeleteGame(currentSelectedSlot);
        titleScreenLoadGameMenu.SetActive(false);
        titleScreenLoadGameMenu.SetActive(true);
        loadMenuReturnButton.Select();
    }

    public void CloseDeleteCharactePopUp()
    {
        deleteCharacterSlotPopUp.SetActive(false);
        loadMenuReturnButton.Select();
    }

}

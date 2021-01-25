using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAnimation : MonoBehaviour
{
    MainMenu mainMenu;
    public GameObject saveFiles, fileOptiones, optiones, delete;


    void Start()
    {
        mainMenu = GetComponent<MainMenu>();
    }

    
    void Update()
    {
        
    } 

    public void OpenFile(int i)
    {
        fileOptiones.GetComponent<MenuInterface>().SetOpening();
    }

    public void OpenOptiones()
    {
        optiones.GetComponent<MenuInterface>().SetOpening();
    }

    public void OpenDelete()
    {
        delete.GetComponent<MenuInterface>().SetOpening();
    }

    public void StartGame()
    {

    }

    public void CreateGame()
    {

    }

    public void CloseSaveFile()
    {
        saveFiles.GetComponent<MenuInterface>().SetClosing();
    }

    public void CloseDelete()
    {
        delete.GetComponent<MenuInterface>().SetClosing();
    }

    public void CloseOptiones()
    {
        optiones.GetComponent<MenuInterface>().SetClosing();
    }

    public void CloseActiones()
    {
        fileOptiones.GetComponent<MenuInterface>().SetClosing();
    }

    public void CloseCreateGame()
    {

    }     

    public void Signal(CurrentMenu nextMenu)
    {
        switch (nextMenu)
        {
            case CurrentMenu.saveFile:
                mainMenu.SetSelectFile();
                break;
            case CurrentMenu.action:
                mainMenu.SetFileAction();
                break;
            case CurrentMenu.optiones:
                mainMenu.SetOptiones();
                break;
            case CurrentMenu.delete:
                mainMenu.SetSelectDelete();
                break;
        }
    }

}

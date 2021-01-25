using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    MainMenuAnimation a;
    MenuTextEdit MTE;
    Settings_save currentSaveSettings;
    public CurrentMenu cMenu = CurrentMenu.saveFile;
    public int saveFileInt;
    bool canPushButtons=true, noSafe=false;

    //Optiones
    public float WPM_steps;
    public float WPM;
    public bool automode;


    void Start()
    {
        a = GetComponent<MainMenuAnimation>();
        MTE = GetComponent<MenuTextEdit>();
    }

    void Update()
    {
        if (!canPushButtons) return;
        switch (cMenu)
        {
            case CurrentMenu.saveFile:
                SelectFile();
                break;
            case CurrentMenu.action:
                SelectFileAction();
                break;
            case CurrentMenu.optiones:
                SelectOptions();
                break;
            case CurrentMenu.delete:
                SelectDelete();
                break;
        }
    }

    public void SelectFile()
    {
        if (Input.GetKeyDown("1"))
        {
            saveFileInt = 0;
            GetSelectFileAction();
        }
        if (Input.GetKeyDown("2"))
        {
            saveFileInt = 1;
            GetSelectFileAction();
        }
        if (Input.GetKeyDown("3"))
        {
            saveFileInt = 2;
            GetSelectFileAction();
        }

        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape))
        {
            
        }
    }

    public void SelectOptions()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(WPM >10)
            {
                WPM -= WPM_steps;
                MTE.SetWPM(WPM);
            }
            

        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (WPM < 100)
            {
                WPM += WPM_steps;
                MTE.SetWPM(WPM);
            }
                
        }
        if (Input.GetKeyDown("1"))
        {

        }
        if (Input.GetKeyDown("2"))
        {
            automode = !automode;
            MTE.SetAutoMode(automode);
        }
        if (Input.GetKeyDown("3"))
        {

        }

        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnSelectOptions();
        }
    }

    public void SelectFileAction()
    {
        if (Input.GetKeyDown("1"))
        {
            if(noSafe)
            {
                CreateGame();
            }
            else
            {
                StartGame();
            }

        }
        if (Input.GetKeyDown("2")&& !noSafe)
        {
            GetSelectOptions();
        }
        if (Input.GetKeyDown("3") && !noSafe)
        {
            GetSelectDelete();
        }

        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnSelectFileAction();
        }
    }

    public void SelectDelete()
    {
        if (Input.GetKeyDown("1"))
        {
            SaveSystem.DeleteCurrentSaveFile(saveFileInt);
            noSafe = true;
            MTE.SetActionUI(false);
            ReturnSelectDelete();
        }
        if (Input.GetKeyDown("2"))
        {
            ReturnSelectDelete();
        }

        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnSelectDelete();
        }
    }

    public void ReturnSelectOptions()
    {
        SaveSystem.OverrideSettings(currentSaveSettings, "/checkpoint_SettingsSave.test" + saveFileInt.ToString(), WPM, automode);
        canPushButtons = false;
        currentSaveSettings = SaveSystem.LordSettings("/checkpoint_SettingsSave.test" + saveFileInt);
        a.CloseOptiones();
    }

    public void ReturnSelectFileAction()
    {
        canPushButtons = false;
        a.CloseActiones();
    }

    public void ReturnSelectDelete()
    {
        canPushButtons = false;
        a.CloseDelete();
    }

    public void ReturnSelectFile()
    {
        canPushButtons = false;
        a.CloseSaveFile();
    }

    public void GetSelectOptions()
    {
        canPushButtons = false;
        a.OpenOptiones();
        MTE.SetWPM(currentSaveSettings.WPM);
        MTE.SetAutoMode(currentSaveSettings.autoMode);
        WPM = currentSaveSettings.WPM;
        automode = currentSaveSettings.autoMode;
    }

    public void SetOptiones()
    {
        canPushButtons = true;
        cMenu = CurrentMenu.optiones;
    }

    public void GetSelectFileAction()
    {
        MTE.SetFileName("Savefile " + (saveFileInt+1).ToString());
        canPushButtons = false;
        a.OpenFile(saveFileInt);
        currentSaveSettings = SaveSystem.LordSettings("/checkpoint_SettingsSave.test" + saveFileInt);
        if(currentSaveSettings ==null)
        {
            noSafe = true;
            MTE.SetActionUI(false);
        }
        else
        {
            noSafe = false;
            MTE.SetActionUI(true);
        }
    }

    public void SetFileAction()
    {
        canPushButtons = true;
        cMenu = CurrentMenu.action;
    }

    public void GetSelectDelete()
    {
        MTE.SetDeleteText("Savefile " + (saveFileInt+1).ToString());
        canPushButtons = false;
        a.OpenDelete();
    }

    public void SetSelectDelete()
    {
        canPushButtons = true;
        cMenu = CurrentMenu.delete;
    }

    //public void GetSelectFile()
    //{
    //    canPushButtons = false;
    //}

    public void SetSelectFile()
    {
        canPushButtons = true;
        cMenu = CurrentMenu.saveFile;
    }

    public void StartGame()
    {
        FindObjectOfType<SettingsSave>().saveFile = saveFileInt;
        SceneManager.LoadScene(currentSaveSettings.scene, LoadSceneMode.Single);
        
    }

    public void CreateGame()
    {
        FindObjectOfType<SettingsSave>().saveFile = saveFileInt;
        SceneManager.LoadScene("WPMTest");
    }

    public void SaveSettings()
    {

    }
}

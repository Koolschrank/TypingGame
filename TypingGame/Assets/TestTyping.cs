using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TestTyping : MonoBehaviour
{
    [TextArea]
    
    public string TestWords;
    public float Time;
    public int caracterCount;
    public Text WPM_text, sliderText;
    public Slider dificulty;
    NewGameSettings state = NewGameSettings.start_test;
    public GameObject test_typing, obtiones;
    public float WPM_steps;
    float WPM;

    public void Start()
    {
        
        WPM = 30;
        dificulty.value = (WPM - 10) / 90;
        sliderText.text = WPM.ToString() + " WPM";
        SetTest();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("1") && (state == NewGameSettings.start_test || state == NewGameSettings.set_optiones))
        {
            SetTest();

        }
        
        if(state == NewGameSettings.set_optiones)
        {
            Obtiones_changer();
            if (Input.GetKeyDown("2"))
            {
                var currentSaveSettings = SaveSystem.LordSettings("/checkpoint_SettingsSave.test" + FindObjectOfType<SettingsSave>().saveFile);
                SaveSystem.OverrideSettings(FindObjectOfType<SettingsSave>().startString.ToString(), "/checkpoint_SettingsSave.test" + FindObjectOfType<SettingsSave>().saveFile.ToString(), WPM, false,false);
                SceneManager.LoadScene(FindObjectOfType<SettingsSave>().startString);
            }
        }
    }

    public void SetTest()
    {
        test_typing.SetActive(true);
        obtiones.SetActive(false);
        state = NewGameSettings.in_test;
        FindObjectOfType<BattleTyper>().Start_typ_test(TestWords, 1000, Time);
    }

    public void ReturnScore(int characterCount)
    {
        test_typing.SetActive(false);
        obtiones.SetActive(true);
        caracterCount = characterCount;
        Debug.Log(characterCount);
        Debug.Log((characterCount/5*60/Time).ToString() + "WPM");
        WPM_text.text = ("your score: "+characterCount / 5 * 60 / Time).ToString() + "WPM";
        state = NewGameSettings.set_optiones;

        WPM = (characterCount / 5 * 60 / Time);
        WPM += 5- WPM % 5; 
        dificulty.value = (WPM - 10) / 90;
        sliderText.text = WPM.ToString() + " WPM";
    }

    public void Obtiones_changer()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (WPM > 10)
            {
                WPM -= WPM_steps;
                dificulty.value = (WPM - 10) / 90;
                sliderText.text = WPM.ToString() + " WPM";
            }


        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (WPM < 100)
            {
                WPM += WPM_steps;
                dificulty.value = (WPM-10) / 90;
                sliderText.text = WPM.ToString() + " WPM";
            }

        }
    }
}

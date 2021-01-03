using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleTyper : MonoBehaviour
{
    PlayerStats player;
    BattleSystem system;
    public InputField input, text_field;
    public Text input_text, text_field_text, correct_text;
    public Slider Time_slider;
    public Color wrong, normal;
    public bool noTyping;
    bool activated, typo_made;
    bool fromPlayer = true;
    float start_time, end_time;
    int _position;
    bool timer_has_started = false;
    [TextArea]
    public string currentText="";

    private void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        input.onValueChange.AddListener(delegate { ValueChangeCheck(input.text); });
        system = FindObjectOfType<BattleSystem>();
    }


    public void Start_typing(string _words, int _count, float _time_per_character, bool _player )
    {
        //if(noTyping)
        //{
        //    EndTyping(0.1f, false);
        //}
        fromPlayer = _player;

        var _text = Set_Random_Text(_words, _count, fromPlayer, textGimmick.none);
        currentText = _text;
        Debug.Log(_text);
        StartTimer(_text,_time_per_character, _player);
        input.readOnly = false;
        
        if (input.isFocused == false)
        {
            input.ActivateInputField();
        }
        typo_made = false;
        _position = 0;
        activated = true;
        
    }

    public void Start_typing(string _words, int _count, float _time_per_character, bool _player, textGimmick gimmick)
    {
        //if(noTyping)
        //{
        //    EndTyping(0.1f, false);
        //}


        fromPlayer = _player;

        var _text = Set_Random_Text(_words, _count, fromPlayer, gimmick);
        Debug.Log(_text);
        currentText = _text;
        StartTimer(_text, _time_per_character,player);
        input.readOnly = false;

        if (input.isFocused == false)
        {
            input.ActivateInputField();
        }
        typo_made = false;
        _position = 0;
        activated = true;
    }

    public void StartTimer(string _text,float _time_per_character, bool _player)
    {
        Settings settings = FindObjectOfType<Settings>();
        start_time = Time.timeSinceLevelLoad;
        if(_player)
        {
            var time = _text.Length* settings.time_per_character* system.Get_current_speed()* _time_per_character * settings.playerTypeTime;
            end_time = time;
        }
        else
        {
            var time = _text.Length * _time_per_character * system.Get_current_speed() * settings.time_per_character * settings.enemyTypeTime;
            end_time = time;
        }
        
        
    }

    private void Update()
    {
        UpdateTimer();

        if (activated)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                CheckWithEnter();
                
            }
            input.ActivateInputField();
        }
        else
        {
            input.DeactivateInputField();
        }

    }

    public void CheckWithEnter()
    {
        input.text.Remove(input.text.Length - 1);
        input.text += " ";
        ValueChangeCheck(input.text);
    }


    public void UpdateTimer()
    {
        if (activated)
        {
            Time_slider.value = 1 - ((Time.timeSinceLevelLoad - start_time) / end_time);
            if (Time.timeSinceLevelLoad - start_time >= end_time)
            {
                EndTyping(0, true);
            }
        }
        else
        {
            Time_slider.value = 1;
        }
    }

    public string Set_Random_Text(string _words,int _count, bool fromPlayer, textGimmick gimmick)
    {
        
        if (!fromPlayer )
        {
            _count = _count / 2;
        }
        List<string> text_array = GetWords_as_list(_words, gimmick);
        string _Text = TextChanger.Set_Word_Count(_count, text_array);
        
        Set_text_field(_Text);

        
        return _Text;
    }


    public List<string> GetWords_as_list(string _words, textGimmick gimmick)
    {
        List<int> all_spaces = TextChanger.Get_All_spaces(_words);
        List<string> text_array = TextChanger.string_into_string_list(all_spaces, _words);

        if(gimmick == textGimmick.firstLetterUppercase)
        {
            List<string> text_array_uppercase = new List<string>();
            foreach (string _text in text_array )
            {
                if(_text.Length >1)
                {
                    char firstLetter = _text[0];
                    string FirstLetter = firstLetter.ToString().ToUpper();

                    text_array_uppercase.Add(FirstLetter + _text.Substring(1));
                }
                else
                {
                    char firstLetter = _text[0];
                    string FirstLetter = firstLetter.ToString().ToUpper();

                    text_array_uppercase.Add(FirstLetter);
                }
                
            }
            return text_array_uppercase;
        }
        else if (gimmick == textGimmick.swap_uppercase_lowercase)
        {
            List<string> text_array_uppercase = new List<string>();
            bool uppercase =false;
            foreach (string _text in text_array)
            {
                if (uppercase)
                {
                    text_array_uppercase.Add(_text.ToUpper());
                    uppercase = false;
                }
                else
                {
                    text_array_uppercase.Add(_text.ToLower());
                    uppercase = true;
                }

            }
            return text_array_uppercase;
        }
        return text_array;
    }

    public void Set_text_field(string _text)
    {
        text_field.text = _text;
    }

    public void ValueChangeCheck(string _text)
    {
        if (_text == TextChanger.Get_Characters(currentText, _position,_text.Length))
        {
            
            input_text.color = normal;
            if(_text.Contains(" "))
            {
                CompleteWord(_text);
            }
        }
        else
        {
            typo_made = true;
            input_text.color = wrong;
        }
        Set_text_field(currentText);
    }

    private void CompleteWord(string _text)
    {
        _position += _text.Length;
        correct_text.text += _text;
        input.text = "";
        int next_line_position = TextChanger.Check_next_line(text_field_text);
        if (next_line_position > 0 && correct_text.text.Length >= next_line_position - 1)
        {
            _position -= correct_text.text.Length;
            currentText = TextChanger.DeleteLine(currentText, correct_text.text.Length);
            correct_text.text = "";
            Set_text_field(currentText);
        }

        if (_position >= text_field.text.Length-1)
        {
            TypingEnd();
        }
    }

    public void TypingEnd()
    {
        EndTyping(1 - ((Time.timeSinceLevelLoad - start_time) / end_time), typo_made);
    }

    public int Check_next_line(Text _field)
    {
        if (_field.cachedTextGenerator.lines.Count>1)
        {
            return _field.cachedTextGenerator.lines[1].startCharIdx;
        }
        return 0;
    }

    public void EndTyping(float score, bool typos)
    {
        if (fromPlayer)
        {
            
            player.Use_Ability(score, typos);
        }
        else
        {
            system.GetEnemiy().Play_Ability(score, typos);
        }
        system.Next_turn();
        text_field.text = "";
        correct_text.text = "";
        currentText = "";
        activated = false;
        input.readOnly = true;
    }

}

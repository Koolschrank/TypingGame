using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class TextChanger 
{
    public static List<int> Get_All_spaces(string All_Words)
    {
        int currentPosition = 0;
        List<int> Space_List = new List<int>();
        Space_List.Add(0);
        for (int i = 0; i < 1000; i++)
        {
            var _lenght = All_Words.Length;

            int new_space = All_Words.IndexOf(" ", currentPosition, _lenght - currentPosition);
            if (new_space == -1)
            {
                break;
            }
            Space_List.Add(new_space);
            currentPosition = new_space + 1;
        }

        return Space_List;
    }


    public static List<string> string_into_string_list(List<int> _spaces, string All_Words)
    {
        List<string> string_list = new List<string>();
        string_list.Add(Return_sub_string(_spaces[0], _spaces[1] - _spaces[0], All_Words));
        for (int i = 1; i < _spaces.Count - 1; i++)
        {
            string_list.Add(Return_sub_string(_spaces[i] + 1, _spaces[i + 1] - _spaces[i] - 1, All_Words));
        }
        return string_list;
    }

    public static string Return_sub_string(int start, int lenght,string _string)
    {
        return _string.Substring(start, lenght);
    }

    public static string Set_Word_Count(int count, List<string> All_Words)
    {
        string _text = "";
        for (int i = 0; i < count; i++)
        {
            _text += All_Words[Random.Range(0, All_Words.Count)] + " ";
        }
        return _text;
    }

    public static string Get_Characters(string _text,int _position, int _character_count)
    {
        return _text.Substring(_position, _character_count);
    }

    public static int Check_next_line(Text _field)
    {
        if (_field.cachedTextGenerator.lines.Count > 1)
        {
            return _field.cachedTextGenerator.lines[1].startCharIdx;
        }
        return 0;
    }

    public static string DeleteLine(string _text, int delet_count)
    {
        return _text.Remove(0, delet_count);
    }

}

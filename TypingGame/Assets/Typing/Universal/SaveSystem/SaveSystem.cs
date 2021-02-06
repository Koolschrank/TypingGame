using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;

public class SaveSystem : MonoBehaviour
{
    public static void SavePlayer(PlayerStats player, string savePostion)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + savePostion;
        FileStream stream = new FileStream(path, FileMode.Create);

        Player_save data = new Player_save(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static Player_save loadPlayerFile(string savePostion)
    {
        string path = Application.persistentDataPath + savePostion;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            Player_save data = formatter.Deserialize(stream) as Player_save;
            Debug.Log(path);
            stream.Close();
            return data;
        }
        else
        {
            Debug.Log("what?");
            return null;
        }
    }

    public static void SaveBackpack(Backpack backpack,string savePostion)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + savePostion;
        FileStream stream = new FileStream(path, FileMode.Create);

        Backpack_Save data = new Backpack_Save(backpack);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static Backpack_Save loadBackpackFile(string savePostion)
    {
        string path = Application.persistentDataPath + savePostion;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            Backpack_Save data = formatter.Deserialize(stream) as Backpack_Save;
            stream.Close();
            return data;
        }
        else
        {
            Debug.Log("what?");
            return null;
        }
    }

    public static void SavePosition(SaveableObject[] _objects,bool at_checkPoint, string savePostion, Position_Save oldSave)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + savePostion;
        FileStream stream = new FileStream(path, FileMode.Create);
        Position_Save data = new Position_Save(_objects, at_checkPoint, oldSave);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static Position_Save loadPostion( string savePostion)
    {
        string path = Application.persistentDataPath + savePostion;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            if(stream.Length ==0)
            {
                Debug.Log("worked i hope");
                stream.Close();
                return null;
            }
            Position_Save data = formatter.Deserialize(stream) as Position_Save;
            stream.Close();
            return data;
        }
        else
        {
            Debug.Log("what?");
            return null;
        }
    }

    public static void SaveSettings(Settings settings, string savePostion)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + savePostion;
        FileStream stream = new FileStream(path, FileMode.Create);

        Settings_save data = new Settings_save(settings);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void OverrideSettings(Settings_save oldSettings, string savePostion, float WPM, bool autoMode, bool doubleWords)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + savePostion;
        FileStream stream = new FileStream(path, FileMode.Create);

        Settings_save data = new Settings_save(oldSettings, WPM, autoMode, doubleWords);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void OverrideSettings(string newString, string savePostion, float WPM, bool autoMode, bool doubleWords)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + savePostion;
        FileStream stream = new FileStream(path, FileMode.Create);

        Settings_save data = new Settings_save(newString, WPM, autoMode, doubleWords);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static Settings_save LordSettings(string savePostion)
    {
        string path = Application.persistentDataPath + savePostion;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            Settings_save data = formatter.Deserialize(stream) as Settings_save;
            stream.Close();
            return data;
        }
        else
        {
            Debug.Log("what?");
            return null;
        }
    }

    public static void Save_Game(SaveableObject[] _objects, PlayerStats player,Backpack backpack, Settings settings, int SaveFile)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        if(_objects != null)
        {
            SavePosition(_objects, true, "/checkpoint_SavePosition.test" + SaveFile.ToString(), loadPostion("/checkpoint_SavePosition.test" + SaveFile.ToString()));
        }

        if(player != null)
        {
            SavePlayer(player, "/checkpoint_SavePlayer.test" + SaveFile.ToString());
        }

        if(backpack != null)
        {
            SaveBackpack(backpack, "/checkpoint_BackpackSave.test" + SaveFile.ToString());
        }

        if (settings != null)
        {
            SaveSettings(settings, "/checkpoint_SettingsSave.test" + SaveFile.ToString());
        }
    }

    public static SaveFile GetSaveFile(int SaveFile)
    {
        Position_Save positons = loadPostion("/checkpoint_SavePosition.test" + SaveFile.ToString());
        Player_save player = loadPlayerFile("/checkpoint_SavePlayer.test" + SaveFile.ToString());
        Backpack_Save backpack = loadBackpackFile("/checkpoint_BackpackSave.test" + SaveFile.ToString());
        Settings_save settings = LordSettings("/checkpoint_SettingsSave.test" + SaveFile.ToString());
        SaveFile file = new SaveFile(positons, player, backpack, settings);
        return file;
    }
#if UNITY_EDITOR
    public static void DeleteAllSaveFills()
    {
        for(int i =0; i<3; i++)
        {
            UnityEditor.FileUtil.DeleteFileOrDirectory(Application.persistentDataPath +"/checkpoint_SavePosition.test" + i);
            UnityEditor.FileUtil.DeleteFileOrDirectory(Application.persistentDataPath+"/checkpoint_SavePlayer.test" + i);
            UnityEditor.FileUtil.DeleteFileOrDirectory(Application.persistentDataPath+"/checkpoint_BackpackSave.test" + i);
            UnityEditor.FileUtil.DeleteFileOrDirectory(Application.persistentDataPath+"/checkpoint_SettingsSave.test" + i);
            AssetDatabase.Refresh();
        }
    }

    public static void DeleteCurrentSaveFileInEditor(int i)
    {
        UnityEditor.FileUtil.DeleteFileOrDirectory(Application.persistentDataPath + "/checkpoint_SavePosition.test" + i);
        UnityEditor.FileUtil.DeleteFileOrDirectory(Application.persistentDataPath + "/checkpoint_SavePlayer.test" + i);
        UnityEditor.FileUtil.DeleteFileOrDirectory(Application.persistentDataPath + "/checkpoint_BackpackSave.test" + i);
        UnityEditor.FileUtil.DeleteFileOrDirectory(Application.persistentDataPath + "/checkpoint_SettingsSave.test" + i);
        AssetDatabase.Refresh();
    }
#endif
    public static void DeleteCurrentSaveFile(int i)
    {
        File.Delete(Application.persistentDataPath + "/checkpoint_SavePosition.test" + i);
        File.Delete(Application.persistentDataPath + "/checkpoint_SavePlayer.test" + i);
        File.Delete(Application.persistentDataPath + "/checkpoint_BackpackSave.test" + i);
        File.Delete(Application.persistentDataPath + "/checkpoint_SettingsSave.test" + i);
        
        //AssetDatabase.Refresh();
    }
}



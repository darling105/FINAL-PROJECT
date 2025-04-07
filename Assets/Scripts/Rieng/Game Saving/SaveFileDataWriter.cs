using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SaveFileDataWriter
{
    public string saveDataDirectoryPath = Application.persistentDataPath;
    public string saveFileName = "";

    public bool CheckToSeeIfFileExists()
    {
        if (File.Exists(Path.Combine(saveDataDirectoryPath,saveFileName)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DeleteSaveFile()
    {
        File.Delete(Path.Combine(saveDataDirectoryPath,saveFileName));
        Debug.Log("Save file deleted: " + Path.Combine(saveDataDirectoryPath,saveFileName));
    }

    public void CreateNewCharacterSaveFile(CharacterSaveData characterSaveData)
    {
        string savePath = Path.Combine(saveDataDirectoryPath,saveFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            Debug.Log("Directory created: " + savePath);

            //serialize the c# game data object into a json string
            string datatoStore = JsonUtility.ToJson(characterSaveData, true);

            //write the json file to our disk
            using (FileStream fileStream = new FileStream(savePath, FileMode.Create))
            {
                using (StreamWriter fileWriter = new StreamWriter(fileStream))
                {
                    fileWriter.Write(datatoStore);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error creating save file: " + savePath + "\n" + ex);
        }
    }


    public CharacterSaveData LoadSaveFile()
    {
        CharacterSaveData characterData = null;

        string loadPath = Path.Combine(saveDataDirectoryPath,saveFileName);

        if (File.Exists(loadPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream fileStream = new FileStream(loadPath, FileMode.Open))
                {
                    using (StreamReader fileReader = new StreamReader(fileStream))
                    {
                        dataToLoad = fileReader.ReadToEnd();
                    }
                }
                characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error loading save file: " + loadPath + "\n" + ex.Message);
            }
        }
        return characterData;

    }
}

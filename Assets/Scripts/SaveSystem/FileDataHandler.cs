using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string _dataDirectoryPath = "";
    private string _dataFileName = "";

    private readonly string _backupExtension = ".bak";

    public FileDataHandler(string dataDirectoryPath, string dataFileName)
    {
        _dataDirectoryPath = dataDirectoryPath;
        _dataFileName = dataFileName;
    }

    public GameData Load(string profileID, bool allowRestoreFromBackup = true)
    {
        // Base case: if the profileID is null, return right away
        if (profileID == null)
        {
            return null;
        }

        // Use Path.Combine to account for different OS's having different path separators.
        string fullPath = Path.Combine(_dataDirectoryPath, profileID, _dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                // Load the serialized data from the file.
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // Deserialize the data from Json back into the C# object.
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                // Since we're calling Load(..) recursively, we need to account for the case where
                // the rollback succeeds, but data is still failing to load for some other reason,
                // which without this check may cause an infinite recursion loop.
                if (allowRestoreFromBackup)
                {
                    Debug.LogWarning("Failed to load data file. Attempting to roll back.\n" + e);
                    bool rollbackSuccess = AttemptRollback(fullPath);
                    if (rollbackSuccess)
                    {
                        // Try to load again recursively
                        loadedData = Load(profileID, false);
                    }
                }
                // If we hit this else block, one possibility is that the backup file is also corrupt
                else
                {
                    Debug.LogError("Error occured when trying to load file at path: "
                        + fullPath + " and backup did not work.\n" + e);
                }
            }
        }
        return loadedData;
    }

    public void Save(GameData data, string profileID)
    {
        // Base case: if the profileID is null, return right away
        if (profileID == null)
        {
            return;
        }

        // Use Path.Combine to account for different OS's having different path separators.
        string fullPath = Path.Combine(_dataDirectoryPath, profileID, _dataFileName);
        string backupFilePath = fullPath + _backupExtension;
        try
        {
            // Create the directory the file will be written to if it doesn't already exist.
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // Serialize the C# game data object into Json.
            string dataToStore = JsonUtility.ToJson(data, true);

            // Write the serialized data to the file.
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }

            // Verify that the newly saved file can be loaded successfully
            GameData verifiedGameData = Load(profileID);
            // If the data can be verified, back it up
            if (verifiedGameData != null)
            {
                File.Copy(fullPath, backupFilePath, true);
            }
            // Otherwise, something went wrong and we should throw an exception
            else
            {
                throw new Exception("Save file could not be verified and backup " +
                    "could not be created");
            }
        }
        catch (Exception e)
        {
            Debug.LogError(
                "Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    public void Delete(string profileID)
    {
        // Base case: if the profileID is null, return right away
        if (profileID == null)
        {
            return;
        }

        string fullPath = Path.Combine(_dataDirectoryPath, profileID, _dataFileName);
        try
        {
            // Ensure the data file exists at this path befor deleting the directory
            if (File.Exists(fullPath))
            {
                // Delete the profile folder and everything within it
                Directory.Delete(Path.GetDirectoryName(fullPath), true);

                // Only delete the data file, not the whole directory
                //File.Delete(fullPath);
            }
            else
            {
                Debug.LogWarning("Tried to delete profile data, but data was not found " +
                    "at path: " + fullPath);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to delete profile data for profileID: " 
                + profileID + " at path: " + fullPath + "\n" + e);
        }
    }

    public Dictionary<string, GameData> LoadAllProfiles()
    {
        Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();

        // Loop over all directory names in the data directory path
        IEnumerable<DirectoryInfo> directoryInfos = 
            new DirectoryInfo(_dataDirectoryPath).EnumerateDirectories();

        foreach (DirectoryInfo directoryInfo in directoryInfos)
        {
            string profileID = directoryInfo.Name;

            // Defensive programming - check if the data file exists
            // If it doesn't, then this folder isn't a profile and should be shipped
            string fullPath = Path.Combine(_dataDirectoryPath, profileID, _dataFileName);
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning("Skipping directory when loading all profiles because " +
                    "it does not contain data:" + profileID);
                continue;
            }

            // Load the game data for this profile and put it in the dictionary
            GameData profileData = Load(profileID);
            // Defensive programming - ensure the profile data isn't null,
            // because if it is then something went wrong and we should let ourselves know
            if (profileData != null)
            {
                profileDictionary.Add(profileID, profileData);
            }
            else
            {
                Debug.LogError("Tried to load profile but something went wrong. ProfileID: "
                    + profileID);
            }
        }

        return profileDictionary;
    }

    public string GetMostRecentlyUpdatedProfileID()
    {
        string mostRecentProfileID = null;

        Dictionary<string, GameData> profilesGameData = LoadAllProfiles();
        foreach (KeyValuePair<string,GameData> pair in profilesGameData)
        {
            string profileID = pair.Key;
            GameData gameData = pair.Value;

            // Skip this entry if the gameData is null
            if (gameData == null)
            {
                continue;
            }

            // If this is the first data we've come across, it's the most recent so far
            if (mostRecentProfileID == null)
            {
                mostRecentProfileID = profileID;
            }
            // Otherwise, compare to see which date is the most recent
            else
            {
                DateTime mostRecentDateTime = 
                    DateTime.FromBinary(profilesGameData[mostRecentProfileID].LastUpdated);

                DateTime newDateTime = DateTime.FromBinary(gameData.LastUpdated);

                // The greatest DateTime value is the most recent
                if (newDateTime > mostRecentDateTime)
                {
                    mostRecentProfileID = profileID;
                }
            }
        }

        return mostRecentProfileID;
    }

    private bool AttemptRollback(string fullPath)
    {
        bool success = false;

        string backupFilePath = fullPath + _backupExtension;
        try
        {
            // If the file exists, attempt to roll back to it by overwriting the original file
            if (File.Exists(backupFilePath))
            {
                File.Copy(backupFilePath, fullPath, true);
                success = true;
                Debug.LogWarning("Had to roll back to backup file at :" + backupFilePath);
            }
            // Otherwise, we don't have a backup file yet, so there's nothing to roll back to
            else
            {
                throw new Exception("Tried to roll back, " +
                    "but no backup file exists to roll back to");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to roll back to backup file at: "
                + backupFilePath + "\n" + e);
        }

        return success;
    }
}
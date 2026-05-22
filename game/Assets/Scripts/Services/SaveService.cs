using System;
using System.IO;
using BastionUA.Core;
using UnityEngine;

namespace BastionUA.Services
{
    public sealed class SaveService
    {
        private readonly string _savePath;

        public SaveService()
        {
            _savePath = Path.Combine(Application.persistentDataPath, GameConstants.SaveFileName);
            Debug.Log($"[SaveService] Save path: {_savePath}");
        }

        public GameState LoadOrCreate()
        {
            try
            {
                if (!File.Exists(_savePath))
                {
                    Debug.Log("[SaveService] Save file not found, creating default state.");
                    return GameState.CreateDefault();
                }

                var json = File.ReadAllText(_savePath);
                var loaded = JsonUtility.FromJson<GameState>(json);

                if (loaded == null || loaded.Regions == null || loaded.Regions.Count == 0)
                {
                    Debug.LogWarning("[SaveService] Save content invalid, fallback to default state.");
                    return GameState.CreateDefault();
                }

                loaded.Normalize();
                Debug.Log("[SaveService] Save loaded successfully.");
                return loaded;
            }
            catch (Exception exception)
            {
                Debug.LogError($"[SaveService] Load failed: {exception}");
                return GameState.CreateDefault();
            }
        }

        public void Save(GameState state)
        {
            try
            {
                state.LastSavedUtc = DateTime.UtcNow;
                var json = JsonUtility.ToJson(state, true);
                File.WriteAllText(_savePath, json);
                Debug.Log("[SaveService] Save completed.");
            }
            catch (Exception exception)
            {
                Debug.LogError($"[SaveService] Save failed: {exception}");
            }
        }

        public void DeleteSave()
        {
            try
            {
                if (File.Exists(_savePath))
                {
                    File.Delete(_savePath);
                    Debug.Log("[SaveService] Save file deleted.");
                }
            }
            catch (Exception exception)
            {
                Debug.LogError($"[SaveService] Delete save failed: {exception}");
            }
        }
    }
}

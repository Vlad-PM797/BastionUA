using System;
using System.IO;
using BastionUA.Core;
using UnityEngine;

namespace BastionUA.Services
{
    [Serializable]
    public sealed class PlaytestMetricsSession
    {
        public string StartedAtUtc;
        public float DurationSeconds;
        public int BattleCount;
        public int EventCount;
        public int UpgradeCount;
        public int PrestigeCount;
        public int TotalBattlesAtEnd;
        public int PrestigeLevelAtEnd;
        public bool HasSeenOnboarding;
    }

    [Serializable]
    public sealed class PlaytestMetricsStore
    {
        public PlaytestMetricsSession[] Sessions = Array.Empty<PlaytestMetricsSession>();
    }

    public sealed class PlaytestMetricsService
    {
        private readonly string _metricsPath;
        private PlaytestMetricsSession _currentSession;
        private float _sessionStartTime;
        private bool _sessionEnded;

        public PlaytestMetricsService()
        {
            _metricsPath = Path.Combine(Application.persistentDataPath, GameConstants.PlaytestMetricsFileName);
            Debug.Log($"[PlaytestMetricsService] Metrics path: {_metricsPath}");
        }

        public void StartSession()
        {
            try
            {
                _sessionStartTime = Time.realtimeSinceStartup;
                _sessionEnded = false;
                _currentSession = new PlaytestMetricsSession
                {
                    StartedAtUtc = DateTime.UtcNow.ToString("o"),
                    DurationSeconds = 0f,
                    BattleCount = 0,
                    EventCount = 0,
                    UpgradeCount = 0,
                    PrestigeCount = 0,
                };
                Debug.Log("[PlaytestMetricsService] Session started.");
            }
            catch (Exception exception)
            {
                Debug.LogError($"[PlaytestMetricsService] StartSession failed: {exception}");
            }
        }

        public void RecordBattle()
        {
            if (_currentSession == null)
            {
                return;
            }

            _currentSession.BattleCount++;
        }

        public void RecordEvent()
        {
            if (_currentSession == null)
            {
                return;
            }

            _currentSession.EventCount++;
        }

        public void RecordUpgrade()
        {
            if (_currentSession == null)
            {
                return;
            }

            _currentSession.UpgradeCount++;
        }

        public void RecordPrestige()
        {
            if (_currentSession == null)
            {
                return;
            }

            _currentSession.PrestigeCount++;
        }

        public void EndSession(GameState gameState)
        {
            if (_currentSession == null || _sessionEnded)
            {
                return;
            }

            _sessionEnded = true;

            try
            {
                _currentSession.DurationSeconds = Mathf.Max(0f, Time.realtimeSinceStartup - _sessionStartTime);
                if (gameState != null)
                {
                    _currentSession.TotalBattlesAtEnd = gameState.TotalBattles;
                    _currentSession.PrestigeLevelAtEnd = gameState.PrestigeLevel;
                    _currentSession.HasSeenOnboarding = gameState.HasSeenOnboarding;
                }

                AppendSession(_currentSession);
                _currentSession = null;
                Debug.Log("[PlaytestMetricsService] Session saved.");
            }
            catch (Exception exception)
            {
                Debug.LogError($"[PlaytestMetricsService] EndSession failed: {exception}");
            }
        }

        private void AppendSession(PlaytestMetricsSession session)
        {
            var store = LoadStore();
            var sessions = store.Sessions ?? Array.Empty<PlaytestMetricsSession>();
            var updated = new PlaytestMetricsSession[sessions.Length + 1];
            Array.Copy(sessions, updated, sessions.Length);
            updated[sessions.Length] = session;

            if (updated.Length > GameConstants.PlaytestMetricsMaxSessions)
            {
                var trimmed = new PlaytestMetricsSession[GameConstants.PlaytestMetricsMaxSessions];
                Array.Copy(
                    updated,
                    updated.Length - GameConstants.PlaytestMetricsMaxSessions,
                    trimmed,
                    0,
                    GameConstants.PlaytestMetricsMaxSessions);
                updated = trimmed;
            }

            store.Sessions = updated;
            SaveStore(store);
        }

        private PlaytestMetricsStore LoadStore()
        {
            try
            {
                if (!File.Exists(_metricsPath))
                {
                    return new PlaytestMetricsStore();
                }

                var json = File.ReadAllText(_metricsPath);
                var store = JsonUtility.FromJson<PlaytestMetricsStore>(json);
                return store ?? new PlaytestMetricsStore();
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"[PlaytestMetricsService] LoadStore failed: {exception.Message}");
                return new PlaytestMetricsStore();
            }
        }

        private void SaveStore(PlaytestMetricsStore store)
        {
            try
            {
                var json = JsonUtility.ToJson(store, true);
                File.WriteAllText(_metricsPath, json);
            }
            catch (Exception exception)
            {
                Debug.LogError($"[PlaytestMetricsService] SaveStore failed: {exception}");
            }
        }
    }
}

using BastionUA.Core;
using UnityEngine;

namespace BastionUA.Services
{
    public static class SfxClipResolver
    {
        public static AudioClip LoadOrCreateTone(
            string resourceName,
            float frequencyHz,
            float durationSeconds,
            float volume)
        {
            try
            {
                var resourcePath = $"{GameConstants.SfxResourcesFolder}/{resourceName}";
                var clip = Resources.Load<AudioClip>(resourcePath);
                if (clip != null)
                {
                    Debug.Log($"[SfxClipResolver] Loaded clip: {resourcePath}");
                    return clip;
                }
            }
            catch (System.Exception exception)
            {
                Debug.LogWarning($"[SfxClipResolver] Resource load failed for {resourceName}: {exception.Message}");
            }

            return ProceduralToneGenerator.CreateTone(frequencyHz, durationSeconds, volume);
        }
    }
}

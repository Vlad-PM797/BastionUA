using BastionUA.Core;
using UnityEngine;

namespace BastionUA.Services
{
    public static class ProceduralToneGenerator
    {
        public static AudioClip CreateTone(float frequencyHz, float durationSeconds, float volume)
        {
            var sampleRate = GameConstants.SfxSampleRate;
            var sampleCount = Mathf.Max(1, Mathf.RoundToInt(sampleRate * durationSeconds));
            var clip = AudioClip.Create("tone", sampleCount, 1, sampleRate, false);
            var samples = new float[sampleCount];

            for (var index = 0; index < sampleCount; index++)
            {
                var time = index / (float)sampleRate;
                var envelope = 1f - (time / durationSeconds);
                samples[index] = Mathf.Sin(Mathf.PI * 2f * frequencyHz * time) * volume * envelope;
            }

            clip.SetData(samples, 0);
            return clip;
        }
    }
}

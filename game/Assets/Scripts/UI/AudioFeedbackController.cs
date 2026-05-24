using BastionUA.Core;
using BastionUA.Services;
using UnityEngine;

namespace BastionUA.UI
{
    public sealed class AudioFeedbackController : MonoBehaviour
    {
        private AudioSource _audioSource;
        private AudioClip _tapClip;
        private AudioClip _battleVictoryClip;
        private AudioClip _battleDefeatClip;
        private AudioClip _eventClip;
        private AudioClip _upgradeClip;
        private AudioClip _prestigeClip;

        private void Awake()
        {
            try
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
                _audioSource.playOnAwake = false;
                _audioSource.volume = GameConstants.SfxMasterVolume;

                _tapClip = SfxClipResolver.LoadOrCreateTone(
                    GameConstants.SfxTapResourceName,
                    GameConstants.SfxTapFrequencyHz,
                    GameConstants.SfxTapDurationSeconds,
                    GameConstants.SfxTapVolume);
                _battleVictoryClip = SfxClipResolver.LoadOrCreateTone(
                    GameConstants.SfxVictoryResourceName,
                    GameConstants.SfxVictoryFrequencyHz,
                    GameConstants.SfxVictoryDurationSeconds,
                    GameConstants.SfxVictoryVolume);
                _battleDefeatClip = SfxClipResolver.LoadOrCreateTone(
                    GameConstants.SfxDefeatResourceName,
                    GameConstants.SfxDefeatFrequencyHz,
                    GameConstants.SfxDefeatDurationSeconds,
                    GameConstants.SfxDefeatVolume);
                _eventClip = SfxClipResolver.LoadOrCreateTone(
                    GameConstants.SfxEventResourceName,
                    GameConstants.SfxEventFrequencyHz,
                    GameConstants.SfxEventDurationSeconds,
                    GameConstants.SfxEventVolume);
                _upgradeClip = SfxClipResolver.LoadOrCreateTone(
                    GameConstants.SfxUpgradeResourceName,
                    GameConstants.SfxUpgradeFrequencyHz,
                    GameConstants.SfxUpgradeDurationSeconds,
                    GameConstants.SfxUpgradeVolume);
                _prestigeClip = SfxClipResolver.LoadOrCreateTone(
                    GameConstants.SfxPrestigeResourceName,
                    GameConstants.SfxPrestigeFrequencyHz,
                    GameConstants.SfxPrestigeDurationSeconds,
                    GameConstants.SfxPrestigeVolume);

                Debug.Log("[AudioFeedbackController] SFX ready (Resources or procedural fallback).");
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"[AudioFeedbackController] Init failed: {exception}");
            }
        }

        public void PlayTap()
        {
            PlayClip(_tapClip);
        }

        public void PlayBattleVictory()
        {
            PlayClip(_battleVictoryClip);
        }

        public void PlayBattleDefeat()
        {
            PlayClip(_battleDefeatClip);
        }

        public void PlayEventChoice()
        {
            PlayClip(_eventClip);
        }

        public void PlayUpgrade()
        {
            PlayClip(_upgradeClip);
        }

        public void PlayPrestige()
        {
            PlayClip(_prestigeClip);
        }

        private void PlayClip(AudioClip clip)
        {
            if (_audioSource == null || clip == null)
            {
                return;
            }

            try
            {
                _audioSource.PlayOneShot(clip);
            }
            catch (System.Exception exception)
            {
                Debug.LogWarning($"[AudioFeedbackController] Play failed: {exception.Message}");
            }
        }
    }
}

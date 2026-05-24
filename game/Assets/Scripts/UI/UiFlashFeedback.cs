using System.Collections;
using BastionUA.Core;
using UnityEngine;
using UnityEngine.UI;

namespace BastionUA.UI
{
    public sealed class UiFlashFeedback : MonoBehaviour
    {
        private Image _targetImage;
        private Color _baseColor = Color.white;
        private Coroutine _flashRoutine;

        public void Bind(Image targetImage, Color baseColor)
        {
            _targetImage = targetImage;
            _baseColor = baseColor;
        }

        public void Play(Color flashColor)
        {
            if (_targetImage == null)
            {
                return;
            }

            if (_flashRoutine != null)
            {
                StopCoroutine(_flashRoutine);
            }

            _flashRoutine = StartCoroutine(FlashRoutine(flashColor));
        }

        private IEnumerator FlashRoutine(Color flashColor)
        {
            var halfDuration = GameUiConstants.TapFlashDurationSeconds * 0.5f;
            var elapsed = 0f;

            while (elapsed < halfDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                var t = elapsed / halfDuration;
                _targetImage.color = Color.Lerp(_baseColor, flashColor, t);
                yield return null;
            }

            elapsed = 0f;
            while (elapsed < halfDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                var t = elapsed / halfDuration;
                _targetImage.color = Color.Lerp(flashColor, _baseColor, t);
                yield return null;
            }

            _targetImage.color = _baseColor;
            _flashRoutine = null;
        }
    }
}

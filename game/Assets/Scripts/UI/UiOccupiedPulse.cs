using BastionUA.Core;
using UnityEngine;
using UnityEngine.UI;

namespace BastionUA.UI
{
    public sealed class UiOccupiedPulse : MonoBehaviour
    {
        private Image _targetImage;
        private Color _baseColor = Color.white;

        public void Bind(Image targetImage)
        {
            _targetImage = targetImage;
            _baseColor = targetImage.color;
        }

        public void SetActivePulse(bool isActive)
        {
            enabled = isActive;
            if (_targetImage == null)
            {
                return;
            }

            if (!isActive)
            {
                _targetImage.color = _baseColor;
            }
        }

        private void Update()
        {
            if (_targetImage == null || !isActiveAndEnabled)
            {
                return;
            }

            var wave = (Mathf.Sin(Time.unscaledTime * GameUiConstants.MapMarkerOccupiedPulseSpeed) + 1f) * 0.5f;
            var alpha = Mathf.Lerp(
                GameUiConstants.MapMarkerOccupiedPulseMinAlpha,
                GameUiConstants.MapMarkerOccupiedPulseMaxAlpha,
                wave);
            var color = _baseColor;
            color.a = alpha;
            _targetImage.color = color;
        }

        public void RefreshBaseColor(Color color)
        {
            _baseColor = color;
            if (!enabled && _targetImage != null)
            {
                _targetImage.color = _baseColor;
            }
        }
    }
}

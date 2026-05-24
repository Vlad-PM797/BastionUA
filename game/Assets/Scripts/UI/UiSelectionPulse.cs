using BastionUA.Core;
using UnityEngine;
using UnityEngine.UI;

namespace BastionUA.UI
{
    public sealed class UiSelectionPulse : MonoBehaviour
    {
        private Image _targetImage;
        private Color _baseColor = Color.white;

        public void Bind(Image targetImage)
        {
            _targetImage = targetImage;
            _baseColor = targetImage.color;
        }

        private void Update()
        {
            if (_targetImage == null || !isActiveAndEnabled)
            {
                return;
            }

            var wave = (Mathf.Sin(Time.unscaledTime * GameUiConstants.MarkerPulseSpeed) + 1f) * 0.5f;
            var alpha = Mathf.Lerp(
                GameUiConstants.MarkerPulseMinAlpha,
                GameUiConstants.MarkerPulseMaxAlpha,
                wave);
            var color = _baseColor;
            color.a = alpha;
            _targetImage.color = color;
        }
    }
}

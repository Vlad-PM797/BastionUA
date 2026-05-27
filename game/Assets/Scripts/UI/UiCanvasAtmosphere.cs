using BastionUA.Core;
using UnityEngine;
using UnityEngine.UI;

namespace BastionUA.UI
{
    public sealed class UiCanvasAtmosphere : MonoBehaviour
    {
        private Image _glowOverlay;

        public void BindGlowOverlay(Image glowOverlay)
        {
            _glowOverlay = glowOverlay;
        }

        private void Update()
        {
            if (_glowOverlay == null)
            {
                return;
            }

            var wave = (Mathf.Sin(Time.unscaledTime * GameUiConstants.AtmospherePulseSpeed) + 1f) * 0.5f;
            var alpha = Mathf.Lerp(
                GameUiConstants.AtmosphereGlowMinAlpha,
                GameUiConstants.AtmosphereGlowMaxAlpha,
                wave);
            var color = GameVisualPalette.AtmosphereBlueGlow;
            color.a = alpha;
            _glowOverlay.color = color;
        }
    }
}

using System.Collections;
using BastionUA.Core;
using UnityEngine;

namespace BastionUA.UI
{
    public sealed class UiPopupFadeIn : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            try
            {
                _canvasGroup = GetComponent<CanvasGroup>();
                if (_canvasGroup == null)
                {
                    _canvasGroup = gameObject.AddComponent<CanvasGroup>();
                }

                _canvasGroup.alpha = 0f;
                _canvasGroup.interactable = true;
                _canvasGroup.blocksRaycasts = true;
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"[UiPopupFadeIn] Awake failed: {exception}");
            }
        }

        private void OnEnable()
        {
            if (_canvasGroup == null)
            {
                return;
            }

            StopAllCoroutines();
            StartCoroutine(FadeInRoutine());
        }

        private IEnumerator FadeInRoutine()
        {
            var duration = GameUiConstants.PopupFadeInDurationSeconds;
            var elapsed = 0f;
            _canvasGroup.alpha = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                _canvasGroup.alpha = Mathf.Clamp01(elapsed / duration);
                yield return null;
            }

            _canvasGroup.alpha = 1f;
        }
    }
}

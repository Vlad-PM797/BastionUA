using BastionUA.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BastionUA.UI
{
    public sealed class UiPressScaleBehavior : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        private Vector3 _defaultScale = Vector3.one;
        private float _pressedScale = GameUiConstants.ButtonPressedScale;

        public void Configure(float pressedScale)
        {
            _pressedScale = pressedScale;
            _defaultScale = transform.localScale;
        }

        private void Awake()
        {
            _defaultScale = transform.localScale;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            transform.localScale = _defaultScale * _pressedScale;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            transform.localScale = _defaultScale;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.localScale = _defaultScale;
        }
    }
}

using BastionUA.Core;
using UnityEngine;

namespace BastionUA.UI
{
    public static class UiIconLoader
    {
        public static Sprite LoadIcon(UiIconKind kind)
        {
            try
            {
                var resourcePath = GetResourcePath(kind);
                var sprite = Resources.Load<Sprite>(resourcePath);
                if (sprite != null)
                {
                    return sprite;
                }
            }
            catch (System.Exception exception)
            {
                Debug.LogWarning($"[UiIconLoader] Resource load failed for {kind}: {exception.Message}");
            }

            return UiIconRasterizer.CreateIcon(kind);
        }

        private static string GetResourcePath(UiIconKind kind)
        {
            switch (kind)
            {
                case UiIconKind.Ammo:
                    return GameVisualPalette.UiIconAmmoResourcePath;
                case UiIconKind.Morale:
                    return GameVisualPalette.UiIconMoraleResourcePath;
                case UiIconKind.Battle:
                    return GameVisualPalette.UiIconBattleResourcePath;
                default:
                    return string.Empty;
            }
        }
    }
}

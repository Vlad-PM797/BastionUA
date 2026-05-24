using UnityEditor;
using UnityEngine;

namespace BastionUA.EditorTools
{
    public static class GameArtAssetBaker
    {
        [MenuItem("BastionUA/Art/Bake All Art Assets")]
        public static void BakeAllArtAssets()
        {
            try
            {
                UkraineMapAssetBaker.BakeUkraineMapPng();
                UkraineMapAssetBaker.BakeUkraineMapV2Png();
                UiIconAssetBaker.BakeUiIconsPng();
                Debug.Log("[GameArtAssetBaker] All art assets baked.");
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"[GameArtAssetBaker] Bake all failed: {exception}");
            }
        }
    }
}

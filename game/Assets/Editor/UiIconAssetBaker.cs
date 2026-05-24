using System.IO;
using BastionUA.Core;
using BastionUA.UI;
using UnityEditor;
using UnityEngine;

namespace BastionUA.EditorTools
{
    public static class UiIconAssetBaker
    {
        private const string OutputFolder = "Assets/Resources/Art/ui";

        [MenuItem("BastionUA/Art/Bake UI Icons PNG")]
        public static void BakeUiIconsPng()
        {
            try
            {
                Directory.CreateDirectory(OutputFolder);
                BakeIcon(UiIconKind.Ammo, "icon_ammo.png");
                BakeIcon(UiIconKind.Morale, "icon_morale.png");
                BakeIcon(UiIconKind.Battle, "icon_battle.png");
                AssetDatabase.Refresh();
                Debug.Log("[UiIconAssetBaker] UI icon PNGs baked.");
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"[UiIconAssetBaker] Bake failed: {exception}");
            }
        }

        private static void BakeIcon(UiIconKind kind, string fileName)
        {
            var sprite = UiIconRasterizer.CreateIcon(kind);
            var texture = sprite.texture;
            var pngBytes = texture.EncodeToPNG();
            Object.DestroyImmediate(texture);
            Object.DestroyImmediate(sprite);

            var outputPath = Path.Combine(OutputFolder, fileName);
            File.WriteAllBytes(outputPath, pngBytes);
            AssetDatabase.Refresh();

            var importer = AssetImporter.GetAtPath(outputPath) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spritePixelsPerUnit = 64f;
                importer.mipmapEnabled = false;
                importer.alphaIsTransparency = true;
                importer.filterMode = FilterMode.Bilinear;
                importer.SaveAndReimport();
            }
        }
    }
}

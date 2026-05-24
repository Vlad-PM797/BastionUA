using System.IO;
using BastionUA.Core;
using BastionUA.UI;
using UnityEditor;
using UnityEngine;

namespace BastionUA.EditorTools
{
    public static class UkraineMapAssetBaker
    {
        private const string OutputFolder = "Assets/Resources/Art";
        private const string OutputFileName = "ukraine_map.png";
        private const string OutputV2FileName = "ukraine_map_v2.png";

        [MenuItem("BastionUA/Art/Bake Ukraine Map PNG")]
        public static void BakeUkraineMapPng()
        {
            BakeMapPng(OutputFileName, MapTextureQuality.Standard);
        }

        [MenuItem("BastionUA/Art/Bake Ukraine Map V2 PNG")]
        public static void BakeUkraineMapV2Png()
        {
            BakeMapPng(OutputV2FileName, MapTextureQuality.Enhanced);
        }

        private static void BakeMapPng(string fileName, MapTextureQuality quality)
        {
            try
            {
                Directory.CreateDirectory(OutputFolder);

                var width = Mathf.RoundToInt(MapUiConstants.MapLandmassWidth * MapUiConstants.MapSilhouetteTextureScale);
                var height = Mathf.RoundToInt(MapUiConstants.MapLandmassHeight * MapUiConstants.MapSilhouetteTextureScale);
                var texture = UkraineMapRasterizer.CreateMapTexture(width, height, quality);
                var pngBytes = texture.EncodeToPNG();
                Object.DestroyImmediate(texture);

                var outputPath = Path.Combine(OutputFolder, fileName);
                File.WriteAllBytes(outputPath, pngBytes);
                AssetDatabase.Refresh();

                var importer = AssetImporter.GetAtPath(outputPath) as TextureImporter;
                if (importer != null)
                {
                    importer.textureType = TextureImporterType.Sprite;
                    importer.spritePixelsPerUnit = MapUiConstants.MapSilhouetteTextureScale;
                    importer.mipmapEnabled = false;
                    importer.alphaIsTransparency = true;
                    importer.filterMode = FilterMode.Bilinear;
                    importer.SaveAndReimport();
                }

                Debug.Log($"[UkraineMapAssetBaker] Saved {outputPath} ({quality}).");
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"[UkraineMapAssetBaker] Bake failed: {exception}");
            }
        }
    }
}

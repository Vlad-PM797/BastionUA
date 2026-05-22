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

        [MenuItem("BastionUA/Art/Bake Ukraine Map PNG")]
        public static void BakeUkraineMapPng()
        {
            try
            {
                Directory.CreateDirectory(OutputFolder);

                var width = Mathf.RoundToInt(MapUiConstants.MapLandmassWidth * MapUiConstants.MapSilhouetteTextureScale);
                var height = Mathf.RoundToInt(MapUiConstants.MapLandmassHeight * MapUiConstants.MapSilhouetteTextureScale);
                var texture = UkraineMapRasterizer.CreateMapTexture(width, height);
                var pngBytes = texture.EncodeToPNG();
                Object.DestroyImmediate(texture);

                var outputPath = Path.Combine(OutputFolder, OutputFileName);
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

                Debug.Log($"[UkraineMapAssetBaker] Saved {outputPath}");
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"[UkraineMapAssetBaker] Bake failed: {exception}");
            }
        }
    }
}

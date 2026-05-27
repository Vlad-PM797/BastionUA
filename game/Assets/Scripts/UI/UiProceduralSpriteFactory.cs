using BastionUA.Core;
using UnityEngine;

namespace BastionUA.UI
{
    public static class UiProceduralSpriteFactory
    {
        private static Sprite _circleFilledSprite;
        private static Sprite _circleRingSprite;
        private static Sprite _hudCornerSprite;
        private static Sprite _canvasGradientSprite;

        public static Sprite GetCircleFilledSprite()
        {
            if (_circleFilledSprite != null)
            {
                return _circleFilledSprite;
            }

            var size = GameUiConstants.ProceduralCircleTextureSize;
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Bilinear;

            var center = (size - 1) * 0.5f;
            var radius = size * 0.46f;

            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var distance = Vector2.Distance(new Vector2(x, y), new Vector2(center, center));
                    var alpha = distance <= radius ? 1f : 0f;
                    texture.SetPixel(x, y, new Color(1f, 1f, 1f, alpha));
                }
            }

            texture.Apply();
            _circleFilledSprite = Sprite.Create(
                texture,
                new Rect(0f, 0f, size, size),
                new Vector2(0.5f, 0.5f),
                size);
            return _circleFilledSprite;
        }

        public static Sprite GetCircleRingSprite()
        {
            if (_circleRingSprite != null)
            {
                return _circleRingSprite;
            }

            var size = GameUiConstants.ProceduralCircleTextureSize;
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Bilinear;

            var center = (size - 1) * 0.5f;
            var outerRadius = size * 0.46f;
            var innerRadius = size * 0.30f;

            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var distance = Vector2.Distance(new Vector2(x, y), new Vector2(center, center));
                    var alpha = distance <= outerRadius && distance >= innerRadius ? 1f : 0f;
                    texture.SetPixel(x, y, new Color(1f, 1f, 1f, alpha));
                }
            }

            texture.Apply();
            _circleRingSprite = Sprite.Create(
                texture,
                new Rect(0f, 0f, size, size),
                new Vector2(0.5f, 0.5f),
                size);
            return _circleRingSprite;
        }

        public static Sprite GetHudCornerSprite()
        {
            if (_hudCornerSprite != null)
            {
                return _hudCornerSprite;
            }

            var size = GameUiConstants.ProceduralCornerTextureSize;
            var thickness = GameUiConstants.HudCornerLinePixels;
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Bilinear;

            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var onHorizontal = y < thickness;
                    var onVertical = x < thickness;
                    var alpha = onHorizontal || onVertical ? 1f : 0f;
                    texture.SetPixel(x, y, new Color(1f, 1f, 1f, alpha));
                }
            }

            texture.Apply();
            _hudCornerSprite = Sprite.Create(
                texture,
                new Rect(0f, 0f, size, size),
                new Vector2(0.5f, 0.5f),
                size);
            return _hudCornerSprite;
        }

        public static Sprite GetCanvasGradientSprite()
        {
            if (_canvasGradientSprite != null)
            {
                return _canvasGradientSprite;
            }

            var width = GameUiConstants.CanvasGradientTextureWidth;
            var height = GameUiConstants.CanvasGradientTextureHeight;
            var texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Bilinear;

            for (var y = 0; y < height; y++)
            {
                var t = y / (float)(height - 1);
                var color = Color.Lerp(
                    GameVisualPalette.CanvasBackgroundBottom,
                    GameVisualPalette.CanvasBackgroundTop,
                    t);
                for (var x = 0; x < width; x++)
                {
                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply();
            _canvasGradientSprite = Sprite.Create(
                texture,
                new Rect(0f, 0f, width, height),
                new Vector2(0.5f, 0.5f),
                width);
            return _canvasGradientSprite;
        }
    }
}

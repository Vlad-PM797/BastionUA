using BastionUA.Core;
using UnityEngine;

namespace BastionUA.Services
{
    public sealed class ResourceService
    {
        private float _tickAccumulator;

        public void ManualTap(GameState state)
        {
            state.Ammo += GameConstants.TapAmmoGain;
            Debug.Log($"[ResourceService] Manual tap +{GameConstants.TapAmmoGain} ammo. Total: {state.Ammo}");
        }

        public void Tick(GameState state, float deltaTime)
        {
            _tickAccumulator += deltaTime;

            if (_tickAccumulator < GameConstants.AutoTickSeconds)
            {
                return;
            }

            var ticks = Mathf.FloorToInt(_tickAccumulator / GameConstants.AutoTickSeconds);
            _tickAccumulator -= ticks * GameConstants.AutoTickSeconds;

            var gained = ticks * GameConstants.AutoTickAmmoGain;
            state.Ammo += gained;

            Debug.Log($"[ResourceService] Auto tick +{gained} ammo ({ticks} ticks). Total: {state.Ammo}");
        }
    }
}

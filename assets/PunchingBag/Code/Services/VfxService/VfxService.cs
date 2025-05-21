namespace PunchingBag.Code.Services.VfxService
{
    using System;
    using System.Collections.Generic;
    using Core.VFX;
    using MageSurvivor.Code.Core.Abstract.Service;
    using PrimeTween;
    using UnityEngine;

    public interface IVfxService
    {
        void PlayVfx(VfxType type, Vector3 position);
        void PlayStrongPunchVfx();
    }

    public enum VfxType
    {
        Hit,
        Punch,
        Sweat
    }

    public class VfxService : Service, IVfxService
    {
        private Dictionary<VfxType,Particles> _vfxDictionary = new Dictionary<VfxType, Particles>();
        private readonly SkyBoxVfxSetting _skyBoxVfxSetting;

        public VfxService(SkyBoxVfxSetting settings)
        {
            _skyBoxVfxSetting = settings;
            _defaultExposure = RenderSettings.skybox.GetFloat("_Exposure");
        }

        public void RegisterVfx(VfxType type, Particles vfx)
        {
            if (_vfxDictionary.ContainsKey(type))
            {
                Debug.LogWarning($"VFX of type {type} is already registered.");
                return;
            }
            _vfxDictionary[type] = vfx;
        }


        public void PlayVfx(VfxType type, Vector3 position)
        {
            if (_vfxDictionary.TryGetValue(type, out var vfx))
            {
                Particles instance;
                switch (type)
                {
                    case VfxType.Hit:
                        instance = vfx.Spawn<HitParticles>();
                        break;
                    case VfxType.Punch:
                        instance = vfx.Spawn<PunchParticles>();
                        break;
                    case VfxType.Sweat:
                        instance = vfx.Spawn<SweatParticles>();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
                instance.transform.position = position;
                instance.Play();
            }
            else
            {
                Debug.LogError($"VFX of type {type} is not registered.");
            }
        }

        private Material _skyBoxMaterial;
        private readonly float _defaultExposure;
        private Tween _tween;

        public void PlayStrongPunchVfx()
        {
            if (_skyBoxVfxSetting == null)
            {
                Debug.LogError("SkyBoxVfxSetting is not assigned.");
                return;
            }
            _skyBoxMaterial??= RenderSettings.skybox;
            var x = _skyBoxMaterial.GetFloat("_Exposure");
            var target = _skyBoxMaterial;
            
        }
        
    }
}
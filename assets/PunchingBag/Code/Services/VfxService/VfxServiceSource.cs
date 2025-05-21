namespace PunchingBag.Code.Services.VfxService
{
    using System;
    using System.Collections.Generic;
    using Core.VFX;
    using MageSurvivor.Code.Core.Abstract.Service;
    using PrimeTween;
    using UnityEngine;

    [CreateAssetMenu(fileName = "VfxSpawnerService", menuName = "PGD/Services/VfxSpawnerService")]
    public class VfxServiceSource : ServiceSource<VfxService>
    {
        [Header("VFX")]
        [SerializeField] List<VFXRecord> _vfxList = new List<VFXRecord>();

        [Header("Strong Punch")] [SerializeField]
        private SkyBoxVfxSetting _skyBoxVfxSetting;
        
        protected override VfxService CreateServiceInstance()
        {
            var vfxService = new VfxService(_skyBoxVfxSetting);
            foreach (var record in _vfxList)
            {
                if (record.Vfx == null)
                {
                    Debug.LogError($"VFX of type {record.Type} is not assigned.");
                    continue;
                }
                
                vfxService.RegisterVfx(record.Type, record.Vfx);
            }
            return vfxService;
        }
    }

    [Serializable]
    public record SkyBoxVfxSetting
    {
        public Color32 color;
        public float duration;
        public Ease ease;
        public AnimationCurve animationCurve;
        public float targetValue = .3f;
    }

    [Serializable]
    public record VFXRecord
    {
        public VfxType Type;
        public Particles Vfx;
    }
}
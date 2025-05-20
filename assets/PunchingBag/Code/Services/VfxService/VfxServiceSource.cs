namespace PunchingBag.Code.Services.VfxService
{
    using System;
    using System.Collections.Generic;
    using Core.VFX;
    using MageSurvivor.Code.Core.Abstract.Service;
    using UnityEngine;

    [CreateAssetMenu(fileName = "VfxSpawnerService", menuName = "PGD/Services/VfxSpawnerService")]
    public class VfxServiceSource : ServiceSource<VfxService>
    {
        [SerializeField] List<VFXRecord> _vfxList = new List<VFXRecord>();
        protected override VfxService CreateServiceInstance()
        {
            var vfxService = new VfxService();
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
    public record VFXRecord
    {
        public VfxType Type;
        public Particles Vfx;
    }
}
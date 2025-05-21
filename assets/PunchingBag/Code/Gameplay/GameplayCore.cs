namespace PunchingBag.Code.Gameplay
{
    using System;
    using System.Threading;
    using Punching;
    using Reflex.Attributes;
    using Services.InputService;
    using Services.VfxService;
    using UnityEngine;

    public class GameplayCore : MonoBehaviour
    {
        private IVfxService _vfxService;
        private IInputService _inputService;
        [SerializeField] private float strongPunchForceMinBorder = 350f;
        public static CancellationTokenSource MainToken = new CancellationTokenSource();
        
        [Inject]
        public void Construct(IInputService inputService, IVfxService vfxService)
        {
            _inputService = inputService;
            _vfxService = vfxService;
            // Constructor logic if needed
            Initialize();
        }
        
        public void Initialize()
        {
            // Initialize the gameplay core
            BoxingGloveMono.OnHit += SpawnVfx;


            Debug.Log("GameplayCore initialized.");
        }

        private void SpawnVfx(HitData hitData)
        {
            _vfxService.PlayVfx(VfxType.Hit, hitData.HitPoint);
            _vfxService.PlayVfx(VfxType.Sweat, hitData.HitPoint, Quaternion.FromToRotation(hitData.HitPoint, hitData.HitDirection));
            Debug.Log($"Hit at {hitData.HitPoint} with force {hitData.HitForce}");
            if (hitData.HitForce >= strongPunchForceMinBorder)
                _vfxService.PlayStrongPunchVfx();
        }

        private void OnDestroy()
        {
            Unsubscribe();
            if (MainToken == null) return;
            
            MainToken.Cancel();
            MainToken.Dispose();
            MainToken = null;
        }

        private void Unsubscribe()
        {
            if (_vfxService != null)
            {
                BoxingGloveMono.OnHit -= SpawnVfx;
            }
        }
    }
}
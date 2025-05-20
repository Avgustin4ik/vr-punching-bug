namespace PunchingBag.Code.Core.VFX
{
    using PunchingBag.Code.Core.Pool;
    using UnityEngine;

    public class Particles : PooledMonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;
        public ParticleSystem ParticleSystem => _particleSystem;
        
        public void Play()
        {
            if (_particleSystem != null)
            {
                _particleSystem.Play();
            }
            else
            {
                Debug.LogWarning("Particle system is not assigned.");
            }
        }
        
        public void Stop()
        {
            if (_particleSystem != null)
            {
                _particleSystem.Stop();
            }
            else
            {
                Debug.LogWarning("Particle system is not assigned.");
            }
        }
        
        
    }
}
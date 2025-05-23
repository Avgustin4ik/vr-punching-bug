namespace PunchingBag.Code.Punching
{
    using System;
    using Core.Pool;
    using Cysharp.Threading.Tasks;
    using MoreMountains.Feedbacks;
    using UniRx;
    using UniRx.Triggers;
    using UnityEngine;

    public class BoxingGloveMono : PooledMonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidBody;
        
        public Rigidbody rigidBody => _rigidBody;

        public static event Action<HitData> OnHit;
        private bool _wasHit = false;
        private IDisposable _stream;
        private void OnEnable()
        {
            _wasHit = false;

            if (_rigidBody == null)
            {
                _rigidBody = GetComponent<Rigidbody>();
            }

            _stream = _rigidBody.OnCollisionEnterAsObservable()
                .FirstOrDefault()
                .Where(x => x.gameObject.TryGetComponent(out Damagable _) && !_wasHit)
                .Subscribe(Hit);
        }

        protected void ReleaseToPool()
        {
            ResetRigidbodyForces();
            Release();
        }
    
        private float _force;
        public void Punch(float force)
        {
            _force = force;
            if (_rigidBody != null)
            {
                _rigidBody.AddForce(transform.forward * force, ForceMode.Impulse);
            }
            else
            {
                Debug.LogWarning("Rigidbody is not assigned.");
            }

        }
        private void Hit(Collision collision)
        {
            _wasHit = true;
            //todo ! replase to Event Bus
            Debug.Log($"Hit {collision.gameObject.name}");

            OnHit?.Invoke(new HitData(collision.contacts[0].point, collision.contacts[0].normal, _force));
            if (collision.rigidbody == null)
                return;
            if (collision.gameObject.TryGetComponent(out Damagable damagableObject))
            {
                damagableObject.TakeDamage(_force);
                ReleaseToPool();
            }
            else
            {
                Debug.Log("No Damagable component found on the object.");
            }
        }


        private UniTask _despawnTask;
        [SerializeField] MMF_Player destroyFeedback;

        private async UniTask DelayAndDespawn(float delay = 0f)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            if (destroyFeedback != null)
            {
                destroyFeedback.PlayFeedbacks();
                await UniTask.Delay(TimeSpan.FromSeconds(destroyFeedback.TotalDuration));
            }
            ResetRigidbodyForces();
            Release();
        }
        
        private void OnDisable()
        {
            _stream?.Dispose();
            _stream = null;
        }
        
        private void ResetRigidbodyForces()
        {
            // Reset forces on the main Rigidbody
            if (_rigidBody != null)
            {
                _rigidBody.velocity = Vector3.zero;
                _rigidBody.angularVelocity = Vector3.zero;
                _rigidBody.Sleep(); // Put Rigidbody to sleep to clear internal forces
            }

            // Reset forces on all child Rigidbodies
            var childRigidbodies = GetComponentsInChildren<Rigidbody>();
            foreach (var rb in childRigidbodies)
            {
                if (rb != _rigidBody) // Skip the main Rigidbody to avoid double reset
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.Sleep();
                }
            }
        }
    }


    [Serializable]
    public struct HitData
    {
        public Vector3 HitPoint;
        public Vector3 HitDirection;
        public float HitForce;

        public HitData(Vector3 hitPoint,
            Vector3 hitDirection,
            float hitForce)
        {
            HitPoint = hitPoint;
            HitDirection = hitDirection;
            HitForce = hitForce;
        }
    }
}
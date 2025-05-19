namespace PunchingBag.Code.Punching
{
    using System;
    using Cysharp.Threading.Tasks;
    using MageSurvivor.Code.Core.Pool;
    using MoreMountains.Feedbacks;
    using UniRx;
    using UniRx.Triggers;
    using UnityEngine;

    public class BoxingGloveMono : PooledMonoBehaviour
    {
        [SerializeField] private float Force = 10f;
        [SerializeField] private Rigidbody _rigidBody;
        
        public Rigidbody rigidBody =>
            _rigidBody;

        public static event Action<HitData> OnHit;

        private void OnEnable()
        {
            if (_rigidBody == null)
            {
                _rigidBody = GetComponent<Rigidbody>();
            }

            _rigidBody.OnCollisionEnterAsObservable()
                .First()
                .Where(x => x.gameObject.TryGetComponent(out Damagable _))
                .Subscribe(x => Hit(x))
                .AddTo(this.cancellationToken.Token);
        }

        public void Punch()
        {
            if (_rigidBody != null)
            {
                _rigidBody.AddForce(transform.forward * Force, ForceMode.Impulse);
            }
            else
            {
                Debug.LogWarning("Rigidbody is not assigned.");
            }
            DelayAndDespawn(2f).Forget();

        }
        private void Hit(Collision collision)
        {
            //todo ! replase to Event Bus
            Debug.Log($"Hit {collision.gameObject.name}");

            OnHit?.Invoke(new HitData(collision.contacts[0].point, collision.contacts[0].normal, Force));
            if (collision.rigidbody == null)
                return;
            if (collision.gameObject.TryGetComponent(out Damagable damagableObject))
            {
                damagableObject.TakeDamage(Force);
            }
            else
            {
                Debug.Log("No Damagable component found on the object.");
            }
        }

        [SerializeField] MMF_Player destroyFeedback;
        private async UniTaskVoid DelayAndDespawn(float delay = 0f)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            if (destroyFeedback != null)
            {
                destroyFeedback.PlayFeedbacks();
                await UniTask.Delay(TimeSpan.FromSeconds(destroyFeedback.TotalDuration));
            }

            Release();
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
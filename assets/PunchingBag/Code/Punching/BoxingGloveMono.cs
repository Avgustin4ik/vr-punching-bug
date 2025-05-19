namespace PunchingBag.Code.Punching
{
    using System;
    using System.Collections.Generic;
    using MageSurvivor.Code.Core.Pool;
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

        private void Awake()
        {
            if (_rigidBody == null)
            {
                _rigidBody = GetComponent<Rigidbody>();
            }

            _rigidBody.OnCollisionEnterAsObservable()
                .First()
                .Subscribe(x => Hit(x))
                .AddTo(this);
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
        }

        private void Hit(Collision collision)
        {
            //todo ! replase to Event Bus
            Debug.Log($"Hit {collision.gameObject.name}");

            OnHit?.Invoke(new HitData(collision.contacts[0].point,
                collision.contacts[0].normal,
                Force));
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
            // other.rigidbody.A
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
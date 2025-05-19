using UnityEngine;

namespace PunchingBag.Code.Punching
{
    using System.Collections.Generic;
    using Reflex.Attributes;
    using Services.InputService;

    public class PunchTest : MonoBehaviour
    {
        [SerializeField] private float force = 10f;
        [SerializeField] private float radius = 1f;
        [SerializeField] private Rigidbody rb;
        private IInputService _inputService;
        
        [Inject]
        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
            _inputService.ActionButton += Hit;
        }

        private Stack<Vector3> hitPoints = new Stack<Vector3>();
        private Stack<Vector3> directions = new Stack<Vector3>();
        private void Hit()
        {
            Vector3 forceDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(0, 1f)).normalized;
            Vector3 hitPoint = transform.position + Random.insideUnitSphere * radius;
            directions.Push(forceDirection);
            hitPoints.Push(hitPoint);
            Debug.DrawLine(hitPoint, hitPoint + forceDirection * 2, Color.red, 2f);
            rb.AddForceAtPosition(forceDirection * force, hitPoint, ForceMode.Impulse);
        }
        
        private void OnDestroy()
        {
            if (_inputService != null)
            {
                _inputService.ActionButton -= Hit;
            }
        }
    }
}
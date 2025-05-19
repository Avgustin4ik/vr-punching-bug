namespace PunchingBag.Code.Punching
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Reflex.Attributes;
    using Services.InputService;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class PunchSpawnerMono : MonoBehaviour
    {
        [SerializeField] private Damagable punchingBag;
        [SerializeField] private BoxingGloveMono boxingGlovePrefab;
        [SerializeField] private BoxCollider spawnArea;
        
        private IInputService _inputService;
        
        [Inject]
        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
            _inputService.ActionButton += SpawnPunch;
        }

        private void OnDestroy()
        {
            if (_inputService != null)
            {
                _inputService.ActionButton -= SpawnPunch;
            }
        }
        
        private Queue<Vector3> positions = new Queue<Vector3>(2);
        private void SpawnPunch()
        {
            if (punchingBag == null)
            {
                Debug.LogError("Punching bag is not assigned.");
                return;
            }
            if (boxingGlovePrefab == null)
            {
                Debug.LogError("Boxing glove prefab is not assigned.");
                return;
            }
            
            //spawn boxing glove
            //do it due pooling
            //set random position and rotation
            var localScale = spawnArea.transform.localScale;
            Vector3 randomPosition = new Vector3(
                Random.Range(-spawnArea.size.x / 2, spawnArea.size.x / 2) * localScale.x,
                Random.Range(-spawnArea.size.y / 2, spawnArea.size.y / 2) * localScale.y,
                Random.Range(-spawnArea.size.z / 2, spawnArea.size.z / 2) * localScale.z
            );
            
            var spawnPos = randomPosition + spawnArea.transform.position;
            positions.Enqueue(spawnPos);
            if (positions.Count > 2)
            {
                positions.Dequeue();
            }

            BoxingGloveMono boxingGlove = boxingGlovePrefab.Spawn<BoxingGloveMono>(
                spawnPos,
                transform.rotation);
            boxingGlove.transform.LookAt(punchingBag.transform);

            boxingGlove.Punch();
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if(positions == null || positions.Count == 0)
                return;
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(positions.First(), 1f);
            if (positions.Count < 2)
                return;
            
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(positions.Last(), 1f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(positions.First(), positions.Last());
            Gizmos.color = Color.white;
        }
#endif
    }
}
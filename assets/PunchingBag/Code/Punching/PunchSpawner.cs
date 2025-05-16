namespace PunchingBag.Code.Punching
{
    using System;
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
            BoxingGloveMono boxingGlove = boxingGlovePrefab.Spawn<BoxingGloveMono>();
            //set random position and rotation
            var localScale = spawnArea.transform.localScale;
            Vector3 randomPosition = new Vector3(
                Random.Range(-spawnArea.size.x / 2, spawnArea.size.x / 2) * localScale.x,
                Random.Range(-spawnArea.size.y / 2, spawnArea.size.y / 2) * localScale.y,
                Random.Range(-spawnArea.size.z / 2, spawnArea.size.z / 2) * localScale.z
            );
            boxingGlove.transform.position = randomPosition + spawnArea.transform.position;
            boxingGlove.transform.LookAt(punchingBag.transform);
            //var move it towatd punching bag

            boxingGlove.Punch();
            //destroy it after hit with small delay (release to pool)
        }
    }
}
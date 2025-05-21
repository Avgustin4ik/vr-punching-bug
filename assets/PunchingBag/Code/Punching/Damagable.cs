namespace PunchingBag.Code.Punching
{
    using System;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public abstract class Damagable : MonoBehaviour
    {
        public DamageIndicator.Bruise[] bruises;
        public AnimationCurve damagePobabilityCurve;
        public float damageProbabilityThreshold = 0.5f;
        private void Awake()
        {
            if (bruises == null || bruises.Length == 0)
            {
                bruises = GetComponentsInChildren<DamageIndicator.Bruise>(true);

            }
            foreach (var bruise in bruises)
            {
                bruise.gameObject.SetActive(false);
            }
        }

        public virtual void TakeDamage(float force)
        {
            var setBruise = Random.Range(0f, 1f);
            var probability = damagePobabilityCurve.Evaluate(setBruise);
            
            if(probability > damageProbabilityThreshold)
                if (bruises.Length > 0)
                {
                    int randomIndex = Random.Range(0, bruises.Length);
                    bruises[randomIndex].gameObject.SetActive(true);
                }
        }
    }
}
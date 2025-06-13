using System.Collections;
using UnityEngine;

namespace EnterpriceLogic
{
    public class SelfDestroyer : MonoBehaviour
    {
        public void Init(float deathDuration)
        {
            StartCoroutine(DestroyAfterDelay(deathDuration));
        }

        private IEnumerator DestroyAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);
        }
    }
}
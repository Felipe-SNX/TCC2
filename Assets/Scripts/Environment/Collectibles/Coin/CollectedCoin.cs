using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace CoinSystem
{
    public class CollectedCoin : MonoBehaviour
    {
        public ParticleSystem CoinParticule; 
        public float Distance; 

        [SerializeField] private AudioClip coinSound; 

        public float moveSpeed = 1.0f; 
        public float originalY; 

        private SpriteRenderer spriteRenderer; 
        private Collider2D coinCollider; 
        private Light2D lightRenderer;

        private void Start()
        {
            originalY = transform.position.y;
            spriteRenderer = GetComponent<SpriteRenderer>();
            coinCollider = GetComponent<Collider2D>();
            lightRenderer = GetComponent<Light2D>();
        }

        private void Update()
        {
            float newY = originalY + Mathf.Sin(Time.time * moveSpeed) * Distance;
            transform.position = new Vector2(transform.position.x, newY);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (UIAudioManager.Instance != null && coinSound != null)
                {
                    UIAudioManager.Instance.PlayCoinSFX(coinSound);
                }
                    
                CreateCoinParticule(transform.position);

                if (MetricsManager.Instance != null)
                {
                    MetricsManager.Instance.RegisterCollectible();
                }

                Collect();
            }
        }

        private void Collect()
        {
            if (spriteRenderer != null) spriteRenderer.enabled = false;
            if (coinCollider != null) coinCollider.enabled = false;
            if (lightRenderer != null) lightRenderer.enabled = false;

            StartCoroutine(DestroyAfterDelay(1f));
        }

        private IEnumerator DestroyAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);
        }

        private void CreateCoinParticule(Vector2 position)
        {
            if (CoinParticule != null)
            {
                Vector3 particlePosition = new(position.x, position.y, 0f);
                CoinParticule.transform.position = particlePosition;
                CoinParticule.Play();
            }
        }
    }
}
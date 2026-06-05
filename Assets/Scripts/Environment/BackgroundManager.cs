using UnityEngine;
using System.Collections.Generic;

namespace Platformer.Environment
{
    /// <summary>
    /// Manages the game background with configurable size and color transitions.
    /// Supports multiple colors that transition as the game progresses.
    /// </summary>
    public class BackgroundManager : MonoBehaviour
    {
        [SerializeField] private BackgroundConfig config;
        [SerializeField] private float progressionSpeed = 1f;

        private SpriteRenderer spriteRenderer;
        private int currentColorIndex = 0;
        private float colorTransitionTimer = 0f;
        private Color currentColor;
        private Color targetColor;
        private float totalProgress = 0f;

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            }

            if (config == null)
            {
                Debug.LogError("BackgroundManager: BackgroundConfig not assigned!");
                return;
            }

            InitializeBackground();
        }

        void Start()
        {
            if (config != null && config.colors.Count > 0)
            {
                currentColor = config.colors[0];
                spriteRenderer.color = currentColor;
            }
        }

        void Update()
        {
            if (config == null || config.colors.Count == 0)
                return;

            UpdateColorTransition();
            UpdateProgression();
        }

        private void InitializeBackground()
        {
            if (config == null)
                return;

            // Set background size
            transform.localScale = config.backgroundSize;

            // Create a simple white texture if no sprite is assigned
            if (spriteRenderer.sprite == null)
            {
                spriteRenderer.sprite = Sprite.Create(
                    CreateSimpleTexture(),
                    new Rect(0, 0, 1, 1),
                    Vector2.one * 0.5f
                );
            }

            // Set initial color
            if (config.colors.Count > 0)
            {
                currentColor = config.colors[0];
                targetColor = config.colors[0];
                spriteRenderer.color = currentColor;
            }
        }

        private void UpdateColorTransition()
        {
            if (config.colors.Count <= 1 || config.colorTransitionDuration <= 0)
                return;

            colorTransitionTimer += Time.deltaTime * progressionSpeed;

            if (colorTransitionTimer >= config.colorTransitionDuration)
            {
                // Move to next color
                currentColorIndex = (currentColorIndex + 1) % config.colors.Count;
                currentColor = config.colors[currentColorIndex];
                targetColor = config.colors[(currentColorIndex + 1) % config.colors.Count];
                colorTransitionTimer = 0f;
            }

            // Smoothly interpolate between colors
            float transitionProgress = colorTransitionTimer / config.colorTransitionDuration;
            spriteRenderer.color = Color.Lerp(currentColor, targetColor, transitionProgress);
        }

        private void UpdateProgression()
        {
            totalProgress += Time.deltaTime * progressionSpeed;
        }

        /// <summary>
        /// Set a new background size
        /// </summary>
        public void SetBackgroundSize(Vector3 newSize)
        {
            transform.localScale = newSize;
        }

        /// <summary>
        /// Set a new color configuration and restart transitions
        /// </summary>
        public void SetColorConfiguration(List<Color> newColors)
        {
            if (newColors == null || newColors.Count == 0)
            {
                Debug.LogWarning("BackgroundManager: Cannot set empty color list!");
                return;
            }

            config.colors = new List<Color>(newColors);
            currentColorIndex = 0;
            colorTransitionTimer = 0f;
            currentColor = config.colors[0];
            targetColor = config.colors[config.colors.Count > 1 ? 1 : 0];
        }

        /// <summary>
        /// Add a new color to the transition sequence
        /// </summary>
        public void AddColor(Color newColor)
        {
            if (config != null)
            {
                config.colors.Add(newColor);
            }
        }

        /// <summary>
        /// Set the progression speed (affects color transition speed)
        /// </summary>
        public void SetProgressionSpeed(float speed)
        {
            progressionSpeed = Mathf.Max(0.1f, speed);
        }

        /// <summary>
        /// Get the current color index
        /// </summary>
        public int GetCurrentColorIndex()
        {
            return currentColorIndex;
        }

        /// <summary>
        /// Get the current progress in the color transition (0 to 1)
        /// </summary>
        public float GetColorTransitionProgress()
        {
            if (config.colorTransitionDuration <= 0)
                return 0f;
            return colorTransitionTimer / config.colorTransitionDuration;
        }

        private Texture2D CreateSimpleTexture()
        {
            Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            texture.SetPixel(0, 0, Color.white);
            texture.Apply();
            return texture;
        }

        public Vector3 GetCurrentSize()
        {
            return transform.localScale;
        }

        public List<Color> GetColors()
        {
            return config != null ? config.colors : new List<Color>();
        }
    }
}

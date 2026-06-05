using UnityEngine;
using Platformer.Environment;

namespace Platformer.Examples
{
    /// <summary>
    /// Example setup script for BackgroundManager.
    /// Shows how to create and configure a background programmatically.
    /// Attach this to a GameObject in your scene to set up the background automatically.
    /// </summary>
    public class BackgroundSetupExample : MonoBehaviour
    {
        [SerializeField] private bool setupOnAwake = true;
        [SerializeField] private string configPath = "BackgroundConfig_Default";

        void Awake()
        {
            if (setupOnAwake)
            {
                SetupBackground();
            }
        }

        public void SetupBackground()
        {
            // Try to load existing config
            BackgroundConfig config = Resources.Load<BackgroundConfig>(configPath);

            if (config == null)
            {
                Debug.LogWarning($"BackgroundSetupExample: Config not found at {configPath}. Creating default...");
                CreateDefaultBackground();
            }
            else
            {
                SetupWithConfig(config);
            }
        }

        private void SetupWithConfig(BackgroundConfig config)
        {
            // Create background GameObject if it doesn't exist
            GameObject bgObject = GameObject.Find("Background");
            if (bgObject == null)
            {
                bgObject = new GameObject("Background");
                bgObject.transform.SetPositionAndRotation(new Vector3(0, 0, 5), Quaternion.identity);
            }

            // Add or get BackgroundManager
            BackgroundManager bgManager = bgObject.GetComponent<BackgroundManager>();
            if (bgManager == null)
            {
                bgManager = bgObject.AddComponent<BackgroundManager>();
            }

            // This is set via inspector, but we can show the pattern:
            Debug.Log("Background setup complete with config: " + config.name);
        }

        private void CreateDefaultBackground()
        {
            GameObject bgObject = new GameObject("Background");
            bgObject.transform.SetPositionAndRotation(new Vector3(0, 0, 5), Quaternion.identity);

            BackgroundManager bgManager = bgObject.AddComponent<BackgroundManager>();
            SpriteRenderer spriteRenderer = bgObject.AddComponent<SpriteRenderer>();

            // Create a simple colored quad
            Texture2D tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            tex.SetPixel(0, 0, Color.white);
            tex.Apply();

            Sprite sprite = Sprite.Create(
                tex,
                new Rect(0, 0, 1, 1),
                Vector2.one * 0.5f
            );

            spriteRenderer.sprite = sprite;
            spriteRenderer.color = Color.white;
            spriteRenderer.sortingOrder = -100;

            Debug.Log("Default background created. Assign a BackgroundConfig via inspector.");
        }

        /// <summary>
        /// Quick setup with predefined colors for a specific theme
        /// </summary>
        public static GameObject CreateBackgroundWithTheme(string theme)
        {
            GameObject bgObject = new GameObject("Background_" + theme);
            bgObject.transform.SetPositionAndRotation(new Vector3(0, 0, 5), Quaternion.identity);

            BackgroundManager bgManager = bgObject.AddComponent<BackgroundManager>();
            SpriteRenderer spriteRenderer = bgObject.AddComponent<SpriteRenderer>();

            // Create white texture
            Texture2D tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            tex.SetPixel(0, 0, Color.white);
            tex.Apply();

            Sprite sprite = Sprite.Create(
                tex,
                new Rect(0, 0, 1, 1),
                Vector2.one * 0.5f
            );

            spriteRenderer.sprite = sprite;
            spriteRenderer.sortingOrder = -100;

            Debug.Log($"Background created with theme: {theme}. Assign BackgroundConfig via inspector.");

            return bgObject;
        }
    }
}

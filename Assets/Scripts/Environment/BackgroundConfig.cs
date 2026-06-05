using UnityEngine;
using System.Collections.Generic;

namespace Platformer.Environment
{
    /// <summary>
    /// Configuration asset for the game background.
    /// Stores size, colors, and transition settings.
    /// </summary>
    [CreateAssetMenu(fileName = "BackgroundConfig", menuName = "Platformer/Background Config", order = 1)]
    public class BackgroundConfig : ScriptableObject
    {
        [System.Serializable]
        public class ColorPhase
        {
            [Tooltip("Color for this phase")]
            public Color color = Color.white;
            
            [Tooltip("Optional description of this phase (e.g., 'Day', 'Sunset', 'Night')")]
            public string phaseName = "";
        }

        [Header("Background Size")]
        [SerializeField]
        [Tooltip("Size of the background in world units (X, Y, Z)")]
        public Vector3 backgroundSize = new Vector3(16f, 10f, 1f);

        [Header("Color Configuration")]
        [SerializeField]
        [Tooltip("List of colors to cycle through")]
        public List<Color> colors = new List<Color>
        {
            Color.white,
            new Color(1f, 0.8f, 0.5f),
            new Color(0.8f, 0.6f, 0.8f),
            new Color(0.2f, 0.2f, 0.4f)
        };

        [Header("Transition Settings")]
        [SerializeField]
        [Range(0.1f, 60f)]
        [Tooltip("Duration in seconds for each color transition")]
        public float colorTransitionDuration = 10f;

        [SerializeField]
        [Range(0f, 1f)]
        [Tooltip("Smoothness of the color transition (0 = linear, 1 = smooth easing)")]
        public float transitionSmoothness = 0.5f;

        [Header("Display Settings")]
        [SerializeField]
        [Tooltip("Sort order of the background sprite (lower numbers appear behind)")]
        public int sortingOrder = -100;

        [SerializeField]
        [Tooltip("Sorting layer name for the background")]
        public string sortingLayerName = "Background";

        public void Validate()
        {
            if (colors == null || colors.Count == 0)
            {
                colors = new List<Color> { Color.white };
                Debug.LogWarning("BackgroundConfig: Colors list was empty. Added default white color.");
            }

            if (backgroundSize.x <= 0 || backgroundSize.y <= 0)
            {
                backgroundSize = new Vector3(16f, 10f, 1f);
                Debug.LogWarning("BackgroundConfig: Background size was invalid. Reset to default.");
            }

            if (colorTransitionDuration <= 0)
            {
                colorTransitionDuration = 10f;
                Debug.LogWarning("BackgroundConfig: Transition duration must be positive. Reset to default.");
            }
        }
    }
}

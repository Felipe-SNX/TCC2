using UnityEngine;

namespace Platformer.Environment
{
    /// <summary>
    /// Controls background color progression based on map progress.
    /// This can be linked to player position or level completion progress.
    /// </summary>
    public class BackgroundProgressionController : MonoBehaviour
    {
        [SerializeField] private BackgroundManager backgroundManager;
        [SerializeField] private Transform targetTransform;
        
        [Header("Progression Settings")]
        [SerializeField]
        [Tooltip("When to start the color progression (in world units or custom metric)")]
        private float progressionStartValue = 0f;
        
        [SerializeField]
        [Tooltip("Maximum progression value before cycle repeats")]
        private float progressionMaxValue = 100f;
        
        [SerializeField]
        [Tooltip("Use target transform position as progression metric")]
        private bool usePositionProgression = true;
        
        [SerializeField]
        [Tooltip("Axis to use for position-based progression (0 = X, 1 = Y, 2 = Z)")]
        private int progressionAxis = 0;

        private float currentProgression = 0f;

        void OnEnable()
        {
            if (backgroundManager == null)
            {
                backgroundManager = GetComponent<BackgroundManager>();
            }

            if (backgroundManager == null)
            {
                Debug.LogError("BackgroundProgressionController: BackgroundManager not found!");
            }
        }

        void Update()
        {
            if (backgroundManager == null)
                return;

            UpdateProgression();
        }

        private void UpdateProgression()
        {
            if (usePositionProgression && targetTransform != null)
            {
                float position = targetTransform.position[progressionAxis];
                currentProgression = position - progressionStartValue;
            }

            // Normalize progression (0 to 1)
            float normalizedProgress = Mathf.Clamp01(currentProgression / progressionMaxValue);

            // Update background manager progression speed based on where we are in the level
            // This affects how fast colors transition
            backgroundManager.SetProgressionSpeed(Mathf.Lerp(0.5f, 2f, normalizedProgress));
        }

        /// <summary>
        /// Set the progression value manually (0 to 1)
        /// </summary>
        public void SetProgression(float normalizedProgress)
        {
            normalizedProgress = Mathf.Clamp01(normalizedProgress);
            currentProgression = normalizedProgress * progressionMaxValue;
        }

        /// <summary>
        /// Get current progression (0 to 1)
        /// </summary>
        public float GetNormalizedProgression()
        {
            return Mathf.Clamp01(currentProgression / progressionMaxValue);
        }

        /// <summary>
        /// Set the target transform to track for progression
        /// </summary>
        public void SetProgressionTarget(Transform target, int axis = 0)
        {
            targetTransform = target;
            progressionAxis = Mathf.Clamp(axis, 0, 2);
        }

        /// <summary>
        /// Enable/disable position-based progression
        /// </summary>
        public void SetUsePositionProgression(bool usePosition)
        {
            usePositionProgression = usePosition;
        }
    }
}

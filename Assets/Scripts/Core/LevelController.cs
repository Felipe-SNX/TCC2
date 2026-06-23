using UnityEngine;

public class LevelController : MonoBehaviour
{
    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayLevelMusic();
        }
    }
}
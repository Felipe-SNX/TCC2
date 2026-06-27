using UnityEngine;

public class LevelController : MonoBehaviour
{
    private void Start()
    {
     
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayLevelMusic();
        }
    }
}
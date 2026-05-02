using UnityEngine;

public class LightInteractable : MonoBehaviour
{
    public virtual void OnLightHit()
    {
        Debug.Log("Objeto energizado!");
    }
}
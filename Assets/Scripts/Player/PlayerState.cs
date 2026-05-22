using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [Header("Estado do Inventário")]
    [SerializeField] private bool hasWater = false;

    private SpriteRenderer sr;
    

    public void CollectWater()
    {
        sr = GetComponent<SpriteRenderer>();
        hasWater = true;
        Debug.Log("Player pegou água!");
        if (hasWater == true && sr != null)
        {
           sr.color = Color.blue;
        } else {
            sr.color = Color.red;
        }
    }

    public bool UseWater()
    {
        if (hasWater)
        {
            hasWater = false;
            Debug.Log("Player usou água!");
            if (hasWater == true && sr != null)
            {
                sr.color = Color.blue;
            } else {
                sr.color = Color.red;
            }
            return true;
        }

        Debug.Log("O jogador não possui água.");
        return false;
    }

    public bool CurrentWaterStatus() => hasWater;
}
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [Header("Estado do Inventário")]
    [SerializeField] private bool hasWater = false;

    public void CollectWater()
    {
        hasWater = true;
        Debug.Log("Player pegou água!");
    }

    public bool UseWater()
    {
        if (hasWater)
        {
            hasWater = false;
            Debug.Log("Player usou água!");
            return true;
        }

        Debug.Log("O jogador não possui água.");
        return false;
    }

    public bool CurrentWaterStatus() => hasWater;
}
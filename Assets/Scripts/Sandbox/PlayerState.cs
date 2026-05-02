using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public bool hasWater = false;

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

        return false;
    }
}
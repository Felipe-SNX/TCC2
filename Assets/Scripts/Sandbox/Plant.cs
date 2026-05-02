using UnityEngine;

public class Plant : MonoBehaviour
{
    public bool isGrown = false;

    public Sprite grownSprite;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isGrown)
        {
            PlayerState player = other.GetComponent<PlayerState>();

            if (player != null && player.UseWater())
            {
                Grow();
            }
        }
    }

    void Grow()
    {
        isGrown = true;
        Debug.Log("Planta cresceu!");

        if (grownSprite != null)
        {
            sr.sprite = grownSprite;
        }
    }
}
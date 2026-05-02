using UnityEngine;

public class MovingPlatform : LightInteractable
{
    public float moveSpeed = 2f;
    public float targetHeight = 3f;

    private Vector3 startPosition;
    private bool activated = false;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (activated)
        {
            Vector3 targetPosition =
                startPosition + Vector3.up * targetHeight;

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );
        }
    }

    public override void OnLightHit()
    {
        Debug.Log("Plataforma ativada!");
        activated = true;
    }
}
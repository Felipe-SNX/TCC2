using UnityEngine;
using UnityEngine.UIElements;

public class EpilepsyWarningController : MonoBehaviour
{
    [SerializeField] private GameObject objectStartScreen;
    public float time = 10f;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        root.schedule.Execute(GoToStartScreen).StartingIn((long)(time * 1000));
    }

    private void GoToStartScreen()
    {
        if (objectStartScreen != null)
        {
            objectStartScreen.SetActive(true);
            gameObject.SetActive(false); 
        }
    }
}
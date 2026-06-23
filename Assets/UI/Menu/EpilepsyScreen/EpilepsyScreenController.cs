using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.UI.Menu.EpilepsyScreen
{
    public class EpilepsyScreenController : MonoBehaviour
    {
        [SerializeField] private GameObject objectStartScreen;
        public float time = 10f;

        private void OnEnable()
        {
            if (GlobalData.avisoEpilepsiaMostrado)
            {
                GoToStartScreen();
                return; 
            }

            var root = GetComponent<UIDocument>().rootVisualElement;
            root.schedule.Execute(GoToStartScreen).StartingIn((long)(time * 1000));
            
            GlobalData.avisoEpilepsiaMostrado = true; 
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
}
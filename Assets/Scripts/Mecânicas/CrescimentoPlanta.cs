using UnityEngine;

public class CrescimentoPlanta : MonoBehaviour
{
    [SerializeField] private int aguaNecessaria = 1;
    private Animator animator;
    private bool jaCresceu = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TentarRegar(int aguaDoPlayer)
    {
        if (!jaCresceu && aguaDoPlayer >= aguaNecessaria)
        {
            Crescer();
        }
        else if (aguaDoPlayer < aguaNecessaria)
        {
            Debug.Log("Água insuficiente!");
        }
    }

    void Crescer()
    {
        jaCresceu = true;
        animator.SetTrigger("Crescer"); 
        
        GetComponent<Collider2D>().enabled = true;
    }
}

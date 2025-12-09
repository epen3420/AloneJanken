using UnityEngine;

public class test_anim : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Animator>().SetTrigger("Appear");
    }
}

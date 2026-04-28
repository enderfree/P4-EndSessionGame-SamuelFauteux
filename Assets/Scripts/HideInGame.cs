using UnityEngine;

public class HideInGame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(gameObject.TryGetComponent<Renderer>(out Renderer renderer))
        {
            renderer.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

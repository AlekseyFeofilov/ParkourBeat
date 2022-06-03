using UnityEngine;

public class TimelineTexture : MonoBehaviour
{

    [SerializeField] private Texture2D texture;
    
    // Start is called before the first frame update
    void Start()
    {
        texture = new Texture2D(8, 8);
        GetComponent<Renderer>().material.mainTexture = texture;
        
        texture.SetPixel(5, 5, Color.red);
        texture.Apply();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

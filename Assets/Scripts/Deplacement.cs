using UnityEngine;

public class Deplacement : MonoBehaviour
{
    public float vitesse = 5f;

    void Update()
    {
        Vector2 deplacement = Vector2.zero;

        if (Input.GetKey(KeyCode.Z))
            deplacement += Vector2.up;

        if (Input.GetKey(KeyCode.S)) 
            deplacement += Vector2.down;

        if (Input.GetKey(KeyCode.Q))
            deplacement += Vector2.left;

        if (Input.GetKey(KeyCode.D)) 
            deplacement += Vector2.right;

        transform.Translate(deplacement * vitesse * Time.deltaTime);
    }
}
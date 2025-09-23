using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GenerationNourriture : MonoBehaviour
{
    public float minX = -5f, maxX = 5f, minY = -5f, maxY = 5f;
    public static int points = 1;

    void Start()
    {
        DeplacerPomme();
    }

    /* Fonction DeplacerPomme : 
     * change les coordonées de la pomme
     */
    public void DeplacerPomme()
    {
        transform.position = new Vector3(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY),
            0
        );
    }

    /* Fonction OnMouseDown : 
     * appelée quand on clique sur la pomme
     * appel la fonction DeplacerPomme
     */
    void OnMouseDown()
    {
        Debug.Log("Pomme cliquée !");
        DeplacerPomme();
    }


    private void Collision(Collider2D other)
    {
        if (other.CompareTag("Pomme"))
        {
            GestionJeu.AjoutScore(points);
            DeplacerPomme();
        }
    }
}

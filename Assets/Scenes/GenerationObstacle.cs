using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationObstacle : MonoBehaviour
{
    public float minX = -5f, maxX = 5f, minY = -5f, maxY = 5f;
    public static int points = 1;
    public GameObject gameOverUI;

    void Start()
    {
        DeplacerBombe();
    }

    void GameOver()
    {
        Time.timeScale = 0f;
        gameOverUI.SetActive(true);
    }

    /* Fonction DeplacerBombe : 
     * change les coordonées de la bombe à chaque fois que le serpent mange la pomme
     * pour la simulation c'est quand on clique en dehors de la bombe
     */
    public void DeplacerBombe()
    {
        transform.position = new Vector3(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY),
            0
        );
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // clic gauche
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            // Raycast pour voir si on clique sur un collider
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider == null || hit.collider.gameObject != gameObject)
            {
                // pas cliqué sur la bombe ? déplacer
                DeplacerBombe();
            }
        }
    }

    

    /* Fonction OnMouseDown : 
     * appelée quand on clique en dehors de la bombe
     * appel la fonction DeplacerBombe
     */
    void OnMouseDown()
    {
        Debug.Log("BOUM!!! Fin de partie !");
        GameOver();

    }

    private void Collision(Collider2D other)
    {
        if (other.CompareTag("Bombe"))
        {
            GameOver();
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mover_mira : MonoBehaviour {

    private Transform miTransform;
    public int velocidad;
    //public GameObject myCanvas;
    //public Transform worldCamera;
    public bool muevase = false;
    public bool disparo = false;
    public bool explosionHecha;
    private GameObject cliente;
    public GameObject explosion;
    public bool overlap;
    public bool overlapEnemy;
    public LayerMask mascaras;
    public LayerMask enemigo;
    public Transform topLeftPos, bottomRightPos;
    private int numJugador;
    private int puntosEnemigo;

    // Use this for initialization
    void Start() {
        miTransform = GetComponent<Transform>();
        cliente = GameObject.Find("Cliente");
        //explosion = (GameObject)Resources.Load("prefabs/disparo_0", typeof(GameObject));
    }

    private void FixedUpdate() {

        //PATOCOLISION
        overlap = Physics2D.OverlapArea(topLeftPos.position, bottomRightPos.position, mascaras);

        if (disparo == true && overlap) {
            cliente.GetComponent<Cliente>().MuertePato("PATOMUERE|" + numJugador);
            if (explosionHecha == false) {
                GameObject explo = Instantiate(explosion);
                explo.transform.localScale = new Vector3(2, 2, 0);
                explo.transform.position = miTransform.position;
                explo.SetActive(true);
                explosionHecha = true;
                puntosEnemigo++;
            }

        }

        //ENEMIGOCOLISION
        overlapEnemy = Physics2D.OverlapArea(topLeftPos.position, bottomRightPos.position, enemigo);

        if (disparo == true && overlapEnemy) {
            Debug.Log("PUM");
            cliente.GetComponent<Cliente>().MuertePato("ENEMIGOMUERE|" + numJugador);
        }
        else {
            disparo = false;
            explosionHecha = false;
        }

        if (puntosEnemigo==2) {
           // Debug.Log("En mover mira lo pilla");
            cliente.GetComponent<Cliente>().Colision("MOVERENEMIGO|");
        }
    }
    public void Disparo(int numJugadores) {
        numJugador = numJugadores;
        disparo = true;
    }

    public void CambiarDireccion(float x, float y) {
        //Vector3 newPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z - transform.position.z)));
        Vector3 newPos = Camera.main.ScreenToWorldPoint(new Vector3(x, y, Mathf.Abs(Camera.main.transform.position.z - transform.position.z)));
        newPos.z = transform.position.z;
        miTransform.position = newPos;
    }

    public Vector3 getMousePosition() {
        
        Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z);

        return position;
    }
}
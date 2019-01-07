using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover_perro : MonoBehaviour {

    private Transform miTransform;
    public float velocidad;
    public string direccion;
    private Animator miAnimator;
    public GameObject perro;
    public GameObject nombre;
    public GameObject boton;
    public GameObject logo;
    public GameObject pato;
    public GameObject nombreJug1;
    public GameObject nombreJug2;


    // Use this for initialization
    void Start() {
        miTransform = this.transform;
        miAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {

        if (direccion.Equals("Horizontal") && miTransform.position.x < -2) {
            miTransform.Translate(Vector3.right * velocidad * Time.deltaTime);
        }
 
        if (miTransform.position.x >= -2){
            miTransform.Translate(Vector3.up * velocidad * Time.deltaTime);
            miAnimator.SetTrigger("salto");
        }
      
        
    }
    public void SalirPerro() {
        perro.SetActive(false);
        nombre.SetActive(false);
        boton.SetActive(false);
        logo.SetActive(false);
        nombreJug1.SetActive(true);
        nombreJug2.SetActive(true);
    }
}
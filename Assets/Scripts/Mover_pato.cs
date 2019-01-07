using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover_pato : MonoBehaviour {

    private Transform miTransform;
    public Vector3 _velocidad;
    public float velocidad = 0;
    private GameObject cliente;
    public bool giro = false;
    private bool muerto = false;
    private Animator miAnimator;
    // Use this for initialization
    void Start() {
        Time.timeScale = 1;
        miTransform = this.transform;
        miAnimator = GetComponent<Animator>();
        cliente = GameObject.Find("Cliente");
    }

    // Update is called once per frame
    void Update() {
        //miTransform.Translate(Vector3.up * velocidad * Time.deltaTime);
        if (!muerto) {
            miTransform.Translate(_velocidad * velocidad * Time.deltaTime);
        }
        else {
            miTransform.Translate(Vector3.down * 2 * Time.deltaTime);
        }

    }

    public void MoverPato(float velocidad, Vector3 _velocidad) {
        if (!muerto) {
            this._velocidad = new Vector3(_velocidad.x, _velocidad.y, 0);
            this.velocidad = velocidad;
        }
    }

    public void MataPato(float velocidad, Vector3 _velocidad) {

        miAnimator.SetTrigger("muerto");
        muerto = true;

    }

    void OnTriggerEnter2D(Collider2D other) {
        
        string NombreJugColi = cliente.GetComponent<Cliente>().playerName + "";
        //if(cliente)
        string jugador1 = cliente.GetComponent<Cliente>().jugadores[0].playerName;

        //cuando choco
        if ((other.transform.tag.Equals("Pared") || miTransform.position.x <= -8 || miTransform.position.x >= 8) && NombreJugColi == jugador1) {
           
                cliente.GetComponent<Cliente>().Colision("PATOCHOQUEX|");

            //miTransform.GetComponent<SpriteRenderer>().flipX = !miTransform.GetComponent<SpriteRenderer>().flipX;
        }
        else if (other.transform.tag.Equals("Cielo") && NombreJugColi == jugador1 && !estaMuerto()) {
            cliente.GetComponent<Cliente>().Colision("PATOCHOQUEY|");
        }
        else if ((miTransform.position.x < -8 || miTransform.position.x > 8)||((other.transform.tag.Equals("Suelo") || miTransform.position.y==-5) && estaMuerto() && NombreJugColi == jugador1)) {
            Reiniciar();
        }


    }

    public void Reiniciar() {
        //UnityEditor.PrefabUtility.ResetToPrefabState(this.gameObject);
        muerto = false;
        GameObject.Find("PoolObjectManager").GetComponent<PoolObject>().AnadirPatos(gameObject);
        gameObject.SetActive(false);
        float xPatoNuevo = Random.Range(-5f, 5f);
        cliente.GetComponent<Cliente>().Colision("NUEVOPATO|"+xPatoNuevo);
        cliente.GetComponent<Cliente>().Colision("MOVERPATO|");
    }

    private void OnEnable() {
        //Invoke("reiniciar", 5);
    }

    private bool estaMuerto() {
        return muerto;
    }

}

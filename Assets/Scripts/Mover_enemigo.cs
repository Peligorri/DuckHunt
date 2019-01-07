using UnityEngine;

public class Mover_enemigo : MonoBehaviour {

    private Transform miTransform;
    public float velocidad;
    public string direccion;
    public float tiempo;
    private float tiempoActual;
    public GameObject enemigo;
    public GameObject animacionExpl;
    private Animator miAnimator;
    private bool explosion = false;

    // Use this for initialization
    void Start() {
        miTransform = this.transform;
        tiempoActual = 0;
        miAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void MoverEnemigo() {
        if (explosion == false) {
            enemigo.SetActive(true);
        }
        
        tiempoActual += Time.deltaTime;

        if (direccion.Equals("Horizontal")) {
            miTransform.Translate(Vector3.right * velocidad * Time.deltaTime);
        }
        else {
            miTransform.Translate(Vector3.left * velocidad * Time.deltaTime);
            
        }

        if (tiempoActual >= tiempo) {
            CambioDireccion();
            miTransform.GetComponent<SpriteRenderer>().flipX = !miTransform.GetComponent<SpriteRenderer>().flipX;
        }
    }

    private void CambioDireccion() {
        velocidad *= -1;
        tiempoActual = 0;
    }

    public void enemigoMuere() {
        enemigo.SetActive(false);
        explosion = true;
        animacionExpl.transform.position = enemigo.transform.position;
        animacionExpl.SetActive(true);
        //Destroy(enemigo);
    }
}

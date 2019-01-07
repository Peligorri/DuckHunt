using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolObject : MonoBehaviour {

    public GameObject patos;
    public int cantidad;
    private List<GameObject> listaPatos;

    // Use this for initialization
    void Start() {
        listaPatos = new List<GameObject>();
        for (int i = 0; i < cantidad; i++) {
            listaPatos.Add(Instantiate(patos));
        }
    }

    public GameObject CrearPatos(Vector3 pos) {
        GameObject patosColocar = listaPatos[0];
        listaPatos.RemoveAt(0);
        GameObject posPato = GameObject.Find("posPato");
        posPato.transform.position = pos;
        
        Vector3 nuevaPos = new Vector3(posPato.transform.position.x, posPato.transform.position.y, 1);

        patosColocar.transform.parent = GameObject.Find("Canvas").transform;
        patosColocar.transform.position = pos;
        patosColocar.SetActive(true);
        return patosColocar;
    }

    public void AnadirPatos(GameObject patosAnadir) {
        listaPatos.Add(patosAnadir);
    }
}
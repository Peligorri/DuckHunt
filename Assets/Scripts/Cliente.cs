using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


//DUCK HUNT
public class Player {
    public string playerName;
    public int posJugador;
    public GameObject avatar;
    public int connectId;
}


public class Cliente : MonoBehaviour {

    private const int MAX_CONNECTION = 100;
    private int port = 5701;

    private int hostId;
    private int webHostId;

    private int reliableChannel;
    private int unReliableChannel;

    private float connectionTime;
    private int connectionId;
    private bool isConnected;
    private bool isStarted = false;

    //para evitar el lag
    private bool movido = false;



    private byte error;

    //el nombre del usuario
    public string playerName;
    private int ourClientId;

    public Transform jugador1, jugador2;
    public GameObject canvas;

    public List<Player> jugadores = new List<Player>();
    public GameObject playerPrefab1;
    public GameObject playerPrefab2;
    private GameObject pato;
    public GameObject enemigo;
    public Vector3 _velocidadPato;
    public float velocidad;
    public GameObject nombre;
    public GameObject boton;
    public GameObject logo;
    public GameObject perro;
    public GameObject miPoolPatos;
    public GameObject win;
    public GameObject lose;
    public GameObject perroPato;
    public GameObject perroRie;
    public Text nombreJug1;
    public Text nombreJug2;
    public Text puntosJug1;
    public Text puntosJug2;
    private bool puntoPuesto = false;
    private bool puntoPuestoExp = false;
    //public Transform worldCamera;

    private int puntos1;
    private int puntos2;

    public void Connect() {



        string pName = GameObject.Find("NameInput").GetComponent<InputField>().text;

        if (pName == "") {
            Debug.Log("Debes escribir un nombre");
            return;
        }

        playerName = pName;

        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();

        reliableChannel = cc.AddChannel(QosType.Reliable);
        unReliableChannel = cc.AddChannel(QosType.Unreliable);

        HostTopology topo = new HostTopology(cc, MAX_CONNECTION);

        hostId = NetworkTransport.AddHost(topo, 0);
        connectionId = NetworkTransport.Connect(hostId, "127.0.0.1", port, 0, out error); // 192.168.6.92   127.0.0.1

        connectionTime = Time.time;
        isConnected = true;

    }

    private void Update() {
        if (!isConnected) {
            return;
        }

        int recHostId;
        int connectionId;
        int channelId;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        byte error;
        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
        switch (recData) {
            /*case NetworkEventType.Nothing:
                break;

            case NetworkEventType.ConnectEvent:
                break;*/

            case NetworkEventType.DataEvent:
                string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                //Debug.Log("receiving: " + msg);
                string[] splitData = msg.Split('|');
                //Debug.Log("dato del case" + splitData[0] + "segundo valor" + splitData[1]);
                switch (splitData[0]) {
                    case "ASKNAME":
                        OnAskName(splitData);
                        break;

                    case "CNN":
                        Debug.Log("dato del cnn " + splitData[1] + " segundo valor " + splitData[2]);
                        SpawnPlayer(splitData[1], int.Parse(splitData[2]));
                        Cursor.visible = false;
                        break;

                    case "DC":
                        break;

                    case "EMPEZAR":
                        float xPatoNuevo = float.Parse(splitData[1]);
                        Vector3 patoNuevo = new Vector3(xPatoNuevo, -3, 1);
                        pato=miPoolPatos.GetComponent<PoolObject>().CrearPatos(patoNuevo);
                        break;

                    case "NUEVOPATO":
                        puntoPuesto = false;
                        xPatoNuevo = float.Parse(splitData[1]);
                        patoNuevo = new Vector3(xPatoNuevo, -3, 1);
                        pato = miPoolPatos.GetComponent<PoolObject>().CrearPatos(patoNuevo);

                        //velocidad = float.Parse(splitData[1]);
                        //_velocidadPato = new Vector3(1, 0.3F, 0);
                        //pato.GetComponent<Mover_pato>().MoverPato(velocidad, _velocidadPato);
                        break;


                    case "MOVERMIRA":
                        float mouseX = float.Parse(splitData[2]);
                        float mouseY = float.Parse(splitData[3]);
                        jugadores.Find(x => x.playerName == splitData[1]).avatar.GetComponent<Mover_mira>().CambiarDireccion(mouseX,mouseY);
                        //Debug.Log(splitData[1]);
                        break;

                    case "MOVERPATO":
                        velocidad = float.Parse(splitData[1]);
                        _velocidadPato = new Vector3(1, 0.3F, 0);
                        pato.GetComponent<Mover_pato>().MoverPato(velocidad, _velocidadPato);
                        break;

                    case "MOVERENEMIGO":
                        //Debug.Log("llega");
                        velocidad = float.Parse(splitData[1]);
                        _velocidadPato = new Vector3(1, 0.3F, 0);
                        enemigo.GetComponent<Mover_enemigo>().MoverEnemigo();
                        break;

                    case "PATOCHOQUEX":
                        Debug.Log("Choca con el fondo");
                        velocidad = float.Parse(splitData[1]);
                        _velocidadPato.x = _velocidadPato.x * -1;
                        pato.GetComponent<Mover_pato>().MoverPato(velocidad, _velocidadPato);
                        pato.GetComponent<SpriteRenderer>().flipX = !pato.GetComponent<SpriteRenderer>().flipX;
                        break;

                    case "PATOCHOQUEY":
                        Debug.Log("Choca con el fondo");
                        velocidad = float.Parse(splitData[1]);
                        _velocidadPato.y = _velocidadPato.y * -1;
                        pato.GetComponent<Mover_pato>().MoverPato(velocidad, _velocidadPato);
                        break;
                  
                    case "DISPARO":
                        Debug.Log("*****DISPARO******");
                        int jugador = 0;
                        jugador = int.Parse(splitData[2]);
                        jugadores.Find(x => x.playerName == splitData[1]).avatar.GetComponent<Mover_mira>().Disparo(jugador);
                        break;

                    case "PATOMUERE":
                        Debug.Log("*****UN pato menos******");
                        _velocidadPato.y = -1;
                        _velocidadPato.x = 0;
                        velocidad = 0;


                        pato.GetComponent<Mover_pato>().MataPato(velocidad, _velocidadPato);
                        Debug.Log(splitData[1]);
                        if (int.Parse(splitData[1]) == 1 && puntoPuesto == false) {
                            puntos1++;
                            puntosJug1.text = "" + puntos1;
                            puntoPuesto = true;
                        }
                        else if (int.Parse(splitData[1]) == 2 && puntoPuesto == false) {
                            puntos2++;
                            puntosJug2.text = "" + puntos2;
                            puntoPuesto = true;
                        }

                        if (ourClientId == 1 && puntos1 == 10) {
                            //GANA EL JUEGO 
                            win.SetActive(true);
                            perroPato.SetActive(true);
                            pato.SetActive(false);
                        }
                        else if (ourClientId == 1 && puntos2 == 10) {
                            lose.SetActive(true);
                            perroRie.SetActive(true);
                            pato.SetActive(false);
                        }

                        if (ourClientId == 2 && puntos2 == 10) {
                            //GANA EL JUEGO 
                            win.SetActive(true);
                            perroPato.SetActive(true);
                            pato.SetActive(false);
                        }
                        else if (ourClientId == 2 && puntos1 == 10) {
                            lose.SetActive(true);
                            perroRie.SetActive(true);
                            pato.SetActive(false);
                        }

                        break;

                    case "ENEMIGOMUERE":
                        
                        enemigo.GetComponent<Mover_enemigo>().enemigoMuere();
                        Debug.Log(splitData[1]);
                        if (int.Parse(splitData[1]) == 1 && puntoPuestoExp == false) {
                            puntos1 = puntos1 + 2;
                            puntosJug1.text = "" + puntos1;
                            puntoPuestoExp = true;
                        }
                        else if (int.Parse(splitData[1]) == 2 && puntoPuestoExp == false) {
                            puntos2 = puntos2 + 2;
                            puntosJug2.text = "" + puntos2;
                            puntoPuestoExp = true;
                        }

                        if (ourClientId == 1 && puntos1 == 10) {
                            //GANA EL JUEGO 
                            win.SetActive(true);
                            perroPato.SetActive(true);
                            pato.SetActive(false);
                        }
                        else if (ourClientId == 1 && puntos2 == 10) {
                            lose.SetActive(true);
                            perroRie.SetActive(true);
                            pato.SetActive(false);
                        }

                        if (ourClientId == 2 && puntos2 == 10) {
                            //GANA EL JUEGO 
                            win.SetActive(true);
                            perroPato.SetActive(true);
                            pato.SetActive(false);
                        }
                        else if (ourClientId == 2 && puntos1 == 10) {
                            lose.SetActive(true);
                            perroRie.SetActive(true);
                            pato.SetActive(false);
                        }

                        break;

                    default:
                        Debug.Log("Mensaje Invalido" + msg);
                        break;

                }
                break;

                /* case NetworkEventType.DisconnectEvent:
                     break;*/
        }
        //control del movimiento
        
        if (Input.GetAxis("Mouse Y") != 0 &&  Input.GetAxis("Mouse X") != 0) {
            //Enviar el nombre al servidor
            Vector3 mousePosition = jugadores.Find(x => x.playerName == playerName).avatar.GetComponent<Mover_mira>().getMousePosition();
            Send("MOVERMIRA|" + playerName + "|" + mousePosition.x + "|" + mousePosition.y, reliableChannel);
        }
       
        else if (Input.GetKey(KeyCode.A) && isStarted == false) {
            //Enviar el nombre al servidor
            float xPatoNuevo = Random.Range(-7f, 7f);
            Send("EMPEZAR|"+xPatoNuevo, reliableChannel);
            Send("MOVERPATO|", reliableChannel);
            isStarted = true;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0)) {
            //Enviar el nombre al servidor
            //Send("DISPARO|" + playerName, reliableChannel);
            if (ourClientId % 2 != 0 && puntoPuesto == false){
                Send("DISPARO|" + playerName +"|1", reliableChannel);
            }
            else if (ourClientId % 2 == 0 && puntoPuesto == false){
                Send("DISPARO|" + playerName + "|2", reliableChannel);
            }
            movido = false;
        }
        
    }

    private void OnAskName(string[] data) {
        //Id del player
        ourClientId = int.Parse(data[1]);

        //Enviar el nombre al servidor
        Send("NAMEIS|" + playerName, reliableChannel);

        //enviar datos al resto de jugadores
        for (int i = 2; i < data.Length - 1; i++) {
            string[] d = data[i].Split('%');
            SpawnPlayer(d[0], int.Parse(d[1]));
        }

    }

    private void Send(string message, int channelId) {
        //Debug.Log("Sending: " + message);
        byte[] msg = Encoding.Unicode.GetBytes(message);
        NetworkTransport.Send(hostId, connectionId, channelId, msg, message.Length * sizeof(char), out error);

    }

    public void Colision(string message) {
        //Debug.Log("Sending colision: " + message);
        byte[] msg = Encoding.Unicode.GetBytes(message);
        NetworkTransport.Send(hostId, connectionId, reliableChannel, msg, message.Length * sizeof(char), out error);

    }
    public void MuertePato(string message) {
        //Debug.Log("Sending colision: " + message);
        byte[] msg = Encoding.Unicode.GetBytes(message);
        NetworkTransport.Send(hostId, connectionId, reliableChannel, msg, message.Length * sizeof(char), out error);

    }

    private void SpawnPlayer(string playerName, int cnnId) {


        if (cnnId == ourClientId) {
            perro.SetActive(true);
        }

        Player p = new Player();
        if (cnnId % 2 != 0) {
            p.avatar = Instantiate(playerPrefab1, jugador1.position, Quaternion.identity);//con esto creo un jugador
            p.avatar.transform.parent = canvas.transform;
            p.avatar.transform.localScale = new Vector3(1, 1, 1);
            nombreJug1.text = playerName+":";
        }
        else {
            p.avatar = Instantiate(playerPrefab2, jugador2.position, Quaternion.identity);//con esto creo un jugador
            p.avatar.transform.parent = canvas.transform;
            p.avatar.transform.localScale = new Vector3(1, 1, 1);
            nombreJug2.text = playerName + ":";
        }
        //p.avatar.GetComponent<Mover_mira>().worldCamera = worldCamera;
        //p.avatar.GetComponent<Mover_mira>().muevase = true;
        //p.avatar.transform.localPosition = new Vector3(0, 0, 0);

        p.playerName = playerName;
        p.connectId = cnnId;
        jugadores.Add(p);

    }

    public int getClienteId() {

        return connectionId;
    }
}

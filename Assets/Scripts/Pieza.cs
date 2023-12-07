using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pieza : MonoBehaviour
{
    public GameObject pieza;
    public int columna;
    public int fila;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Pieza(GameObject pieza, int columna, int fila)
    {
        this.pieza = pieza;
        this.columna = columna;
        this.fila = fila;
    }

    public void bajarUnaFila()
    {
        Debug.Log($"Columna {columna}, fila {fila}");
        if (fila == 0)
        {
            Destroy(pieza);
        }
        else
        {
            fila--;
            pieza.transform.position = new Vector3(columna, fila, 0);
        }
    }

}

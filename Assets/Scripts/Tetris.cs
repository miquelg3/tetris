using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetris : MonoBehaviour
{
    public int columnas = 10;
    GameObject cuboJugador;

    public int tiempoMovimiento = 1;


    // Start is called before the first frame update
    void Start()
    {
        // Tablero
        for (int i = 21; i > 0; i--)
        {
            GameObject cuboTablero = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cuboTablero.transform.position = new Vector3(0, i, 0);
            GameObject cuboTableroFinal = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cuboTableroFinal.transform.position = new Vector3(columnas + 1, i, 0);
            if (i == 1)
            {
                for (int x = 1; x < columnas + 1; x++)
                {
                    GameObject cuboLineaFinal = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cuboLineaFinal.transform.position = new Vector3(x, i, 0);
                }
            }
        }

        // Llamar a una nueva pieza
        SpawnPieza();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKey(KeyCode.RightArrow))
        {
            cuboJugador.transform.position = new Vector3(cuboJugador.transform.position.x + 1, cuboJugador.transform.position.y, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            cuboJugador.transform.position = new Vector3(cuboJugador.transform.position.x - 1, cuboJugador.transform.position.y, 0);
        }*/
    }

    void SpawnPieza()
    {
        cuboJugador = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cuboJugador.transform.position = new Vector3(columnas / 2, 21, 0);
        StartCoroutine(CaerPieza());
    }

    IEnumerator CaerPieza()
    {
        while (cuboJugador.transform.position.y != 2)
        {
            cuboJugador.transform.position = new Vector3(cuboJugador.transform.position.x, cuboJugador.transform.position.y - 1, 0);
            if (Input.GetKey(KeyCode.RightArrow))
            {
                cuboJugador.transform.position = new Vector3(cuboJugador.transform.position.x + 1, cuboJugador.transform.position.y, 0);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                cuboJugador.transform.position = new Vector3(cuboJugador.transform.position.x - 1, cuboJugador.transform.position.y, 0);
            }
            yield return new WaitForSeconds(tiempoMovimiento);
        }
    }
}

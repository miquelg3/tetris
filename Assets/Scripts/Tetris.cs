using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tetris : MonoBehaviour
{
    public int columnas = 10;
    GameObject cuboJugador;
    public int tiempoMovimiento = 1;
    public KeyCode derecha = KeyCode.RightArrow;
    public KeyCode izquierda = KeyCode.LeftArrow;
    public KeyCode abajo = KeyCode.DownArrow;
    bool[,] posiciones;
    GameObject[] pieza;

    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < 4; x++)
        {
            pieza[x] = GameObject.CreatePrimitive(PrimitiveType.Cube);
        }
        posiciones = new bool[columnas, 20];
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
        // Para que no traspase la pared ni las demás piezas
        // Mover a derecha
        if (Input.GetKeyUp(derecha) && cuboJugador.transform.position.x < columnas && !posiciones[(int)cuboJugador.transform.position.x, (int)cuboJugador.transform.position.y - 2])
        {
            cuboJugador.transform.position = new Vector3(cuboJugador.transform.position.x + 1, cuboJugador.transform.position.y, 0);
        }
        // Mover a izquierda
        if (Input.GetKeyUp(izquierda) && cuboJugador.transform.position.x > 1 && !posiciones[(int)cuboJugador.transform.position.x - 2, (int)cuboJugador.transform.position.y - 2])
        {
            cuboJugador.transform.position = new Vector3(cuboJugador.transform.position.x - 1, cuboJugador.transform.position.y, 0);
        }
        // Mover abajo
        if (Input.GetKeyUp(abajo) && cuboJugador.transform.position.y > 2 && !posiciones[(int)cuboJugador.transform.position.x - 1, (int)cuboJugador.transform.position.y - 3])
        {
            cuboJugador.transform.position = new Vector3(cuboJugador.transform.position.x, cuboJugador.transform.position.y - 1, 0);
        }

    }

    // Crear pieza
    void SpawnPieza()
    {
        /*cuboJugador = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cuboJugador.transform.position = new Vector3(columnas / 2, 21, 0);*/
        int numPieza = UnityEngine.Random.Range(0, 4);
        switch (numPieza)
        {
            case 0:
                SpawnS();
                break;
            case 1:
                SpawnI(); 
                break;
            case 2:
                SpawnL();
                break;
            case 3:
                SpawnT();
                break;
            case 4:
                SpawnO();
                break;
        }
        StartCoroutine(CaerPieza());
    }

    IEnumerator CaerPieza()
    {
        while (!posiciones[(int)cuboJugador.transform.position.x - 1, (int)cuboJugador.transform.position.y - 3])
        {
            cuboJugador.transform.position = new Vector3(cuboJugador.transform.position.x, cuboJugador.transform.position.y - 1, 0);
            yield return new WaitForSeconds(tiempoMovimiento);
            // Si llega al final, llamar a una nueva pieza
            if (cuboJugador.transform.position.y == 2 || posiciones[(int)cuboJugador.transform.position.x - 1, (int)cuboJugador.transform.position.y - 3])
            {
                posiciones[(int)cuboJugador.transform.position.x - 1, (int)cuboJugador.transform.position.y - 2] = true;
                Debug.Log($"Posición {cuboJugador.transform.position.x - 1}, {cuboJugador.transform.position.y - 2}");
                LineaLlena((int)cuboJugador.transform.position.y - 2);
                SpawnPieza();
                break;
            }
        }
    }

    // Comprobar si una línea está completa
    void LineaLlena(int numeroDeLinea)
    {
        if (Enumerable.Range(0, posiciones.GetLength(0)).All(j => posiciones[j, numeroDeLinea]))
        {
            Debug.Log($"Línea {numeroDeLinea} llena");
        }
    }

    // Pieza s
    void SpawnS()
    {
        pieza[0].transform.position = new Vector3(columnas / 2 - 1, 22, 0);
        pieza[1].transform.position = new Vector3(columnas / 2, 22, 0);
        pieza[2].transform.position = new Vector3(columnas / 2, 21, 0);
        pieza[3].transform.position = new Vector3(columnas / 2 + 1, 21, 0);
    }

    void SpawnI()
    {
        pieza[0].transform.position = new Vector3(columnas / 2, 23, 0);
        pieza[1].transform.position = new Vector3(columnas / 2, 22, 0);
        pieza[2].transform.position = new Vector3(columnas / 2, 21, 0);
        pieza[3].transform.position = new Vector3(columnas / 2, 20, 0);
    }

    void SpawnL()
    {
        pieza[0].transform.position = new Vector3(columnas / 2, 23, 0);
        pieza[1].transform.position = new Vector3(columnas / 2, 22, 0);
        pieza[2].transform.position = new Vector3(columnas / 2, 21, 0);
        pieza[3].transform.position = new Vector3(columnas / 2 + 1, 21, 0);
    }

    void SpawnT()
    {
        pieza[0].transform.position = new Vector3(columnas / 2 - 1, 22, 0);
        pieza[1].transform.position = new Vector3(columnas / 2, 22, 0);
        pieza[2].transform.position = new Vector3(columnas / 2 + 1, 22, 0);
        pieza[3].transform.position = new Vector3(columnas / 2, 21, 0);
    }

    void SpawnO()
    {
        pieza[0].transform.position = new Vector3(columnas / 2 - 1, 22, 0);
        pieza[1].transform.position = new Vector3(columnas / 2, 22, 0);
        pieza[2].transform.position = new Vector3(columnas / 2 - 1, 21, 0);
        pieza[3].transform.position = new Vector3(columnas / 2, 21, 0);
    }
}

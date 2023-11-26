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
    GameObject[] piezas = new GameObject[4];

    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < 4; x++)
        {
            piezas[x] = GameObject.CreatePrimitive(PrimitiveType.Cube);
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
        // Para que no traspase la pared ni las dem�s piezas
        // Mover a derecha
        //movimientos();

        foreach (var pieza in piezas)
        {
            if (Input.GetKeyUp(derecha) && pieza.transform.position.x < columnas && !posiciones[(int)pieza.transform.position.x, (int)pieza.transform.position.y - 2])
            {
                pieza.transform.position = new Vector3(pieza.transform.position.x + 1, pieza.transform.position.y, 0);
            }
            // Mover a izquierda
            if (Input.GetKeyUp(izquierda) && pieza.transform.position.x > 1 && !posiciones[(int)pieza.transform.position.x - 2, (int)pieza.transform.position.y - 2])
            {
                pieza.transform.position = new Vector3(pieza.transform.position.x - 1, pieza.transform.position.y, 0);
            }
            // Mover abajo
            if (Input.GetKeyUp(abajo) && pieza.transform.position.y > 2 && !posiciones[(int)pieza.transform.position.x - 1, (int)pieza.transform.position.y - 3])
            {
                pieza.transform.position = new Vector3(pieza.transform.position.x, pieza.transform.position.y - 1, 0);
            }
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
        /*while (!posiciones[(int)cuboJugador.transform.position.x - 1, (int)cuboJugador.transform.position.y - 3])
        {
            cuboJugador.transform.position = new Vector3(cuboJugador.transform.position.x, cuboJugador.transform.position.y - 1, 0);
            yield return new WaitForSeconds(tiempoMovimiento);
            // Si llega al final, llamar a una nueva pieza
            if (cuboJugador.transform.position.y == 2 || posiciones[(int)cuboJugador.transform.position.x - 1, (int)cuboJugador.transform.position.y - 3])
            {
                posiciones[(int)cuboJugador.transform.position.x - 1, (int)cuboJugador.transform.position.y - 2] = true;
                Debug.Log($"Posici�n {cuboJugador.transform.position.x - 1}, {cuboJugador.transform.position.y - 2}");
                LineaLlena((int)cuboJugador.transform.position.y - 2);
                SpawnPieza();
                break;
            }
        }*/

        while (poderIrAbajo())
        {
            foreach (var pieza in piezas)
            {
                pieza.transform.position = new Vector3(pieza.transform.position.x, pieza.transform.position.y - 1, 0);
            }
            yield return new WaitForSeconds(tiempoMovimiento);
        }

        foreach (var pieza in piezas)
        {
            posiciones[(int)pieza.transform.position.x - 1, (int)pieza.transform.position.y - 2] = true;
            Debug.Log($"Posici�n {pieza.transform.position.x - 1}, {pieza.transform.position.y - 2}");
        }
    }

    void movimientos()
    {
        foreach(var pieza in piezas)
        {
            if (Input.GetKeyUp(derecha) && pieza.transform.position.x < columnas && !posiciones[(int)pieza.transform.position.x, (int)pieza.transform.position.y - 2])
            {
                pieza.transform.position = new Vector3(pieza.transform.position.x + 1, pieza.transform.position.y, 0);
            }
            // Mover a izquierda
            if (Input.GetKeyUp(izquierda) && pieza.transform.position.x > 1 && !posiciones[(int)pieza.transform.position.x - 2, (int)pieza.transform.position.y - 2])
            {
                pieza.transform.position = new Vector3(pieza.transform.position.x - 1, pieza.transform.position.y, 0);
            }
            // Mover abajo
            if (Input.GetKeyUp(abajo) && pieza.transform.position.y > 2 && !posiciones[(int)pieza.transform.position.x - 1, (int)pieza.transform.position.y - 3])
            {
                pieza.transform.position = new Vector3(pieza.transform.position.x, pieza.transform.position.y - 1, 0);
            }
        }
    }

    bool poderIrAbajo()
    {
        foreach (var pieza in piezas)
        {
            int x = (int)pieza.transform.position.x - 1;
            int y = (int)pieza.transform.position.y - 3;

            if (y <= 0 || posiciones[x, y - 1])
            {
                return false;
            }
        }

        return true;
    }

    // Comprobar si una l�nea est� completa
    void LineaLlena(int numeroDeLinea)
    {
        if (Enumerable.Range(0, posiciones.GetLength(0)).All(j => posiciones[j, numeroDeLinea]))
        {
            Debug.Log($"L�nea {numeroDeLinea} llena");
        }
    }

    // Piezas
    void SpawnS()
    {
        piezas[0].transform.position = new Vector3(columnas / 2 - 1, 22, 0);
        piezas[1].transform.position = new Vector3(columnas / 2, 22, 0);
        piezas[2].transform.position = new Vector3(columnas / 2, 21, 0);
        piezas[3].transform.position = new Vector3(columnas / 2 + 1, 21, 0);
    }

    void SpawnI()
    {
        piezas[0].transform.position = new Vector3(columnas / 2, 23, 0);
        piezas[1].transform.position = new Vector3(columnas / 2, 22, 0);
        piezas[2].transform.position = new Vector3(columnas / 2, 21, 0);
        piezas[3].transform.position = new Vector3(columnas / 2, 20, 0);
    }

    void SpawnL()
    {
        piezas[0].transform.position = new Vector3(columnas / 2, 23, 0);
        piezas[1].transform.position = new Vector3(columnas / 2, 22, 0);
        piezas[2].transform.position = new Vector3(columnas / 2, 21, 0);
        piezas[3].transform.position = new Vector3(columnas / 2 + 1, 21, 0);
    }

    void SpawnT()
    {
        piezas[0].transform.position = new Vector3(columnas / 2 - 1, 22, 0);
        piezas[1].transform.position = new Vector3(columnas / 2, 22, 0);
        piezas[2].transform.position = new Vector3(columnas / 2 + 1, 22, 0);
        piezas[3].transform.position = new Vector3(columnas / 2, 21, 0);
    }

    void SpawnO()
    {
        piezas[0].transform.position = new Vector3(columnas / 2 - 1, 22, 0);
        piezas[1].transform.position = new Vector3(columnas / 2, 22, 0);
        piezas[2].transform.position = new Vector3(columnas / 2 - 1, 21, 0);
        piezas[3].transform.position = new Vector3(columnas / 2, 21, 0);
    }
}

using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Tetris : MonoBehaviour
{
    public int columnas = 10;
    [Range(0,1)]
    public float tiempoMovimiento = 1f;
    public KeyCode derecha = KeyCode.RightArrow;
    public KeyCode izquierda = KeyCode.LeftArrow;
    public KeyCode abajo = KeyCode.DownArrow;
    public KeyCode rotar = KeyCode.Space;
    public Color[] colores = new Color[4];
    
    bool[,] posiciones;
    GameObject[] piezas = new GameObject[4];
    List<Pieza> todasLasPiezas = new List<Pieza>();
    private int colorNum;
    bool encontradaLineaLlena;

    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < 4; x++)
        {
            piezas[x] = GameObject.CreatePrimitive(PrimitiveType.Cube);
        }
        posiciones = new bool[columnas, 20];
        // Tablero
        for (int y = 19; y > - 2; y--)
        {
            GameObject cuboTablero = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cuboTablero.transform.position = new Vector3(-1, y, 0);
            GameObject cuboTableroFinal = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cuboTableroFinal.transform.position = new Vector3(columnas, y, 0);
            if (y == -1)
            {
                for (int x = -1; x < columnas; x++)
                {
                    GameObject cuboLineaFinal = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cuboLineaFinal.transform.position = new Vector3(x, y, 0);
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
        GameObject piezaDerecha = null;
        float posicionMasDerecha = -1;

        GameObject piezaIzquierda = null;
        float posicionMasIzquierda = columnas;

        GameObject piezaAbajo = null;
        float posicionMasAbajo = 30;

        // Recoger pieza de cada extremo
        foreach (var pieza in piezas)
        {
            if (pieza.transform.position.x > posicionMasDerecha)
            {
                posicionMasDerecha = pieza.transform.position.x;
                piezaDerecha = pieza;
            }
            if (pieza.transform.position.x < posicionMasIzquierda)
            {
                posicionMasIzquierda = pieza.transform.position.x;
                piezaIzquierda = pieza;
            }
            if (pieza.transform.position.y < posicionMasAbajo)
            {
                posicionMasAbajo = pieza.transform.position.y;
                piezaAbajo = pieza;
            }
        }
        // Mover derecha
        if (Input.GetKeyUp(derecha) && PoderIrDerecha(piezaDerecha))
        {
            foreach (var pieza in piezas)
            {
                pieza.transform.position = new Vector3(pieza.transform.position.x + 1, pieza.transform.position.y, 0);
            }
        }
        // Mover a izquierda
        if (Input.GetKeyUp(izquierda) && PoderIrIzquierda(piezaIzquierda))
        {
            foreach (var pieza in piezas)
            {
                pieza.transform.position = new Vector3(pieza.transform.position.x - 1, pieza.transform.position.y, 0);
            }
        }
        // Mover abajo
        if (Input.GetKeyUp(abajo) && PoderIrAbajoTecla(piezaAbajo))
        {
            foreach (var pieza in piezas)
            {
                pieza.transform.position = new Vector3(pieza.transform.position.x, pieza.transform.position.y - 1, 0);
            }
        }
        // Rotar pieza
        if (Input.GetKeyUp(rotar))
        {
            Vector3[] posicionesAlRotar = RotarPieza();
            if (PoderRotarPieza(posicionesAlRotar))
            {
                for (int i = 0; i < posicionesAlRotar.Length; i++)
                {
                    piezas[i].transform.position = posicionesAlRotar[i];
                }
            }
        }
    }

    // Crear pieza
    void SpawnPieza()
    {
        colorNum = UnityEngine.Random.Range(0, 4);
        int numPieza = UnityEngine.Random.Range(0, 5);
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
        foreach (var pieza in piezas)
        {
            pieza.GetComponent<Renderer>().material.color = colores[colorNum];
        }
        StartCoroutine(CaerPieza());
    }

    IEnumerator CaerPieza()
    {
        while (PoderIrAbajo())
        {
            encontradaLineaLlena = false;
            foreach (var pieza in piezas)
            {
                pieza.transform.position = new Vector3(pieza.transform.position.x, pieza.transform.position.y - 1, 0);
                Debug.Log($"Posici�n {pieza.transform.position.x}, {pieza.transform.position.y}");
            }
            yield return new WaitForSeconds(tiempoMovimiento);
        }

        // Llega abajo, guarda posici�n y spawnea nueva pieza
        if (!PoderIrAbajo())
        {
            foreach (var pieza in piezas)
            {
                if (pieza.transform.position.y < 20)
                {
                    Debug.Log($"Posici�n X: {pieza.transform.position.x}, Posici�n Y: {pieza.transform.position.y}");
                    posiciones[(int)pieza.transform.position.x, (int)pieza.transform.position.y] = true;
                    GameObject nuevaPosicion = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    nuevaPosicion.transform.position = pieza.transform.position;
                    nuevaPosicion.transform.GetComponent<Renderer>().material.color = colores[colorNum];
                    todasLasPiezas.Add(new Pieza(nuevaPosicion, (int)pieza.transform.position.x, (int)pieza.transform.position.y));
                    Debug.LogWarning($"Posici�n {pieza.transform.position.x}, {pieza.transform.position.y}");
                }
            }
            for (int i = 0; i < 20; i++)
            {
                if (LineaLlena(i)) encontradaLineaLlena = true;
            }
            if (encontradaLineaLlena) AjustarPosiciones(); encontradaLineaLlena = false;
            SpawnPieza();
        }
    }

    bool PoderIrAbajo()
    {
        foreach (var pieza in piezas)
        {
            int x = (int)pieza.transform.position.x;
            int y = (int)pieza.transform.position.y - 1;

            if (y < 0 || posiciones[x, y])
            {
                return false;
            }
        }
        return true;
    }

    bool PoderIrDerecha(GameObject piezaDerecha)
    {
        if (piezaDerecha.transform.position.x == columnas - 1) return false;
        else if (piezaDerecha.transform.position.x < columnas && !posiciones[(int)piezaDerecha.transform.position.x + 1, (int)piezaDerecha.transform.position.y]) return true;
        else return false;
    }

    bool PoderIrIzquierda(GameObject piezaIzquierda)
    {
        if (piezaIzquierda.transform.position.x > 0 && !posiciones[(int)piezaIzquierda.transform.position.x - 1, (int)piezaIzquierda.transform.position.y]) return true;
        else return false;
    }

    bool PoderIrAbajoTecla(GameObject piezaAbajo)
    {
        if (piezaAbajo.transform.position.y > 0 && !posiciones[(int)piezaAbajo.transform.position.x, (int)piezaAbajo.transform.position.y - 1]) return true;
        else return false;
    }

    // Piezas
    void SpawnS()
    {
        piezas[0].transform.position = new Vector3(columnas / 2 - 1, 20, 0);
        piezas[1].transform.position = new Vector3(columnas / 2, 20, 0);
        piezas[2].transform.position = new Vector3(columnas / 2, 19, 0);
        piezas[3].transform.position = new Vector3(columnas / 2 + 1, 19, 0);
    }

    void SpawnI()
    {
        piezas[0].transform.position = new Vector3(columnas / 2, 20, 0);
        piezas[1].transform.position = new Vector3(columnas / 2, 19, 0);
        piezas[2].transform.position = new Vector3(columnas / 2, 18, 0);
        piezas[3].transform.position = new Vector3(columnas / 2, 17, 0);
    }

    void SpawnL()
    {
        piezas[0].transform.position = new Vector3(columnas / 2, 20, 0);
        piezas[1].transform.position = new Vector3(columnas / 2, 19, 0);
        piezas[2].transform.position = new Vector3(columnas / 2, 18, 0);
        piezas[3].transform.position = new Vector3(columnas / 2 + 1, 18, 0);
    }

    void SpawnT()
    {
        piezas[0].transform.position = new Vector3(columnas / 2 - 1, 20, 0);
        piezas[1].transform.position = new Vector3(columnas / 2, 20, 0);
        piezas[2].transform.position = new Vector3(columnas / 2 + 1, 20, 0);
        piezas[3].transform.position = new Vector3(columnas / 2, 19, 0);
    }

    void SpawnO()
    {
        piezas[0].transform.position = new Vector3(columnas / 2 - 1, 20, 0);
        piezas[1].transform.position = new Vector3(columnas / 2, 20, 0);
        piezas[2].transform.position = new Vector3(columnas / 2 - 1, 19, 0);
        piezas[3].transform.position = new Vector3(columnas / 2, 19, 0);
    }

    Vector3[] RotarPieza()
    {
        // Obtener el centro de rotaci�n (usando el segundo cubo en este caso)
        Vector3 pivot = piezas[1].transform.position;
        Vector3[] posicionesAlRotar = new Vector3[4];
        // Rotar cada cubo alrededor del pivote
        for (int i = 0; i < piezas.Length; i++)
        {
            Vector3 relativePos = piezas[i].transform.position - pivot;
            posicionesAlRotar[i] = new Vector3(-relativePos.y + pivot.x, relativePos.x + pivot.y, 0);
        }
        return posicionesAlRotar;
    }

    bool PoderRotarPieza(Vector3[] posicionesAlRotar)
    {
        bool resultado = true;
        for (int i = 0; i < posicionesAlRotar.Length; i++)
        {
            if (posicionesAlRotar[i].x <= 0 || posicionesAlRotar[i].y <= 0 || posicionesAlRotar[i].x >= columnas || posiciones[(int)posicionesAlRotar[i].x, (int)posicionesAlRotar[i].y])
            {
                resultado = false;
                break;
            }
        }
        return resultado;
    }

    // Comprobar si una l�nea est� completa
    bool LineaLlena(int numeroDeLinea)
    {
        Debug.Log("Numero de l�nea: " + numeroDeLinea);
        if (numeroDeLinea >= 0 && numeroDeLinea < 20)
        {
            if (Enumerable.Range(0, posiciones.GetLength(0)).All(j => posiciones[j, numeroDeLinea]))
            {
                Debug.Log($"L�nea {numeroDeLinea} llena");
                foreach (Pieza pieza in todasLasPiezas)
                {
                    pieza.bajarUnaFila();
                }
                return true;
            }
            return false;
        }
        return false;
    }

    void AjustarPosiciones()
    {
        for (int x = 0; x < columnas; x++)
        {
            for (int y = 1; y < 20; y++)
            {
                posiciones[x, y - 1] = posiciones[x, y];
                if (y == 19)
                {
                    posiciones[x, y] = false;
                }
            }
        }
    }

}

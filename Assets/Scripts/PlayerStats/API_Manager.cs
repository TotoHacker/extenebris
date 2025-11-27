using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class API_Manager : MonoBehaviour
{
    // 🔥 IMPORTANTE: Reemplaza esto con la URL real de tu API cuando la despliegues.
    // Ejemplo si usaras la URL de Supabase directamente o un servicio de hosting:
    // private const string API_URL = "https://tu-api-deployada.com/api/stats";
    private const string API_URL = "http://localhost:5000/api/stats";

    // --- GUARDAR DATOS (POST/UPSERT) ---
    public IEnumerator SaveStats(PlayerStats stats)
    {
        // 1. Serializar el objeto C# a una cadena JSON
        string jsonStats = JsonUtility.ToJson(stats);
        Debug.Log("Enviando JSON: " + jsonStats);

        // 2. Crear la petición POST. Usamos UnityWebRequest.Post(URL, "") para personalizar el contenido
        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(API_URL, ""))
        {
            // Convertir el JSON a bytes
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonStats);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerBuffer();

            // Establecer el encabezado Content-Type para indicar que estamos enviando JSON
            www.SetRequestHeader("Content-Type", "application/json");

            // 3. Enviar la petición y esperar la respuesta
            yield return www.SendWebRequest();

            // 4. Verificar errores y manejar la respuesta
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error al guardar datos ({www.responseCode}): {www.error}");
                // Mostrar la respuesta completa del servidor si hay errores
                Debug.LogError("Respuesta detallada: " + www.downloadHandler.text);
            }
            else
            {
                Debug.Log("Datos guardados con éxito. Respuesta del servidor: " + www.downloadHandler.text);
            }
        }
    }

    // Método de ejemplo para llamar a la coroutine (Actualizado para incluir playerId)
    public void TestSave()
    {
        // El playerId es crucial para el upsert en Supabase
        PlayerStats currentStats = new PlayerStats("user123", 1500, 3, "Bosque Eterno");
        StartCoroutine(SaveStats(currentStats));
    }

    // --------------------------------------------------------------------------------

    // --- CARGAR DATOS (GET) ---
    public IEnumerator LoadStats(string playerId, System.Action<PlayerStats> callback)
    {
        // Añade el ID del jugador al endpoint de la API
        string fullUrl = API_URL + "/" + playerId;

        using (UnityWebRequest www = UnityWebRequest.Get(fullUrl))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error al cargar datos ({www.responseCode}): {www.error}");
                callback(null); // Llamar al callback con null en caso de error
            }
            else
            {
                // 1. Obtener la cadena JSON de la respuesta
                string jsonResponse = www.downloadHandler.text;
                Debug.Log("JSON recibido: " + jsonResponse);

                // 2. Deserializar la cadena JSON a un objeto C# PlayerStats
                PlayerStats loadedStats = JsonUtility.FromJson<PlayerStats>(jsonResponse);
                callback(loadedStats); // Llamar al callback con los datos cargados
            }
        }
    }

    // Método de ejemplo para llamar a la coroutine
    public void TestLoad()
    {
        // Usamos el mismo ID de prueba
        StartCoroutine(LoadStats("user123", (stats) =>
        {
            if (stats != null)
            {
                Debug.Log($"Datos de Carga: ID: {stats.playerId}, Puntos: {stats.score}, Muertes: {stats.deaths}, Zona: {stats.zone}");
                // Aquí puedes actualizar la UI o la lógica del juego con los datos
            }
            else
            {
                Debug.Log("No se pudieron cargar los datos.");
            }
        }));
    }
}
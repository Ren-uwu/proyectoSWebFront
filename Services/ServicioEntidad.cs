using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ProyectoSWebfront.Services
{
    /// <summary>
    /// Servicio genérico para realizar operaciones CRUD con cualquier entidad a través de la API genérica.
    /// </summary>
    public class ServicioEntidad
    {
        private readonly HttpClient _clienteHttp;
        private readonly JsonSerializerOptions _opcionesJson;  // Opciones para configurar cómo se serializa/deserializa el JSON

        public ServicioEntidad(HttpClient clienteHttp)
        {
            _clienteHttp = clienteHttp;
            _opcionesJson = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true   // Permitir que coincidan propiedades aunque tengan diferente capitalización 
                                                     // (por ejemplo: "Nombre" coincidirá con "nombre")
            };
        }

        /// <summary>
        /// Obtiene todas las entidades de una tabla específica.
        /// </summary>
        /// <param name="nombreProyecto">Nombre del proyecto en la API.</param>
        /// <param name="nombreTabla">Nombre de la tabla a consultar.</param>
        public async Task<List<Dictionary<string, object>>?> ObtenerTodosAsync(string nombreProyecto, string nombreTabla)
        {
            try
            {
                var url = $"{nombreProyecto}/{nombreTabla}";
                return await _clienteHttp.GetFromJsonAsync<List<Dictionary<string, object>>>(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener datos: {ex.Message}");
                return new List<Dictionary<string, object>>();
            }
        }

        /// <summary>
        /// Obtiene una entidad por su clave primaria.
        /// </summary>
        /// <param name="nombreProyecto">Nombre del proyecto en la API.</param>
        /// <param name="nombreTabla">Nombre de la tabla a consultar.</param>
        /// <param name="nombreClave">Nombre del campo clave.</param>
        /// <param name="valorClave">Valor de la clave a buscar.</param>
        public async Task<Dictionary<string, object>?> ObtenerPorClaveAsync(
            string nombreProyecto,
            string nombreTabla,
            string nombreClave,
            string valorClave)
        {
            try
            {
                var url = $"{nombreProyecto}/{nombreTabla}/{nombreClave}/{valorClave}";
                var resultado = await _clienteHttp.GetFromJsonAsync<List<Dictionary<string, object>>>(url);
                return resultado?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener entidad: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Crea una nueva entidad.
        /// </summary>
        /// <param name="nombreProyecto">Nombre del proyecto en la API.</param>
        /// <param name="nombreTabla">Nombre de la tabla donde crear la entidad.</param>
        /// <param name="entidad">Datos de la entidad a crear.</param>
        public async Task<bool> CrearAsync(
            string nombreProyecto,
            string nombreTabla,
            Dictionary<string, object> entidad)
        {
            try
            {
                var url = $"{nombreProyecto}/{nombreTabla}";

                // Convertir el diccionario a JSON y prepararlo para enviarlo en la petición HTTP
                var contenido = new StringContent(
                    JsonSerializer.Serialize(entidad),  // Convertir el diccionario a una cadena JSON
                    Encoding.UTF8,                      // Usar codificación UTF-8 (estándar para JSON)
                    "application/json");                // Indicar que el contenido es de tipo JSON

                var respuesta = await _clienteHttp.PostAsync(url, contenido);
                return respuesta.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear entidad: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Actualiza una entidad existente.
        /// </summary>
        /// <param name="nombreProyecto">Nombre del proyecto en la API.</param>
        /// <param name="nombreTabla">Nombre de la tabla a actualizar.</param>
        /// <param name="nombreClave">Nombre del campo clave.</param>
        /// <param name="valorClave">Valor de la clave de la entidad a actualizar.</param>
        /// <param name="entidad">Datos actualizados de la entidad.</param>
        public async Task<bool> ActualizarAsync(
            string nombreProyecto,
            string nombreTabla,
            string nombreClave,
            string valorClave,
            Dictionary<string, object> entidad)
        {
            try
            {
                var url = $"{nombreProyecto}/{nombreTabla}/{nombreClave}/{valorClave}";
                var contenido = new StringContent(
                    JsonSerializer.Serialize(entidad),
                    Encoding.UTF8,
                    "application/json");

                var respuesta = await _clienteHttp.PutAsync(url, contenido);

                return respuesta.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar entidad: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Elimina una entidad por su clave primaria.
        /// </summary>
        /// <param name="nombreProyecto">Nombre del proyecto en la API.</param>
        /// <param name="nombreTabla">Nombre de la tabla donde eliminar.</param>
        /// <param name="nombreClave">Nombre del campo clave.</param>
        /// <param name="valorClave">Valor de la clave de la entidad a eliminar.</param>
        public async Task<bool> EliminarAsync(
            string nombreProyecto,
            string nombreTabla,
            string nombreClave,
            string valorClave)
        {
            try
            {
                var url = $"{nombreProyecto}/{nombreTabla}/{nombreClave}/{valorClave}";
                var respuesta = await _clienteHttp.DeleteAsync(url);
                return respuesta.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar entidad: {ex.Message}");
                return false;
            }
        }
    public async Task<List<Dictionary<string, object>>?> EjecutarProcedimientoAsync(
    string nombreProyecto,
    string nombreTabla,
    string nombreSP,
    Dictionary<string, object> parametros)
    {
        try
        {
            var url = $"{nombreProyecto}/{nombreTabla}/ejecutar-sp";
            // Agregar el nombre del SP como parámetro adicional
            parametros["nombreSP"] = nombreSP;
            var contenido = new StringContent(
            JsonSerializer.Serialize(parametros),
            Encoding.UTF8,
            "application/json"
            );
            var respuesta = await _clienteHttp.PostAsync(url, contenido);
            if (respuesta.IsSuccessStatusCode)
            {
                var resultado = await respuesta.Content.ReadFromJsonAsync<List<Dictionary<string, object>>>(_opcionesJson);
                return resultado;
            }
            Console.WriteLine($"Error al ejecutar procedimiento: {respuesta.ReasonPhrase}");
            return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción en procedimiento: {ex.Message}");
                return null;
            }
        }
    }
}

using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prueba_wilson_castro.Models;
using System.Data;
using System.Data.SqlClient;

namespace prueba_wilson_castro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacienteController : ControllerBase
    {

        private readonly string cadenaSQL;

        public PacienteController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }
        //Ruta para traer listado de pacientes 
        [EnableCors("ReglasCors")]
        [HttpGet]
        [Route("Lista")]

        public IActionResult Lista()
        {

            List<Paciente> pacientes = new List<Paciente>();
            try
            {

                using (var conexion = new SqlConnection(cadenaSQL))
                {

                    conexion.Open();
                    //Hacemos el query por medio de el sp guardado
                    var cmd = new SqlCommand("sp_lista_pacientes", conexion);
                    //Especificamos que se hace por medio de SP
                   
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var rd = cmd.ExecuteReader())
                    {
                        //Leemos cada item de nuestra tabla
                        while (rd.Read())
                        {
                            pacientes.Add(new Paciente
                            {
                                Id = Convert.ToInt32(rd["Id"]),
                                NumeroDocumento = Convert.ToInt32(rd["NumeroDocumento"]),
                                Tipo_documento = rd["Tipo_documento"].ToString(),
                                Nombres = rd["Nombres"].ToString(),
                                Apellidos = rd["Apellidos"].ToString(),
                                Correo = rd["Correo"].ToString(),
                                Telefono = rd["Telefono"].ToString(),
                                FechaNacimiento = Convert.ToDateTime(rd["FechaNacimiento"]),
                                Estado = Convert.ToBoolean(rd["Estado"])
                            });
                        }
                    }

                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Lista de pacientes", data = pacientes });


            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error al traer datos: " + error });

                {


                }
            }



        }

        // Traer solo uno
        [HttpGet]
        [Route("Lista/{Id:int}")]
        public IActionResult Obtener(int Id)
        {
            List<Paciente> lista = new List<Paciente>();
            Paciente paciente = new Paciente();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_lista_pacientes", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Paciente
                            {
                                Id = Convert.ToInt32(rd["Id"]),
                                NumeroDocumento = Convert.ToInt32(rd["NumeroDocumento"]),
                                Tipo_documento = rd["Tipo_documento"].ToString(),
                                Nombres = rd["Nombres"].ToString(),
                                Apellidos = rd["Apellidos"].ToString(),
                                Correo = rd["Correo"].ToString(),
                                Telefono = rd["Telefono"].ToString(),
                                FechaNacimiento = Convert.ToDateTime(rd["FechaNacimiento"]),
                                Estado = Convert.ToBoolean(rd["Estado"])
                            });
                        }
                    }


                }
                paciente = lista.Where(item => item.Id == Id).FirstOrDefault();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Ok", reponse = paciente });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, data = paciente });
            }

        }

        // Guardar paciente
        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Paciente objeto)
        {

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                     var cmd = new SqlCommand("sp_guardar_pacientes", conexion);
                  
                  
                    cmd.Parameters.AddWithValue("NumeroDocumento", objeto.NumeroDocumento);
                    cmd.Parameters.AddWithValue("Tipo_documento", objeto.Tipo_documento);
                    cmd.Parameters.AddWithValue("Nombres", objeto.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", objeto.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", objeto.Correo);
                    cmd.Parameters.AddWithValue("Telefono", objeto.Telefono);
                    cmd.Parameters.AddWithValue("FechaNacimiento", objeto.FechaNacimiento);
                    cmd.Parameters.AddWithValue("Estado", objeto.Estado);
              
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();


                };

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Paciente Creado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }

        }

        // Editar paciente
        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Paciente objeto)
        {

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_editar_pacientes", conexion);
                    cmd.Parameters.AddWithValue("iD", objeto.Id == 0 ? DBNull.Value : objeto.Id);
                    cmd.Parameters.AddWithValue("NumeroDocumento", objeto.NumeroDocumento == 0 ? DBNull.Value : objeto.NumeroDocumento);
                    cmd.Parameters.AddWithValue("Tipo_documento", objeto.Tipo_documento is null ? DBNull.Value : objeto.Tipo_documento);
                    cmd.Parameters.AddWithValue("Nombres", objeto.Nombres is null ? DBNull.Value : objeto.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", objeto.Apellidos is null ? DBNull.Value : objeto.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", objeto.Correo is null ? DBNull.Value : objeto.Correo);
                    cmd.Parameters.Add("FechaNacimiento", SqlDbType.DateTime).Value = (object)objeto.FechaNacimiento ?? DBNull.Value;
                    cmd.Parameters.AddWithValue("Telefono", objeto.Telefono is null ? DBNull.Value : objeto.Telefono);
                    cmd.Parameters.AddWithValue("Estado", objeto.Estado);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();


                };

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Paciente editado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }

        }

        // Eliminar paciente
        [HttpDelete]
        [Route("Eliminar/{Id:int}")]
        public IActionResult Eliminar(int Id)
        {

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_eliminar_pacientes", conexion);
                    cmd.Parameters.AddWithValue("Id", Id);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();


                };

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Paciente eliminado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }

        }
    }

}


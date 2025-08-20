using Bna.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Bna.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RendezVousController : ControllerBase
    {
        private readonly string _connectionString;

        public RendezVousController(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        // GET: api/RendezVous
        [HttpGet]
        public IActionResult GetAll()
        {
            var list = new List<RendezVous>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT * FROM RendezVous", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new RendezVous
                        {
                            IdRendezVous = (int)reader["IdRendezVous"],
                            NomClient = reader["NomClient"].ToString(),
                            Telephone = reader["Telephone"]?.ToString(),
                            Vehicule = reader["Vehicule"]?.ToString(),
                            DateRendezVous = (DateTime)reader["DateRendezVous"]
                        });
                    }
                }
            }

            return Ok(list);
        }

        // GET: api/RendezVous/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            RendezVous rdv = null;

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT * FROM RendezVous WHERE IdRendezVous = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            rdv = new RendezVous
                            {
                                IdRendezVous = (int)reader["IdRendezVous"],
                                NomClient = reader["NomClient"].ToString(),
                                Telephone = reader["Telephone"]?.ToString(),
                                Vehicule = reader["Vehicule"]?.ToString(),
                                DateRendezVous = (DateTime)reader["DateRendezVous"]
                            };
                        }
                    }
                }
            }

            if (rdv == null)
                return NotFound();

            return Ok(rdv);
        }

        // POST: api/RendezVous
        [HttpPost]
        public IActionResult Create([FromBody] RendezVous rdv)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(@"
                    INSERT INTO RendezVous (NomClient, Telephone, Vehicule, DateRendezVous)
                    VALUES (@NomClient, @Telephone, @Vehicule, @DateRendezVous);
                    SELECT SCOPE_IDENTITY();", conn))
                {
                    cmd.Parameters.AddWithValue("@NomClient", rdv.NomClient);
                    cmd.Parameters.AddWithValue("@Telephone", rdv.Telephone ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Vehicule", rdv.Vehicule ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DateRendezVous", rdv.DateRendezVous);

                    rdv.IdRendezVous = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            return CreatedAtAction(nameof(GetById), new { id = rdv.IdRendezVous }, rdv);
        }
    }
}

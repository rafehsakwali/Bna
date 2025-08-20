using Bna.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Bna.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FicheInspectionController : ControllerBase
    {
        private readonly string _connectionString;

        public FicheInspectionController(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        // GET: api/FicheInspection
        [HttpGet]
        public IActionResult GetAll()
        {
            var list = new List<FicheInspection>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT * FROM FicheInspection", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(MapFiche(reader));
                    }
                }
            }

            return Ok(list);
        }

        // GET: api/FicheInspection/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            FicheInspection fiche = null;

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT * FROM FicheInspection WHERE Id = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            fiche = MapFiche(reader);
                        }
                    }
                }
            }

            if (fiche == null)
                return NotFound();

            return Ok(fiche);
        }

        // GET: api/FicheInspection/ByRendezVous/3
        [HttpGet("ByRendezVous/{idRdv}")]
        public IActionResult GetByRendezVous(int idRdv)
        {
            var list = new List<FicheInspection>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT * FROM FicheInspection WHERE IdRendezVous = @IdRdv", conn))
                {
                    cmd.Parameters.AddWithValue("@IdRdv", idRdv);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(MapFiche(reader));
                        }
                    }
                }
            }

            return Ok(list);
        }

        // POST: api/FicheInspection
        [HttpPost]
        public IActionResult Create([FromBody] FicheInspection fiche)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(@"
                    INSERT INTO FicheInspection 
                    (HeureEntree, DateInspection, CommandeTravaux, Agent, NomPrenom, Telephone, Immatriculation, VIN, Kilometrage, Motorisation,
                     PlaquesPolice, VitresPareBrise, BalaisEssuieGlace, Eclairage, Retroviseurs, Pneus, CommentaireTour,
                     ControleVehicule, Commentaire40Points, ControleCapot, CommentaireCapot,
                     ControleSousVehicule, CommentaireSousVehicule, AutresPrestations, CommentairePrestations, Images, IdRendezVous)
                    VALUES 
                    (@HeureEntree, @DateInspection, @CommandeTravaux, @Agent, @NomPrenom, @Telephone, @Immatriculation, @VIN, @Kilometrage, @Motorisation,
                     @PlaquesPolice, @VitresPareBrise, @BalaisEssuieGlace, @Eclairage, @Retroviseurs, @Pneus, @CommentaireTour,
                     @ControleVehicule, @Commentaire40Points, @ControleCapot, @CommentaireCapot,
                     @ControleSousVehicule, @CommentaireSousVehicule, @AutresPrestations, @CommentairePrestations, @Images, @IdRendezVous);
                    SELECT SCOPE_IDENTITY();", conn))
                {
                    cmd.Parameters.AddWithValue("@HeureEntree", fiche.HeureEntree);
                    cmd.Parameters.AddWithValue("@DateInspection", fiche.DateInspection);
                    cmd.Parameters.AddWithValue("@CommandeTravaux", fiche.CommandeTravaux ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Agent", fiche.Agent ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@NomPrenom", fiche.NomPrenom ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Telephone", fiche.Telephone ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Immatriculation", fiche.Immatriculation ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@VIN", fiche.VIN ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Kilometrage", fiche.Kilometrage);
                    cmd.Parameters.AddWithValue("@Motorisation", fiche.Motorisation ?? (object)DBNull.Value);

                    cmd.Parameters.AddWithValue("@PlaquesPolice", fiche.PlaquesPolice ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@VitresPareBrise", fiche.VitresPareBrise ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@BalaisEssuieGlace", fiche.BalaisEssuieGlace ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Eclairage", fiche.Eclairage ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Retroviseurs", fiche.Retroviseurs ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Pneus", fiche.Pneus ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CommentaireTour", fiche.CommentaireTour ?? (object)DBNull.Value);

                    cmd.Parameters.AddWithValue("@ControleVehicule", fiche.ControleVehicule ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Commentaire40Points", fiche.Commentaire40Points ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ControleCapot", fiche.ControleCapot ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CommentaireCapot", fiche.CommentaireCapot ?? (object)DBNull.Value);

                    cmd.Parameters.AddWithValue("@ControleSousVehicule", fiche.ControleSousVehicule ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CommentaireSousVehicule", fiche.CommentaireSousVehicule ?? (object)DBNull.Value);

                    cmd.Parameters.AddWithValue("@AutresPrestations", fiche.AutresPrestations ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CommentairePrestations", fiche.CommentairePrestations ?? (object)DBNull.Value);

                    cmd.Parameters.AddWithValue("@Images", fiche.Images ?? (object)DBNull.Value);

                    cmd.Parameters.AddWithValue("@IdRendezVous", fiche.IdRendezVous ?? (object)DBNull.Value);

                    var newId = Convert.ToInt32(cmd.ExecuteScalar());
                    fiche.Id = newId;
                    fiche.Horodateur = DateTime.Now;
                }
            }

            return CreatedAtAction(nameof(GetById), new { id = fiche.Id }, fiche);
        }

        // 🔹 Méthode utilitaire pour lire un enregistrement
        private FicheInspection MapFiche(SqlDataReader reader)
        {
            return new FicheInspection
            {
                Id = (int)reader["Id"],
                Horodateur = (DateTime)reader["Horodateur"],
                HeureEntree = (TimeSpan)reader["HeureEntree"],
                DateInspection = (DateTime)reader["DateInspection"],
                CommandeTravaux = reader["CommandeTravaux"]?.ToString(),
                Agent = reader["Agent"]?.ToString(),
                NomPrenom = reader["NomPrenom"]?.ToString(),
                Telephone = reader["Telephone"]?.ToString(),
                Immatriculation = reader["Immatriculation"]?.ToString(),
                VIN = reader["VIN"]?.ToString(),
                Kilometrage = reader["Kilometrage"] != DBNull.Value ? (int)reader["Kilometrage"] : 0,
                Motorisation = reader["Motorisation"]?.ToString(),

                PlaquesPolice = reader["PlaquesPolice"]?.ToString(),
                VitresPareBrise = reader["VitresPareBrise"]?.ToString(),
                BalaisEssuieGlace = reader["BalaisEssuieGlace"]?.ToString(),
                Eclairage = reader["Eclairage"]?.ToString(),
                Retroviseurs = reader["Retroviseurs"]?.ToString(),
                Pneus = reader["Pneus"]?.ToString(),
                CommentaireTour = reader["CommentaireTour"]?.ToString(),

                ControleVehicule = reader["ControleVehicule"]?.ToString(),
                Commentaire40Points = reader["Commentaire40Points"]?.ToString(),
                ControleCapot = reader["ControleCapot"]?.ToString(),
                CommentaireCapot = reader["CommentaireCapot"]?.ToString(),

                ControleSousVehicule = reader["ControleSousVehicule"]?.ToString(),
                CommentaireSousVehicule = reader["CommentaireSousVehicule"]?.ToString(),

                AutresPrestations = reader["AutresPrestations"]?.ToString(),
                CommentairePrestations = reader["CommentairePrestations"]?.ToString(),

                Images = reader["Images"]?.ToString(),

                IdRendezVous = reader["IdRendezVous"] != DBNull.Value ? (int)reader["IdRendezVous"] : (int?)null
            };
        }
    }
}

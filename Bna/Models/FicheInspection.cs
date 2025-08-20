namespace Bna.Models
{
    public class FicheInspection
    {
        public int Id { get; set; }
        public DateTime Horodateur { get; set; }

        // Informations générales
        public TimeSpan HeureEntree { get; set; }
        public DateTime DateInspection { get; set; }
        public string CommandeTravaux { get; set; }
        public string Agent { get; set; }
        public string NomPrenom { get; set; }
        public string Telephone { get; set; }
        public string Immatriculation { get; set; }
        public string VIN { get; set; }
        public int Kilometrage { get; set; }
        public string Motorisation { get; set; }

        // Tour du véhicule
        public string PlaquesPolice { get; set; }
        public string VitresPareBrise { get; set; }
        public string BalaisEssuieGlace { get; set; }
        public string Eclairage { get; set; }
        public string Retroviseurs { get; set; }
        public string Pneus { get; set; }
        public string CommentaireTour { get; set; }

        // 40 points de contrôle
        public string ControleVehicule { get; set; }
        public string Commentaire40Points { get; set; }

        // Sous le capot
        public string ControleCapot { get; set; }
        public string CommentaireCapot { get; set; }

        // Sous le véhicule
        public string ControleSousVehicule { get; set; }
        public string CommentaireSousVehicule { get; set; }

        // Autres prestations
        public string AutresPrestations { get; set; }
        public string CommentairePrestations { get; set; }

        public int? IdRendezVous { get; set; }

        // Fichiers joints
        public string Images { get; set; }  // stocker chemins ou urls séparés par ";"
    }

}

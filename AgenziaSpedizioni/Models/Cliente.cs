using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AgenziaSpedizioni.Models
{
    public class Cliente
    {

        public int IDCliente { get; set; }

        [Display(Name = "Nome e cognome cliente o nominativo azienda")]
        [Required(ErrorMessage = "Il campo Nome/Nominativo è obbligatorio")]
        public string NomeNominativo { get; set; }

        public bool IsAzienda { get; set; }

        [Display(Name = "Codice fiscale cliente")]
        //[RegularExpression(@"\d{16}", ErrorMessage = "Codice fiscale non valido")]
        public string CodFisc { get; set; }

        [Display(Name = "P.Iva (Azienda)")]
        [RegularExpression(@"\d{11}", ErrorMessage = "Partita IVA non valida")]
        public string PIva { get; set; }
    }
}
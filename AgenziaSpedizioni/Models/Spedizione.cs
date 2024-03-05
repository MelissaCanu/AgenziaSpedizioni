using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AgenziaSpedizioni.Models
{
    public class Spedizione
    {
        public int IDSpedizione { get; set; }
        public int IDCliente { get; set; }

        //per definire formato gg/MM/yyyy e aggiungere validazione data uso DisplayFormat
        //e Remote insieme alle DataAnnotations
        [Display(Name = "Data di spedizione")]
        [Required(ErrorMessage = "La data di spedizione è obbligatoria")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Remote("IsValidData", "Spedizioni", HttpMethod = "POST", ErrorMessage = "La data di spedizione non può essere inferiore a oggi")]
        public DateTime DataSpedizione { get; set; }

        [Display(Name = "Peso(kg)")]
        [Required(ErrorMessage = "Il peso è obbligatorio")]
        public float Peso { get; set; }


        [Display(Name = "Nominativo del destinatario")]
        [StringLength(200, MinimumLength = 3,
            ErrorMessage = "Il nominativo del destinatario deve essere compreso tra 3 e 200 caratteri")]
        [Required(ErrorMessage = "Il nominativo del destinatario è obbligatorio")]
        public string NominativoDestinatario { get; set; }

        [Display(Name = "Costo della spedizione")]
        [Required(ErrorMessage = "Il costo della spedizione è obbligatorio")]
        public decimal CostoSpedizione { get; set; }

        [DisplayName("Inserisci la citta di destinazione")]
        [Required(ErrorMessage = "la destinazione e' obbligatoria")]
        [StringLength(50, MinimumLength = 3,
       ErrorMessage = "la citta deve essere compresa tra 3 e 50 caratteri")]
        public string CittaDestinazione { get; set; }

        [DisplayName("Inserisci l'indirizzo di destinazione")]
        [StringLength(200, MinimumLength = 3,
            ErrorMessage = "L'indirizzo di destinazione deve essere compreso tra 3 e 200 caratteri")]
        [Required(ErrorMessage = "l'indirizzo di destinazione e' obbligatorio")]
        public string IndirizzoDestinatario { get; set; }

        [DisplayName("Data di consegna prevista")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Remote("IsValidData2", "Spedizioni", HttpMethod = "POST", ErrorMessage = "La data di consegna non può essere inferiore a oggi")]
        public DateTime DataConsegnaPrevista { get; set; }
    }



}


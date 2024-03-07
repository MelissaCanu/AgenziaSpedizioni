using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace AgenziaSpedizioni.Models
{
    public class StatoSpedizione
    {
        public int IDStatoSpedizione { get; set; }

        //FK che collega StatoSpedizione a Spedizione. Ogni stato è collegato ad una spedizione (1 a molti) 
        //Poi creerò una nuova action nel controller SpedizioniController per visualizzare lo stato di una spedizione
        [Required]
        public int IDSpedizione { get; set; }

        [Required]
        [Display(Name = "Stato")] 
        public string Stato { get; set; }

        [Required]
        [Display(Name = "Luogo")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Il luogo deve essere compreso tra 3 e 200 caratteri")]
        public string Luogo { get; set; }

        [Display(Name = "Descrizione")]
        [StringLength(500, MinimumLength = 3, ErrorMessage = "La descrizione deve essere compresa tra 3 e 500 caratteri")]
        public string Descrizione { get; set; }

        [Required]
        [Display(Name = "Data e ora")]
        [DataType(DataType.DateTime)]
        public DateTime DataOraAggiornamento { get; set; }

    }
}
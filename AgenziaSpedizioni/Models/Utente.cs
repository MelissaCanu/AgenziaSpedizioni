using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AgenziaSpedizioni.Models
{
    //1. Creo la classe Utente con le proprietà IDUtente, Username e Password
    

    public class Utente
    {
        public int IDUtente { get; set; }

        [Required(ErrorMessage = "Inserire lo username")]
        [StringLength(50, MinimumLength =3, ErrorMessage = "Lo username deve essere compreso tra 3 e 50 caratteri")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Inserire la password")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "La password deve essere compresa tra 5 e 50 caratteri")]
        public string Password { get; set; }

        //Aggiungo la proprietà Ruolo per gestire i permessi dell'utente (es. admin, user)
        public string Ruolo { get; set; }
    }
}
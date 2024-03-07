using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgenziaSpedizioni.Models
{
    //classe per visualizzare lo stato di una spedizione con 
    public class VisualizzaStatoViewModel
    {
        public int IDSpedizione { get; set; }
        public string codFisc { get; set; }

        public string PIva { get; set; }

        //lista di stati di una spedizione perché la mia view VisualizzaStato si aspetta un modello di tipo VisualizzaStatoViewModel
        public List<StatoSpedizione> StatiSpedizione { get; set; }
    }
}
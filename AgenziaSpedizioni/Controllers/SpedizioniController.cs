using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using AgenziaSpedizioni.Models;


namespace AgenziaSpedizioni.Controllers
{
    public class SpedizioniController : Controller
    {

        // GET: Spedizione - view per inserire nuova spedizione
        public ActionResult NuovaSpedizione()
        {
            return View();
        }

        //POST: Spedizione - ricevo i dati inviati dal form, valido e salvo in db
        //se success reindirizzo a pagina di conferma
        [HttpPost]
        //ValidateAntiForgeryToken per evitare attacchi CSRF (Cross-Site Request Forgery)
        [ValidateAntiForgeryToken] //poi aggiungo @Html.AntiForgeryToken() nella view


        //Bind: specifico quali campi del modello posso ricevere dal form (per evitare attacchi di overposting)
        public ActionResult NuovaSpedizione([Bind(Include = "IDCliente,DataSpedizione,Peso,NominativoDestinatario,CostoSpedizione,CittaDestinazione,IndirizzoDestinatario,DataConsegnaPrevista")] Spedizione spedizione)
        {
            if (ModelState.IsValid)
            {
                //salvo i dati nel mio db
                string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
                //connection = null per evitare errori nel caso in cui la connessione non venga aperta
                SqlConnection connection = new SqlConnection(connectionString);
                try                
                {
                    connection.Open();
                    //inserisco i dati della spedizione nel db
                    SqlCommand cmd = new SqlCommand("INSERT INTO Spedizioni (IDCliente,DataSpedizione,Peso,NominativoDestinatario,CostoSpedizione,CittaDestinazione,IndirizzoDestinatario,DataConsegnaPrevista) VALUES (@IDCliente,@DataSpedizione,@Peso,@NominativoDestinatario,@CostoSpedizione,@CittaDestinazione,@IndirizzoDestinatario,@DataConsegnaPrevista)", connection);
                    cmd.Parameters.AddWithValue("@IDCliente", spedizione.IDCliente);
                    cmd.Parameters.AddWithValue("@DataSpedizione", spedizione.DataSpedizione);
                    cmd.Parameters.AddWithValue("@Peso", spedizione.Peso);
                    cmd.Parameters.AddWithValue("@NominativoDestinatario", spedizione.NominativoDestinatario);
                    cmd.Parameters.AddWithValue("@CostoSpedizione", spedizione.CostoSpedizione);
                    cmd.Parameters.AddWithValue("@CittaDestinazione", spedizione.CittaDestinazione);
                    cmd.Parameters.AddWithValue("@IndirizzoDestinatario", spedizione.IndirizzoDestinatario);
                    cmd.Parameters.AddWithValue("@DataConsegnaPrevista", spedizione.DataConsegnaPrevista);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    connection.Close();
                }




                //reindirizzo a pagina di conferma
                return RedirectToAction("SpedizioneSalvata");
            }
            //se il modello non è valido, torno alla view con i dati inseriti
            return View(spedizione);
        }


        //GET: Spedizione/SpedizioneSalvata - pagina di conferma per il salvataggio del cliente
        public ActionResult SpedizioneSalvata()
        {
            //tempdata per messaggio di conferma
            TempData["Message"] = "Spedizione salvata con successo!";
            //pagina di conferma per il salvataggio del cliente 
            return View();
        }


        //POST: Spedizione/IsValidData - controllo che la data sia maggiore a quella odierna 
        [HttpPost]

        public JsonResult IsValidData(DateTime DataSpedizione)
        {
            bool isValid = DataSpedizione >= DateTime.Today;
            return Json(isValid, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsValidData2(DateTime DataConsegnaPrevista)
        {
            bool isValid = DataConsegnaPrevista >= DateTime.Today;
            return Json(isValid, JsonRequestBehavior.AllowGet);
        }

    }
}
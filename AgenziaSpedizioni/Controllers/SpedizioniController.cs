using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AgenziaSpedizioni.Models;

//in questo controller gestisco le operazioni relative alle spedizioni: inserimento di una nuova spedizione, controllo della data di spedizione e di consegna prevista e stato di consegna
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

        //GET: Spedizione/AggiornaStato - visualizzo lo stato di una spedizione
        public ActionResult AggiornaStato()
        {
            //creo un nuovo oggetto StatoSpedizione 
            var statoSpedizione = new StatoSpedizione();

            //tempdata per messaggio di conferma
            TempData["SuccessMessage"] = "Stato della spedizione aggiornato con successo!";
            //tempdata per messaggio di errore
            TempData["ErrorMessage"] = "Errore nell'aggiornamento dello stato della spedizione!";
            //reindirizzo alla view StatoSpedizione
            return View(statoSpedizione);

        }

        //POST: Spedizione/AggiornaStato - ricevo i dati inviati dal form, valido e salvo in db per poi passare alla view StatoSpedizione
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AggiornaStato(StatoSpedizione statoSpedizione)
        {
            if (ModelState.IsValid)
            {
                //salvo i dati nel mio db
                string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
                SqlConnection connection = new SqlConnection(connectionString);
                try
                {
                    connection.Open();
                    //inserisco i dati dello stato della spedizione nel db
                    SqlCommand cmd = new SqlCommand("INSERT INTO StatoSpedizioni (IDSpedizione,Stato,Luogo,Descrizione,DataOraAggiornamento) VALUES (@IDSpedizione,@Stato,@Luogo,@Descrizione,@DataOraAggiornamento)", connection);
                    cmd.Parameters.AddWithValue("@IDSpedizione", statoSpedizione.IDSpedizione);
                    cmd.Parameters.AddWithValue("@Stato", statoSpedizione.Stato);
                    cmd.Parameters.AddWithValue("@Luogo", statoSpedizione.Luogo);
                    cmd.Parameters.AddWithValue("@Descrizione", statoSpedizione.Descrizione);
                    cmd.Parameters.AddWithValue("@DataOraAggiornamento", statoSpedizione.DataOraAggiornamento);
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

                //reindirizzo alla view AdminHome
                return RedirectToAction("AdminHome", "Home");
            }
            //se il modello non è valido, torno alla view con i dati inseriti
            return View(statoSpedizione);
        }


        //GET: Spedizione/VisualizzaStato - visualizzo lo stato di una spedizione

        public ActionResult VisualizzaStato()
        {
            var model = new VisualizzaStatoViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult VisualizzaStato(VisualizzaStatoViewModel model)
        {
            if (ModelState.IsValid)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
                SqlConnection connection = new SqlConnection(connectionString);
                //creo una lista di StatoSpedizione per contenere gli stati di una spedizione 
                List<StatoSpedizione> statiSpedizione = new List<StatoSpedizione>();

                try
                {
                    connection.Open();
                    //seleziono gli stati di una spedizione e li ordino per data di aggiornamento
                    SqlCommand cmd = new SqlCommand("SELECT * FROM StatoSpedizioni WHERE IDSpedizione = @IDSpedizione ORDER BY DataOraAggiornamento DESC", connection);
                    cmd.Parameters.AddWithValue("@IDSpedizione", model.IDSpedizione);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        StatoSpedizione stato = new StatoSpedizione
                        {
                            IDStatoSpedizione = reader.GetInt32(0),
                            IDSpedizione = reader.GetInt32(1),
                            Stato = reader.GetString(2),
                            Luogo = reader.GetString(3),
                            Descrizione = reader.GetString(4),
                            DataOraAggiornamento = reader.GetDateTime(5)
                        };

                        statiSpedizione.Add(stato);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    connection.Close();
                }

                if (!statiSpedizione.Any())
                {
                    // Mostro errore se non ci sono stati di spedizione
                    return HttpNotFound();
                }

                // Imposto la lista StatiSpedizione del modello con la lista statiSpedizione
                model.StatiSpedizione = statiSpedizione;

                // Restituisco  la vista con il modello contenente la lista di stati di spedizione
                return View("VisualizzaStato", model);
            }

            //se il model non è valido, torno alla view con i dati inseriti
            return View(model);
        }

        //QUERY CON CHIAMATE ASINCRONE

        // GET: Spedizioni/SpedizioniOggi
        public JsonResult SpedizioniOggi()
        {
            List<StatoSpedizione> spedizioniOggi = new List<StatoSpedizione>();
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM StatoSpedizioni WHERE Stato = 'In consegna'";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            spedizioniOggi.Add(new StatoSpedizione
                            {
                                IDSpedizione = reader.GetInt32(1),
                                Stato = reader.GetString(2),
                                Luogo = reader.GetString(3),
                                Descrizione = reader.GetString(4),
                                DataOraAggiornamento = reader.GetDateTime(5)
                            });
                        }
                    }
                }
            }

            return Json(spedizioniOggi, JsonRequestBehavior.AllowGet);
        }
 // GET: Spedizioni/SpedizioniInAttesa

        public JsonResult SpedizioniInAttesa()
        {
            int spedizioniInAttesa = 0;
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM StatoSpedizioni WHERE Stato = 'In attesa'";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    spedizioniInAttesa = (int)command.ExecuteScalar(); //execute scalar restituisce la prima colonna della prima riga del risultato della query
                }
            }

            return Json(spedizioniInAttesa, JsonRequestBehavior.AllowGet);
        }


        // GET: Spedizioni/SpedizioniPerCitta
        public JsonResult SpedizioniPerCitta()

            //creo un dizionario per contenere le spedizioni per città, con la città come chiave e il conteggio come valore
        {
            Dictionary<string, int> spedizioniPerCitta = new Dictionary<string, int>();
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT CittaDestinazione, COUNT(*) FROM Spedizioni GROUP BY CittaDestinazione";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Aggiungi ogni città e il relativo conteggio al dizionario
                            spedizioniPerCitta.Add(reader.GetString(0), reader.GetInt32(1));
                        }
                    }
                }
            }

            return Json(spedizioniPerCitta, JsonRequestBehavior.AllowGet);
        }

    }


}


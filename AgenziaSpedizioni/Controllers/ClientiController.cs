using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using AgenziaSpedizioni.Models;

namespace AgenziaSpedizioni.Controllers
{
    public class ClientiController : Controller
    {
        // GET: Clienti
        public ActionResult NuovoCliente()
        {
            return View();
        }

        // POST : Clienti/NuovoCliente
        [HttpPost]
        [ValidateAntiForgeryToken]
        //action NuovoCliente con bind per evitare overposting
        public ActionResult NuovoCliente([Bind(Include = "NomeNominativo, IsAzienda, CodFisc, PIva")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                //salvo i dati nel mio db
                string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
                SqlConnection connection = new SqlConnection(connectionString);
                try
                {
                    connection.Open();
                    //inserisco i dati del cliente nel db
                    //creo un oggetto SqlCommand per eseguire un'istruzione SQL sul database
                    SqlCommand cmd;
                    //query diversa a seconda che il cliente sia un'azienda o una persona fisica
                    if (cliente.IsAzienda)
                    {
                        cmd = new SqlCommand("INSERT INTO Clienti (NomeNominativo, IsAzienda, PIva) VALUES (@NomeNominativo, @IsAzienda, @PIva)", connection);
                        cmd.Parameters.AddWithValue("@PIva", cliente.PIva);
                    }
                    else
                    {
                        cmd = new SqlCommand("INSERT INTO Clienti (NomeNominativo, IsAzienda, CodFisc) VALUES (@NomeNominativo, @IsAzienda, @CodFisc)", connection);
                        cmd.Parameters.AddWithValue("@CodFisc", cliente.CodFisc);
                    }
                    cmd.Parameters.AddWithValue("@NomeNominativo", cliente.NomeNominativo);
                    cmd.Parameters.AddWithValue("@IsAzienda", cliente.IsAzienda);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw; // Rilancia l'eccezione
                }
                finally
                {
                    connection.Close();
                }
                return RedirectToAction("ClienteSalvato");
            }
            return View(cliente);
        }

        //GET: Clienti/ClienteSalvato
        public ActionResult ClienteSalvato()
        {
            //Tempdata per passare messaggio di conferma alla view
            TempData["Message"] = "Cliente salvato con successo!";
            return View();
        }

    }
}

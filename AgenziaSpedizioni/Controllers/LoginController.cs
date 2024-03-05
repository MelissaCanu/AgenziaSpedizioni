using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//System.Web.SessionState serve per utilizzare la sessione
using System.Web.SessionState;
//System.Web.Helpers serve per utilizzare la classe Crypto
using System.Web.Helpers;
//System.Data.SqlClient serve per utilizzare le classi per la gestione dei dati
using System.Data.SqlClient;
//System.Data serve per utilizzare le classi per la gestione dei dati
using System.Data;
//System.Configuration serve per leggere le informazioni dal file Web.config
using System.Configuration;
//System.Web.Configuration serve per leggere le informazioni dal file Web.config
using System.Web.Configuration;
//Questo namespace serve a gestire la crittografia
using System.Web.Security;
//Aggiungo namespace per il modello Utente
using AgenziaSpedizioni.Models;


//Creo il controller LoginController per gestire il login dell'utente
//Dopo che un utente si è autenticato con successo, il codice utilizza il CustomRoleProvider per ottenere i ruoli dell'utente.
//Se l'utente ha il ruolo "admin", viene reindirizzato alla home page dell'admin. Altrimenti, viene reindirizzato alla home page dell'utente.


namespace AgenziaSpedizioni.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Models.Utente utente)
        {
            if (ModelState.IsValid)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
                //connection = null per evitare errori nel caso in cui la connessione non venga aperta
                SqlConnection connection = null;
                try
                {
                    connection = new SqlConnection(connectionString);
                    connection.Open();
                    //seleziono l'utente con lo username e la password inseriti
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Utenti WHERE Username = @Username AND Password = @Password", connection);
                    cmd.Parameters.AddWithValue("@Username", utente.Username);
                    cmd.Parameters.AddWithValue("@Password", utente.Password);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {   
                        //Se l'utente esiste, viene creato un cookie di autenticazione con il nome dell'utente
                        FormsAuthentication.SetAuthCookie(utente.Username, false);
                        //Viene utilizzato il CustomRoleProvider per ottenere i ruoli dell'utente
                        RoleProvider roleProvider = Roles.Provider;
                        //Se l'utente ha il ruolo "admin", viene reindirizzato alla home page dell'admin
                        string[] roles = roleProvider.GetRolesForUser(utente.Username);
                        if (roles.Contains("admin"))
                        {
                            return RedirectToAction("AdminHome", "Home");
                        }
                        else //Altrimenti, viene reindirizzato alla home page dell'utente
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {   //Se l'utente non esiste, viene visualizzato un messaggio di errore
                        ModelState.AddModelError("", "Username o password non validi.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                finally
                {   //Chiudo la connessione se è stata aperta con successo
                    if (connection != null)
                    {
                        connection.Close();
                    }
                }
            }

            //ritorno la vista con i dati dell'utente
            return View(utente);
        }

    }
}


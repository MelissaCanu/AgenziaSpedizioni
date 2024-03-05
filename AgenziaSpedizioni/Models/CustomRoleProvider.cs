using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;


//ROLE PROVIDER: è un servizio che consente di gestire i ruoli degli utenti. 
//Un ruolo è un insieme di autorizzazioni che possono essere assegnate a un utente.
//Per utilizzare il Role Provider è necessario implementare la classe CustomRoleProvider che eredita da RoleProvider e implementare i metodi richiesti dall'interfaccia RoleProvider.
//In questo caso implemento solo il metodo GetRolesForUser che restituisce i ruoli dell'utente.
//Per utilizzare il Role Provider è necessario configurare il file Web.config aggiungendo la sezione <roleManager> e specificare la classe CustomRoleProvider come provider dei ruoli.
namespace AgenziaSpedizioni.Models
{ //Creo la classe CustomRoleProvider 
    public class CustomRoleProvider : RoleProvider
    {
        public override string[] GetRolesForUser(string username)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT Ruolo FROM Utenti WHERE Username = @Username", connection))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<string> roles = new List<string>();
                        while (reader.Read())
                        {
                            roles.Add(reader["Ruolo"].ToString());
                        }
                        return roles.ToArray();
                    }
                    else
                    {
                        return new string[] { };
                    }
                }
            }
        }

        //Implemento i metodi dell'interfaccia RoleProvider che non verranno utilizzati, è obbligatorio implementarli ma non è necessario scriverne il codice.
        public override void AddUsersToRoles(string[] usernames, string[] roleNames) { throw new NotImplementedException(); }
        public override string ApplicationName { get { throw new NotImplementedException(); } set { throw new NotImplementedException(); } }
        public override void CreateRole(string roleName) { throw new NotImplementedException(); }
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole) { throw new NotImplementedException(); }
        public override string[] FindUsersInRole(string roleName, string usernameToMatch) { throw new NotImplementedException(); }
        public override string[] GetAllRoles() { throw new NotImplementedException(); }
        public override string[] GetUsersInRole(string roleName) { throw new NotImplementedException(); }
        public override bool IsUserInRole(string username, string roleName) { throw new NotImplementedException(); }
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames) { throw new NotImplementedException(); }
        public override bool RoleExists(string roleName) { throw new NotImplementedException(); }
    }
    }
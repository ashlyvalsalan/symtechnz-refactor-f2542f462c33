using refactor_this.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Http;

namespace refactor_this.Controllers
{
    public class AccountController : ApiController
    {
        //API call GET(id)
        [HttpGet, Route("api/Accounts/{id}")]
        public IHttpActionResult GetById(Guid id)
        {
            try
            {
                using (var connection = Helpers.NewConnection())
                {
                    return Ok(Get(id));
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
       
        }
        //API call GET()
        [HttpGet, Route("api/Accounts")]
        public IHttpActionResult Get()
        {
            try
            {
                using (var connection = Helpers.NewConnection())
                {
                    SqlCommand command = new SqlCommand($"select Id from Accounts", connection);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    var accounts = new List<Account>();
                    while (reader.Read())
                    {
                        var id = Guid.Parse(reader["Id"].ToString());
                        var account = Get(id);
                        accounts.Add(account);
                    }
                    connection.Close();
                    return Ok(accounts);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return null;
            }
     
        }
        //Getting the account details by paasing the ID
        private Account Get(Guid id)
        {
            try
            {
                using (var connection = Helpers.NewConnection())
                {
                    SqlCommand command = new SqlCommand($"select * from Accounts where Id = '{id}'", connection);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (!reader.Read())
                        throw new ArgumentException();

                    var account = new Account(id);
                    account.Name = reader["Name"].ToString();
                    account.Number = reader["Number"].ToString();
                    account.Amount = float.Parse(reader["Amount"].ToString());
                    connection.Close();
                    return account;
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return null;
            }

        }
        //API call Post
        [HttpPost, Route("api/Accounts")]
        public IHttpActionResult Add(Account account)
        {
            try
            {
                account.Save();
                return Ok();

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return null;
            }
            
        }
        //API call Put
        [HttpPut, Route("api/Accounts/{id}")]
        public IHttpActionResult Update(Guid id, Account account)
        {
            try
            {
                var existing = Get(id);
                existing.Name = account.Name;
                existing.Save();
                return Ok();

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return null;
            }
         
        }
        //API call Delete
        [HttpDelete, Route("api/Accounts/{id}")]
        public IHttpActionResult Delete(Guid id)
        {
            try
            {
                var existing = Get(id);
                existing.Delete();
                return Ok();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return null;
            }
           
        }
    }
}
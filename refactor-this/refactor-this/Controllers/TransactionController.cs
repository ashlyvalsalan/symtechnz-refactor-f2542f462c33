using refactor_this.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Http;

namespace refactor_this.Controllers
{
    public class TransactionController : ApiController
    {
        [HttpGet, Route("api/Accounts/{id}/Transactions")]
        public IHttpActionResult GetTransactions(Guid id)
        {
            try
            {
                //Getting the previous transaction details
                using (var connection = Helpers.NewConnection())
                {
                    SqlCommand command = new SqlCommand($"select Amount, Date from Transactions where AccountId = '{id}'", connection);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    var transactions = new List<Transaction>();
                    while (reader.Read())
                    {
                        var amount = (float)reader.GetDouble(0);
                        var date = reader.GetDateTime(1);
                        transactions.Add(new Transaction(amount, date));
                    }
                    connection.Close();
                    return Ok(transactions);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

        }

        [HttpPost, Route("api/Accounts/{id}/Transactions")]
        public IHttpActionResult AddTransaction(Guid id, Transaction transaction)
        {
            try
            {
               
                using (var connection = Helpers.NewConnection())
                {
                    //updating the account
                    SqlCommand command = new SqlCommand($"update Accounts set Amount = Amount + {transaction.Amount} where Id = '{id}'", connection);
                    connection.Open();
                    if (command.ExecuteNonQuery() != 1)
                        return BadRequest("Could not update account amount");
                    //Adding a new transacction
                    command = new SqlCommand($"INSERT INTO Transactions (Id, Amount, Date, AccountId) VALUES ('{Guid.NewGuid()}', {transaction.Amount}, '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', '{id}')", connection);
                    if (command.ExecuteNonQuery() != 1)
                        return BadRequest("Could not insert the transaction");
                    connection.Close();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

        }
    }
}
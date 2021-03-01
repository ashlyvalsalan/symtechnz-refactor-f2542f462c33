using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace refactor_this.Models
{
    public class Account
    {
        private bool isNew;

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Number { get; set; }

        public float Amount { get; set; }

        public Account()
        {
            isNew = true;
        }

        public Account(Guid id)
        {
            isNew = false;
            Id = id;
        }
        //Saving the account detsils
        public void Save()
        {
            try
            {
                using (var connection = Helpers.NewConnection())
                {                   
                    SqlCommand command;
                    if (isNew)
                        //creating a new account
                        command = new SqlCommand($"insert into Accounts (Id, Name, Number, Amount) values ('{Guid.NewGuid()}', '{Name}', {Number}, 0)", connection);
                    else
                        //updating  a existing account
                        command = new SqlCommand($"update Accounts set Name = '{Name}' where Id = '{Id}'", connection);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
        //Delete an account
        public void Delete()
        {
            try
            {
                using (var connection = Helpers.NewConnection())
                {
                    //delete the existing account
                    SqlCommand command = new SqlCommand($"delete from Accounts where Id = '{Id}'", connection);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
      
        }
    }
}
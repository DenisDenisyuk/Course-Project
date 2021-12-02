using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace WebApp2015_5Var8.DAO
{
    public class DAO
    {
        // общий класс, отвечающий за соединение с базой данных
        private const string ConnectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog = demobd;Integrated Security = True;Pooling=False";
                                            //    Data Source = LOCALHOST\SQLEXPRESS;Initial Catalog = demobd; Integrated Security = True
        protected SqlConnection Connection { get; set; }
        public void Connect()
        {
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
        }
        public void Disconnect()
        {
            Connection.Close();
        }

    }
}
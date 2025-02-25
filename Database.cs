using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace GerenciadorDePedidos
{
    public class Database
    {
        public string connection = "Data Source=orders.db;";

        public void CreateDatabase()
        {
            try
            {
                using (var conn = new SqliteConnection(connection))
                {
                    conn.Open();
                    string sqlClientes = "CREATE TABLE IF NOT EXISTS clients (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT NOT NULL, email TEXT NOT NULL UNIQUE)";
                    using (var cmd = new SqliteCommand(sqlClientes, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    string sqlProducts = "CREATE TABLE IF NOT EXISTS products (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT NOT NULL, quant INTEGER NOT NULL, price REAL NOT NULL)";
                    using (var cmd = new SqliteCommand(sqlProducts, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    string sqlOrders = "CREATE TABLE IF NOT EXISTS orders (id INTEGER PRIMARY KEY AUTOINCREMENT, clientId INTEGER NOT NULL, data TEXT NOT NULL, FOREIGN KEY (clientId) REFERENCES clients(id))";
                    using (var cmd = new SqliteCommand(sqlOrders, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    string sqlOrderItems = "CREATE TABLE IF NOT EXISTS orderItems (id INTEGER PRIMARY KEY AUTOINCREMENT, orderId INTEGER NOT NULL, productId INTEGER NOT NULL, quant INTEGER NOT NULL, totalPrice REAL NOT NULL, FOREIGN KEY (OrderId) REFERENCES orders(id), FOREIGN KEY (productId) REFERENCES products(id))";
                    using (var cmd = new SqliteCommand(sqlOrderItems, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

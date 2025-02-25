using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace GerenciadorDePedidos
{
    public class Orders
    {
        Database database = new Database();
        Clients clients = new Clients();
        public void AddOrderToClient()
        {
            clients.ListClients();
            Console.Write("\nInforme o ID do cliente para adicionar o pedido: ");
            int clientId = int.Parse(Console.ReadLine());
            Console.Write("Data do pedido: ");
            string orderDate = Console.ReadLine();

            try
            {
                using (var conn = new SqliteConnection(database.connection))
                {
                    conn.Open();
                    string sql = "INSERT INTO orders (clientId, data) VALUES (@clientId, @data)";
                    using (var cmd = new SqliteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@clientId", clientId);
                        cmd.Parameters.AddWithValue("@data", orderDate);
                        cmd.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("\nPedido adicionado com sucesso!\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void AddProductToOrder()
        {
            ListOrders();
            Console.Write("\nInforme o ID do pedido para adicionar o produto: ");
            int orderId = int.Parse(Console.ReadLine());

            Products products = new Products();
            products.ListProducts();
            Console.Write("\nID do produto: ");
            int productId = int.Parse(Console.ReadLine());

            Console.Write("Quantidade do produto: ");
            int quantity = int.Parse(Console.ReadLine());

            decimal productPrice = 0;
            int availableQuantity = 0;

            try
            {
                using (var conn = new SqliteConnection(database.connection))
                {
                    conn.Open();
                    string sql = "SELECT price, quant FROM products WHERE id = @productId";
                    using (var cmd = new SqliteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@productId", productId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                productPrice = reader.GetDecimal(0);
                                availableQuantity = reader.GetInt32(1);
                            }
                            else
                            {
                                Console.WriteLine("\nProduto não encontrado.\n");
                                return;
                            }
                        }
                    }

                    if (availableQuantity < quantity)
                    {
                        Console.WriteLine("\nQuantidade insuficiente em estoque.\n");
                        return;
                    }

                    decimal totalPrice = productPrice * quantity;

                    string insertItemSql = "INSERT INTO orderItems (orderId, productId, quant, totalPrice) VALUES (@orderId, @productId, @quant, @totalPrice)";
                    using (var cmd = new SqliteCommand(insertItemSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@orderId", orderId);
                        cmd.Parameters.AddWithValue("@productId", productId);
                        cmd.Parameters.AddWithValue("@quant", quantity);
                        cmd.Parameters.AddWithValue("@totalPrice", totalPrice);
                        cmd.ExecuteNonQuery();
                    }

                    string updateProductSql = "UPDATE products SET quant = quant - @quantity WHERE id = @productId";
                    using (var cmd = new SqliteCommand(updateProductSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@quantity", quantity);
                        cmd.Parameters.AddWithValue("@productId", productId);
                        cmd.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("\nProduto adicionado ao pedido!\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void ListOrdersForClient()
        {
            clients.ListClients();
            Console.Write("\nInforme o ID do cliente para listar os pedidos: ");
            int clientId = int.Parse(Console.ReadLine());

            try
            {
                using (var conn = new SqliteConnection(database.connection))
                {
                    conn.Open();

                    string sql = "SELECT id, data FROM orders WHERE clientId = @clientId";
                    using (var cmd = new SqliteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@clientId", clientId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                Console.WriteLine("\nNenhum pedido encontrado para este cliente.\n");
                                return;
                            }

                            Console.WriteLine("\nPedidos do cliente:");

                            while (reader.Read())
                            {
                                int orderId = reader.GetInt32(0);
                                string orderDate = reader.GetString(1);

                                decimal totalOrderValue = 0;
                                string productSql = "SELECT productId, quant, totalPrice FROM orderItems WHERE orderId = @orderId";
                                using (var productCmd = new SqliteCommand(productSql, conn))
                                {
                                    productCmd.Parameters.AddWithValue("@orderId", orderId);
                                    using (var productReader = productCmd.ExecuteReader())
                                    {
                                        Console.WriteLine($"Pedido ID: {orderId}, Data: {orderDate}");
                                        while (productReader.Read())
                                        {
                                            int productId = productReader.GetInt32(0);
                                            int quantity = productReader.GetInt32(1);
                                            decimal totalPrice = productReader.GetDecimal(2);
                                            totalOrderValue += totalPrice;

                                            string productName = "";
                                            string nameSql = "SELECT name FROM products WHERE id = @productId";
                                            using (var nameCmd = new SqliteCommand(nameSql, conn))
                                            {
                                                nameCmd.Parameters.AddWithValue("@productId", productId);
                                                using (var nameReader = nameCmd.ExecuteReader())
                                                {
                                                    if (nameReader.Read())
                                                    {
                                                        productName = nameReader.GetString(0);
                                                    }
                                                }
                                            }

                                            Console.WriteLine($"Produto: {productName}, Quantidade: {quantity}, Total: R${totalPrice:C}");
                                        }
                                    }
                                }

                                Console.WriteLine($"Valor Total do Pedido {orderId}: R${totalOrderValue:C}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void ListOrders()
        {
            try
            {
                using (var conn = new SqliteConnection(database.connection))
                {
                    conn.Open();

                    string sql = @"SELECT o.id AS OrderId, o.data AS OrderDate, c.name AS ClientName FROM orders o JOIN clients c ON o.clientId = c.id ORDER BY o.id";

                    using (var cmd = new SqliteCommand(sql, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                Console.WriteLine("\nNenhum pedido encontrado.\n");
                                return;
                            }

                            Console.WriteLine("\nPedidos:");

                            while (reader.Read())
                            {
                                int orderId = reader.GetInt32(0);
                                string orderDate = reader.GetString(1);
                                string clientName = reader.GetString(2);

                                Console.WriteLine($"ID: {orderId}, Data: {orderDate}, Cliente: {clientName}");
                            }
                        }
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

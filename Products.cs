using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace GerenciadorDePedidos
{
    public class Products
    {
        Database database = new Database();
        public int id { get; private set; }
        public string name { get; private set; }
        public int quant { get; private set; }
        public decimal price { get; private set; }

        public void AddProduct()
        {
            Console.Write("\nNome do produto: ");
            name = Console.ReadLine();
            Console.Write("Quantidade: ");
            quant = int.Parse(Console.ReadLine());
            Console.Write("Preço: R$");
            price = decimal.Parse(Console.ReadLine());

            try
            {
                using (var conn = new SqliteConnection(database.connection))
                {
                    conn.Open();
                    string sql = "INSERT INTO products (name, quant, price) VALUES (@name, @quant, @price)";
                    using (var cmd = new SqliteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@quant", quant);
                        cmd.Parameters.AddWithValue("@price", price);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("\nProduto adicionado com sucesso!\n");
                        }
                        else
                        {
                            Console.WriteLine("\nErro ao adicionar um produto!\n");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void DeleteProduct()
        {
            ListProducts();
            Console.Write("\nInforme o ID do produto a ser removido: ");
            int productId = int.Parse(Console.ReadLine());

            try
            {
                using (var conn = new SqliteConnection(database.connection))
                {
                    conn.Open();
                    string sql = "DELETE FROM products WHERE id = @id";
                    using (var cmd = new SqliteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", productId);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("\nProduto removido com sucesso!\n");
                        }
                        else
                        {
                            Console.WriteLine("\nProduto não encontrado.\n");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public void UpdateProduct()
        {
            ListProducts();
            Console.Write("\nInforme o ID do produto a ser atualizado: ");
            int clientId = int.Parse(Console.ReadLine());
            Console.Write("Informe a quantidade do produto: ");
            int newQuant = int.Parse(Console.ReadLine());
            Console.Write("Informe o novo preço do produto: R$");
            decimal newPrice = decimal.Parse(Console.ReadLine());

            try
            {
                using (var conn = new SqliteConnection(database.connection))
                {
                    conn.Open();
                    string sql = "UPDATE clients SET quant = @quant, price = @price WHERE id = @id";
                    using (var cmd = new SqliteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", clientId);
                        cmd.Parameters.AddWithValue("@quant", newQuant);
                        cmd.Parameters.AddWithValue("@price", newPrice);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("\nProduto atualizado com sucesso!\n");
                        }
                        else
                        {
                            Console.WriteLine("\nProduto não encontrado.\n");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public void ListProducts()
        {

            try
            {
                using (var conn = new SqliteConnection(database.connection))
                {
                    conn.Open();
                    string sql = "SELECT id, name, quant, price FROM products";
                    using (var cmd = new SqliteCommand(sql, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            Console.WriteLine("\nProdutos cadastrados:");
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                string name = reader.GetString(1);
                                int email = reader.GetInt32(2);
                                decimal price = reader.GetDecimal(3);
                                Console.WriteLine($"ID: {id}, Nome: {name}, Quantidade: {email}, Preço: R${price}");
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
        public void ShowMostAddedProduct()
        {

            try
            {
                using (var conn = new SqliteConnection(database.connection))
                {
                    conn.Open();

                    string sql = @"SELECT p.name, SUM(oi.quant) AS totalQuantity FROM orderItems oi JOIN products p ON oi.productId = p.id GROUP BY p.name ORDER BY totalQuantity DESC LIMIT 1";

                    using (var cmd = new SqliteCommand(sql, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string productName = reader.GetString(0);
                                int totalQuantity = reader.GetInt32(1);
                                Console.WriteLine($"\nO produto mais vendido é: {productName}");
                                Console.WriteLine($"Quantidade total vendida: {totalQuantity}");
                            }
                            else
                            {
                                Console.WriteLine("\nNão há produtos registrados nos pedidos.\n");
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
using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace GerenciadorDePedidos
{
    public class Clients
    {
        Database database = new Database();
        public int id { get; private set; }
        public string name { get; private set; }
        public string email { get; private set; }

        public void AddClient()
        {
            Console.Write("\nNome do cliente: ");
            name = Console.ReadLine();
            Console.Write("Email do cliente: ");
            email = Console.ReadLine();

            try
            {
                using (var conn = new SqliteConnection(database.connection))
                {
                    conn.Open();
                    string sql = "INSERT INTO clients (name, email) VALUES (@name, @email)";
                    using (var cmd = new SqliteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("\nCliente adicionado com sucesso!\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void DeleteClient()
        {
            ListClients();
            Console.Write("\nInforme o ID do cliente a ser deletado: ");
            int clientId = int.Parse(Console.ReadLine());

            try
            {
                using (var conn = new SqliteConnection(database.connection))
                {
                    conn.Open();
                    string sql = "DELETE FROM clients WHERE id = @id";
                    using (var cmd = new SqliteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", clientId);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("\nCliente deletado com sucesso!\n");
                        }
                        else
                        {
                            Console.WriteLine("\nCliente não encontrado.\n");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void UpdateClient()
        {
            ListClients();
            Console.Write("\nInforme o ID do cliente a ser atualizado: ");
            int clientId = int.Parse(Console.ReadLine());

            Console.Write("Informe o novo email do cliente: ");
            string newEmail = Console.ReadLine();

            try
            {
                using (var conn = new SqliteConnection(database.connection))
                {
                    conn.Open();
                    string sql = "UPDATE clients SET email = @email WHERE id = @id";
                    using (var cmd = new SqliteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", clientId);
                        cmd.Parameters.AddWithValue("@email", newEmail);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("\nCliente atualizado com sucesso!\n");
                        }
                        else
                        {
                            Console.WriteLine("\nCliente não encontrado.\n");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void ListClients()
        {
            try
            {
                using (var conn = new SqliteConnection(database.connection))
                {
                    conn.Open();
                    string sql = "SELECT id, name, email FROM clients";
                    using (var cmd = new SqliteCommand(sql, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            Console.WriteLine("\nClientes cadastrados:");
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                string name = reader.GetString(1);
                                string email = reader.GetString(2);
                                Console.WriteLine($"ID: {id}, Nome: {name}, Email: {email}");
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
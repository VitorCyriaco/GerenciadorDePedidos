namespace GerenciadorDePedidos
{
    class Program
    {

        static void Main(string[] args)
        {
            Database database = new Database();
            database.CreateDatabase();

            while (true)
            {
                Console.WriteLine("\nEscolha uma opção");
                Console.WriteLine("1- Clientes\n2- Produtos\n3- Pedidos\n0- Sair");
                Console.Write("\nOpção: ");

                int opc = int.Parse(Console.ReadLine());

                switch (opc) 
                {
                    case 0:
                        return;
                    case 1:
                        MenuClient();
                        break;
                    case 2:
                        MenuProducts();
                        break;
                    case 3:
                        MenuOrder();
                        break;
                    default:
                        Console.WriteLine("\nOpção inválida!\n");
                        break;
                }
            }
        }

        static void MenuClient()
        {
            Clients clients = new Clients();

            while (true)
            {
                Console.WriteLine("\nO que deseja fazer?\n");
                Console.WriteLine("1- Cadastrar cliente\n2- Atualizar email\n3- Listar clientes\n4- Remover cliente\n0- Voltar");
                Console.Write("\nOpção: ");
                int opcC = int.Parse(Console.ReadLine());

                switch (opcC)
                {
                    case 1:
                        clients.AddClient();
                        break;
                    case 2:
                        clients.UpdateClient();
                        break;
                    case 3:
                        clients.ListClients();
                        break;
                    case 4:
                        clients.DeleteClient();
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("\nOpção inválida!\n");
                        break;
                }
            }
        }

        static void MenuProducts()
        {
            Products products = new Products();
            while (true)
            {
                Console.WriteLine("\nO que deseja fazer?\n");
                Console.WriteLine("1- Cadastrar produto\n2- Atualizar produto\n3- Listar produtos\n4- Produto mais vendido\n5- Remover produto\n0- Voltar");
                Console.Write("\nOpção: ");
                int opcC = int.Parse(Console.ReadLine());

                switch (opcC)
                {
                    case 1:
                        products.AddProduct();
                        break;
                    case 2:
                        products.UpdateProduct();
                        break;
                    case 3:
                        products.ListProducts();
                        break;
                    case 4:
                        products.ShowMostAddedProduct();
                        break;
                    case 5:
                        products.DeleteProduct();
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("\nOpção inválida!\n");
                        break;
                }
            }
        }
        static void MenuOrder()
        {
            Orders orders = new Orders();

            while (true)
            {
                Console.WriteLine("\nO que deseja fazer?\n");
                Console.WriteLine("1- Adicionar pedido\n2- Adicionar produto ao pedido\n3- Listar pedidos\n0- Voltar");
                Console.Write("\nOpção: ");
                int opcC = int.Parse(Console.ReadLine());

                switch (opcC)
                {
                    case 1:
                        orders.AddOrderToClient();
                        break;
                    case 2:
                        orders.AddProductToOrder();
                        break;
                    case 3:
                        orders.ListOrdersForClient();
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("\nOpção inválida!\n");
                        break;
                }
            }
        }
    }
}
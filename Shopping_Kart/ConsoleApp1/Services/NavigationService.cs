using Serilog;

namespace Shopping_Kart.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IProductService _productService;
        private readonly ITransactionService _transactionService;
        public NavigationService(IProductService productService, ITransactionService transactionService)
        {
            _productService = productService;
            _transactionService = transactionService;
        }
        public void Run()
        {

            bool endRun = false;
            try
            {
                while (endRun == false)
                {
                    Console.Clear();
                    Console.WriteLine("Which function is required");
                    Console.WriteLine("[F1] Transaction");
                    Console.WriteLine("[F2] Add Product");
                    Console.WriteLine("[F3] Remove Product");
                    //todo add list, update
                    Console.WriteLine("[Esc] Quit");

                    var input = Console.ReadKey();

                    switch (input.Key)
                    {
                        case ConsoleKey.F1:
                            _transactionService.NewTransaction();
                            break;
                        case ConsoleKey.F2:
                            _productService.AddToDB();
                            break;
                        case ConsoleKey.F3:
                            _productService.RemoveFromDB();
                            break;
                        case ConsoleKey.Escape:
                            endRun = true;
                            break;
                    }

                }


            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message, ex);
                Console.WriteLine($"An error has occured: {ex.Message}");
            }
        }

    }
}

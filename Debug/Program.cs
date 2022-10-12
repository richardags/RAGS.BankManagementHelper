using BankManagementHelper;

namespace Debug
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BankManagementNS bank = new();
            bank.SetAmount(10); //obligatory - fixed hand/initial amount
            bank.type = BankManagementNS.Strategy.Soros; //obligatory - strategy type
            bank.SetSoros(2); //obligatory - configuration

            //example for Soros

            Console.WriteLine(bank.GetAmount);

            bank.Win(10); //specify profit earned

            Console.WriteLine(bank.GetAmount);

            bank.Loss();

            Console.WriteLine(bank.GetAmount);

            bank.Win(10);

            Console.WriteLine(bank.GetAmount);

            bank.Loss();

            Console.WriteLine(bank.GetAmount);

            bank.Loss();

            Console.WriteLine(bank.GetAmount);

            bank.Loss();

            Console.WriteLine(bank.GetAmount);

            bank.Loss();

            Console.WriteLine(bank.GetAmount);

            Console.ReadKey();
        }
    }
}
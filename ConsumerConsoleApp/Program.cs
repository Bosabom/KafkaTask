using System;
using Confluent.Kafka;
using System.Threading;
namespace Consumer_console_app_
{
    class Program
    {
        static void Main(string[] args)
        {
            Consumer consumer = new Consumer();
            Console.WriteLine("1 - Length;\n2 - Wrong symblols;\n3 - Step time;\n4 - Exit");

            int choice = Convert.ToInt32(Console.ReadLine());
            do
            {
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Enter the restriction on future messages length(number)");
                        string input_length = Console.ReadLine();
                        consumer.ProduceLimitations("Length", input_length);

                        Console.WriteLine("Choose again");
                        choice = Convert.ToInt32(Console.ReadLine());
                        break;
                    case 2:
                        Console.WriteLine("Enter the sequence of specific characters");
                        string input_spec_symbols = Console.ReadLine();
                        var str_without_spaces = input_spec_symbols.Replace(" ", string.Empty);
                        consumer.ProduceLimitations("Specific symbols", str_without_spaces);

                        Console.WriteLine("Choose again");
                        choice = Convert.ToInt32(Console.ReadLine());
                        break;
                    case 3:
                        Console.WriteLine("Enter step time(in seconds):");
                        string input_steptime = Console.ReadLine();
                        consumer.ProduceLimitations("Step time", input_steptime);

                        Console.WriteLine("Choose again");
                        choice = Convert.ToInt32(Console.ReadLine());
                        break;

                    default:
                        break;
                }
            } while (choice != 4);

            Console.WriteLine("Starting consumer(press Ctrl+C to stop and exit)");
            consumer.ConsumeMessages();

        }

    }   
}


using System;
using System.Threading;
using System.Windows;

namespace WpfLab2
{
    public partial class MainWindow : Window
    {
        private static Mutex mutex = new Mutex();

        private static int[] dataArray = new int[10];

        public MainWindow()
        {
            InitializeComponent();

            Thread thread1 = new Thread(ModifyArray);
            Thread thread2 = new Thread(FindMaxValue);

            thread1.Start();
            thread2.Start();
        }

        private static void ModifyArray()
        {
            Random random = new Random();
            for (int i = 0; i < dataArray.Length; i++)
            {
                mutex.WaitOne();
                dataArray[i] += random.Next(1, 10);
                mutex.ReleaseMutex();
            }
            Console.WriteLine("Массив после модификации: " + string.Join(", ", dataArray));
        }

        private static void FindMaxValue()
        {
            Thread.Sleep(100);

            mutex.WaitOne();
            int maxValue = int.MinValue;
            foreach (int value in dataArray)
            {
                if (value > maxValue)
                {
                    maxValue = value;
                }
            }
            mutex.ReleaseMutex();

            Console.WriteLine("Максимальное значение в массиве: " + maxValue);
        }
    }
}

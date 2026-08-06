namespace LoopThroughArrayWithoutLoopingAndWithoutArray
{
    internal unsafe class Program
    {
        static int ArraySummation(int* start, int* end)
        {
            int total;
            total = 0;

        start:
            total += *start;
            start++;
            if (start != end) goto start;
            goto end;

        end:
            Console.WriteLine("Total: " + total);
            return total;
        }
        static void Main(string[] args)
        {
            int[] arr = { 6, 7, 4, 1 };
            fixed (int* start = arr)
            {
                int* end = start + arr.Length;
                Console.WriteLine(ArraySummation(start, end));
            }
        }
    }
}
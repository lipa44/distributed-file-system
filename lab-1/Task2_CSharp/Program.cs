using Microsoft.FSharp.Collections;
using NUnit.Framework;

namespace Task2_CSharp;

public class Program
{
    public static void Main()
    {
        List<int> arr = new () { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };

        var HrrrusticsList = ListModule.OfSeq(arr);

        Console.WriteLine(nameof(Hrrrustic.stupidSort));
        foreach (var value in Hrrrustic.stupidSort(HrrrusticsList)) 
            Console.Write($" -> {value}");

        Console.WriteLine("\n\n" + nameof(Hrrrustic.reverse));
        foreach (var value in Hrrrustic.reverse(HrrrusticsList)) 
            Console.Write($" -> {value}");

        Console.WriteLine("\n\n" + nameof(Hrrrustic.skip));
        foreach (var value in Hrrrustic.skip(HrrrusticsList, 1)) 
            Console.Write($" -> {value}");

        Console.WriteLine("\n\n" + nameof(Hrrrustic.take));
        foreach (var value in Hrrrustic.take(HrrrusticsList, 2)) 
            Console.Write($" -> {value}");

        Console.WriteLine("\n\n" + nameof(Hrrrustic.evenOnly));
        foreach (var value in Hrrrustic.evenOnly(HrrrusticsList)) 
            Console.Write($" -> {value}");

        Console.WriteLine("\n\n" + nameof(Hrrrustic.oddOnly));
        foreach (var value in Hrrrustic.oddOnly(HrrrusticsList)) 
            Console.Write($" -> {value}");

        // Assert
        CollectionAssert.AreEqual(arr.OrderByDescending(x => x), Hrrrustic.reverse(HrrrusticsList));
        CollectionAssert.AreEqual(arr.Skip(1), Hrrrustic.skip(HrrrusticsList, 1));
        CollectionAssert.AreEqual(arr.Take(2), Hrrrustic.take(HrrrusticsList, 2));
        CollectionAssert.AreEqual(arr.Where(x => x % 2 == 0), Hrrrustic.evenOnly(HrrrusticsList));
        CollectionAssert.AreEqual(arr.Where(x => x % 2 != 0), Hrrrustic.oddOnly(HrrrusticsList));

        // *** Discriminated Union ***
        Console.WriteLine();
        Console.WriteLine();

        var boolItem = Hrrrustic.MultiType.NewBool(true);
        Console.WriteLine($"Here is {typeof(Hrrrustic.MultiType.Bool)}: {boolItem}");

        var intItem = Hrrrustic.MultiType.NewInt(228);
        Console.WriteLine($"Here is {typeof(Hrrrustic.MultiType.Int)}: {intItem}");

        var doubleItem = Hrrrustic.MultiType.NewDouble(22.8);
        Console.WriteLine($"Here is {typeof(Hrrrustic.MultiType.Double)}: {doubleItem}");

        var floatItem = Hrrrustic.MultiType.NewFloat(2.28);
        Console.WriteLine($"Here is {typeof(Hrrrustic.MultiType.Float)}: {floatItem}");

        var tupleItem = Hrrrustic.MultiType.NewTuple(2, 28);
        Console.WriteLine($"Here is {typeof(Hrrrustic.MultiType.Tuple)}: {tupleItem}");

        var personItem = Hrrrustic.MultiType.NewPerson(
            new Hrrrustic.Person("Misha", "Vadimovich", "Libchenko", 19));

        Console.WriteLine($"Here is {typeof(Hrrrustic.MultiType.Person)}: {personItem}");

        var weekdays = Hrrrustic.weekdays(true);
        var weekdays1 = Hrrrustic.weekdays(false);

        foreach (var sWeekday in weekdays)
            Console.WriteLine(sWeekday);

        foreach (var sWeekday in weekdays1)
            Console.WriteLine(sWeekday);
    }
}
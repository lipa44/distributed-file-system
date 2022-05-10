using LipaWebClient;

namespace ConsoleApp;

public static class Program
{
    public static async Task Main()
    {
        UserEntity user = new (4, "ФРЕДИ КИСИКОВИЧ", "КАЦ");
        TaskEntity todo = new (2, "lab-2", false, user);

        Console.WriteLine("***CREATE TASK***");
        var task = await TaskControllerApi.createTodo(todo);
        Console.WriteLine(task + "\n");
        
        Console.WriteLine("***ONE USER***");
        var gainUser = await UserControllerApi.getOneUser(2);
        Console.WriteLine(gainUser + "\n");
        
        Console.WriteLine("***ALL USERS***");
        var users = await UserControllerApi.getAll();
        users.ForEach(Console.WriteLine);
        
        Console.WriteLine("\n**REMOVED USER ID***");
        var removedUser = await UserControllerApi.deleteUser(4);
        Console.WriteLine(removedUser);
        
        Console.WriteLine("\n***ALL USERS***");
        users = await UserControllerApi.getAll();
        users.ForEach(Console.WriteLine);
    }
}
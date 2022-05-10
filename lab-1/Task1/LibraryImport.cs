using System.Runtime.InteropServices;

namespace Task1;

public class LibraryImport
{
    public static ILibraryImport Select()
    {
        // choosing between x64/x86 systems depending on IntPtr.Size
        if (IntPtr.Size == 4) // 32-bit application
        {
            return new LibraryImportX86();
        }
        else // 64-bit application
        {
            return new LibraryImportX64();
        }
    }

    public interface ILibraryImport
    {
        int Sum(int first, int second);
    }

    internal class LibraryImportX86 : ILibraryImport
    {
        [DllImport("SumFunction", CallingConvention = CallingConvention.StdCall, ExactSpelling = false, EntryPoint = "_sum@12")]
        private static extern int SumInternal(int first, int second);

        int ILibraryImport.Sum(int first, int second)
        {
            return SumInternal(first, second);
        }
    }

    internal class LibraryImportX64 : ILibraryImport
    {
        [DllImport("SumFunction", CallingConvention = CallingConvention.StdCall, ExactSpelling = false, EntryPoint = "sum")]
        private static extern int SumInternal(int first, int second);

        int ILibraryImport.Sum(int first, int second)
        {
            return SumInternal(first, second);
        }
    }
}
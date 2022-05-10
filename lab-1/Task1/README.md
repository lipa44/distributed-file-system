# Интероп между языками

## Задание

> Изучить механизм интеропа между языками, попробовать у себя вызывать C/C++ (Не C++/CLI) код (суммы чисел достаточно) из Java и C#.
> В отчёте описать логику работы, сложности и ограничения этих механизмов.

## Теормин

### Интероп

**Интероп** - технология, позволяющая взаимодействовать различным языкам благодаря тому, что написанный нами код переводится в **IL-код** _(Intermediate Language Code)_, который не привязан к ЯП, и впоследствии будет переведён в машинный код _(байто**любство**)_.

Благодаря этому мы можем указать тому или иному языку, с помощью встроенного функционала, что мы хотим подгрузить какую-то функцию из другого IL-кода _(.so/.dylib/.dll)_ и интегрировать код, написанный на другом языке, в нашем приложении.

### Библиотеки

**Статические библиотеки** — все двоичные файлы библиотек будут включены как часть нашего исполняемого файла в процессе компоновки.
**Общие библиотеки** — окончательный исполняемый файл содержит только ссылки на библиотеки, а не на сам код.

## Решение

Для начала, мы должны написать код на `C++`, который в дальнейшем будем экспортировать в `C#` и `Java`.

После этого нам нужно скомпилировать наш `C++` код в IL-код _(.so/.dylib/.dll)_, и подключить эти файлы _(общие библиотеки)_ в коды на `C#` и `Java` соответственно.

### Объявление методов для экспорта

```c++
#include "jni.h"
#include "ExportSumFunction.h"

// Java - обязательный аттрибут в нимени расшаренного метода
// com_company_Lab1_1Java - имя класса, в котором будет вызыватсья эта функция
// Sum - имя метода, который будет вызыватсья
JNIEXPORT int JNICALL Java_com_company_Lab1_1Java_Sum(JNIEnv *env, jobject thisObject, int first, int second) {
    return first + second;
}

int32_t sum(int32_t first, int32_t second) {
    return first + second;
}
```

`JNIEXPORT` - помечает функцию в общей библиотеке как экспортируемую, поэтому она будет включена в таблицу функций, и, таким образом, JNI сможет ее найти.

`JNICALL` — в сочетании с JNIEXPORT обеспечивает доступность наших методов для среды JNI.

### Реализация методов C++

```c++
#define __stdcall

#include "jni_md.h"
#include "jni.h"

// говорим шобы оно после компиляции не меняли имена параметров и функции, чтобы можно было вызвать метод извне
extern "C" {
    JNIEXPORT int JNICALL Java_com_company_Lab1_1Java_Sum(JNIEnv *env, jobject thisObject, int first, int second);
    int32_t __stdcall sum(int32_t first, int32_t second);
};
```

### Дополнительно

В нашем коде `C++` мы подключаем библиотеки `jni.h` и `jni_md.h` - это детали реализации, которые используются джавой для того, чтобы использовать ключевые слова, без которых джава не в состоянии понять, что ей нужно подгрузить функцию `<func_name>`

Для этого мы прописываем в нашем `CMakeLists.txt` подключение этим библиотекам, которые хранятся в `$JAVA_HOME` директории

```c++
add_library(lab1 SHARED ExportSumFunction.cpp ExportSumFunction.h)
include_directories(/Users/lipa/Library/Java/JavaVirtualMachines/openjdk-17.0.2/Contents/Home/include)
include_directories(/Users/lipa/Library/Java/JavaVirtualMachines/openjdk-17.0.2/Contents/Home/include/darwin)
link_directories(/Users/lipa/Library/Java/JavaVirtualMachines/openjdk-17.0.2/Contents/Home/include)
link_directories(/Users/lipa/Library/Java/JavaVirtualMachines/openjdk-17.0.2/Contents/Home/include/darwin)
```

## Интероп в C #

Подгрузка сторонних библиоткек _(.so/.dylib/.dll)_ в `C#` происходит с помощью аттрибута `[DllImport]`, в который передаётся путь до библиотеки и другие параметры для её подгрузки.

```csharp
using System.Runtime.InteropServices;

namespace Task1;

public class LibraryImport
{
    public interface ILibraryImport
    {
        int Sum(int first, int second);
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
```

## Интероп в Java ~~кал~~

Подгрузка в `Java` происходит **НЕМНОГО** по-другому, с помощью `JNI (Java Native Interface)`.

**Java Native Interface (JNI)** — стандартный механизм для запуска кода под управлением виртуальной машины Java (JVM), который скомпонован в виде динамических библиотек и позволяет не использовать **статическое** связывание.

`Java` предоставляет ключевое слово **native**, которое используется для указания того, что реализация метода будет обеспечиваться сторонним кодом.

- Ключевое слово **native** превращает наш метод в своего рода абстрактный метод.
- Любой метод, помеченный как нативный, должен быть реализован в нативной shared библиотеке.
- Мы должны указать полный путь до библиотеки для того, чтобы `Java` смог его подхватить.
- Будет создана таблица с указателями в памяти на реализацию всех наших нативных методов, чтобы их можно было вызывать из нашего Java-кода.

```csharp
public class Lab1_Java {

    static {
        try{
            System.load("/Users/lipa/Programming/CLionProjects/TechLabs/lab1/SumFunction.dylib");
        }
        catch (UnsatisfiedLinkError e) {
            System.err.println("Native code library failed to load.\n" + e);
            System.exit(1);
        }
    }

    public static void main(String[] args) {
        System.out.println(new Lab1_Java().Sum(100, 128));
    }

    private native int Sum(int first, int second);
}
```

## Плюсы и минусы

1. Компиляция кода для конкретной платформы _(обычно)_ делает его быстрее, чем запуск байт-кода.
2. Дополнительный код для каждой отдельной платформы, которую мы поддерживаем _(видимо, из-за совместимости типов и разных механизмов подключения)_.
3. Иногда нет прямого преобразования между типами.

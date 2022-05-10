# Декомпиляция кода и использование кода на совместимых языках

## Задание

> Написать немного кода на Scala и F# с использованием уникальных возможностей языка - Pipe operator, Discriminated Union, Computation expressions и т.д.
> Вызвать написанный код из обычных соответствующих ООП языков (Java и С#) и посмотреть во что превращается написанный раннее код после декомпиляции в них.

## Теормин

### Pipe Operator

**Pipe Operator** - это функция, которая позволяет нам последовательно передавать результат одной функции/аргумента другой _(как конвеер)_.

### Discriminated Union

**Discriminated Union** - это "сумма типов" - объединение типов в одном.
Эта конструкция безопасна, так как доступ к её элементам доступен только из самого объединения.

### Computation expressions

**Computation expressions** - это **удобный (спорно)** синтаксис для написания вычислений, которые можно упорядочивать и комбинировать с использованием конструкций потока управления и привязок.

## Решение

Пишем вышеупомянутые методы в языках `Scala` и `F#`, после чего подключаем в языки `Java` и `C#` соответственно, декомпилируем и смотрим, что произошло.

### F#

#### Pipe Operator в F#

```f#
let stupidSort arr : list<int> =
    arr
    |> List.filter (fun n -> n % 2 = 0)
    |> List.rev
    |> List.take 3
```

#### Декомпиляция Pipe Operator в C#

```csharp
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.FSharp.Collections;
using Microsoft.FSharp.Core;

[assembly: FSharpInterfaceDataVersion(2, 0, 0)]
[assembly: AssemblyVersion("0.0.0.0")]
[CompilationMapping(SourceConstructFlags.Module)]
public static class @_
{
    [Serializable]
    internal sealed class stupidSort@3 : FSharpFunc<int, bool>
    {
        internal static readonly stupidSort@3 @_instance = new stupidSort@3();

        [CompilerGenerated]
        [DebuggerNonUserCode]
        internal stupidSort@3()
        {
        }

        public override bool Invoke(int n)
        {
            return n % 2 == 0;
        }
    }

    [Serializable]
    internal sealed class evenOnly@7 : FSharpFunc<int, bool>
    {
        internal static readonly evenOnly@7 @_instance = new evenOnly@7();

        [CompilerGenerated]
        [DebuggerNonUserCode]
        internal evenOnly@7()
        {
        }

        public override bool Invoke(int n)
        {
            return n % 2 == 0;
        }
    }

    [Serializable]
    internal sealed class oddOnly@9 : FSharpFunc<int, bool>
    {
        internal static readonly oddOnly@9 @_instance = new oddOnly@9();

        [CompilerGenerated]
        [DebuggerNonUserCode]
        internal oddOnly@9()
        {
        }

        public override bool Invoke(int n)
        {
            return n % 2 != 0;
        }
    }

    public static FSharpList<int> stupidSort(FSharpList<int> arr)
    {
        return ListModule.Take(3, ListModule.Reverse(ListModule.Filter(stupidSort@3.@_instance, arr)));
    }

    public static FSharpList<int> evenOnly(FSharpList<int> arr)
    {
        return ListModule.Filter(evenOnly@7.@_instance, arr);
    }

    public static FSharpList<int> oddOnly(FSharpList<int> arr)
    {
        return ListModule.Filter(oddOnly@9.@_instance, arr);
    }
}
namespace <StartupCode$_>
{
    internal static class $_
    {
    }
}
```

#### Discriminated Union в F#

```f#
type MultiType =
    | Int of int
    | Bool of bool
    | Double of double
    | Float of float
    | Tuple of int * int
    | Person of Person
```

#### Декомпиляция Discriminated Union в C#

```csharp
using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.FSharp.Core;

[assembly: FSharpInterfaceDataVersion(2, 0, 0)]
[assembly: AssemblyVersion("0.0.0.0")]
[CompilationMapping(SourceConstructFlags.Module)]
public static class @_
{
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    [DebuggerDisplay("{__DebugDisplay(),nq}")]
    [CompilationMapping(SourceConstructFlags.SumType)]
    public abstract class MultiType : IEquatable<MultiType>, IStructuralEquatable, IComparable<MultiType>, IComparable, IStructuralComparable
    {
        public static class Tags
        {
            public const int Int = 0;

            public const int Bool = 1;

            public const int Double = 2;

            public const int Float = 3;

            public const int Tuple = 4;
        }

        [Serializable]
        [SpecialName]
        [DebuggerTypeProxy(typeof(Int@DebugTypeProxy))]
        [DebuggerDisplay("{__DebugDisplay(),nq}")]
        public class Int : MultiType
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            [CompilerGenerated]
            [DebuggerNonUserCode]
            internal readonly int item;

            [CompilationMapping(SourceConstructFlags.Field, 0, 0)]
            [CompilerGenerated]
            [DebuggerNonUserCode]
            public int Item
            {
                [CompilerGenerated]
                [DebuggerNonUserCode]
                get
                {
                    return item;
                }
            }

            [CompilerGenerated]
            [DebuggerNonUserCode]
            internal Int(int item)
                : base(0)
            {
                this.item = item;
            }
        }

        [Serializable]
        [SpecialName]
        [DebuggerTypeProxy(typeof(Bool@DebugTypeProxy))]
        [DebuggerDisplay("{__DebugDisplay(),nq}")]
        public class Bool : MultiType
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            [CompilerGenerated]
            [DebuggerNonUserCode]
            internal readonly bool item;

            [CompilationMapping(SourceConstructFlags.Field, 1, 0)]
            [CompilerGenerated]
            [DebuggerNonUserCode]
            public bool Item
            {
                [CompilerGenerated]
                [DebuggerNonUserCode]
                get
                {
                    return item;
                }
            }

            [CompilerGenerated]
            [DebuggerNonUserCode]
            internal Bool(bool item)
                : base(1)
            {
                this.item = item;
            }
        }

        [Serializable]
        [SpecialName]
        [DebuggerTypeProxy(typeof(Double@DebugTypeProxy))]
        [DebuggerDisplay("{__DebugDisplay(),nq}")]
        public class Double : MultiType
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            [CompilerGenerated]
            [DebuggerNonUserCode]
            internal readonly double item;

            [CompilationMapping(SourceConstructFlags.Field, 2, 0)]
            [CompilerGenerated]
            [DebuggerNonUserCode]
            public double Item
            {
                [CompilerGenerated]
                [DebuggerNonUserCode]
                get
                {
                    return item;
                }
            }

            [CompilerGenerated]
            [DebuggerNonUserCode]
            internal Double(double item)
                : base(2)
            {
                this.item = item;
            }
        }

        [Serializable]
        [SpecialName]
        [DebuggerTypeProxy(typeof(Float@DebugTypeProxy))]
        [DebuggerDisplay("{__DebugDisplay(),nq}")]
        public class Float : MultiType
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            [CompilerGenerated]
            [DebuggerNonUserCode]
            internal readonly double item;

            [CompilationMapping(SourceConstructFlags.Field, 3, 0)]
            [CompilerGenerated]
            [DebuggerNonUserCode]
            public double Item
            {
                [CompilerGenerated]
                [DebuggerNonUserCode]
                get
                {
                    return item;
                }
            }

            [CompilerGenerated]
            [DebuggerNonUserCode]
            internal Float(double item)
                : base(3)
            {
                this.item = item;
            }
        }

        [Serializable]
        [SpecialName]
        [DebuggerTypeProxy(typeof(Tuple@DebugTypeProxy))]
        [DebuggerDisplay("{__DebugDisplay(),nq}")]
        public class Tuple : MultiType
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            [CompilerGenerated]
            [DebuggerNonUserCode]
            internal readonly int item1;

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            [CompilerGenerated]
            [DebuggerNonUserCode]
            internal readonly int item2;

            [CompilationMapping(SourceConstructFlags.Field, 4, 0)]
            [CompilerGenerated]
            [DebuggerNonUserCode]
            public int Item1
            {
                [CompilerGenerated]
                [DebuggerNonUserCode]
                get
                {
                    return item1;
                }
            }

            [CompilationMapping(SourceConstructFlags.Field, 4, 1)]
            [CompilerGenerated]
            [DebuggerNonUserCode]
            public int Item2
            {
                [CompilerGenerated]
                [DebuggerNonUserCode]
                get
                {
                    return item2;
                }
            }

            [CompilerGenerated]
            [DebuggerNonUserCode]
            internal Tuple(int item1, int item2)
                : base(4)
            {
                this.item1 = item1;
                this.item2 = item2;
            }
        }

        [SpecialName]
        internal class Int@DebugTypeProxy
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            [CompilerGenerated]
            [DebuggerNonUserCode]
            internal Int _obj;

            [CompilationMapping(SourceConstructFlags.Field, 0, 0)]
            [CompilerGenerated]
            [DebuggerNonUserCode]
            public int Item
            {
                [CompilerGenerated]
                [DebuggerNonUserCode]
                get
                {
                    return _obj.item;
                }
            }

            [CompilerGenerated]
            [DebuggerNonUserCode]
            public Int@DebugTypeProxy(Int obj)
            {
                _obj = obj;
            }
        }

        [SpecialName]
        internal class Bool@DebugTypeProxy
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            [CompilerGenerated]
            [DebuggerNonUserCode]
            internal Bool _obj;

            [CompilationMapping(SourceConstructFlags.Field, 1, 0)]
            [CompilerGenerated]
            [DebuggerNonUserCode]
            public bool Item
            {
                [CompilerGenerated]
                [DebuggerNonUserCode]
                get
                {
                    return _obj.item;
                }
            }

            [CompilerGenerated]
            [DebuggerNonUserCode]
            public Bool@DebugTypeProxy(Bool obj)
            {
                _obj = obj;
            }
        }

        [SpecialName]
        internal class Double@DebugTypeProxy
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            [CompilerGenerated]
            [DebuggerNonUserCode]
            internal Double _obj;

            [CompilationMapping(SourceConstructFlags.Field, 2, 0)]
            [CompilerGenerated]
            [DebuggerNonUserCode]
            public double Item
            {
                [CompilerGenerated]
                [DebuggerNonUserCode]
                get
                {
                    return _obj.item;
                }
            }

            [CompilerGenerated]
            [DebuggerNonUserCode]
            public Double@DebugTypeProxy(Double obj)
            {
                _obj = obj;
            }
        }

        [SpecialName]
        internal class Float@DebugTypeProxy
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            [CompilerGenerated]
            [DebuggerNonUserCode]
            internal Float _obj;

            [CompilationMapping(SourceConstructFlags.Field, 3, 0)]
            [CompilerGenerated]
            [DebuggerNonUserCode]
            public double Item
            {
                [CompilerGenerated]
                [DebuggerNonUserCode]
                get
                {
                    return _obj.item;
                }
            }

            [CompilerGenerated]
            [DebuggerNonUserCode]
            public Float@DebugTypeProxy(Float obj)
            {
                _obj = obj;
            }
        }

        [SpecialName]
        internal class Tuple@DebugTypeProxy
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            [CompilerGenerated]
            [DebuggerNonUserCode]
            internal Tuple _obj;

            [CompilationMapping(SourceConstructFlags.Field, 4, 0)]
            [CompilerGenerated]
            [DebuggerNonUserCode]
            public int Item1
            {
                [CompilerGenerated]
                [DebuggerNonUserCode]
                get
                {
                    return _obj.item1;
                }
            }

            [CompilationMapping(SourceConstructFlags.Field, 4, 1)]
            [CompilerGenerated]
            [DebuggerNonUserCode]
            public int Item2
            {
                [CompilerGenerated]
                [DebuggerNonUserCode]
                get
                {
                    return _obj.item2;
                }
            }

            [CompilerGenerated]
            [DebuggerNonUserCode]
            public Tuple@DebugTypeProxy(Tuple obj)
            {
                _obj = obj;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [CompilerGenerated]
        [DebuggerNonUserCode]
        internal readonly int _tag;

        [CompilerGenerated]
        [DebuggerNonUserCode]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int Tag
        {
            [CompilerGenerated]
            [DebuggerNonUserCode]
            get
            {
                return _tag;
            }
        }

        [CompilerGenerated]
        [DebuggerNonUserCode]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsInt
        {
            [CompilerGenerated]
            [DebuggerNonUserCode]
            get
            {
                return Tag == 0;
            }
        }

        [CompilerGenerated]
        [DebuggerNonUserCode]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsBool
        {
            [CompilerGenerated]
            [DebuggerNonUserCode]
            get
            {
                return Tag == 1;
            }
        }

        [CompilerGenerated]
        [DebuggerNonUserCode]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsDouble
        {
            [CompilerGenerated]
            [DebuggerNonUserCode]
            get
            {
                return Tag == 2;
            }
        }

        [CompilerGenerated]
        [DebuggerNonUserCode]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsFloat
        {
            [CompilerGenerated]
            [DebuggerNonUserCode]
            get
            {
                return Tag == 3;
            }
        }

        [CompilerGenerated]
        [DebuggerNonUserCode]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsTuple
        {
            [CompilerGenerated]
            [DebuggerNonUserCode]
            get
            {
                return Tag == 4;
            }
        }

        [CompilerGenerated]
        [DebuggerNonUserCode]
        internal MultiType(int _tag)
        {
            this._tag = _tag;
        }

        [CompilationMapping(SourceConstructFlags.UnionCase, 0)]
        public static MultiType NewInt(int item)
        {
            return new Int(item);
        }

        [CompilationMapping(SourceConstructFlags.UnionCase, 1)]
        public static MultiType NewBool(bool item)
        {
            return new Bool(item);
        }

        [CompilationMapping(SourceConstructFlags.UnionCase, 2)]
        public static MultiType NewDouble(double item)
        {
            return new Double(item);
        }

        [CompilationMapping(SourceConstructFlags.UnionCase, 3)]
        public static MultiType NewFloat(double item)
        {
            return new Float(item);
        }

        [CompilationMapping(SourceConstructFlags.UnionCase, 4)]
        public static MultiType NewTuple(int item1, int item2)
        {
            return new Tuple(item1, item2);
        }

        [SpecialName]
        [CompilerGenerated]
        [DebuggerNonUserCode]
        internal object __DebugDisplay()
        {
            return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<MultiType, string>, Unit, string, string, string>("%+0.8A")).Invoke(this);
        }

        [CompilerGenerated]
        public override string ToString()
        {
            return ExtraTopLevelOperators.PrintFormatToString(new PrintfFormat<FSharpFunc<MultiType, string>, Unit, string, string, MultiType>("%+A")).Invoke(this);
        }

        [CompilerGenerated]
        public sealed override int CompareTo(MultiType obj)
        {
            if (this != null)
            {
                if (obj != null)
                {
                    int tag = _tag;
                    int tag2 = obj._tag;
                    if (tag == tag2)
                    {
                        return CompareTo$cont@1(this, obj, null);
                    }
                    return tag - tag2;
                }
                return 1;
            }
            if (obj != null)
            {
                return -1;
            }
            return 0;
        }

        [CompilerGenerated]
        public sealed override int CompareTo(object obj)
        {
            return CompareTo((MultiType)obj);
        }

        [CompilerGenerated]
        public sealed override int CompareTo(object obj, IComparer comp)
        {
            MultiType multiType = (MultiType)obj;
            if (this != null)
            {
                if ((MultiType)obj != null)
                {
                    int tag = _tag;
                    int tag2 = multiType._tag;
                    if (tag == tag2)
                    {
                        return CompareTo$cont@1-1(comp, this, multiType, null);
                    }
                    return tag - tag2;
                }
                return 1;
            }
            if ((MultiType)obj != null)
            {
                return -1;
            }
            return 0;
        }

        [CompilerGenerated]
        public sealed override int GetHashCode(IEqualityComparer comp)
        {
            if (this != null)
            {
                return GetHashCode$cont@1(comp, this, null);
            }
            return 0;
        }

        [CompilerGenerated]
        public sealed override int GetHashCode()
        {
            return GetHashCode(LanguagePrimitives.GenericEqualityComparer);
        }

        [CompilerGenerated]
        public sealed override bool Equals(object obj, IEqualityComparer comp)
        {
            if (this != null)
            {
                return Equals$cont@1(this, obj, null);
            }
            return obj == null;
        }

        [CompilerGenerated]
        public sealed override bool Equals(MultiType obj)
        {
            if (this != null)
            {
                if (obj != null)
                {
                    int tag = _tag;
                    int tag2 = obj._tag;
                    if (tag == tag2)
                    {
                        return Equals$cont@1-1(this, obj, null);
                    }
                    return false;
                }
                return false;
            }
            return obj == null;
        }

        [CompilerGenerated]
        public sealed override bool Equals(object obj)
        {
            MultiType multiType = obj as MultiType;
            if (multiType != null)
            {
                return Equals(multiType);
            }
            return false;
        }
    }

    [CompilerGenerated]
    internal static int CompareTo$cont@1(MultiType @this, MultiType obj, Unit unitVar)
    {
        switch (@this.Tag)
        {
            default:
            {
                MultiType.Int @int = (MultiType.Int)@this;
                MultiType.Int int2 = (MultiType.Int)obj;
                IComparer genericComparer = LanguagePrimitives.GenericComparer;
                int num = @int.item;
                int item = int2.item;
                if (num < item)
                {
                    return -1;
                }
                return (num > item) ? 1 : 0;
            }
            case 1:
            {
                MultiType.Bool @bool = (MultiType.Bool)@this;
                MultiType.Bool bool2 = (MultiType.Bool)obj;
                IComparer genericComparer = LanguagePrimitives.GenericComparer;
                bool item5 = @bool.item;
                bool item6 = bool2.item;
                if ((item5 ? 1 : 0) < (item6 ? 1 : 0))
                {
                    return -1;
                }
                return ((item5 ? 1 : 0) > (item6 ? 1 : 0)) ? 1 : 0;
            }
            case 2:
            {
                MultiType.Double @double = (MultiType.Double)@this;
                MultiType.Double double2 = (MultiType.Double)obj;
                IComparer genericComparer = LanguagePrimitives.GenericComparer;
                double item3 = @double.item;
                double item4 = double2.item;
                if (item3 < item4)
                {
                    return -1;
                }
                if (item3 > item4)
                {
                    return 1;
                }
                if (item3 == item4)
                {
                    return 0;
                }
                return LanguagePrimitives.HashCompare.GenericComparisonWithComparerIntrinsic(genericComparer, item3, item4);
            }
            case 3:
            {
                MultiType.Float @float = (MultiType.Float)@this;
                MultiType.Float float2 = (MultiType.Float)obj;
                IComparer genericComparer = LanguagePrimitives.GenericComparer;
                double item3 = @float.item;
                double item4 = float2.item;
                if (item3 < item4)
                {
                    return -1;
                }
                if (item3 > item4)
                {
                    return 1;
                }
                if (item3 == item4)
                {
                    return 0;
                }
                return LanguagePrimitives.HashCompare.GenericComparisonWithComparerIntrinsic(genericComparer, item3, item4);
            }
            case 4:
            {
                MultiType.Tuple tuple = (MultiType.Tuple)@this;
                MultiType.Tuple tuple2 = (MultiType.Tuple)obj;
                IComparer genericComparer = LanguagePrimitives.GenericComparer;
                int item = tuple.item1;
                int item2 = tuple2.item1;
                int num = ((item >= item2) ? ((item > item2) ? 1 : 0) : (-1));
                if (num < 0)
                {
                    return num;
                }
                if (num > 0)
                {
                    return num;
                }
                genericComparer = LanguagePrimitives.GenericComparer;
                item = tuple.item2;
                item2 = tuple2.item2;
                if (item < item2)
                {
                    return -1;
                }
                return (item > item2) ? 1 : 0;
            }
        }
    }

    [CompilerGenerated]
    internal static int CompareTo$cont@1-1(IComparer comp, MultiType @this, MultiType objTemp, Unit unitVar)
    {
        switch (@this.Tag)
        {
            default:
            {
                MultiType.Int @int = (MultiType.Int)@this;
                MultiType.Int int2 = (MultiType.Int)objTemp;
                int num = @int.item;
                int item = int2.item;
                if (num < item)
                {
                    return -1;
                }
                return (num > item) ? 1 : 0;
            }
            case 1:
            {
                MultiType.Bool @bool = (MultiType.Bool)@this;
                MultiType.Bool bool2 = (MultiType.Bool)objTemp;
                bool item5 = @bool.item;
                bool item6 = bool2.item;
                if ((item5 ? 1 : 0) < (item6 ? 1 : 0))
                {
                    return -1;
                }
                return ((item5 ? 1 : 0) > (item6 ? 1 : 0)) ? 1 : 0;
            }
            case 2:
            {
                MultiType.Double @double = (MultiType.Double)@this;
                MultiType.Double double2 = (MultiType.Double)objTemp;
                double item3 = @double.item;
                double item4 = double2.item;
                if (item3 < item4)
                {
                    return -1;
                }
                if (item3 > item4)
                {
                    return 1;
                }
                if (item3 == item4)
                {
                    return 0;
                }
                return LanguagePrimitives.HashCompare.GenericComparisonWithComparerIntrinsic(comp, item3, item4);
            }
            case 3:
            {
                MultiType.Float @float = (MultiType.Float)@this;
                MultiType.Float float2 = (MultiType.Float)objTemp;
                double item3 = @float.item;
                double item4 = float2.item;
                if (item3 < item4)
                {
                    return -1;
                }
                if (item3 > item4)
                {
                    return 1;
                }
                if (item3 == item4)
                {
                    return 0;
                }
                return LanguagePrimitives.HashCompare.GenericComparisonWithComparerIntrinsic(comp, item3, item4);
            }
            case 4:
            {
                MultiType.Tuple tuple = (MultiType.Tuple)@this;
                MultiType.Tuple tuple2 = (MultiType.Tuple)objTemp;
                int item = tuple.item1;
                int item2 = tuple2.item1;
                int num = ((item >= item2) ? ((item > item2) ? 1 : 0) : (-1));
                if (num < 0)
                {
                    return num;
                }
                if (num > 0)
                {
                    return num;
                }
                item = tuple.item2;
                item2 = tuple2.item2;
                if (item < item2)
                {
                    return -1;
                }
                return (item > item2) ? 1 : 0;
            }
        }
    }

    [CompilerGenerated]
    internal static int GetHashCode$cont@1(IEqualityComparer comp, MultiType @this, Unit unitVar)
    {
        int num = 0;
        switch (@this.Tag)
        {
            default:
            {
                MultiType.Int @int = (MultiType.Int)@this;
                num = 0;
                return -1640531527 + (@int.item + ((num << 6) + (num >> 2)));
            }
            case 1:
            {
                MultiType.Bool @bool = (MultiType.Bool)@this;
                num = 1;
                return -1640531527 + ((@bool.item ? 1 : 0) + ((num << 6) + (num >> 2)));
            }
            case 2:
            {
                MultiType.Double @double = (MultiType.Double)@this;
                num = 2;
                return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, @double.item) + ((num << 6) + (num >> 2)));
            }
            case 3:
            {
                MultiType.Float @float = (MultiType.Float)@this;
                num = 3;
                return -1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic(comp, @float.item) + ((num << 6) + (num >> 2)));
            }
            case 4:
            {
                MultiType.Tuple tuple = (MultiType.Tuple)@this;
                num = 4;
                num = -1640531527 + (tuple.item2 + ((num << 6) + (num >> 2)));
                return -1640531527 + (tuple.item1 + ((num << 6) + (num >> 2)));
            }
        }
    }

    [CompilerGenerated]
    internal static bool Equals$cont@1(MultiType @this, object obj, Unit unitVar)
    {
        MultiType multiType = obj as MultiType;
        if (multiType != null)
        {
            int tag = @this._tag;
            int tag2 = multiType._tag;
            if (tag == tag2)
            {
                switch (@this.Tag)
                {
                    default:
                    {
                        MultiType.Int @int = (MultiType.Int)@this;
                        MultiType.Int int2 = (MultiType.Int)multiType;
                        return @int.item == int2.item;
                    }
                    case 1:
                    {
                        MultiType.Bool @bool = (MultiType.Bool)@this;
                        MultiType.Bool bool2 = (MultiType.Bool)multiType;
                        return @bool.item == bool2.item;
                    }
                    case 2:
                    {
                        MultiType.Double @double = (MultiType.Double)@this;
                        MultiType.Double double2 = (MultiType.Double)multiType;
                        return @double.item == double2.item;
                    }
                    case 3:
                    {
                        MultiType.Float @float = (MultiType.Float)@this;
                        MultiType.Float float2 = (MultiType.Float)multiType;
                        return @float.item == float2.item;
                    }
                    case 4:
                    {
                        MultiType.Tuple tuple = (MultiType.Tuple)@this;
                        MultiType.Tuple tuple2 = (MultiType.Tuple)multiType;
                        if (tuple.item1 == tuple2.item1)
                        {
                            return tuple.item2 == tuple2.item2;
                        }
                        return false;
                    }
                }
            }
            return false;
        }
        return false;
    }

    [CompilerGenerated]
    internal static bool Equals$cont@1-1(MultiType @this, MultiType obj, Unit unitVar)
    {
        switch (@this.Tag)
        {
            default:
            {
                MultiType.Int @int = (MultiType.Int)@this;
                MultiType.Int int2 = (MultiType.Int)obj;
                return @int.item == int2.item;
            }
            case 1:
            {
                MultiType.Bool @bool = (MultiType.Bool)@this;
                MultiType.Bool bool2 = (MultiType.Bool)obj;
                return @bool.item == bool2.item;
            }
            case 2:
            {
                MultiType.Double @double = (MultiType.Double)@this;
                MultiType.Double double2 = (MultiType.Double)obj;
                double item = @double.item;
                double item2 = double2.item;
                if (item == item2)
                {
                    return true;
                }
                if (item != item)
                {
                    return item2 != item2;
                }
                return false;
            }
            case 3:
            {
                MultiType.Float @float = (MultiType.Float)@this;
                MultiType.Float float2 = (MultiType.Float)obj;
                double item = @float.item;
                double item2 = float2.item;
                if (item == item2)
                {
                    return true;
                }
                if (item != item)
                {
                    return item2 != item2;
                }
                return false;
            }
            case 4:
            {
                MultiType.Tuple tuple = (MultiType.Tuple)@this;
                MultiType.Tuple tuple2 = (MultiType.Tuple)obj;
                if (tuple.item1 == tuple2.item1)
                {
                    return tuple.item2 == tuple2.item2;
                }
                return false;
            }
        }
    }
}
namespace <StartupCode$_>
{
    internal static class $_
    {
    }
}


```

#### Computation expressions в F#

```f#
let weekdays (includeWeekend : bool) =
    seq {
        "Monday"
        "Tuesday"
        "Wednesday"
        "Thursday"
        "Friday"

        if includeWeekend then
            "Saturday"
            "Sunday"
    }
    
let duplicate input = seq { 
  for num in input do
    yield num 
    yield num }
```

#### Декомпиляция Computation expressions в C#

```csharp
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.FSharp.Core;
using Microsoft.FSharp.Core.CompilerServices;

[assembly: FSharpInterfaceDataVersion(2, 0, 0)]
[assembly: AssemblyVersion("0.0.0.0")]
[CompilationMapping(SourceConstructFlags.Module)]
public static class @_
{
    [Serializable]
    [SpecialName]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    [CompilationMapping(SourceConstructFlags.Closure)]
    internal sealed class duplicate@2<a> : GeneratedSequenceBase<a>
    {
        public IEnumerable<a> input;

        public a num;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [CompilerGenerated]
        [DebuggerNonUserCode]
        public IEnumerator<a> @enum;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [CompilerGenerated]
        [DebuggerNonUserCode]
        public int pc;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [CompilerGenerated]
        [DebuggerNonUserCode]
        public a current;

        public duplicate@2(IEnumerable<a> input, a num, IEnumerator<a> @enum, int pc, a current)
        {
            this.input = input;
            this.num = num;
            this.@enum = @enum;
            this.pc = pc;
            this.current = current;
            base..ctor();
        }

        public override int GenerateNext(ref IEnumerable<a> next)
        {
            switch (pc)
            {
                default:
                    @enum = input.GetEnumerator();
                    pc = 1;
                    goto IL_0042;
                case 2:
                    pc = 3;
                    current = num;
                    return 1;
                case 3:
                {
                    a val = default(a);
                    num = val;
                    goto IL_0042;
                }
                case 1:
                    pc = 4;
                    LanguagePrimitives.IntrinsicFunctions.Dispose(@enum);
                    @enum = null;
                    pc = 4;
                    break;
                case 4:
                    break;
                    IL_0042:
                    if (@enum.MoveNext())
                    {
                        num = @enum.Current;
                        pc = 2;
                        current = num;
                        return 1;
                    }
                    goto case 1;
            }
            current = default(a);
            return 0;
        }

        public override void Close()
        {
            Exception ex = default(Exception);
            a val = default(a);
            while (true)
            {
                switch (pc)
                {
                    case 4:
                        if (ex != null)
                        {
                            throw ex;
                        }
                        return;
                }
                try
                {
                    switch (pc)
                    {
                        default:
                            pc = 4;
                            LanguagePrimitives.IntrinsicFunctions.Dispose(@enum);
                            break;
                        case 0:
                        case 4:
                            break;
                    }
                    pc = 4;
                    current = val;
                }
                catch (object obj)
                {
                    Exception ex2 = (Exception)obj;
                    ex = ex2;
                }
            }
        }

        public override bool get_CheckClose()
        {
            switch (pc)
            {
                default:
                    return true;
                case 2:
                    return true;
                case 1:
                    return true;
                case 0:
                case 4:
                    return false;
            }
        }

        [CompilerGenerated]
        [DebuggerNonUserCode]
        public override a get_LastGenerated()
        {
            return current;
        }

        [CompilerGenerated]
        [DebuggerNonUserCode]
        public override IEnumerator<a> GetFreshEnumerator()
        {
            a val = default(a);
            return new duplicate@2<a>(input, val, null, 0, default(a));
        }
    }

    public static IEnumerable<a> duplicate<a>(IEnumerable<a> input)
    {
        a num = default(a);
        return new duplicate@2<a>(input, num, null, 0, default(a));
    }
}
namespace <StartupCode$_>
{
    internal static class $_
    {
    }
}
```

### Scala

#### Pipe Operator в Scala

```markdown
  def этоЛамбаАЭтоГеликВладЭтоБумагаАБумагаЭтоДеньги(single: Int): Int = {
    single | sum(single, single) | multiply(single, single)
  }
```

#### Декомпиляция Pipe Operator в Java

```markdown
    public int PipeFunction(final int single) {
        return single | this.sum(single, single) | this.multiply(single, single);
    }
```

#### Discriminated Union в Scala

```markdown
  class Shape(centerX: Int, centerY: Int):
  case class Square(side: Int, centerX: Int, centerY: Int) extends Shape(centerY, centerX)
  case class Rectangle(length: Int, height: Int, centerX: Int, centerY: Int) extends Shape(centerX, centerY)
  case class Circle(radius: Int, centerX: Int, centerY: Int) extends Shape(centerX, centerY)
```

#### Декомпиляция Discriminated Union в Java

```markdown
//decompiled from Shape.class
import java.io.Serializable;
import scala.Product;
import scala.collection.Iterator;
import scala.runtime.BoxesRunTime;
import scala.runtime.Statics;

public class Shape {
   public final Shape.Square$ Square$lzy1 = new Shape.Square$(this);
   public final Shape.Rectangle$ Rectangle$lzy1 = new Shape.Rectangle$(this);
   public final Shape.Circle$ Circle$lzy1 = new Shape.Circle$(this);

   public Shape(final int centerX, final int centerY) {
   }

   public final Shape.Square$ Square() {
      return this.Square$lzy1;
   }

   public final Shape.Rectangle$ Rectangle() {
      return this.Rectangle$lzy1;
   }

   public final Shape.Circle$ Circle() {
      return this.Circle$lzy1;
   }

   public class Circle extends Shape implements Product, Serializable {
      private final int radius;
      private final int centerX;
      private final int centerY;
      private final Shape $outer;

      public Circle(final Shape $outer, final int radius, final int centerX, final int centerY) {
         this.radius = radius;
         this.centerX = centerX;
         this.centerY = centerY;
         if ($outer == null) {
            throw new NullPointerException();
         } else {
            this.$outer = $outer;
            super(centerX, centerY);
         }
      }

      // $FF: synthetic method
      // $FF: bridge method
      public Iterator productIterator() {
         return Product.productIterator$(this);
      }

      // $FF: synthetic method
      // $FF: bridge method
      public Iterator productElementNames() {
         return Product.productElementNames$(this);
      }

      public int hashCode() {
         int var1 = -889275714;
         var1 = Statics.mix(var1, this.productPrefix().hashCode());
         var1 = Statics.mix(var1, this.radius());
         var1 = Statics.mix(var1, this.centerX());
         var1 = Statics.mix(var1, this.centerY());
         return Statics.finalizeHash(var1, 3);
      }

      public boolean equals(final Object x$0) {
         boolean var10000;
         if (this != x$0) {
            if (x$0 instanceof Shape.Circle && ((Shape.Circle)x$0).Shape$Circle$$$outer() == this.$outer) {
               Shape.Circle var3 = (Shape.Circle)x$0;
               var10000 = this.radius() == var3.radius() && this.centerX() == var3.centerX() && this.centerY() == var3.centerY() && var3.canEqual(this);
            } else {
               var10000 = false;
            }

            if (!var10000) {
               var10000 = false;
               return var10000;
            }
         }

         var10000 = true;
         return var10000;
      }

      public String toString() {
         return scala.runtime.ScalaRunTime..MODULE$._toString(this);
      }

      public boolean canEqual(final Object that) {
         return that instanceof Shape.Circle;
      }

      public int productArity() {
         return 3;
      }

      public String productPrefix() {
         return "Circle";
      }

      public Object productElement(final int n) {
         int var10000;
         switch(n) {
         case 0:
            var10000 = this._1();
            break;
         case 1:
            var10000 = this._2();
            break;
         case 2:
            var10000 = this._3();
            break;
         default:
            throw new IndexOutOfBoundsException(BoxesRunTime.boxToInteger(n).toString());
         }

         return BoxesRunTime.boxToInteger(var10000);
      }

      public String productElementName(final int n) {
         String var10000;
         switch(n) {
         case 0:
            var10000 = "radius";
            break;
         case 1:
            var10000 = "centerX";
            break;
         case 2:
            var10000 = "centerY";
            break;
         default:
            throw new IndexOutOfBoundsException(BoxesRunTime.boxToInteger(n).toString());
         }

         return var10000;
      }

      public int radius() {
         return this.radius;
      }

      public int centerX() {
         return this.centerX;
      }

      public int centerY() {
         return this.centerY;
      }

      public Shape.Circle copy(final int radius, final int centerX, final int centerY) {
         return this.$outer.new Circle(this.$outer, radius, centerX, centerY);
      }

      public int copy$default$1() {
         return this.radius();
      }

      public int copy$default$2() {
         return this.centerX();
      }

      public int copy$default$3() {
         return this.centerY();
      }

      public int _1() {
         return this.radius();
      }

      public int _2() {
         return this.centerX();
      }

      public int _3() {
         return this.centerY();
      }

      public final Shape Shape$Circle$$$outer() {
         return this.$outer;
      }
   }

   public final class Circle$ implements scala.deriving.Mirror.Product, Serializable {
      private final Shape $outer;

      public Circle$(final Shape $outer) {
         if ($outer == null) {
            throw new NullPointerException();
         } else {
            this.$outer = $outer;
            super();
         }
      }

      public Shape.Circle apply(final int radius, final int centerX, final int centerY) {
         return this.$outer.new Circle(this.$outer, radius, centerX, centerY);
      }

      public Shape.Circle unapply(final Shape.Circle x$1) {
         return x$1;
      }

      public String toString() {
         return "Circle";
      }

      public Shape.Circle fromProduct(final Product x$0) {
         return this.$outer.new Circle(this.$outer, BoxesRunTime.unboxToInt(x$0.productElement(0)), BoxesRunTime.unboxToInt(x$0.productElement(1)), BoxesRunTime.unboxToInt(x$0.productElement(2)));
      }

      public final Shape Shape$Circle$$$$outer() {
         return this.$outer;
      }

      // $FF: synthetic method
      // $FF: bridge method
      public Object fromProduct(final Product p) {
         return this.fromProduct(p);
      }
   }

   public class Rectangle extends Shape implements Product, Serializable {
      private final int length;
      private final int height;
      private final int centerX;
      private final int centerY;
      private final Shape $outer;

      public Rectangle(final Shape $outer, final int length, final int height, final int centerX, final int centerY) {
         this.length = length;
         this.height = height;
         this.centerX = centerX;
         this.centerY = centerY;
         if ($outer == null) {
            throw new NullPointerException();
         } else {
            this.$outer = $outer;
            super(centerX, centerY);
         }
      }

      // $FF: synthetic method
      // $FF: bridge method
      public Iterator productIterator() {
         return Product.productIterator$(this);
      }

      // $FF: synthetic method
      // $FF: bridge method
      public Iterator productElementNames() {
         return Product.productElementNames$(this);
      }

      public int hashCode() {
         int var1 = -889275714;
         var1 = Statics.mix(var1, this.productPrefix().hashCode());
         var1 = Statics.mix(var1, this.length());
         var1 = Statics.mix(var1, this.height());
         var1 = Statics.mix(var1, this.centerX());
         var1 = Statics.mix(var1, this.centerY());
         return Statics.finalizeHash(var1, 4);
      }

      public boolean equals(final Object x$0) {
         boolean var10000;
         if (this != x$0) {
            if (x$0 instanceof Shape.Rectangle && ((Shape.Rectangle)x$0).Shape$Rectangle$$$outer() == this.$outer) {
               Shape.Rectangle var3 = (Shape.Rectangle)x$0;
               var10000 = this.length() == var3.length() && this.height() == var3.height() && this.centerX() == var3.centerX() && this.centerY() == var3.centerY() && var3.canEqual(this);
            } else {
               var10000 = false;
            }

            if (!var10000) {
               var10000 = false;
               return var10000;
            }
         }

         var10000 = true;
         return var10000;
      }

      public String toString() {
         return scala.runtime.ScalaRunTime..MODULE$._toString(this);
      }

      public boolean canEqual(final Object that) {
         return that instanceof Shape.Rectangle;
      }

      public int productArity() {
         return 4;
      }

      public String productPrefix() {
         return "Rectangle";
      }

      public Object productElement(final int n) {
         int var10000;
         switch(n) {
         case 0:
            var10000 = this._1();
            break;
         case 1:
            var10000 = this._2();
            break;
         case 2:
            var10000 = this._3();
            break;
         case 3:
            var10000 = this._4();
            break;
         default:
            throw new IndexOutOfBoundsException(BoxesRunTime.boxToInteger(n).toString());
         }

         return BoxesRunTime.boxToInteger(var10000);
      }

      public String productElementName(final int n) {
         String var10000;
         switch(n) {
         case 0:
            var10000 = "length";
            break;
         case 1:
            var10000 = "height";
            break;
         case 2:
            var10000 = "centerX";
            break;
         case 3:
            var10000 = "centerY";
            break;
         default:
            throw new IndexOutOfBoundsException(BoxesRunTime.boxToInteger(n).toString());
         }

         return var10000;
      }

      public int length() {
         return this.length;
      }

      public int height() {
         return this.height;
      }

      public int centerX() {
         return this.centerX;
      }

      public int centerY() {
         return this.centerY;
      }

      public Shape.Rectangle copy(final int length, final int height, final int centerX, final int centerY) {
         return this.$outer.new Rectangle(this.$outer, length, height, centerX, centerY);
      }

      public int copy$default$1() {
         return this.length();
      }

      public int copy$default$2() {
         return this.height();
      }

      public int copy$default$3() {
         return this.centerX();
      }

      public int copy$default$4() {
         return this.centerY();
      }

      public int _1() {
         return this.length();
      }

      public int _2() {
         return this.height();
      }

      public int _3() {
         return this.centerX();
      }

      public int _4() {
         return this.centerY();
      }

      public final Shape Shape$Rectangle$$$outer() {
         return this.$outer;
      }
   }

   public final class Rectangle$ implements scala.deriving.Mirror.Product, Serializable {
      private final Shape $outer;

      public Rectangle$(final Shape $outer) {
         if ($outer == null) {
            throw new NullPointerException();
         } else {
            this.$outer = $outer;
            super();
         }
      }

      public Shape.Rectangle apply(final int length, final int height, final int centerX, final int centerY) {
         return this.$outer.new Rectangle(this.$outer, length, height, centerX, centerY);
      }

      public Shape.Rectangle unapply(final Shape.Rectangle x$1) {
         return x$1;
      }

      public String toString() {
         return "Rectangle";
      }

      public Shape.Rectangle fromProduct(final Product x$0) {
         return this.$outer.new Rectangle(this.$outer, BoxesRunTime.unboxToInt(x$0.productElement(0)), BoxesRunTime.unboxToInt(x$0.productElement(1)), BoxesRunTime.unboxToInt(x$0.productElement(2)), BoxesRunTime.unboxToInt(x$0.productElement(3)));
      }

      public final Shape Shape$Rectangle$$$$outer() {
         return this.$outer;
      }

      // $FF: synthetic method
      // $FF: bridge method
      public Object fromProduct(final Product p) {
         return this.fromProduct(p);
      }
   }

   public class Square extends Shape implements Product, Serializable {
      private final int side;
      private final int centerX;
      private final int centerY;
      private final Shape $outer;

      public Square(final Shape $outer, final int side, final int centerX, final int centerY) {
         this.side = side;
         this.centerX = centerX;
         this.centerY = centerY;
         if ($outer == null) {
            throw new NullPointerException();
         } else {
            this.$outer = $outer;
            super(centerY, centerX);
         }
      }

      // $FF: synthetic method
      // $FF: bridge method
      public Iterator productIterator() {
         return Product.productIterator$(this);
      }

      // $FF: synthetic method
      // $FF: bridge method
      public Iterator productElementNames() {
         return Product.productElementNames$(this);
      }

      public int hashCode() {
         int var1 = -889275714;
         var1 = Statics.mix(var1, this.productPrefix().hashCode());
         var1 = Statics.mix(var1, this.side());
         var1 = Statics.mix(var1, this.centerX());
         var1 = Statics.mix(var1, this.centerY());
         return Statics.finalizeHash(var1, 3);
      }

      public boolean equals(final Object x$0) {
         boolean var10000;
         if (this != x$0) {
            if (x$0 instanceof Shape.Square && ((Shape.Square)x$0).Shape$Square$$$outer() == this.$outer) {
               Shape.Square var3 = (Shape.Square)x$0;
               var10000 = this.side() == var3.side() && this.centerX() == var3.centerX() && this.centerY() == var3.centerY() && var3.canEqual(this);
            } else {
               var10000 = false;
            }

            if (!var10000) {
               var10000 = false;
               return var10000;
            }
         }

         var10000 = true;
         return var10000;
      }

      public String toString() {
         return scala.runtime.ScalaRunTime..MODULE$._toString(this);
      }

      public boolean canEqual(final Object that) {
         return that instanceof Shape.Square;
      }

      public int productArity() {
         return 3;
      }

      public String productPrefix() {
         return "Square";
      }

      public Object productElement(final int n) {
         int var10000;
         switch(n) {
         case 0:
            var10000 = this._1();
            break;
         case 1:
            var10000 = this._2();
            break;
         case 2:
            var10000 = this._3();
            break;
         default:
            throw new IndexOutOfBoundsException(BoxesRunTime.boxToInteger(n).toString());
         }

         return BoxesRunTime.boxToInteger(var10000);
      }

      public String productElementName(final int n) {
         String var10000;
         switch(n) {
         case 0:
            var10000 = "side";
            break;
         case 1:
            var10000 = "centerX";
            break;
         case 2:
            var10000 = "centerY";
            break;
         default:
            throw new IndexOutOfBoundsException(BoxesRunTime.boxToInteger(n).toString());
         }

         return var10000;
      }

      public int side() {
         return this.side;
      }

      public int centerX() {
         return this.centerX;
      }

      public int centerY() {
         return this.centerY;
      }

      public Shape.Square copy(final int side, final int centerX, final int centerY) {
         return this.$outer.new Square(this.$outer, side, centerX, centerY);
      }

      public int copy$default$1() {
         return this.side();
      }

      public int copy$default$2() {
         return this.centerX();
      }

      public int copy$default$3() {
         return this.centerY();
      }

      public int _1() {
         return this.side();
      }

      public int _2() {
         return this.centerX();
      }

      public int _3() {
         return this.centerY();
      }

      public final Shape Shape$Square$$$outer() {
         return this.$outer;
      }
   }

   public final class Square$ implements scala.deriving.Mirror.Product, Serializable {
      private final Shape $outer;

      public Square$(final Shape $outer) {
         if ($outer == null) {
            throw new NullPointerException();
         } else {
            this.$outer = $outer;
            super();
         }
      }

      public Shape.Square apply(final int side, final int centerX, final int centerY) {
         return this.$outer.new Square(this.$outer, side, centerX, centerY);
      }

      public Shape.Square unapply(final Shape.Square x$1) {
         return x$1;
      }

      public String toString() {
         return "Square";
      }

      public Shape.Square fromProduct(final Product x$0) {
         return this.$outer.new Square(this.$outer, BoxesRunTime.unboxToInt(x$0.productElement(0)), BoxesRunTime.unboxToInt(x$0.productElement(1)), BoxesRunTime.unboxToInt(x$0.productElement(2)));
      }

      public final Shape Shape$Square$$$$outer() {
         return this.$outer;
      }

      // $FF: synthetic method
      // $FF: bridge method
      public Object fromProduct(final Product p) {
         return this.fromProduct(p);
      }
   }
}
```

#### Computation expressions в Scala

```markdown
  def ComputationExpression(): Unit = {
    val f: Future[String] = Future {
      Thread.sleep(2000)
      "future value"
    }

    val f2 = f map { s =>
      println("OK!")
      println("OK!")
    }

    Await.ready(f2, 60 seconds)
    println("exit")
  }

  def aboba(): Unit = {
    println("aboba")
  }
```

#### Декомпиляция Computation expressions в Java

```markdown
//decompiled from ImportClassScala.class

public class ImportClassScala {
   public void ComputationExpression() {
      Future f = .MODULE$.apply(ImportClassScala::$anonfun$1, scala.concurrent.ExecutionContext.Implicits..MODULE$.global());
      Future f2 = f.map((s) -> {
         scala.Predef..MODULE$.println("OK!");
         scala.Predef..MODULE$.println("OK!");
      }, scala.concurrent.ExecutionContext.Implicits..MODULE$.global());
      scala.concurrent.Await..MODULE$.ready(f2, (new DurationInt(scala.concurrent.duration.package..MODULE$.DurationInt(60))).seconds());
      scala.Predef..MODULE$.println("exit");
   }

   private static final String $anonfun$1() {
      Thread.sleep(2000L);
      return "future value";
   }

   // $FF: synthetic method
   private static Object $deserializeLambda$(SerializedLambda var0) {
      return Class.lambdaDeserialize<invokedynamic>(var0);
   }
}
```

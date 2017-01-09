using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace FastReflectionLib.Samples.Benchmarks
{
    public class Test
    {
        public Test() { }

        public Test(int i) { }

        public object ObjectProperty { get; set; }

        public int Int32Property { get; set; }

        public void MethodWithoutArgs() { }

        public void MethodWithArgs(int a1, string a2) { }
    }

    public class TestResult
    {
        public string Message { get; set; }

        public TimeSpan Direct { get; set; }

        public TimeSpan Reflection { get; set; }

        public TimeSpan FastReflection { get; set; }

        public TimeSpan RawFastReflection { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            int times = 1000000;
            Array.ForEach(new TestResult[]
            {
                TestObjectPropertyGet(times),
                TestInt32PropertyGet(times),
                TestMethodWithArgs(times),
                TestMethodWithoutArgs(times)
            },
            result =>
            {
                Console.WriteLine(result.Message);
                Console.WriteLine(result.Direct);
                Console.WriteLine(result.Reflection);
                Console.WriteLine(result.FastReflection);
                Console.WriteLine(result.RawFastReflection);
                Console.WriteLine();
            });

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        static TestResult TestObjectPropertyGet(int times)
        {
            var obj = new Test();
            var propertyInfo = obj.GetType().GetProperty("ObjectProperty");
            var accessor = FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo);

            var watch = new Stopwatch();
            var result = new TestResult
            {
                Message = "Get an object property for " + times + " times."
            };

            // Direct
            watch.Start();
            for (int i = 0; i < times; i++) 
            {
                var value = obj.ObjectProperty;
            }
            watch.Stop();
            result.Direct = watch.Elapsed;
            watch.Reset();

            // Reflection
            watch.Start();
            for (int i = 0; i < times; i++)
            {
                var value = propertyInfo.GetValue(obj, null);
            }
            watch.Stop();
            result.Reflection = watch.Elapsed;
            watch.Reset();

            // Fast Reflection
            watch.Start();
            for (int i = 0; i < times; i++)
            {
                var value = propertyInfo.FastGetValue(obj);
            }
            watch.Stop();
            result.FastReflection = watch.Elapsed;
            watch.Reset();

            // Raw Fast Reflection
            watch.Start();
            for (int i = 0; i < times; i++)
            {
                var value = accessor.GetValue(obj);
            }
            watch.Stop();
            result.RawFastReflection = watch.Elapsed;
            watch.Reset();

            return result;
        }

        static TestResult TestInt32PropertyGet(int times)
        {
            var obj = new Test();
            var propertyInfo = obj.GetType().GetProperty("Int32Property");
            var accessor = FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo);

            var watch = new Stopwatch();
            var result = new TestResult
            {
                Message = "Get an int property for " + times + " times."
            };

            // Direct
            watch.Start();
            for (int i = 0; i < times; i++)
            {
                var value = obj.Int32Property;
            }
            watch.Stop();
            result.Direct = watch.Elapsed;
            watch.Reset();

            // Reflection
            watch.Start();
            for (int i = 0; i < times; i++)
            {
                var value = (int)propertyInfo.GetValue(obj, null);
            }
            watch.Stop();
            result.Reflection = watch.Elapsed;
            watch.Reset();

            // Fast Reflection
            watch.Start();
            for (int i = 0; i < times; i++)
            {
                var value = (int)propertyInfo.FastGetValue(obj);
            }
            watch.Stop();
            result.FastReflection = watch.Elapsed;
            watch.Reset();

            // Raw Fast Reflection
            watch.Start();
            for (int i = 0; i < times; i++)
            {
                var value = (int)accessor.GetValue(obj);
            }
            watch.Stop();
            result.RawFastReflection = watch.Elapsed;
            watch.Reset();

            return result;
        }

        static TestResult TestMethodWithoutArgs(int times)
        {
            var obj = new Test();
            var methodInfo = obj.GetType().GetMethod("MethodWithoutArgs");
            var invoker = FastReflectionCaches.MethodInvokerCache.Get(methodInfo);

            var watch = new Stopwatch();
            var result = new TestResult
            {
                Message = "Execute method without arguments for " + times + " times."
            };

            // Direct
            watch.Start();
            for (int i = 0; i < times; i++)
            {
                obj.MethodWithoutArgs();
            }
            watch.Stop();
            result.Direct = watch.Elapsed;
            watch.Reset();

            // Reflection
            watch.Start();
            for (int i = 0; i < times; i++)
            {
                methodInfo.Invoke(obj, null);
            }
            watch.Stop();
            result.Reflection = watch.Elapsed;
            watch.Reset();

            // Fast Reflection
            watch.Start();
            for (int i = 0; i < times; i++)
            {
                methodInfo.FastInvoke(obj, null);
            }
            watch.Stop();
            result.FastReflection = watch.Elapsed;
            watch.Reset();

            // Raw Fast Reflection
            watch.Start();
            for (int i = 0; i < times; i++)
            {
                invoker.Invoke(obj, null);
            }
            watch.Stop();
            result.RawFastReflection = watch.Elapsed;
            watch.Reset();

            return result;
        }

        static TestResult TestMethodWithArgs(int times)
        {
            var obj = new Test();
            var methodInfo = obj.GetType().GetMethod("MethodWithArgs");
            var invoker = FastReflectionCaches.MethodInvokerCache.Get(methodInfo);

            var a1 = 1;
            var a2 = "";
            var args = new object[] { a1, a2 };

            var watch = new Stopwatch();
            var result = new TestResult
            {
                Message = "Execute method with arguments for " + times + " times."
            };

            // Direct
            watch.Start();
            for (int i = 0; i < times; i++)
            {
                obj.MethodWithArgs(a1, a2);
            }
            watch.Stop();
            result.Direct = watch.Elapsed;
            watch.Reset();

            // Reflection
            watch.Start();
            for (int i = 0; i < times; i++)
            {
                methodInfo.Invoke(obj, args);
            }
            watch.Stop();
            result.Reflection = watch.Elapsed;
            watch.Reset();

            // Fast Reflection
            watch.Start();
            for (int i = 0; i < times; i++)
            {
                methodInfo.FastInvoke(obj, args);
            }
            watch.Stop();
            result.FastReflection = watch.Elapsed;
            watch.Reset();

            // Raw Fast Reflection
            watch.Start();
            for (int i = 0; i < times; i++)
            {
                invoker.Invoke(obj, args);
            }
            watch.Stop();
            result.RawFastReflection = watch.Elapsed;
            watch.Reset();

            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace ReflectionLesson
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите адрес библиотеки:");
            var path = Console.ReadLine();
            var assembly = Assembly.LoadFile($"{path}");

            var textInfoDLL = $"{assembly.FullName}";

            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                //Console.WriteLine("***********");
                var textInfo = textInfoDLL;
                //var programType = typeof(Program);
                //var name = "Имя";
                //var strType = name.GetType();
                var typeIn = type.IsClass ? "класс" : "другой тип";
                textInfo += $"{type.Name}, {typeIn} \n";

                foreach (var memberInfo in type.GetMembers())
                {
                    if (memberInfo is MethodInfo)
                    {
                        var methodInfo = memberInfo as MethodInfo;
                        textInfo += $"{methodInfo.Name}, {methodInfo.ReturnType} \n";
                        foreach (var parameter in methodInfo.GetParameters())
                        {
                            textInfo += $"{parameter.ParameterType},{parameter.Name}\n";
                        }

                        if (type.Name == "MessageService" && memberInfo.Name == "Dis")
                        {
                            object messageService = Activator.CreateInstance(type, new object[] { "cooбщение" });
                            methodInfo.Invoke(messageService, new object[] { });
                        }

                    }

                    else if (memberInfo is ConstructorInfo)
                    {
                        var constructorInfo = memberInfo as ConstructorInfo;
                        textInfo += $"{constructorInfo.Name}\n";
                        foreach (var parameter in constructorInfo.GetParameters())
                        {
                            textInfo += $"{parameter.ParameterType},{parameter.Name}\n";
                        }
                    }
                }

                if (!type.IsInterface)
                {
                    var pathSave = @"C:\Новая папка\" + $"{type.Name}" + ".txt";
                    using (var stream = new FileStream(pathSave, FileMode.OpenOrCreate))
                    {
                        var bytes = Encoding.Default.GetBytes(textInfo);

                        stream.Write(bytes, 0, bytes.Length);
                    }
                }
            }
            Console.ReadLine();
        }
    }
}

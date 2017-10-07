using System;
using System.Globalization;
using GwhrEngineering.Translations.Examples;
using System.Diagnostics;
using GwhrEngineering.Translations;

namespace GwhrTranslations.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            Console.WriteLine(CultureInfo.CurrentCulture);
            Console.WriteLine(CultureInfo.CurrentUICulture);

            GwhrExampleTranslations objTranslations = new GwhrExampleTranslations()
                .SetBasePath(AppContext.BaseDirectory)
                .EnableLanguage(SupportedLanguage.EnglishUnitedStates).EnableLanguage(SupportedLanguage.EnglishUnitedKingdom)
                .Load(string.Empty);

            //objTranslations.EnableLanguage(GwhrEngineering.Translations.SupportedLanguage.EnglishUnitedStates) ;

            Debug.WriteLine($"Initial key value is {objTranslations.HelloWorld}");
            objTranslations.HelloWorld = "Hola mundo";
            objTranslations.Brandon = "Brandon";
            objTranslations.Save();

            Console.WriteLine("US translations file");
            Console.WriteLine($"HelloWorld: {objTranslations.HelloWorld}");
            Console.WriteLine($"Brandon: { objTranslations.Brandon}");

            objTranslations.CurrentLanguage = SupportedLanguage.EnglishUnitedKingdom;

            Console.WriteLine("GB TRanslations file");

            Console.WriteLine($"HelloWorld: {objTranslations.HelloWorld}");
            Console.WriteLine($"Brandon: { objTranslations.Brandon}");
        }
    }
}

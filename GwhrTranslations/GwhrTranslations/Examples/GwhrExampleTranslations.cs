using System;
using System.Collections.Generic;
using System.Text;

namespace GwhrEngineering.Translations.Examples
{
    public class GwhrExampleTranslations : GwhrTranslationsProvider<GwhrExampleTranslations>
    {
        public string HelloWorld
        {
            get => GetValue("Hello World");
            set => SetValue(value);
        }

        public string Brandon
        {
            get => GetValue("Brandon");
            set => SetValue(value);
        }
    }
}

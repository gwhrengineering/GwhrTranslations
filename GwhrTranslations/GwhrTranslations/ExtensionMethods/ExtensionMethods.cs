using GwhrEngineering.Translations.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GwhrEngineering.Translations.ExtensionMethods
{
    public static class ExtensionMethods
    {
        public static string GetUICulture(this SupportedLanguage enmLanguage)
        {
            string x= enmLanguage.GetType().GetMember(enmLanguage.ToString()).FirstOrDefault()?.GetCustomAttribute<ValueAttribute>()?.Value;

            return x;

            var field = enmLanguage.GetType().GetField(enmLanguage.ToString());

            ValueAttribute objValueAttribute = (ValueAttribute)Attribute.GetCustomAttribute(field.GetType(), typeof(ValueAttribute));
            if (objValueAttribute == null)
            {
                //Attribute not found.  Something is wrong. 
                throw new Exception("Fatal error.  SupportedLanguage ValueAttribute not found.  Are you missing a Value attribute on the selected supported language enum?");
            }

            string strLangCode = objValueAttribute.Value;
            return strLangCode;
        }
    }
}

using System;
namespace GwhrEngineering.Translations.Attributes
{
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
    public class ValueAttribute : Attribute
    {
        //Internal fields
        private string _strValue;
        private string _strDescription;

        public ValueAttribute(string strValue)
        {
            this._strValue = strValue;
        }

        public ValueAttribute(string strValue, string strDesc)
        {
            this._strValue = strValue;
            this._strDescription = strDesc;
        }

        public string Value => _strValue;
        public string Description => _strDescription;
    }
}

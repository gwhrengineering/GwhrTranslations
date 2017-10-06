using System;
using GwhrSettings.Core;

namespace GwhrEngineering.Translations
{
    public class GwhrTranslationsProvider<T> : GwhrSettingsBase<T> where T : GwhrTranslationsProvider<T>
    {
        //Internal fields
        public override T Load(string strFileName)
        {
            throw new NotImplementedException();
        }
    }
}

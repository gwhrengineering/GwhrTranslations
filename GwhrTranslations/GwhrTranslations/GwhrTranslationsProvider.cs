using System;
using GwhrSettings.Core;
using System.Threading;
using System.Collections.Generic;
using System.Reflection;
using GwhrEngineering.Translations.Attributes;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Concurrent;
using System.Globalization;
using GwhrEngineering.Translations.ExtensionMethods;
using System.Diagnostics;

namespace GwhrEngineering.Translations
{
    public class GwhrTranslationsProvider<T> : GwhrSettingsBase<T> where T : GwhrTranslationsProvider<T>
    {
        //Internal fields
        private ReaderWriterLockSlim _objFileLock = new ReaderWriterLockSlim();
        private HashSet<SupportedLanguage> _lstEnabledLanguage = new HashSet<SupportedLanguage>();
        private SupportedLanguage _enmCurrentLanguage = SupportedLanguage.EnglishUnitedStates;
        private string _strFilePath = string.Empty;
        private string _strBasePath = string.Empty;

        #region Public properties

        /// <summary>
        /// Gets or sets the language to use
        /// </summary>
        [Ignore]
        public SupportedLanguage CurrentLanguage
        {
            get => _enmCurrentLanguage;
            set
            {
                if (!_lstEnabledLanguage.Contains(value))
                {
                    Debug.WriteLine($"The language {value} is not enabled.  Please enable this language first.");
                    return;
                }

                //Set the new value
                _enmCurrentLanguage = value;

                //Get the ui culture corresponding to the supported language
                string strCurrentUiCulture = _enmCurrentLanguage.GetUICulture();

                //Replace the dashes with underscores
                strCurrentUiCulture = strCurrentUiCulture.Replace("-", "_");

                //Build the string
                _strFilePath = $"{_strBasePath}/{strCurrentUiCulture}.json";

                //Load the resource file into memory
                ReadToDictionary();

                _blnHasBeenBuilt = true;

                //Calls the INotifyPropertyChanged event on each property
                RefreshValues();
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Enables the use of a language.  Only languages that are enabled will be used, regardless of whether the translations file exists.
        /// </summary>
        public T EnableLanguage(SupportedLanguage enmLanguage)
        {
            _lstEnabledLanguage.Add(enmLanguage);
            return (T)this;
        }

        /// <summary>
        /// Sets the base path (the folder) where the translation files are located.
        /// </summary>
        public T SetBasePath(string strBasePath)
        {
            _strBasePath = strBasePath;
            return (T)this;
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Loads the translations for the current system's ui culture
        /// </summary>
        public override T Load(string strFileName)
        {
            foreach (SupportedLanguage enmLang in this._lstEnabledLanguage)
            {
                if (enmLang.GetUICulture() == CultureInfo.CurrentUICulture.ToString())
                {
                    this.CurrentLanguage = enmLang;
                    break;
                }
            }

            this.CurrentLanguage = SupportedLanguage.EnglishUnitedStates;

            return (T)this;
        }

        /// <summary>
        /// Save the translations to disk
        /// </summary>
        public override void Save()
        {
            base.Save();//Resets all settings state to unchanged

            EnsureResourceFileExists();
            try
            {
                _objFileLock.EnterWriteLock();

                //Clear existing file text
                File.WriteAllText(_strFilePath, "");

                //Write the dictionary to the file
                File.WriteAllText(_strFilePath, JsonConvert.SerializeObject(_dicSettings));
            }
            finally
            {
                _objFileLock.ExitWriteLock();
            }
        }

        #endregion

        #region Private methods

        //Ensure the supplied base path and filename point to a valid file
        private void EnsureResourceFileExists()
        {
            if (File.Exists(_strFilePath))
            {
                return;
            }
            //Assumes file does not exist
            try
            {
                _objFileLock.EnterWriteLock();

                //Clear existing file text
                File.WriteAllText(_strFilePath, "{}");

                //Write the dictionary to the file
                File.WriteAllText(_strFilePath, JsonConvert.SerializeObject(_dicSettings));
            }
            finally
            {
                _objFileLock.ExitWriteLock();
            }

        }

        //Parses the settings json file to memory
        private void ReadToDictionary()
        {
            EnsureResourceFileExists();
            try
            {
                _objFileLock.EnterReadLock();
                _dicSettings = JsonConvert.DeserializeObject<ConcurrentDictionary<string, GwhrSetting>>(File.ReadAllText(_strFilePath));
            }
            finally
            {
                _objFileLock.ExitReadLock();
            }
        }

        #endregion

        #region Abstract methods

        protected void RefreshValues()
        {
            PropertyInfo[] aryProperties = typeof(T).GetProperties();

            foreach (PropertyInfo objProperty in aryProperties)
            {
                IgnoreAttribute objIgnoreAttribute = (IgnoreAttribute)Attribute.GetCustomAttribute(objProperty.GetType(), typeof(IgnoreAttribute));
                if (objIgnoreAttribute == null)
                {
                    OnPropertyChanged(objProperty.Name);
                }
            }
        }

        #endregion
    }
}

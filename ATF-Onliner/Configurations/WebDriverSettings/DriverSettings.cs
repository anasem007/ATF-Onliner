using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ATF_Onliner.Services;
using OpenQA.Selenium;

namespace ATF_Onliner.Configurations.WebDriverSettings
{
    public abstract class DriverSettings
    {
        private string DriverSettingsPath => $"driverSettings:{BrowserName.ToString().ToLowerInvariant()}";

        private IReadOnlyDictionary<string, object> _options;
        private IReadOnlyDictionary<string, object> _capabilities;
        private IReadOnlyList<string> _startArguments;

        public string WebDriverVersion => Configurator.Configuration[($"{DriverSettingsPath}:WebDriverVersion")];

        // public Architecture SystemArchitecture => SettingsFile.GetValueOrDefault($"{DriverSettingsPath}.systemArchitecture", Architecture.Auto).ToEnum<Architecture>();

        public DriverOptions DriverOptions { get; }

        //    public PageLoadStrategy PageLoadStrategy => Configurator.Configuration[($"{DriverSettingsPath}.pageLoadStrategy", PageLoadStrategy.Normal).ToEnum<PageLoadStrategy>()];

        protected IReadOnlyDictionary<string, object> BrowserCapabilities
        {
            get
            {
                if (_capabilities != null) return _capabilities;

                _capabilities = Configurator.Configuration.GetSection($"{DriverSettingsPath}:Capabilities")
                    .Get<Capabilities>();

                if (_capabilities.Any())
                {
                    AqualityServices.LocalizedLogger.Debug("loc.browser.capabilities",
                        args: string.Join(",",
                            _capabilities.Select(cap => $"{Environment.NewLine}{cap.Key}: {cap.Value}")));
                }

                return _capabilities;
            }
        }

        protected IReadOnlyDictionary<string, object> BrowserOptions
        {
            get
            {
                if (_options == null)
                {
                    _options = SettingsFile.GetValueDictionaryOrEmpty<object>(
                        $"{DriverSettingsPath}.{nameof(_options)}");
                    if (_options.Any())
                    {
                        AqualityServices.LocalizedLogger.Debug("loc.browser.options",
                            args: string.Join(",",
                                _options.Select(opt => $"{Environment.NewLine}{opt.Key}: {opt.Value}")));
                    }
                }

                return _options;
            }
        }

        protected IReadOnlyList<string> BrowserStartArguments
        {
            get
            {
                if (_startArguments == null)
                {
                    _startArguments =
                        SettingsFile.GetValueListOrEmpty<string>($"{DriverSettingsPath}.{nameof(_startArguments)}");

                    if (_startArguments.Any())
                    {
                        AqualityServices.LocalizedLogger.Debug("loc.browser.arguments",
                            args: string.Join(" ", _startArguments));
                    }
                }

                return _startArguments;
            }
        }

        protected abstract BrowserName BrowserName { get; }

        protected virtual IDictionary<string, Action<DriverOptions, object>> KnownCapabilitySetters =>
            new Dictionary<string, Action<DriverOptions, object>>();

        protected void SetPageLoadStrategy(DriverOptions options)
        {
            options.PageLoadStrategy = PageLoadStrategy;
        }

        protected void SetCapabilities(DriverOptions options, Action<string, object> addCapabilityMethod = null)
        {
            foreach (var capability in BrowserCapabilities)
            {
                try
                {
                    var defaultAddCapabilityMethod = addCapabilityMethod ?? options.AddAdditionalCapability;
                    defaultAddCapabilityMethod(capability.Key, capability.Value);
                }
                catch (ArgumentException exception)
                {
                    if (exception.Message.StartsWith("There is already an option"))
                    {
                        SetKnownProperty(options, capability, exception);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        private void SetKnownProperty(DriverOptions options, KeyValuePair<string, object> capability,
            ArgumentException exception)
        {
            if (KnownCapabilitySetters.ContainsKey(capability.Key))
            {
                KnownCapabilitySetters[capability.Key](options, capability.Value);
            }
            else
            {
                SetOptionByPropertyName(options, capability, exception);
            }
        }

        protected void SetOptionsByPropertyNames(DriverOptions options)
        {
            foreach (var option in BrowserOptions)
            {
                SetOptionByPropertyName(options, option,
                    new NotSupportedException($"Property for option {option.Key} is not supported"));
            }
        }

        private void SetOptionByPropertyName(DriverOptions options, KeyValuePair<string, object> option,
            Exception exception)
        {
            var optionProperty = options
                                     .GetType()
                                     .GetProperties()
                                     .FirstOrDefault(property =>
                                         IsPropertyNameMatchOption(property.Name, option.Key) && property.CanWrite)
                                 ?? throw exception;
            var propertyType = optionProperty.PropertyType;
            var valueToSet = IsEnumValue(propertyType, option.Value)
                ? ParseEnumValue(propertyType, option.Value)
                : option.Value;
            optionProperty.SetValue(options, valueToSet);
        }

        private object ParseEnumValue(Type propertyType, object optionValue)
        {
            return optionValue is string
                ? Enum.Parse(propertyType, optionValue.ToString(), ignoreCase: true)
                : Enum.ToObject(propertyType, Convert.ChangeType(optionValue, Enum.GetUnderlyingType(propertyType)));
        }

        private bool IsEnumValue(Type propertyType, object optionValue)
        {
            var valueAsString = optionValue.ToString();
            if (!propertyType.IsEnum || string.IsNullOrEmpty(valueAsString))
            {
                return false;
            }

            var normalizedValue = char.ToUpper(valueAsString[0]) +
                                  (valueAsString.Length > 1 ? valueAsString.Substring(1) : string.Empty);
            return propertyType.IsEnumDefined(normalizedValue)
                   || propertyType.IsEnumDefined(valueAsString)
                   || (IsValueOfIntegralNumericType(optionValue)
                       && propertyType.IsEnumDefined(Convert.ChangeType(optionValue,
                           Enum.GetUnderlyingType(propertyType))));
        }

        private bool IsValueOfIntegralNumericType(object value)
        {
            return value is byte || value is sbyte
                                 || value is ushort || value is short
                                 || value is uint || value is int
                                 || value is ulong || value is long;
        }

        private bool IsPropertyNameMatchOption(string propertyName, string optionKey)
        {
            return propertyName.Equals(optionKey, StringComparison.InvariantCultureIgnoreCase)
                   || optionKey.ToLowerInvariant().Contains(propertyName.ToLowerInvariant());
        }
    }
}

}
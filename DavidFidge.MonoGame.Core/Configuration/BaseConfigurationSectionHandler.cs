using System;
using System.Configuration;
using System.Linq;
using System.Xml;

using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer.ExtensionModel.Xml;

namespace DavidFidge.MonoGame.Core.Configuration
{
    [Serializable]
    public class BaseConfigurationSectionHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            var serialiser = new ConfigurationContainer()
                .EnableImplicitTyping(GetType())
                .Create();

            var deserialisedSetting = serialiser.Deserialize(new XmlNodeReader(section.ParentNode)) as GraphicsSettings;

            return deserialisedSetting;
        }

        public static T Load<T>()
        {
            var typeName = typeof(T).Name;

            var camelCaseTypeName = typeName.First().ToString().ToLower() + typeName.Substring(1);

            var setting = (T)ConfigurationManager.GetSection(camelCaseTypeName);

            return setting;
        }
    }
}
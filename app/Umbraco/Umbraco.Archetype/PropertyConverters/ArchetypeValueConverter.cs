using System;
using System.Collections.Generic;
using System.Linq;
using Archetype.Extensions;
using Archetype.Models;
using Newtonsoft.Json;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;
using Umbraco.Core.Services;

namespace Archetype.PropertyConverters
{
    [PropertyValueType(typeof(Models.ArchetypeModel))]
    [PropertyValueCache(PropertyCacheValue.All, PropertyCacheLevel.Content)]
    public class ArchetypeValueConverter : PropertyValueConverterBase
    {

        public ServiceContext Services
        {
            get { return ApplicationContext.Current.Services; }
        }

        public override bool IsConverter(PublishedPropertyType propertyType)
        {
            return propertyType.PropertyEditorAlias.Equals(Constants.PropertyEditorAlias);
        }

        public override object ConvertDataToSource(PublishedPropertyType propertyType, object source, bool preview)
        {
            var defaultValue = new Models.ArchetypeModel();

            if (source == null)
                return defaultValue;

            var sourceString = source.ToString();

            if (!sourceString.DetectIsJson())
                return defaultValue;

            using (var timer = DisposableTimer.DebugDuration<ArchetypeValueConverter>(string.Format("ConvertDataToSource ({0})", propertyType.PropertyTypeAlias)))
            {
                var archetype = ArchetypeHelper.Instance.DeserializeJsonToArchetype(sourceString,
                    (propertyType != null ? propertyType.DataTypeId : -1),
                    (propertyType != null ? propertyType.ContentType : null));

                return archetype;
            }
        }
    }
}
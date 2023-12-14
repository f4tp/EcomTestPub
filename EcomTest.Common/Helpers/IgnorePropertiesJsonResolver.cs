using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomTest.Common.Helpers
{
    /// <summary>
    /// Custom resolver to ignore given properties during serialization
    /// </summary>
    public class IgnorePropertiesJsonResolver : DefaultContractResolver
    {
        private readonly IEnumerable<string> _propertiesToIgnore;

        public IgnorePropertiesJsonResolver(IEnumerable<string> propertiesToIgnore)
        {
            _propertiesToIgnore = propertiesToIgnore ?? Enumerable.Empty<string>();
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            return base.CreateProperties(type, memberSerialization)
                       .Where(p => !_propertiesToIgnore.Contains(p.PropertyName))
                       .ToList();
        }
    }
}

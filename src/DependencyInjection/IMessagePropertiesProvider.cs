using System;
using System.Collections.Generic;
using System.Text;

namespace Willezone.Azure.WebJobs.Extensions.DependencyInjection
{
    public interface IMessagePropertiesProvider
    {
        Dictionary<string, object> GetProperties();
    }

    class MessagePropertiesProvider : IMessagePropertiesProvider
    {
        private Dictionary<string, object> _properties;

        public Dictionary<string, object> GetProperties()
        {
            return _properties;
        }

        internal void SetProperties(Dictionary<string, object> properties)
        {
            _properties = properties;
        }
    }
}

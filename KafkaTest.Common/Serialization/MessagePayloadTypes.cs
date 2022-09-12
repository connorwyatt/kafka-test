using System.Reflection;
using KafkaTest.Common.Models;

namespace KafkaTest.Common.Serialization;

public class MessagePayloadTypes
{
  private readonly IReadOnlyDictionary<string, Type> _types;

  public MessagePayloadTypes(IEnumerable<Assembly> assemblies)
  {
    _types = assemblies.SelectMany(
        assembly => assembly.DefinedTypes.Where(t => t.ImplementedInterfaces.Contains(typeof(IMessagePayload))))
      .ToDictionary(t => t.Name, t => t.AsType());
  }

  public Type GetPayloadType(string typeName)
  {
    if (!_types.TryGetValue(typeName, out var type))
    {
      throw new InvalidOperationException($"Could not get payload type \"{typeName}\"");
    }

    return type;
  }
}

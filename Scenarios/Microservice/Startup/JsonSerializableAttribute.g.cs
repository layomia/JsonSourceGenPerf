using System;

namespace System.Text.Json.SourceGeneration
{
    /// <summary>
    /// Instructs the System.Text.Json source generator to generate serialization metadata for a specified type at compile time.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class JsonSerializableAttribute : Attribute
    {
        /// <summary>
        /// Indicates whether the specified type might be the runtime type of an object instance which was declared as
        /// a different type (polymorphic serialization).
        /// </summary>
        public bool CanBeDynamic { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="JsonSerializableAttribute"/> with the specified type.
        /// </summary>
        /// <param name="type">The Type of the property.</param>
        public JsonSerializableAttribute(Type type) { }
    }
}
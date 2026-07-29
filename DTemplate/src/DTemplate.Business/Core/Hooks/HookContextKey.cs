namespace DTemplate.Business.Core.Hooks
{
    /// <summary>
    /// Represents a typed key used to share values between hooks and handler extension points.
    /// </summary>
    /// <typeparam name="TValue">The type of value associated with the key.</typeparam>
    public sealed class HookContextKey<TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HookContextKey{TValue}"/> class.
        /// </summary>
        /// <param name="name">The descriptive name of the key.</param>
        public HookContextKey(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Hook context key name cannot be empty.", nameof(name));

            Name = name;
        }

        /// <summary>  
        /// Gets the descriptive name of the key.
        /// </summary>
        public string Name { get; }

        /// <inheritdoc/>
        public override string ToString()
            => Name;
    }
}

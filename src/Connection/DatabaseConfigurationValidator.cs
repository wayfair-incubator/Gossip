using System.Diagnostics.CodeAnalysis;

namespace Gossip.Connection
{
    internal class DatabaseConfigurationValidator
    {
        public ValidationResult Validate(DatabaseConfiguration config)
        {
            if (config.CommandTimeoutInSeconds <= 0)
            {
                return new ValidationResult { IsValid = false };
            }

            return new ValidationResult { IsValid = true };
        }
    }

    [ExcludeFromCodeCoverage]
    internal class ValidationResult
    {
        public bool IsValid { get; set; }
    }
}
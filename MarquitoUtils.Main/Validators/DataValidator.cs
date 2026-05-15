namespace MarquitoUtils.Main.Validators
{
    /// <summary>
    /// Class that serves as a base for data validation. 
    /// </summary>
    /// <typeparam name="T">The type of data to be validated. Must be a reference type (class).</typeparam>
    /// <typeparam name="TErrorEnum">The type of error enumeration. Must be a struct that implements <see cref="IConvertible"/> (typically an enum).</typeparam>
    public abstract class DataValidator<T, TErrorEnum>
        where T : class
        where TErrorEnum : struct, IConvertible
    {
        /// <summary>
        /// Gets the data to be validated.
        /// </summary>
        protected T Data { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataValidator{T, TErrorEnum}"/> class with the specified data to be validated.
        /// </summary>
        /// <param name="value">The data to be validated</param>
        protected DataValidator(T value)
        {
            Data = value;
        }

        /// <summary>
        /// Validates the data and returns a boolean indicating whether the validation was successful.
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        public abstract bool Validate(out HashSet<TErrorEnum> errors);
    }
}

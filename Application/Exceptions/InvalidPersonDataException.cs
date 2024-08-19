namespace Application.Exceptions
{
    /// <summary>
    /// Исключение, возникающее при недопустимых данных человека.
    /// </summary>
    public class InvalidPersonDataException : Exception
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="InvalidPersonDataException"/> с сообщением об ошибке.
        /// </summary>
        /// <param name="message">Сообщение об ошибке.</param>
        public InvalidPersonDataException(string message)
            : base(message)
        {
        }
    }
}

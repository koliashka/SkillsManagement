namespace Application.Exceptions
{
    /// <summary>
    /// Исключение, возникающее, когда операция с человеком не может быть выполнена.
    /// </summary>
    public class PersonOperationException : Exception
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PersonOperationException"/> с сообщением об ошибке.
        /// </summary>
        /// <param name="message">Сообщение об ошибке.</param>
        public PersonOperationException(string message)
            : base(message)
        {
        }
    }
}

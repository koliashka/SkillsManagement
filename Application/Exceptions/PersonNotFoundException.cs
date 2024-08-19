namespace Application.Exceptions
{
    /// <summary>
    /// Исключение, возникающее, когда запрашиваемый человек не найден.
    /// </summary>
    public class PersonNotFoundException : Exception
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PersonNotFoundException"/> с идентификатором человека.
        /// </summary>
        /// <param name="id">Идентификатор не найденного человека.</param>
        public PersonNotFoundException(long id)
            : base($"Person with ID {id} was not found.")
        {
        }
    }
}

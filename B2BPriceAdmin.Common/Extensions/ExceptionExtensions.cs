namespace B2BPriceAdmin.Common.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Exception"/> class.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Exception Extension Method
        /// </summary>
        /// <param name="ex"></param>
        /// <returns>Returns Error Message</returns>
        public static string ErrorExceptionMessage(this Exception ex)
        {
            string ErrorMessage = ex.InnerException == null
            ? ex.Message
            : ex.InnerException.InnerException == null
            ? ex.InnerException.Message
            : ex.InnerException.InnerException.Message;

            return ErrorMessage;
        }
    }
}

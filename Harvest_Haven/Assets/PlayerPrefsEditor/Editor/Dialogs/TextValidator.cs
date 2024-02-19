using System;
using System.Text.RegularExpressions;

namespace BgTools.Dialogs
{
    internal class TextValidator
    {
        internal enum ErrorType
        {
            Invalid = -1,
            Info = 0,
            Warning = 1,
            Error = 2
        }

        [NonSerialized]
        internal ErrorType m_errorType = ErrorType.Invalid;

        [NonSerialized]
        private string m_regEx = string.Empty;

        [NonSerialized]
        private Func<string, bool> m_validationFunction;

        [NonSerialized]
        internal string m_failureMsg = string.Empty;

        /// <summary>
        /// Validator for TextFieldDialog based on regex.
        /// </summary>
        /// <param name="errorType">Categorie of the error.</param>
        /// <param name="failureMsg">Message that described the reason why the validation fail.</param>
        /// <param name="regEx">String with regular expression. It need to describe the valid state.</param>
        internal TextValidator(ErrorType errorType, string failureMsg, string regEx)
        {
            m_errorType = errorType;
            m_failureMsg = failureMsg;
            m_regEx = regEx;
        }

        /// <summary>
        /// Validator for TextFieldDialog based on regex.
        /// </summary>
        /// <param name="errorType">Categorie of the error.</param>
        /// <param name="failureMsg">Message that described the reason why the validation fail.</param>
        /// <param name="validationFunction">Function that validate the input. Get the current input as string and need to return a bool. Nedd to return 'false' if the validation fails.</param>
        internal TextValidator(ErrorType errorType, string failureMsg, Func<string, bool> validationFunction)
        {
            m_errorType = errorType;
            m_failureMsg = failureMsg;
            m_validationFunction = validationFunction;
        }

        internal bool Validate(string srcString)
        {
            if (m_regEx != string.Empty)
                return Regex.IsMatch(srcString, m_regEx);
            else if (m_validationFunction != null)
                return m_validationFunction(srcString);

            return false;
        }
    }
}
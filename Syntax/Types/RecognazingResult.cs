using System;

namespace Syntax.Types
{
    /// <summary>
    ///     Recognazing result class
    /// </summary>
    public class RecognazingResult
    {
        /// <summary>
        ///     Default constructor
        /// </summary>
        ///
        /// <param name="errorElementName">Error element name</param>
        /// <param name="isMan">Is man</param>
        public RecognazingResult(string errorElementName, bool isMan)
        {
            ErrorElementName = errorElementName;
            IsMan = isMan;
        }

        /// <summary>
        ///     Error element name
        /// </summary>
        public String ErrorElementName { get; set; }

        /// <summary>
        ///     Is man
        /// </summary>
        public bool IsMan { get; set; }
    }
}
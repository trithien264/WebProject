using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusService
{
    public class BusErrorException : Exception
    {
        #region Standard constructors - do not use

        /// <summary>
        /// Create a new <see cref="PCIAppException"/>. Do not use this constructor, it
        /// does not take any of the data that makes this type useful.
        /// </summary>
        public BusErrorException()
        {

        }

        /// <summary>
        /// Create a new <see cref="PCIAppException"/>. Do not use this constructor, it
        /// does not take any of the data that makes this type useful.
        /// </summary>
        /// <param name="message">Error message, ignored.</param>
        public BusErrorException(string message) : base(message)
        {
        }

        /// <summary>
        /// Create a new <see cref="PCIAppException"/>. Do not use this constructor, it
        /// does not take any of the data that makes this type useful.
        /// </summary>
        /// <param name="message">Error message, ignored.</param>
        /// <param name="innerException">Inner exception.</param>
        public BusErrorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion
    }

    public class BusInfoException : Exception
    {
        #region Standard constructors - do not use

        /// <summary>
        /// Create a new <see cref="PCIAppException"/>. Do not use this constructor, it
        /// does not take any of the data that makes this type useful.
        /// </summary>
        public BusInfoException()
        {

        }

        /// <summary>
        /// Create a new <see cref="PCIAppException"/>. Do not use this constructor, it
        /// does not take any of the data that makes this type useful.
        /// </summary>
        /// <param name="message">Error message, ignored.</param>
        public BusInfoException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Create a new <see cref="PCIAppException"/>. Do not use this constructor, it
        /// does not take any of the data that makes this type useful.
        /// </summary>
        /// <param name="message">Error message, ignored.</param>
        /// <param name="innerException">Inner exception.</param>
        public BusInfoException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion
    }

    [Serializable]
    public class BusNeedLoginException : Exception
    {


        #region Standard constructors - do not use

        /// <summary>
        /// Create a new <see cref="PCIAppException"/>. Do not use this constructor, it
        /// does not take any of the data that makes this type useful.
        /// </summary>
        public BusNeedLoginException()
        {

        }

        /// <summary>
        /// Create a new <see cref="PCIAppException"/>. Do not use this constructor, it
        /// does not take any of the data that makes this type useful.
        /// </summary>
        /// <param name="message">Error message, ignored.</param>
        public BusNeedLoginException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Create a new <see cref="PCIAppException"/>. Do not use this constructor, it
        /// does not take any of the data that makes this type useful.
        /// </summary>
        /// <param name="message">Error message, ignored.</param>
        /// <param name="innerException">Inner exception.</param>
        public BusNeedLoginException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion

    }

    [Serializable]
    public class BusNoPermissionException : Exception
    {


        #region Standard constructors - do not use

        /// <summary>
        /// Create a new <see cref="PCIAppException"/>. Do not use this constructor, it
        /// does not take any of the data that makes this type useful.
        /// </summary>
        public BusNoPermissionException()
        {

        }

        /// <summary>
        /// Create a new <see cref="PCIAppException"/>. Do not use this constructor, it
        /// does not take any of the data that makes this type useful.
        /// </summary>
        /// <param name="message">Error message, ignored.</param>
        public BusNoPermissionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Create a new <see cref="PCIAppException"/>. Do not use this constructor, it
        /// does not take any of the data that makes this type useful.
        /// </summary>
        /// <param name="message">Error message, ignored.</param>
        /// <param name="innerException">Inner exception.</param>
        public BusNoPermissionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion

       
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace com.continental.TDM.HelperLibrary
{
    /// <summary>
    /// A class that can be used as a result. Can be positive (okay) or negative (fail).
    /// If failed a error code != 0 (see HelperLibrary.Constants.Error)
    /// See Unittest for usage examples
    /// </summary>
    [DataContract]
    public class Result
    {
        public const string DEFAULT_ERROR_MESSAGE = "Error";

        private Constants.Error _errCode;
        private string _message;

        /// <summary>
        /// New postive result
        /// </summary>
        public Result()
        {
            IsOk = true;
            Error = null;
            Message = string.Empty;
            ErrorCode = Constants.Error.ERR_NO_ERROR;
        }

        /// <summary>
        /// New Result created with the provided information. Could be positive or negative.
        /// </summary>
        /// <param name="infoMessage"></param>
        /// <param name="errorCode">optional</param>
        /// <param name="isOk">optional</param>
        /// <param name="error">optional</param>
        public Result(string infoMessage, Constants.Error errorCode = Constants.Error.ERR_UNSPECIFIC_ERROR, bool isOk = true, Exception error = null)
        {
            this.IsOk = isOk;
            this.Error = error;
            this.Message = infoMessage;
            this.ErrorCode = errorCode;
        }

        /// <summary>
        /// Creates a new positive (ok) Result
        /// </summary>
        public static Result Ok
        {
            get { return new Result(); }
        }

        /// <summary>
        /// Creates a new negative (failed) Result, with UNSPECIFIC_ERROR
        /// </summary>
        public static Result Fail
        {
            get { return new Result(DEFAULT_ERROR_MESSAGE); }
        }

        /// <summary>
        /// Creates a new negative (failed) Result, with the provided message and the other optional attributes
        /// </summary>
        /// <param name="message"></param>
        /// <param name="errorCode">optional</param>
        /// <param name="error">optional</param>
        /// <returns></returns>
        public static Result Failure(string message, Constants.Error errorCode = Constants.Error.ERR_UNSPECIFIC_ERROR, Exception error = null)
        {
            return new Result(message, errorCode, false, error);
        }

        [DataMember]
        public bool IsOk { get; set; }

        [DataMember]
        public object Content { get; set; }

        [DataMember]
        public Exception Error { get; set; }

        [DataMember]
        public string Message 
        { 
            get
            {
                return !String.IsNullOrWhiteSpace(_message) ? _message : (Error != null ? Error.Message : (Failed ? Enum.GetName(typeof(Constants.Error), ErrorCode) : String.Empty)); //can be empty if nothing went wrong! -> Result.Ok
            }
            set
            {
                _message = value;
            }
        }

        [DataMember]
        public Constants.Error ErrorCode
        {
            get
            {
                return _errCode;
            }
            set
            {
                _errCode = value;
                if (value == Constants.Error.ERR_NO_ERROR)
                {
                    Message = string.Empty;
                    Error = null;
                }
                IsOk = value == Constants.Error.ERR_NO_ERROR;
            }
        }

        /// <summary>
        /// Checks if Result is not okay
        /// </summary>
        public bool Failed
        {
            get { return !IsOk; }
        }

        public void Fails(string message, Constants.Error errorCode = Constants.Error.ERR_UNSPECIFIC_ERROR, Exception error = null)
        {
            this.Message = message;
            this.ErrorCode = errorCode;
            this.Error = error;
        }

        public void Fails(Constants.Error errorCode, Exception error = null)
        {
            this.ErrorCode = errorCode;
            this.Error = error;
        }

        public void SetOk(object content = null)
        {
            ErrorCode = Constants.Error.ERR_NO_ERROR;
            Content = content;
        }
    }
}

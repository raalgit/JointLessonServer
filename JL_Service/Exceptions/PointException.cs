using JL_Utility.Logger;

namespace JL_Service.Exceptions
{
    public class PointException : Exception
    {
        public PointException(string message, IJLLogger logger, JLLogType type = JLLogType.ERROR) : base(message)
        {
            logger.Log(message, type);
        }
    }
}

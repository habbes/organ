namespace Habbes.Organ
{
    public static class ResponseHelpers
    {
        public static ResponseStatus CreateOkStatus()
        {
            return new ResponseStatus() { Ok = true, ErrorMessage = string.Empty };
        }

        public static ResponseStatus CreateErrorStatus(string errorMessage)
        {
            return new ResponseStatus() { Ok = false, ErrorMessage = errorMessage };
        }
    }
}
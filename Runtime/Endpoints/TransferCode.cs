namespace OmiLAXR.Endpoints
{
    public enum TransferCode : int
    {
        NoStatements = 204,
        InvalidCredentials = 401,
        Error = 500,
        Busy = 503,
        Success = 200
    }

}
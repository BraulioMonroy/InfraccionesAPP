namespace Infracciones.Services.Sqlite
{
    public interface IPathService
    {
        string GetDatabasePath();
        string GetInternalFolder();
        string GetPublicExternalFolder();
        string GetPrivateExternalFolder();
    }
}

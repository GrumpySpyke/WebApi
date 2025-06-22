namespace WebApplication1.Common.Contracts
{
    public interface ISecretManager
    {
        string GetDbUsername();

        string GetDbPassword();
    }
}

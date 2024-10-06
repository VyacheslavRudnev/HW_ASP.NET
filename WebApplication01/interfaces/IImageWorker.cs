namespace WebApplication01.interfaces;

public interface IImageWorker
{
    string Save(string url);
    void Delete(string fileName);
}
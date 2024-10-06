namespace WebApplication01.interfaces;

public interface IImageWorker
{
    string Save(string url);
    string Save(IFormFile file);
    void Delete(string fileName);
}
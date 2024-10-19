namespace WebApplication01.Interfaces;

public interface IImageWorker
{
    string Save(string url);
    string Save(IFormFile file);
    void Delete(string fileName);
}
namespace WebApplication01.interfaces;

public interface IImageWorker //Інтерфейс відповідає за обробку зображень.
{
    string Save(string url);//Метод Save приймає параметр url, який є шляхом до зображення, яке потрібно зберегти на сервері.
    string Save(IFormFile file);//Метод Save приймає параметр file, який є файлом зображення, яке потрібно зберегти на сервері.
    void Delete(string fileName);//Метод Delete приймає параметр fileName, який є назвою файлу зображення, яке потрібно видалити з сервера.
}
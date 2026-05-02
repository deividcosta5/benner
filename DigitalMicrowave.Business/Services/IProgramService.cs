using DigitalMicrowave.Business.Models;

namespace DigitalMicrowave.Business.Services;

public interface IProgramService
{
    IEnumerable<HeatingProgram> GetAll();
    HeatingProgram? GetById(int id);
    HeatingProgram Create(HeatingProgram program);
    bool Delete(int id);
}

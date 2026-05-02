using DigitalMicrowave.Business.Models;

namespace DigitalMicrowave.Business.Repositories;

public interface IProgramRepository
{
    IEnumerable<HeatingProgram> GetAll();
    HeatingProgram? GetById(int id);
    IEnumerable<HeatingProgram> GetCustomPrograms();
    HeatingProgram Add(HeatingProgram program);
    bool Delete(int id);
    IEnumerable<char> GetAllHeatingChars();
}

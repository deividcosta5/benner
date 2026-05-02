using System.Text.Json;
using DigitalMicrowave.Business.Models;

namespace DigitalMicrowave.Business.Repositories;

public class JsonProgramRepository : IProgramRepository
{
    private readonly string _filePath;
    private List<HeatingProgram> _custom;
    private int _nextId = 100;

    public JsonProgramRepository(string filePath = "custom_programs.json")
    {
        _filePath = filePath;
        _custom = LoadFromFile();
        if (_custom.Count > 0)
            _nextId = _custom.Max(p => p.Id) + 1;
    }

    public IEnumerable<HeatingProgram> GetAll()
        => PredefinedPrograms.All.Concat(_custom);

    public HeatingProgram? GetById(int id)
        => GetAll().FirstOrDefault(p => p.Id == id);

    public IEnumerable<HeatingProgram> GetCustomPrograms()
        => _custom.AsReadOnly();

    public HeatingProgram Add(HeatingProgram program)
    {
        program.Id = _nextId++;
        _custom.Add(program);
        Save();
        return program;
    }

    public bool Delete(int id)
    {
        var program = _custom.FirstOrDefault(p => p.Id == id);
        if (program is null) return false;

        _custom.Remove(program);
        Save();
        return true;
    }

    public IEnumerable<char> GetAllHeatingChars()
        => GetAll().Select(p => p.HeatingChar);

    private List<HeatingProgram> LoadFromFile()
    {
        if (!File.Exists(_filePath)) return [];

        try
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<HeatingProgram>>(json) ?? [];
        }
        catch
        {
            return [];
        }
    }

    private void Save()
    {
        var json = JsonSerializer.Serialize(_custom, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}

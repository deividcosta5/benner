using DigitalMicrowave.Business.Exceptions;
using DigitalMicrowave.Business.Models;
using DigitalMicrowave.Business.Services;
using DigitalMicrowave.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace DigitalMicrowave.Web.Controllers;

public class ProgramsController : Controller
{
    private readonly IProgramService _programService;

    public ProgramsController(IProgramService programService)
    {
        _programService = programService;
    }

    [HttpGet]
    public IActionResult Index()
        => View(_programService.GetAll().ToList());

    [HttpGet]
    public IActionResult Create()
        => View(new CreateProgramViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CreateProgramViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        try
        {
            _programService.Create(new HeatingProgram
            {
                Name = vm.Name!,
                Food = vm.Food!,
                TimeInSeconds = vm.TimeInSeconds,
                Power = vm.Power,
                HeatingChar = vm.HeatingChar,
                Instructions = vm.Instructions
            });

            TempData["Success"] = $"Programa \"{vm.Name}\" cadastrado com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        catch (MicrowaveBusinessException ex)
        {
            ModelState.AddModelError(ex.Field, ex.Message);
            return View(vm);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        try
        {
            _programService.Delete(id);
            TempData["Success"] = "Programa excluído com sucesso.";
        }
        catch (MicrowaveBusinessException ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(Index));
    }
}

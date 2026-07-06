using Microsoft.AspNetCore.Mvc;
using VibeCodeAI.Services;
using VibeCodeAI.ViewModels;

namespace VibeCodeAI.Controllers;

public class AnalysisController : Controller
{
    private readonly GeminiService _gemini;

    public AnalysisController(GeminiService gemini)
    {
        _gemini = gemini;
    }

    // ==========================
    // AI Workspace
    // ==========================
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(AnalysisRequest request)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Response = "❌ Please fill all required fields.";
            return View(request);
        }

        try
        {
            var result = await _gemini.AnalyzeCode(
    request.Language,
    request.Tool,
    request.ExplanationLanguage,
    request.Level,
    request.Code,
    request.Feedback
);

ViewBag.Response = result;

// DEBUG
Console.WriteLine(result);

return View(request);
        }
        catch (Exception ex)
{
    ViewBag.Response =
$@"ERROR

Message:
{ex.Message}

Inner:
{ex.InnerException?.Message}

Stack:
{ex.StackTrace}";
    return View(request);
}
    }

    // ==========================
    // Dashboard
    // ==========================
    [HttpGet]
    public IActionResult Dashboard()
    {
        return View();
    }

    // ==========================
    // History (Temporary)
    // ==========================
    [HttpGet]
    public IActionResult History()
    {
        return View();
    }
}
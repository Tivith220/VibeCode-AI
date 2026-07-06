using System.ComponentModel.DataAnnotations;

namespace VibeCodeAI.ViewModels;

public class AnalysisRequest
{
    [Required(ErrorMessage = "Please select a programming language.")]
    public string Language { get; set; } = "";

    [Required(ErrorMessage = "Please select an AI agent.")]
    public string Tool { get; set; } = "";

    public string ExplanationLanguage { get; set; } = "English";

    public string Level { get; set; } = "Beginner";

    [Required(ErrorMessage = "Please enter your source code.")]
    public string Code { get; set; } = "";

    public string? Feedback { get; set; }
}
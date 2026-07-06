using System.Text;
using System.Text.Json;

namespace VibeCodeAI.Services;

public class GeminiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public GeminiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<string> AnalyzeCode(
        string language,
        string tool,
        string explanationLanguage,
        string level,
        string code,
        string? feedback)
    {
        var apiKey = _configuration["Gemini:ApiKey"];
        var model = _configuration["Gemini:Model"];

        string agentInstruction = tool switch
        {
            "Code Reviewer" => """
You are acting as the Code Reviewer Agent.

Review the source code.
Identify bad coding practices.
Suggest improvements.
Recommend clean coding standards.
""",

            "Bug Detector" => """
You are acting as the Bug Detector Agent.

Detect syntax errors.
Detect logical errors.
Detect runtime exceptions.
Explain every issue clearly.
""",

            "Code Explainer" => """
You are acting as the Code Explainer Agent.

Explain the code step-by-step.
Explain functions.
Explain algorithms.
""",

            "Documentation Generator" => """
You are acting as the Documentation Generator Agent.

Generate:
- Purpose
- Function Documentation
- Class Documentation
- Usage Example
- Project Summary
""",

            _ => ""
        };

        var prompt = $"""
You are VibeCode AI.

{agentInstruction}

Programming Language:
{language}

Explanation Language:
{explanationLanguage}

Difficulty:
{level}

Additional Feedback:
{(string.IsNullOrWhiteSpace(feedback) ? "None" : feedback)}

=========================
SOURCE CODE
=========================

{code}

=========================
END OF SOURCE CODE
=========================

Rules

1. Never ask user to provide code.
2. Analyze only this code.
3. Follow the selected difficulty.
4. Reply completely in {explanationLanguage}.

If Beginner:
- Explain line by line.
- Use simple English.

If Intermediate:
- Mention Time Complexity.
- Mention Space Complexity.

If Expert:
- Mention SOLID principles.
- Mention Best Practices.
- Mention Optimizations.
- Mention Security Improvements.

Generate this report:

# 🤖 VibeCode AI Analysis Report

## Language

## Code Quality Score

## Code Explanation

## Bugs

## Performance Improvements

## Documentation

## Best Practices

## Final Verdict
""";

        var body = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new
                        {
                            text = prompt
                        }
                    }
                }
            }
        };

        var json = JsonSerializer.Serialize(body);

        var response = await _httpClient.PostAsync(
            $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}",
            new StringContent(json, Encoding.UTF8, "application/json"));

        var responseBody = await response.Content.ReadAsStringAsync();

        // Return actual API error if request failed
        if (!response.IsSuccessStatusCode)
        {
            return $"""
❌ Gemini API Error

Status Code:
{(int)response.StatusCode}

Response:

{responseBody}
""";
        }

        try
        {
            using JsonDocument doc = JsonDocument.Parse(responseBody);

            if (doc.RootElement.TryGetProperty("candidates", out JsonElement candidates))
            {
                if (candidates.GetArrayLength() > 0)
                {
                    return candidates[0]
                        .GetProperty("content")
                        .GetProperty("parts")[0]
                        .GetProperty("text")
                        .GetString()
                        ?? "No response generated.";
                }
            }

            if (doc.RootElement.TryGetProperty("error", out JsonElement error))
            {
                return $"❌ Gemini Error\n\n{error}";
            }

            return "❌ Gemini returned an unexpected response.\n\n" + responseBody;
        }
        catch (Exception ex)
        {
            return $"""
❌ Failed to parse Gemini response

Exception:
{ex.Message}

Raw Response:

{responseBody}
""";
        }
    }
}
namespace Cas.SaaS.API.Options;

/// <summary>
/// Настройки сообщений для авторизации
/// </summary>
public class CodeTemplateOptions 
{
    /// <summary>
    /// От кого
    /// </summary>
    public string From { get; set; } = string.Empty;

    /// <summary>
    /// Кому
    /// </summary>
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// Тело
    /// </summary>
    public string Body { get; set; } = string.Empty;
}
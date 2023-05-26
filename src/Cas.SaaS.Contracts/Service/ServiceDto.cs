namespace Cas.SaaS.Contracts.Service;

/// <summary>
/// ������ ������ ������
/// </summary>
public class ServiceDto
{
    /// <summary>
    /// ������������� ������
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// �������� ������
    /// </summary>
    public string Name { get; set; } = string.Empty;
}
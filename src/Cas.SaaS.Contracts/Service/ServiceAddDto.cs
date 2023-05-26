namespace Cas.SaaS.Contracts.Service;

/// <summary>
/// ������ ���������� ������
/// </summary>
public class ServiceAddDto
{
    /// <summary>
    /// �������� ������
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// �������� ������
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// ����������� ����������� ��� �������� ������
    /// </summary>
    public List<string> Tools { get; set; } = null!;
    /// <summary>
    /// ������������� �������
    /// </summary>
    public Guid ClientId { get; set; }
}
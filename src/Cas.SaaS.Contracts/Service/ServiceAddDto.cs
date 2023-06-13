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
    public List<Tool> Tools { get; set; } = new List<Tool>();
}
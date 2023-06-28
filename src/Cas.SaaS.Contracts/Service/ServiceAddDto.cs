using System.ComponentModel.DataAnnotations;

namespace Cas.SaaS.Contracts.Service;

/// <summary>
/// ������ ���������� ������
/// </summary>
public class ServiceAddDto
{
    /// <summary>
    /// �������� ������
    /// </summary>
    [Required(ErrorMessage = "������� �������� ������!")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// �������� ������
    /// </summary>
    [Required(ErrorMessage = "������� �������� ������!")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// ����������� ����������� ��� �������� ������
    /// </summary>
    [Required(ErrorMessage = "�������� ����������� ����������� ��� �������� ������!")]
    public List<Tool> Tools { get; set; } = new List<Tool>();
}
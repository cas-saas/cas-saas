using Cas.SaaS.Models;
using System.ComponentModel.DataAnnotations;

namespace Cas.SaaS.Contracts.Employee;

/// <summary>
/// ������ ���������� ����������
/// </summary>
public class EmployeeAddDto
{
    /// <summary>
    /// ��� ������������
    /// </summary>
    public string Login { get; set; } = string.Empty;

    /// <summary>
    /// ������ ������������
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// ����� �������� ������������
    /// </summary>
    [Required(ErrorMessage = "������ ������ ��������!")]
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// �������� ����� ������������
    /// </summary>
    [Required(ErrorMessage = "������� �������� �����!")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// ���
    /// </summary>
    [Required(ErrorMessage = "������� ���!")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// �������
    /// </summary>
    [Required(ErrorMessage = "������� �������!")]
    public string Surname { get; set; } = string.Empty;

    /// <summary>
    /// ��������
    /// </summary>
    public string? Patronymic { get; set; }
}
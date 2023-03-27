using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using SoftUni.Data;
using SoftUni.Models;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace SoftUni;

public class StartUp
{
    static async Task Main(string[] args)
    {
        SoftUniContext dbContext = new SoftUniContext();

        string result = RemoveTown(dbContext);
        Console.WriteLine(result);
    }

    //Exercise 3
    public static string GetEmployeesFullInformation(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();

        foreach (var e in context.Employees
            .OrderBy(e => e.EmployeeId))
        {
            sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}");
        }

        return sb.ToString().TrimEnd();
    }

    //Exercise 4
    public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();

        var employees = context.Employees
            .Where(e => e.Salary > 50000)
            .OrderBy(e => e.FirstName);

        foreach (var e in employees)
        {
            sb.AppendLine($"{e.FirstName} - {e.Salary:f2}");
        }

        return sb.ToString().TrimEnd();
    }

    //Exercise 5
    public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
    {
        var employeesInRnD = context.Employees
            .Where(e => e.Department.Name == "Research and Development")
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                DepartmentName = e.Department.Name,
                e.Salary
            })
            .OrderBy(e => e.Salary)
            .ThenByDescending(e => e.FirstName)
            .ToList();

        StringBuilder sb = new StringBuilder();

        foreach (var e in employeesInRnD)
        {
            sb.AppendLine($"{e.FirstName} {e.LastName} from {e.DepartmentName} - ${e.Salary:f2}");
        }

        return sb.ToString().TrimEnd();
    }

    //Exercise 6
    public static string AddNewAddressToEmployee(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();

        Address newAddress = new Address()
        {
            AddressText = "Vitoshka 15",
            TownId = 4
        };

        context.Addresses.AddAsync(newAddress);

        Employee? employee = context.Employees
            .FirstOrDefault(e => e.LastName == "Nakov");
        employee!.Address = newAddress;

        context.SaveChangesAsync();

        var employeesAddresses = context.Employees
            .OrderByDescending(e => e.AddressId)
            .Take(10)
            .Select(e => e.Address!.AddressText)
            .ToArray();

        return string.Join(Environment.NewLine, employeesAddresses);
    }

    //Exercise 7
    public static string GetEmployeesInPeriod(SoftUniContext context)
    {
        var employees = context.Employees
            .Take(10)
            .Include(e => e.Manager)
            .Include(e => e.EmployeesProjects)
            .ToList();

        StringBuilder sb = new StringBuilder();

        foreach (var e in employees)
        {
            sb.AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.Manager.FirstName} {e.Manager.LastName}");

            var employeeProjects = context.EmployeesProjects
                .Where(x => x.EmployeeId == e.EmployeeId &&
                            x.Project.StartDate.Year >= 2001 &&
                            x.Project.StartDate.Year <= 2003)
                .Include(x => x.Project)
                .ToList();

            foreach (var p in employeeProjects)
            {
                if (p.Project.EndDate == null)
                {
                    sb.AppendLine($"--{p.Project.Name} - {p.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)} - not finished");
                }
                else
                {
                    sb.AppendLine($"--{p.Project.Name} - {p.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)} - {p.Project.EndDate?.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)}");
                }
            }
        }

        return sb.ToString().TrimEnd();
    }

    //Exercise 8
    public static string GetAddressesByTown(SoftUniContext context)
    {
        var addresses = context.Addresses
            .Include(a => a.Employees)
            .Include(a => a.Town)
            .OrderByDescending(a => a.Employees.Count)
            .ThenBy(a => a.Town.Name)
            .ThenBy(a => a.AddressText)
            .Take(10);

        StringBuilder sb = new StringBuilder();

        foreach (var a in addresses)
        {
            sb.AppendLine($"{a.AddressText}, {a.Town.Name} - {a.Employees.Count} employees");
        }

        return sb.ToString().TrimEnd();
    }

    //Exercise 9
    public static string GetEmployee147(SoftUniContext context)
    {
        var employee = context.Employees
            .FirstOrDefault(e => e.EmployeeId == 147);
        var employeeProjects = context.EmployeesProjects
            .Where(ep => ep.EmployeeId == employee.EmployeeId)
            .Include(ep => ep.Project)
            .OrderBy(ep => ep.Project.Name)
            .ToList();

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

        foreach (var employeeProject in employeeProjects)
        {
            sb.AppendLine(employeeProject.Project.Name);
        }

        return sb.ToString().TrimEnd();
    }

    //Exercise 10
    public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
    {
        var departments = context.Departments
            .Include(d => d.Employees)
            .Include(d => d.Manager)
            .Where(d => d.Employees.Count > 5)
            .OrderBy(d => d.Employees.Count)
            .ThenBy(d => d.Name)
            .ToList();

        var sb = new StringBuilder();

        foreach (var dep in departments)
        {
            sb.AppendLine($"{dep.Name} - {dep.Manager.FirstName} {dep.Manager.LastName}");

            foreach (var emp in dep.Employees)
            {
                sb.AppendLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle}");
            }
        }

        return sb.ToString().TrimEnd();
    }

    //Exercise 11
    public static string GetLatestProjects(SoftUniContext context)
    {
        var projects = context.Projects
            .OrderByDescending(p => p.StartDate)
            .Take(10);

        var sb = new StringBuilder();

        foreach (var pr in projects
            .OrderBy(p => p.Name))
        {
            sb.AppendLine(pr.Name);
            sb.AppendLine(pr.Description);
            sb.AppendLine(pr.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture));
        }

        return sb.ToString().TrimEnd();
    }

    //Exercise 12
    public static string IncreaseSalaries(SoftUniContext context)
    {
        var employees = context.Employees
            .Include(e => e.Department)
            .Where(e => e.Department.Name == "Engineering" ||
                        e.Department.Name == "Tool Design" ||
                        e.Department.Name == "Marketing" ||
                        e.Department.Name == "Information Services")
            .ToList();

        var sb = new StringBuilder();

        foreach (var emp in employees
            .OrderBy(e => e.FirstName)
            .ThenBy(e => e.LastName))
        {
            emp.Salary = emp.Salary * (decimal)1.12;

            sb.AppendLine($"{emp.FirstName} {emp.LastName} (${emp.Salary:f2})");
        }

        context.SaveChanges();
        return sb.ToString().TrimEnd();
    }

    //Exercise 13
    public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
    {
        var employees = context.Employees
            .Where(e => e.FirstName.ToLower().StartsWith("sa"))
            .OrderBy(e => e.FirstName)
            .ToList();

        var sb = new StringBuilder();

        foreach (var emp in employees)
        {
            sb.AppendLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle} - (${emp.Salary:f2})");
        }

        return sb.ToString().TrimEnd();
    }

    //Exercise 14
    public static string DeleteProjectById(SoftUniContext context)
    {
        var project = context.Projects
            .FirstOrDefault(p => p.ProjectId == 2);

        var employeesWithProject = context.EmployeesProjects
            .Where(e => e.ProjectId == 2)
            .ToList();

        foreach (var ep in employeesWithProject)
        {
            context.EmployeesProjects.Remove(ep);
        }

        context.Projects.Remove(project!);
        context.SaveChanges();

        var projects = context.Projects
            .Take(10)
            .ToList();
        var sb = new StringBuilder();

        foreach (var pr in projects)
        {
            sb.AppendLine(pr.Name);
        }

        return sb.ToString().TrimEnd();
    }

    //Exercise 15
    public static string RemoveTown(SoftUniContext context)
    {
        var town = context.Towns
            .FirstOrDefault(t => t.Name == "Seattle");
        var addressesToDelete = context.Addresses
            .Where(a => a.TownId == town!.TownId)
            .ToList();
        var employees = context.Employees
            .Include(e => e.Address)
            .Where(e => addressesToDelete.Contains(e.Address))
            .ToList();

        int countDeletedAddresses = 0;

        foreach (var employee in employees)
        {
            employee.AddressId = null;
        }
        foreach (var address in addressesToDelete)
        {
            context.Addresses.Remove(address);
            countDeletedAddresses++;
        }

        context.Towns.Remove(town!);
        context.SaveChanges();
        return $"{countDeletedAddresses} addresses in Seattle were deleted";
    }
}
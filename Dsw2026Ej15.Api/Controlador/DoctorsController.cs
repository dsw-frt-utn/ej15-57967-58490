using Dsw2026Ej15.Api.Models;
using Dsw2026Ej15.Data;
using Dsw2026Ej15.Domain;
using Dsw2026Ej15.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Dsw2026Ej15.Api.Controlador;

[ApiController]
[Route("api/doctors")]
public class DoctorsController : ControllerBase
{
    private readonly IPersistence _persistence;

    public DoctorsController(IPersistence persistence)
    {
        _persistence = persistence;
    }

    [HttpPost]
    public IActionResult Insert([FromBody] DoctorInsertDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ValidationException("El nombre del médico es requerido.");

        if (string.IsNullOrWhiteSpace(dto.LicenseNumber))
            throw new ValidationException("La matrícula (LicenseNumber) es requerida.");

        var speciality = _persistence.GetSpecialityById(dto.SpecialityId);
        if (speciality is null)
            throw new ValidationException("La especialidad indicada no existe.");

        var doctor = new Doctor
        {
            Name = dto.Name,
            LicenseNumber = dto.LicenseNumber,
            Speciality = speciality,
            IsActive = true
        };

        _persistence.AddDoctor(doctor);

        return CreatedAtAction(nameof(GetById), new { id = doctor.Id }, ToDto(doctor));
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var doctors = _persistence.GetActiveDoctors().Select(ToDto);
        return Ok(doctors);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        var doctor = _persistence.GetDoctorById(id);
        if (doctor is null || !doctor.IsActive)
            return NotFound("El médico no existe o no se encuentra activo.");

        return Ok(ToDto(doctor));
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        var doctor = _persistence.GetDoctorById(id);
        if (doctor is null || !doctor.IsActive)
            return NotFound("El médico no existe o no se encuentra activo.");

        doctor.IsActive = false;
        return NoContent();
    }

    private static DoctorDto ToDto(Doctor doctor) => new()
    {
        Id = doctor.Id,
        Name = doctor.Name,
        LicenseNumber = doctor.LicenseNumber,
        SpecialityName = doctor.Speciality.Name
    };
}
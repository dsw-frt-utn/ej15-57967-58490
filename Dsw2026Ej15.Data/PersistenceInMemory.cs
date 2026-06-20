using Dsw2026Ej15.Domain;
using Dsw2026Ej15.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Dsw2026Ej15.Data;

public class PersistenceInMemory : IPersistence
{
    private readonly List<Speciality> _specialities;
    private readonly List<Doctor> _doctors;

    public PersistenceInMemory()
    {
        _specialities = LoadSpecialities();
        _doctors = new List<Doctor>();
    }

    public List<Speciality> GetSpecialities() => _specialities;

    public Speciality? GetSpecialityById(Guid id) =>
        _specialities.FirstOrDefault(s => s.Id == id);

    public List<Doctor> GetActiveDoctors() =>
        _doctors.Where(d => d.IsActive).ToList();

    public Doctor? GetDoctorById(Guid id) =>
        _doctors.FirstOrDefault(d => d.Id == id);

    public void AddDoctor(Doctor doctor) => _doctors.Add(doctor);

    private List<Speciality> LoadSpecialities()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "specialities.json");
        var json = File.ReadAllText(path);

        var specialities = JsonSerializer.Deserialize<List<Speciality>>(json);
        return specialities ?? new List<Speciality>();
    }
}

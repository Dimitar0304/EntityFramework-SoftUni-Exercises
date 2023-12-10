namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ImportDtos;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data!";
        private const string SuccessfullyImportedPharmacy = "Successfully imported pharmacy - {0} with {1} medicines.";
        private const string SuccessfullyImportedPatient = "Successfully imported patient - {0} with {1} medicines.";

        public static string ImportPatients(MedicinesContext context, string jsonString)
        {
            var patientDtos = JsonConvert.DeserializeObject<ImportPatientDto[]>(jsonString);

            StringBuilder sb = new StringBuilder();
            var patientsToAdd = new List<Patient>();
            foreach (var pDto in patientDtos)
            {
                if (!IsValid(pDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if ((AgeGroup)int.Parse(pDto.AgeGroup) == null || int.Parse(pDto.AgeGroup) > 2 || int.Parse(pDto.AgeGroup) < 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if ((Gender)int.Parse(pDto.Gender) == null || int.Parse(pDto.Gender) > 1 || int.Parse(pDto.Gender) < 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Patient patient = new Patient()
                {
                    FullName = pDto.FullName,
                    AgeGroup = (AgeGroup)int.Parse(pDto.AgeGroup),
                    Gender = (Gender)int.Parse(pDto.Gender),

                };
                var patientMedicines = new List<PatientMedicine>();
                foreach (var id in pDto.Medicines)
                {
                    if (patientMedicines.Any(pm => pm.MedicineId == id))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    PatientMedicine patientMedicine = new PatientMedicine()
                    {
                        PatientId = patient.Id,
                        Patient = patient,
                        MedicineId = id,
                        Medicine = context.Medicines.Where(m => m.Id == id).First()
                    };
                    patientMedicines.Add(patientMedicine);
                    
                }
                sb.AppendLine(string.Format(SuccessfullyImportedPatient, pDto.FullName, patientMedicines.Count()));
                patient.PatientsMedicines = patientMedicines;
                context.PatientsMedicines.AddRange(patientMedicines);
            }
            context.Patients.AddRange(patientsToAdd);
            context.SaveChanges();
            return sb.ToString().Trim();
        }

        public static string ImportPharmacies(MedicinesContext context, string xmlString)
        {
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
            StringBuilder sb = new StringBuilder();
            XmlRootAttribute root = new XmlRootAttribute("Pharmacies");
            XmlSerializer serializer = new XmlSerializer(typeof(ImportPharmacyDto[]), root);
            StringReader reader = new StringReader(xmlString);
            ImportPharmacyDto[] pharmaciesDtos = (ImportPharmacyDto[])serializer.Deserialize(reader);
            var pharmacies = new List<Pharmacy>();

            foreach (ImportPharmacyDto phDto in pharmaciesDtos)
            {
                if (!IsValid(phDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (phDto.IsNonStop != "false" && phDto.IsNonStop != "true")
                {
                    //if is non stop value is INVALID
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var medicines = new List<Medicine>();
                foreach (var medDto in phDto.Medicines)
                {
                    if (!IsValid(medDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    //if productionDate < ExpiryDate
                    if (DateTime.Parse(medDto.ProductionDate) >= DateTime.Parse(medDto.ExpiryDate))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (medicines.Any(m => m.Name == medDto.Name && m.Producer == medDto.Producer))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    Medicine medicine = new Medicine()
                    {
                        Name = medDto.Name,
                        Producer = medDto.Producer,
                        ProductionDate = DateTime.Parse(medDto.ProductionDate),
                        ExpiryDate = DateTime.Parse(medDto.ExpiryDate),
                        Category = (CategoryType)medDto.Category,
                        Price = medDto.Price,
                    };
                    medicines.Add(medicine);
                }
                //create Pharmacy
                Pharmacy pharmacy = new Pharmacy()
                {
                    Name = phDto.Name,
                    PhoneNumber = phDto.PhoneNumber,
                    IsNonStop = bool.Parse(phDto.IsNonStop),
                    Medicines = medicines
                };
                pharmacies.Add(pharmacy);
                sb.AppendLine(string.Format(SuccessfullyImportedPharmacy, phDto.Name, medicines.Count()));
            }
            context.Pharmacies.AddRange(pharmacies);
            context.SaveChanges();
            return sb.ToString().Trim();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}

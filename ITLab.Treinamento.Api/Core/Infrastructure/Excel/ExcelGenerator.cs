using ITLab.Treinamento.Api.Core.Upload;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ITLab.Treinamento.Api.Core.Infrastructure.Excel
{
    /// <summary>
    /// Generate a excel file from a datasource
    /// </summary>
    public class ExcelGenerator
    {
        private readonly string worksheetName;
        private readonly string path;
        private readonly string username;

        /// <summary>
        /// Create a new instance of the ExcelGenerator
        /// </summary>
        /// <param name="worksheetName">name of the worksheet</param>
        /// <param name="path">The file location</param>
        /// <param name="username">username</param>
        public ExcelGenerator(string worksheetName, string path, string username)
        {
            if (string.IsNullOrEmpty(this.worksheetName = worksheetName))
                throw new ArgumentNullException(nameof(worksheetName));

            if (string.IsNullOrEmpty(this.path = path))
                throw new ArgumentNullException(nameof(path));

            if (string.IsNullOrEmpty(this.username = username))
                throw new ArgumentNullException(nameof(username));
        }

        /// <summary>
        /// Generate the excel file
        /// </summary>
        /// <param name="dataSource">Data Source</param>
        /// <returns>Generated file name</returns>
        /// <example>
        /// var excelGenerator = new ExcelGenerator("Customers", UploadHelper.GetDirectoryTempPathOrCreateIfNotExists());
        /// excelGenerator.Generate(clients);
        /// </example>
        public string Generate<T>(IEnumerable<T> dataSource)
        {
            if (!dataSource.Any()) return null;
            var data = dataSource.FirstOrDefault();

            return Generate(dataSource, (w) => FillHeader(w, data), FillRow);
        }

        /// <summary>
        /// Generate the excel file
        /// </summary>
        /// <param name="dataSource">Data Source</param>
        /// <param name="createCustomHeader">Action to customize excel header (first row)</param>
        /// <param name="createCustomRow">Action to customize excel row</param>
        /// <returns>Generated file name</returns>
        /// <example>
        /// var excelGenerator = new ExcelGenerator("Customers", UploadHelper.GetDirectoryTempPathOrCreateIfNotExists());
        /// excelGenerator.Generate(clients, (e) =>
        /// {
        ///     e.Cells[1, 1].Value = "Id";
        ///     e.Cells[1, 2].Value = "CPF/CNPJ";
        ///     e.Cells[1, 3].Value = "E-mail";
        ///     e.Cells[1, 4].Value = "Telefone";
        ///     e.Cells[1, 5].Value = "Status";
        /// }, (e, i, data) => {
        ///     e.Cells[i, 1].Value = data.Id;
        ///     e.Cells[i, 2].Value = data.CPF ?? data.CNPJ;
        ///     e.Cells[i, 3].Value = data.Email;
        ///     e.Cells[i, 4].Value = data.Telephone;
        ///     e.Cells[i, 5].Value = data.Active? "Sim" : "Não";
        /// });
        /// </example>
        public string Generate<T>(IEnumerable<T> dataSource, Action<ExcelWorksheet> createCustomHeader, Action<ExcelWorksheet, int, dynamic> createCustomRow)
        {
            if (!dataSource.Any()) return null;

            if (createCustomHeader == null)
                throw new ArgumentNullException(nameof(createCustomHeader));

            if (createCustomRow == null)
                throw new ArgumentNullException(nameof(createCustomRow));

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(worksheetName);
                createCustomHeader(worksheet);

                for (int i = 0; i < dataSource.Count(); i++)
                {
                    var rowIndex = i + 2;
                    var currentData = dataSource.ElementAt(i);
                    createCustomRow(worksheet, rowIndex, currentData);
                }

                var fileName = String.Format("{0}.{1}.{2:MM.dd.yyyy-HH.mm.ss}.xlsx", username, worksheetName, DateTime.Now);
                var filePath = Path.Combine(path, fileName);
                package.SaveAs(new FileInfo(filePath));

                return fileName;
            }
        }

        /// <summary>
        /// Create worksheet reader from data properties
        /// </summary>
        /// <param name="worksheet">Worksheet</param>
        /// <param name="data">Current data from datasource</param>
        private void FillHeader(ExcelWorksheet worksheet, dynamic data)
        {
            var type = data.GetType();
            var properties = type.GetProperties();

            for (int i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                worksheet.Cells[1, i + 1].Value = property.Name;
            }
        }

        /// <summary>
        /// Create worksheet row from data source properties and values
        /// </summary>
        /// <param name="worksheet">Worksheet</param>
        /// <param name="rowIndex">Row index</param>
        /// <param name="data">Current data from datasource</param>
        private void FillRow(ExcelWorksheet worksheet, int rowIndex, dynamic data)
        {
            var type = data.GetType();
            var properties = type.GetProperties();

            for (int i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                worksheet.Cells[rowIndex, i + 1].Value = type.GetProperty(property.Name).GetValue(data, null);
            }
        }
    }
}
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace bot_brainsly_one.src.utils
{
    public class ReportUtils
    {
        public void ExportDataSet(DataSet dataSet)
        {
            SpreadsheetDocument workbook;

            string fileName = $"{new FileUtils().BaseProjectDirectory}\\reports\\bot-tasks-report.xlsx";

            if(!File.Exists(fileName)) return;

            if (new FileUtils().FileIsOpen(fileName)) return;

            using (SpreadsheetDocument spreadSheet = SpreadsheetDocument.Open(fileName, true))
            {
                WorksheetPart worksheetPart = GetWorksheetPartByName(spreadSheet, Program.accountInstagram);
                if (worksheetPart != null)
                {
                    SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();

                    //grab the last row
                    Row lastRow = worksheetPart.Worksheet.Descendants<Row>().LastOrDefault();

                    UInt32Value ActualRowIndex = lastRow.RowIndex + 1;

                    string cellOfToday = GetCellValue(GetCell(sheetData, $"E{lastRow.RowIndex}"), spreadSheet.WorkbookPart);

                    if(cellOfToday == DateTime.Now.ToString("dd/MM/yyyy"))
                    {
                        ActualRowIndex = lastRow.RowIndex;
                    }

                    // Create new row
                    Row row = new Row() { RowIndex = ActualRowIndex };

                    foreach (DataColumn column in dataSet.Tables[0].Columns)
                    {
                        // get the cell value
                        object value = dataSet.Tables[0].Rows[0][column];
                        string cellValue = (value != null ? value.ToString() : "");

                        // Create new cell
                        Cell cell = new Cell() { CellReference = column.ColumnName + ActualRowIndex, DataType = CellValues.Number, CellValue = new CellValue(cellValue) };
                        
                        // Append cell to row
                        row.Append(cell);
                    }

                    // Append row to sheetData
                    sheetData.Append(row);

                    worksheetPart.Worksheet.Save();
                }
                spreadSheet.WorkbookPart.Workbook.Save();
            }
        }

        private WorksheetPart GetWorksheetPartByName(SpreadsheetDocument document, string sheetName)
        {
            IEnumerable<Sheet> sheets =
               document.WorkbookPart.Workbook.GetFirstChild<Sheets>().
               Elements<Sheet>().Where(s => s.Name == sheetName);

            if (sheets?.Count() == 0)
            {
                // The specified worksheet does not exist.
                return null;
            }

            string relationshipId = sheets?.First().Id.Value;

            WorksheetPart worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(relationshipId);

            return worksheetPart;
        }

        private Cell GetCell(SheetData sheetData, string cellAddress)
        {
            uint rowIndex = uint.Parse(Regex.Match(cellAddress, @"[0-9]+").Value);
            return sheetData.Descendants<Row>().FirstOrDefault(p => p.RowIndex == rowIndex).Descendants<Cell>().FirstOrDefault(p => p.CellReference == cellAddress);
        }

        private string GetCellValue(Cell cell, WorkbookPart wbPart)
        {
            string value = cell.InnerText;
            if (cell.DataType != null)
            {
                switch (cell.DataType.Value)
                {
                    case CellValues.SharedString:
                        var stringTable = wbPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                        if (stringTable != null)
                        {
                            value = stringTable.SharedStringTable.ElementAt(int.Parse(value)).InnerText;
                        }
                        break;

                    case CellValues.Boolean:
                        switch (value)
                        {
                            case "0":
                                value = "FALSE";
                                break;
                            default:
                                value = "TRUE";
                                break;
                        }
                        break;
                }
            }
            return value;
        }
    }
}

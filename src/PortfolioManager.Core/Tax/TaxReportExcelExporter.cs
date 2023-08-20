namespace PortfolioManager.Core.Tax
{
using NPOI.POIFS.Crypt;
using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;

    public class TaxReportExcelExporter : ITaxReportExcelExporter
    {
        public void Export(TaxReport taxReport, string targetFileName, int year)
        {
            var workbook = new XSSFWorkbook();
            var subReportsForYear = taxReport.SubReports.Where(sr => sr.Key.Year == year).ToList();

            CreateOverallSummarySheet(workbook, subReportsForYear.Select(sr => sr.Value).ToList());
            CreateOverallSummarySheet_T(workbook, subReportsForYear.Select(sr => sr.Value).ToList());
            CreateTransactionListSheet(workbook, taxReport.AllTransactions, "AllTransactions");

            foreach (var subReport in subReportsForYear)
            {
                CreateSubReportSheets(workbook, subReport.Key, subReport.Value);
            }


            //sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, 10));
            //var rowIndex = 0;
            //IRow row = sheet1.CreateRow(rowIndex);
            //row.Height = 30 * 80;
            //row.CreateCell(0).SetCellValue("this is content");
            //sheet1.AutoSizeColumn(0);
            //rowIndex++;

            //var sheet2 = workbook.CreateSheet("Sheet2");
            //var style1 = workbook.CreateCellStyle();
            //style1.FillForegroundColor = HSSFColor.Blue.Index2;
            //style1.FillPattern = FillPattern.SolidForeground;

            //var style2 = workbook.CreateCellStyle();
            //style2.FillForegroundColor = HSSFColor.Yellow.Index2;
            //style2.FillPattern = FillPattern.SolidForeground;

            //var cell2 = sheet2.CreateRow(0).CreateCell(0);
            //cell2.CellStyle = style1;
            //cell2.SetCellValue(0);

            //cell2 = sheet2.CreateRow(1).CreateCell(0);
            //cell2.CellStyle = style2;
            //cell2.SetCellValue(1);


            using var fs = new FileStream(targetFileName, FileMode.Create, FileAccess.Write);
            workbook.Write(fs);
        }

        private void CreateOverallSummarySheet(IWorkbook workbook, List<TaxSubReport> subReports)
        {
            var sheet = workbook.CreateSheet("Summary");

            var rowIndex = 0;
            var headerRow = sheet.CreateRow(rowIndex++);
            headerRow.CreateCell(0);
            for (var i = 0; i < subReports.Count; i++)
            {
                headerRow.CreateCell(i + 1).SetCellValue(subReports[i].Asset.Code);
            }

            var profitRow = sheet.CreateRow(rowIndex++);
            profitRow.CreateCell(0).SetCellValue("RealizedProfit");
            for (var i = 0; i < subReports.Count; i++)
            {
                profitRow.CreateCell(i + 1).SetCellValue((double)subReports[i].RealizedProfit);
            }

            var expenditureRow = sheet.CreateRow(rowIndex++);
            expenditureRow.CreateCell(0).SetCellValue("TotalExpenditure");
            for (var i = 0; i < subReports.Count; i++)
            {
                expenditureRow.CreateCell(i + 1).SetCellValue((double)subReports[i].TotalExpenditure);
            }

            var taxObligationRow = sheet.CreateRow(rowIndex++);
            taxObligationRow.CreateCell(0).SetCellValue("TaxObligation");
            for (var i = 0; i < subReports.Count; i++)
            {
                taxObligationRow.CreateCell(i + 1).SetCellValue((double)subReports[i].TaxObligation);
            }

            for (var i = 0; i < subReports.Count + 1; i++)
                sheet.AutoSizeColumn(i);
        }

        private void CreateOverallSummarySheet_T(IWorkbook workbook, List<TaxSubReport> subReports)
        {
            var sheet = workbook.CreateSheet("Summary_T");

            var rowIndex = 0;
            var headerRow = sheet.CreateRow(rowIndex++);
            headerRow.CreateCell(0);
            headerRow.CreateCell(1).SetCellValue("RealizedProfit");
            headerRow.CreateCell(2).SetCellValue("TotalExpenditure");
            headerRow.CreateCell(3).SetCellValue("TaxObligation");
            headerRow.CreateCell(4).SetCellValue("TotalSold");

            foreach (var subReport in subReports)
            {
                var row = sheet.CreateRow(rowIndex++);
                row.CreateCell(0).SetCellValue(subReport.Asset.Code);
                row.CreateCell(1).SetCellValue((double)subReport.RealizedProfit);
                row.CreateCell(2).SetCellValue((double)subReport.TotalExpenditure);
                row.CreateCell(3).SetCellValue((double)subReport.TaxObligation);
                row.CreateCell(4).SetCellValue((double)subReport.TotalSold);
            }

            for (var i = 0; i < 10 + 1; i++)
                sheet.AutoSizeColumn(i);
        }

        private void CreateSubReportSheets(IWorkbook workbook, AssetYearKey key, TaxSubReport subReport)
        {
            CreateSummarySheet(workbook, key, subReport);
            CreateTransactionListSheet(workbook, subReport.BuyTransactionQueue, $"{key}_BuyTransactionQueue");
            CreateTransactionListSheet(workbook, subReport.SellTransactionsForCurrentYear, $"{key}_SellTransCurrentYear");
            CreateTransactionListSheet(workbook, subReport.TransactionPairings.Values.SelectMany(v => v).ToList(), $"{key}_TransactionPairings");
        }

        private void CreateTransactionListSheet(IWorkbook workbook, List<Transaction> transactions, string sheetName)
        {
            var sheet = workbook.CreateSheet(sheetName);

            var rowIndex = 0;
            var headerRow = sheet.CreateRow(rowIndex++);
            headerRow.CreateCell(0).SetCellValue("Id");
            headerRow.CreateCell(1).SetCellValue("CreatedUtc");
            headerRow.CreateCell(2).SetCellValue("BuyAmount");
            headerRow.CreateCell(3).SetCellValue("BuyAsset");
            headerRow.CreateCell(4).SetCellValue("SellAmount");
            headerRow.CreateCell(5).SetCellValue("SellAsset");
            headerRow.CreateCell(6).SetCellValue("UnitPrice");
            headerRow.CreateCell(7).SetCellValue("TransactionSource");
            headerRow.CreateCell(8).SetCellValue("Order");
            headerRow.CreateCell(9).SetCellValue("RowIndex");

            foreach (var tr in transactions)
            {
                var trRow = sheet.CreateRow(rowIndex++);
                trRow.CreateCell(0).SetCellValue(tr.Id.ToString());
                trRow.CreateCell(1).SetCellValue(tr.CreatedUtc.ToString("dd.MM.yyyy"));
                trRow.CreateCell(2).SetCellValue((double)tr.BuyAmount);
                trRow.CreateCell(3).SetCellValue(tr.BuyAsset.ToString());
                trRow.CreateCell(4).SetCellValue((double)tr.SellAmount);
                trRow.CreateCell(5).SetCellValue(tr.SellAsset.ToString());
                trRow.CreateCell(6).SetCellValue((double)(1 / tr.BuyAmount * tr.SellAmount));
                trRow.CreateCell(7).SetCellValue(tr.TransactionSource.ToString());
                trRow.CreateCell(8).SetCellValue(tr.Order);
                trRow.CreateCell(9).SetCellValue(tr.RowIndex ?? -1);
            }

            for (var i = 0; i < 10; i++)
                sheet.AutoSizeColumn(i);
        }

        private static void CreateSummarySheet(IWorkbook workbook, AssetYearKey key, TaxSubReport subReport)
        {
            var sheet = workbook.CreateSheet($"{key}_Summary");

            var rowIndex = 0;
            var row = sheet.CreateRow(rowIndex++);
            row.CreateCell(0).SetCellValue("RealizedProfit");
            row.CreateCell(1).SetCellValue((double) subReport.RealizedProfit);

            row = sheet.CreateRow(rowIndex++);
            row.CreateCell(0).SetCellValue("TotalExpenditure");
            row.CreateCell(1).SetCellValue((double) subReport.TotalExpenditure);

            row = sheet.CreateRow(rowIndex++);
            row.CreateCell(0).SetCellValue("TaxObligation");
            row.CreateCell(1).SetCellValue((double) subReport.TaxObligation);

            row = sheet.CreateRow(rowIndex++);
            row.CreateCell(0).SetCellValue("TotalSold");
            row.CreateCell(1).SetCellValue((double) subReport.TotalSold);

            for (var i = 0; i < 10; i++)
                sheet.AutoSizeColumn(i);
        }
    }
}
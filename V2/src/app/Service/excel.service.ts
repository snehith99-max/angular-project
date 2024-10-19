import { Injectable } from '@angular/core';
import { C } from '@fullcalendar/core/internal-common';
import * as FileSaver from 'file-saver';
import * as XLSX from 'xlsx';

const EXCEL_TYPE = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8';
const EXCEL_EXTENSION = '.xlsx';

@Injectable({
  providedIn: 'root'
})
export class ExcelService {
  
  constructor() { }

  public exportAsExcelFile(json: any[], excelFileName: string, gid?: string  ): void {
    debugger
    console.log(json);
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(json);
    
  
    if (gid == 'upload_gid') 
      { // Column key to hide
      const range = XLSX.utils.decode_range(worksheet['!ref']!);
      for (let C = range.s.c; C <= range.e.c; ++C) {
        const address = XLSX.utils.encode_col(C) + "1"; // Assuming the key is in the first row
        if (worksheet[address] && worksheet[address].v === gid) {
          if (!worksheet['!cols']) {
            worksheet['!cols'] = [];
          }
          worksheet['!cols'][C] = { hidden: true };
        }
      }
    }   

    const workbook: XLSX.WorkBook = 
    { Sheets: { 'Sheet1': worksheet },
     SheetNames: ['Sheet1'] };

    const excelBuffer: any = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    this.saveAsExcelFile(excelBuffer, excelFileName);
  }

  private saveAsExcelFile(buffer: any, fileName: string): void {
    debugger

    const data: Blob = new Blob([buffer], { type: EXCEL_TYPE });
    FileSaver.saveAs(data, fileName + '_Export' + EXCEL_EXTENSION);
  }

}

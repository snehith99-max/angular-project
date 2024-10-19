import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { FormBuilder } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner'

@Component({
  selector: 'app-acc-rpt-creditorreportview',
  templateUrl: './acc-rpt-creditorreportview.component.html',
  styleUrls: ['./acc-rpt-creditorreportview.component.scss']
})
export class AccRptCreditorreportviewComponent {
  account_gid: any;
  responsedata: any;
  CreditorreportView_List: any[] = [];
  CreditorreportVendorView_List: any[] = [];
  CreditorreportOpening_List: any[] = [];
  totalCredit: any;
  totalDebit: any;
  vendor_name: any;
  remarks: any;
  closing_amount:any;
  totcrd:any;
  totdeb:any;
  closingamount: any;
  opening_balance: any;

  constructor(public service: SocketService, private router: ActivatedRoute, private route: Router, private NgxSpinnerService: NgxSpinnerService) {
  }

  ngOnInit(): void {
    const account_gid = this.router.snapshot.paramMap.get('account_gid');
    this.account_gid = account_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.account_gid, secretKey).toString(enc.Utf8);
    this.account_gid = deencryptedParam;
    console.log(deencryptedParam)
    this.GetAccTrnCreditorReportView(deencryptedParam);

    var url = 'AccTrnBankbooksummary/GetCreditorReportVendorView'
    debugger
    let param = {
      account_gid: deencryptedParam
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.CreditorreportVendorView_List = this.responsedata.GetCreditorreportVendorView_List;
      this.vendor_name = this.CreditorreportVendorView_List[0].vendor_name;
    });

    var url = 'AccTrnBankbooksummary/GetCreditorReportOpeningBlnc'
    debugger
    let params = {
      account_gid: deencryptedParam
    }
    this.service.getparams(url, params).subscribe((result: any) => {
      this.responsedata = result;
      this.CreditorreportOpening_List = this.responsedata.GetCreditorreportOpening_List;
      this.opening_balance = this.CreditorreportOpening_List[0].opening_balance;
    });

  }

  GetAccTrnCreditorReportView(account_gid: any) {
    this.NgxSpinnerService.show();
    var url = 'AccTrnBankbooksummary/GetCreditorReportView'
    let param = {
      account_gid: account_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.CreditorreportView_List = result.GetCreditorreportView_List;
      this.NgxSpinnerService.hide();
      // setTimeout(() => {
      //   $('#CreditorreportView_List').DataTable(
      //     {
      //       // code by snehith for customized pagination
      //       "pageLength": 50, // Number of rows to display per page
      //       "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
      //     }
      //   );
      // }, 1);

      if (this.CreditorreportView_List != null) {
        this.totalCredit = this.CreditorreportView_List.reduce((total: any, data: any) => total + parseFloat(data.credit_amount.replace(',', '')), 0);
        this.totalDebit = this.CreditorreportView_List.reduce((total: any, data: any) => total + parseFloat(data.debit_amount.replace(',', '')), 0);

        // Format the totals with commas and .00
        const formatter = new Intl.NumberFormat('en-US', {
          minimumFractionDigits: 2,
          maximumFractionDigits: 2
        });

        this.totalCredit = formatter.format(this.totalCredit);
        this.totalDebit = formatter.format(this.totalDebit);

  
        let totcrd = this.parseValue1(this.totalCredit);
        let totdeb = this.parseValue1(this.totalDebit);

        this.closing_amount = totdeb - totcrd;

        this.closingamount = this.formatValue1(this.closing_amount);
      }

    });
  }

  parseValue1(value: any): number {
    // Convert the value to a number, removing any non-numeric characters (e.g., commas)
    return parseFloat(value.toString().replace(/[^0-9.-]+/g, '')) || 0;
  }

  formatValue1(value: number): string {
    // Format the number as a string with comma separators and two decimal places
    return value.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
  }

  popmodal(parameter: string) {
    this.remarks = parameter;
  }

  exportExcel(): void {
    const CreditorreportViewList = this.CreditorreportView_List.map(item => ({
      'Transaction Date': item.transaction_date || '',
      'Reference Number': item.journal_refno || '',
      'Transaction Type': item.transaction_type || '',
      'Customer Name': item.customer_name || '',
      'Remarks': item.remarks || '',
      'Opening Balance': item.openingbalance || '',
      'Debit Amount': item.debit_amount || '',
      'Credit Amount': item.credit_amount || '',
      'Closing Balance': item.closingbalance || '',
    }));
    
    // Create a new table element
    const table = document.createElement('table');

    // Add header row with background color
    const headerRow = table.insertRow();
    Object.keys(CreditorreportViewList[0]).forEach(header => {
      const cell = headerRow.insertCell();
      cell.textContent = header;
      cell.style.backgroundColor = '#00317a';
      cell.style.color = '#FFFFFF';
      cell.style.fontWeight = 'bold';
      cell.style.border = '1px solid #000000';
    });

    // Add data rows
    CreditorreportViewList.forEach(item => {
      const dataRow = table.insertRow();
      Object.values(item).forEach(value => {
        const cell = dataRow.insertCell();
        cell.textContent = value;
        cell.style.border = '1px solid #000000';
      });
    });

    const totalsRow1 = table.insertRow();
    const numColumns1 = Object.keys(CreditorreportViewList[0]).length;
  
    for (let i = 0; i < numColumns1 - 4; i++) { 
      const cell = totalsRow1.insertCell();
      cell.textContent = '';
      cell.style.border = '1px solid #000000';
    }
  
    const totalLabelCell = totalsRow1.insertCell();
    totalLabelCell.textContent = 'Total:';
    totalLabelCell.style.fontWeight = 'bold';
    totalLabelCell.style.border = '1px solid #000000';
  
  
    const totalDebitCell = totalsRow1.insertCell();
    totalDebitCell.textContent = this.totalDebit || '0';
    totalDebitCell.style.fontWeight = 'bold';
    totalDebitCell.style.border = '1px solid #000000';
  

    const totalCreditCell = totalsRow1.insertCell();
    totalCreditCell.textContent = this.totalCredit || '0';
    totalCreditCell.style.fontWeight = 'bold';
    totalCreditCell.style.border = '1px solid #000000';

    const emptycell = totalsRow1.insertCell();
    emptycell.textContent = ' ';
    emptycell.style.fontWeight = 'bold';
    emptycell.style.border = '1px solid #000000';

    // Convert the table to a data URI
    const tableHtml = table.outerHTML;
    const dataUri = 'data:application/vnd.ms-excel;base64,' + btoa(unescape(encodeURIComponent(tableHtml)));

    // Trigger download
    const link = document.createElement('a');
    link.href = dataUri;
    link.download = 'Creditor Account Statement.xls';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }

}

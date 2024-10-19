import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AES } from 'crypto-js';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-acc-rpt-creditorreport',
  templateUrl: './acc-rpt-creditorreport.component.html',
  styleUrls: ['./acc-rpt-creditorreport.component.scss']
})
export class AccRptCreditorreportComponent {
  Creditorreport_List: any[] = [];
  responsedata: any;
  totalCredit: any;
  totalDebit: any;
  showOptionsDivId:any;
  total_gross: any;
  total_openingbalance: any;
  
  constructor(public service :SocketService,private router: Router,private route: ActivatedRoute,private NgxSpinnerService: NgxSpinnerService) {
    
  }

  ngOnInit(): void {
    this.AccTrnCreditorReportsummary();

    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });

  }

  AccTrnCreditorReportsummary(){
    this.NgxSpinnerService.show();
    var url='AccTrnBankbooksummary/GetCreditorReportSummary'      
    this.service.get(url).subscribe((result:any)=>{   
      this.responsedata=result;
      this.Creditorreport_List = this.responsedata.GetCreditorreport_List;  
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#Creditorreport_List').DataTable();
      }, 1);

      if (this.Creditorreport_List != null) {
        this.totalCredit = this.Creditorreport_List.reduce((total: any, data: any) => total + parseFloat(data.credit_amount.replace(',', '')), 0);
        this.totalDebit = this.Creditorreport_List.reduce((total: any, data: any) => total + parseFloat(data.debit_amount.replace(',', '')), 0);
        this.total_gross = this.Creditorreport_List.reduce((total: any, data: any) => total + parseFloat(data.closing_amount.replace(',', '')), 0);
        this.total_openingbalance = this.Creditorreport_List.reduce((total: any, data: any) => total + parseFloat(data.opening_amount.replace(',', '')), 0);
  
        // Format the totals with commas and .00
        const formatter = new Intl.NumberFormat('en-US', {
          minimumFractionDigits: 2,
          maximumFractionDigits: 2
        });
  
        this.totalCredit = formatter.format(this.totalCredit);
        this.totalDebit = formatter.format(this.totalDebit);
        this.total_openingbalance = formatter.format(this.total_openingbalance);
        this.total_gross = formatter.format(this.total_gross);
      }      
  });
  }

  calculateTotal(): number {

    let totalAmount = 0;

    for (const data of this.Creditorreport_List) {

      totalAmount += parseFloat(data.debit_amount);

    }

    return totalAmount;

  }

  onclick(params:any){
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.router.navigate(['/finance/AccRptCreditordetailedreport',encryptedParam]) 
  }
  
  vendor360(params:any){
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.router.navigate(['/finance/AccRptVendor360',encryptedParam]) 
  }

  exportExcel(): void {
    const CreditorreportList = this.Creditorreport_List.map(item => ({
      'Customer Name': item.vendor_companyname || '',
      'Opening Balance': item.opening_amount || '',
      'Debit Amount': item.debit_amount || '',
      'Credit Amount': item.credit_amount || '',
      'Closing Balance': item.closing_amount || '',
    }));
    
    // Create a new table element
    const table = document.createElement('table');

    // Add header row with background color
    const headerRow = table.insertRow();
    Object.keys(CreditorreportList[0]).forEach(header => {
      const cell = headerRow.insertCell();
      cell.textContent = header;
      cell.style.backgroundColor = '#00317a';
      cell.style.color = '#FFFFFF';
      cell.style.fontWeight = 'bold';
      cell.style.border = '1px solid #000000';
    });

    // Add data rows
    CreditorreportList.forEach(item => {
      const dataRow = table.insertRow();
      Object.values(item).forEach(value => {
        const cell = dataRow.insertCell();
        cell.textContent = value;
        cell.style.border = '1px solid #000000';
      });
    });

    const totalsRow = table.insertRow();
    const numColumns = Object.keys(CreditorreportList[0]).length;
 
    for (let i = 0; i < numColumns - 4; i++) {
      const cell = totalsRow.insertCell();
      cell.textContent = '';
      cell.style.border = '1px solid #000000';
    }
 
    const totalLabelCell = totalsRow.insertCell();
    totalLabelCell.textContent = 'Total:';
    totalLabelCell.style.fontWeight = 'bold';
    totalLabelCell.style.border = '1px solid #000000';
 
    const totalDebitCell = totalsRow.insertCell();
    totalDebitCell.textContent = this.totalDebit || '0';
    totalDebitCell.style.fontWeight = 'bold';
    totalDebitCell.style.border = '1px solid #000000';
 
    const totalCreditCell = totalsRow.insertCell();
    totalCreditCell.textContent = this.totalCredit || '0';
    totalCreditCell.style.fontWeight = 'bold';
    totalCreditCell.style.border = '1px solid #000000';

    const emptycell = totalsRow.insertCell();
    emptycell.textContent = ' ';
    emptycell.style.fontWeight = 'bold';
    emptycell.style.border = '1px solid #000000';

    // Convert the table to a data URI
    const tableHtml = table.outerHTML;
    const dataUri = 'data:application/vnd.ms-excel;base64,' + btoa(unescape(encodeURIComponent(tableHtml)));

    // Trigger download
    const link = document.createElement('a');
    link.href = dataUri;
    link.download = 'Creditor Report.xls';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }

  toggleOptions(data: any) {
    if (this.showOptionsDivId === data) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = data;
    }
  }

}

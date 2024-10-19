import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AES } from 'crypto-js';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-acc-rpt-debtorreport',
  templateUrl: './acc-rpt-debtorreport.component.html',
  styleUrls: ['./acc-rpt-debtorreport.component.scss']
})
export class AccRptDebtorreportComponent {
  debtorreport_list: any[] = [];
  responsedata: any;
  totalCredit: any;
  totalDebit: any;
  showOptionsDivId:any;
  total_Gross: any;
  total_openingbalance: any;
  tds_amount: any;

  constructor(public service :SocketService,private router: Router,private route: ActivatedRoute,private NgxSpinnerService: NgxSpinnerService) {
    
  }

  ngOnInit(): void {
    this.AccTrnDebtorReportsummary();

    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });

  }

  AccTrnDebtorReportsummary(){

    this.NgxSpinnerService.show();
    var url='AccTrnBankbooksummary/GetDebitorReportSummary'      
    this.service.get(url).subscribe((result:any)=>{   
      this.responsedata=result;
      this.debtorreport_list = this.responsedata.GetDebitorreport_List;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#debtorreport_list').DataTable( );
      }, 1);
      
      if (this.debtorreport_list != null) {
      this.totalCredit = this.debtorreport_list.reduce((total: number, data: any) => {const numericValue = parseFloat(data.credit_amount.replace(/,/g, ''));return total + (isNaN(numericValue) ? 0 : numericValue);}, 0);
      this.totalDebit = this.debtorreport_list.reduce((total: number, data: any) => {const numericValue = parseFloat(data.debit_amount.replace(/,/g, ''));return total + (isNaN(numericValue) ? 0 : numericValue);}, 0);
      this.total_openingbalance = this.debtorreport_list.reduce((total: number, data: any) => {const numericValue = parseFloat(data.opening_amount.replace(/,/g, ''));return total + (isNaN(numericValue) ? 0 : numericValue);}, 0);
      this.tds_amount = this.debtorreport_list.reduce((total: number, data: any) => {const numericValue = parseFloat(data.tds.replace(/,/g, ''));return total + (isNaN(numericValue) ? 0 : numericValue);}, 0);
      this.total_Gross = this.debtorreport_list.reduce((total: number, data: any) => {const numericValue = parseFloat(data.closing_amount.replace(/,/g, ''));return total + (isNaN(numericValue) ? 0 : numericValue);}, 0);
      

      // Format the totals with commas and .00
      const formatter = new Intl.NumberFormat('en-US', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
      });

      this.totalCredit = formatter.format(this.totalCredit);
      this.totalDebit = formatter.format(this.totalDebit);
      this.total_Gross = formatter.format(this.total_Gross);
      this.total_openingbalance = formatter.format(this.total_openingbalance);
      this.tds_amount = formatter.format(this.tds_amount);
    }
      
  });
  }

  calculateTotal(): number {

    let totalAmount = 0;

    for (const data of this.debtorreport_list) {

      totalAmount += parseFloat(data.debit_amount);

    }
    return totalAmount;
  }

  onclick(params:any){
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.router.navigate(['/finance/AccRptDebtorReportView',encryptedParam]) 
  }

  exportExcel(): void {
    const debtorreportlist = this.debtorreport_list.map(item => ({
      'Customer Name': item.customer_name || '',
      'Opening Amount': item.opening_amount || '',
      'Debit Amount': item.debit_amount || '',
      'Credit Amount': item.credit_amount || '',
      'Closing Amount': item.closing_amount || ''
    }));
  
    // Create a new table element
    const table = document.createElement('table');
  
    // Add header row with background color
    const headerRow = table.insertRow();
    Object.keys(debtorreportlist[0]).forEach(header => {
      const cell = headerRow.insertCell();
      cell.textContent = header;
      cell.style.backgroundColor = '#00317a';
      cell.style.color = '#FFFFFF';
      cell.style.fontWeight = 'bold';
      cell.style.border = '1px solid #000000';
    });
  
    // Add data rows
    debtorreportlist.forEach(item => {
      const dataRow = table.insertRow();
      Object.values(item).forEach(value => {
        const cell = dataRow.insertCell();
        cell.textContent = value;
        cell.style.border = '1px solid #000000';
      });
    });
  

    const totalsRow = table.insertRow();
    const numColumns = Object.keys(debtorreportlist[0]).length;
  
  
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
    link.download = 'Debtor Report.xls';
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
  click360(customergid: any){
    const key = 'storyboard';
    const param = customergid;
    const lspage1 = 'debtorReport';
    const customer_gid = AES.encrypt(param,key).toString();
    const lspage = AES.encrypt(lspage1,key).toString();
    this.router.navigate(['/smr/SmrTrnSales360',customer_gid,customer_gid,customer_gid,lspage]);
  }
  DetailedReport(account_gid: any, customer_gid: any){
    debugger
    const key = 'storyboard';
    const param = account_gid;
    const param1 = customer_gid;
    const accountgid = AES.encrypt(param,key).toString();
    const customergid = AES.encrypt(param1,key).toString();
    this.router.navigate(['/finance/AccRptDetailedReport',accountgid,customergid])
  }
}

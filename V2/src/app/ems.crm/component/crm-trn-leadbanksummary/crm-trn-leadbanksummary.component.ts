import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ExcelService } from 'src/app/Service/excel.service';
import * as XLSX from 'xlsx';
import { Subject } from 'rxjs';
interface LeadReportItem {
  Leadbank_Id: string;
  Leadbank_Name: string;
  Remarks: string;
  Leadbankcontact_Name: string;
  Mobile_Number: string;
  Email_Address: string;
  Region_Name: string;
  Leadbank_Address1: string;
  Leadbank_Address2: string;
  Leadbank_City: string;
  Leadbank_State: string;
  Leadbank_Pin: string;
  Country_Name: string;
  Created_Date: string;
  Source_Name: string;
  Customer_Type: string;
}
interface ILeadBank {
  //sourceedit_name: any;
  remarks: string;
  created_date: string;
  company_name: string;
  leadbank_name: string;
  leadbankcontact_name: string;
  contact_details: string;
  region_name: string;
  source_name: string;
  created_by: string;
  lead_status: string;
  assign_to: string;
  leadbank_gid: string;
  companyedit_name: string;



}
@Component({
  selector: 'app-crm-trn-leadbanksummary',
  templateUrl: './crm-trn-leadbanksummary.component.html',
  styleUrls: ['./crm-trn-leadbanksummary.component.scss'],
  styles: [`
  table thead th, 
  .table tbody td { 
   position: relative; 
  z-index: 0;
  } 
  .table thead th:last-child, 
  
  .table tbody td:last-child { 
   position: sticky; 
  
  right: 0; 
   z-index: 0; 
  
  } 
  .table td:last-child, 
  
  .table th:last-child { 
  
  padding-right: 50px; 
  
  } 
  .table.table-striped tbody tr:nth-child(odd) td:last-child { 
  
   background-color: #ffffff; 
    
    } 
    .table.table-striped tbody tr:nth-child(even) td:last-child { 
     background-color: #f2fafd; 
  
  } 
  `]
})
export class CrmTrnLeadbanksummaryComponent {
  noResultsMessage: any;
  file!: File;
  reactiveForm!: FormGroup;
  reactiveFormEdit!: FormGroup;
  responsedata: any;
  parameterValue: any;
  leadbank_name: any;
  parameterValue1: any;
  leadbank_list: any[] = [];
  customertype_list: any[] = [];
  leadbank_list1: any[] = [];
  leadbank_list2: any[] = [];
  LeadBankCountList: any[] = [];
  LeadsCountList: any[] = [];
  firstCustomertype: any;
  firstCustomertype2: any;
  firstCustomertype3: any;
  leadbank!: ILeadBank;
  remarks: any;
  fileInputs: any;
  isExcelFile!: boolean;
  spinnerEnabled = false;
  dataSheet = new Subject();
  data: any[] = [];
  showOptionsDivId: any;
  filteredItems: any[] = [];
  currentPage: number = 1; // Current page number
  itemsPerPage: number = 200; // Number of items to display per page
  totalItems: number = 0;
  itemsPerPageOptions: number[] = [200, 500, 1000, 1500];
  sortBy: string = '';
  sortOrder: boolean = true;
  maxSize: number = 5;
  constructor(private excelService: ExcelService, private formBuilder: FormBuilder, private route: Router, private router: Router, private ToastrService: ToastrService, public service: SocketService, private NgxSpinnerService: NgxSpinnerService) {
    this.leadbank = {} as ILeadBank;
  }
  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.GetLeadbankSummary2();
    this.reactiveForm = new FormGroup({

      company_name: new FormControl(this.leadbank.company_name, [
        Validators.required,
      ]),
      leadbankcontact_name: new FormControl(this.leadbank.leadbankcontact_name, [
        Validators.required,
      ]),
      leadbank_name: new FormControl(this.leadbank.leadbank_name),
    });
    this.reactiveFormEdit = new FormGroup({
      companyedit_name: new FormControl(this.leadbank.companyedit_name, [
        Validators.required,
      ]),
      leadbank_gid: new FormControl(''),
    });
    var api = 'Leadbank/GetLeadBankCount'
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.LeadBankCountList = this.responsedata.LeadBankCount_List;
    });
  }

  GetLeadbankSummary2() {
    this.NgxSpinnerService.show();
    const url = 'Leadbank/GetLeadbankSummary2';
    this.service.get(url).subscribe(
      (result: any) => {
        this.leadbank_list2 = result.leadbank_list2;
        this.filteredItems = this.leadbank_list2;
        this.totalItems = this.leadbank_list2.length;
        this.NgxSpinnerService.hide();
      },
      (error) => {
        this.NgxSpinnerService.hide();
        console.error('Error fetching data', error);
      }
    );

  }
  search(event: Event) {
    const input = event.target as HTMLInputElement;
    const query = input?.value.trim().toLowerCase(); // Trim whitespace and convert to lowercase
    if (!query) {
      this.filteredItems = this.leadbank_list2;
    } else {
      this.filteredItems = this.leadbank_list2.filter((item: any) => {
        // Convert all properties to lowercase before comparing
        const leadbankName = item.leadbank_name.toLowerCase();
        const contactDetails = item.contact_details.toLowerCase();
        const regionName = item.region_name.toLowerCase();
        const sourceName = item.source_name.toLowerCase();
        const createdBy = item.created_by.toLowerCase();

        // Filter items based on the main list properties
        const matchesMainList =
          leadbankName.includes(query) ||
          contactDetails.includes(query) ||
          regionName.includes(query) ||
          sourceName.includes(query) ||
          createdBy.includes(query);

        // Return true if any of the conditions match
        return matchesMainList;
      });
    }
    this.totalItems = this.filteredItems.length; // Update totalItems based on filtered results
    this.currentPage = 1; // Reset currentPage to 1 when performing a new search
    if (this.totalItems === 0) {
      // Display "No matching records found" message if no items match the query
      this.noResultsMessage = "No matching records found";
    } else {
      this.noResultsMessage = "";
    }
  }

  get pagedItems(): any[] {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    return this.filteredItems.slice(startIndex, endIndex);
  }
  sort(field: string) {
    if (this.sortBy === field) {
      this.sortOrder = !this.sortOrder; // Toggle sort order
    } else {
      this.sortBy = field;
      this.sortOrder = true; // Default to ascending
    }
    this.filteredItems.sort((a: any, b: any) => {
      if (a[field] < b[field]) {
        return this.sortOrder ? -1 : 1;
      } else if (a[field] > b[field]) {
        return this.sortOrder ? 1 : -1;
      } else {
        return 0;
      }
    });
  }

  onItemsPerPageChange(): void {
    this.currentPage = 1; // Reset to the first page when items per page changes
  }

  pageChanged(event: any): void {
    this.currentPage = event.page;
  }


  get startIndex(): number {
    return (this.currentPage - 1) * this.itemsPerPage;
  }

  get endIndex(): number {
    return Math.min(this.startIndex + this.itemsPerPage, this.totalItems);
  }


  toggleOptions(leadbank_gid: any) {
    if (this.showOptionsDivId === leadbank_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = leadbank_gid;
    }
  }
  GetCustomerTypeSummary() {
    var api = 'Leadbank/GetCustomerTypeSummary'
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.customertype_list = this.responsedata.customertype_list1;
      this.firstCustomertype = this.customertype_list[0].customer_type1;
      this.firstCustomertype2 = this.customertype_list[1].customer_type1;
      this.firstCustomertype3 = this.customertype_list[2].customer_type1;
    });
  }
  onview(params: any) {
    debugger
    const secretKey = 'storyboarderp';
    const param = (params);
    const lspage1 = "LeadBankdistributor";
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    const lspage = AES.encrypt(lspage1, secretKey).toString();
    console.log(encryptedParam);
    this.route.navigate(['/crm/CrmTrnLeadbankview', encryptedParam, lspage])
  }
  onbranch(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    //console.log(param);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    console.log(encryptedParam);
    this.route.navigate(['/crm/CrmTrnLeadBankbranch', encryptedParam])
  }

  onedit(params: any) {
    this.NgxSpinnerService.show();
    const secretKey = 'storyboarderp';
    const param = (params);
    const lspage1 = "LeadBankdistributor";
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    const lspage = AES.encrypt(lspage1, secretKey).toString();
    console.log(encryptedParam);
    this.NgxSpinnerService.hide();
    this.route.navigate(['/crm/CrmTrnLeadbankedit', encryptedParam, lspage])
  }
  onaddcontact(params: any) {
    this.NgxSpinnerService.show();
    const secretKey = 'storyboarderp';
    const param = (params);
    const leadbank_gid = AES.encrypt(param, secretKey).toString();
    console.log(leadbank_gid);
    this.NgxSpinnerService.hide();
    this.route.navigate(['/crm/CrmTrnLeadbankcontact', leadbank_gid])
  }
  ////////////Delete ////////

  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }
  ondelete() {
    this.NgxSpinnerService.show();
    var url = 'Leadbank/deleteLeadbankSummary'
    let param = {
      leadbank_gid: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.Status == true) {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });

        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message)

      }
      else {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });

        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)
      }
      this.GetLeadbankSummary2();

    });
  }
  onChange1(event: any) {
    this.file = event.target.files[0];
    // var api='Employeelist/EmployeeProfileUpload'
    // //console.log(this.file)
    //   this.service.EmployeeProfileUpload(api,this.file).subscribe((result:any) => {
    //     this.responsedata=result;
    //   });
  }
  // importexcel() {
  //   this.NgxSpinnerService.show();
  //   let formData = new FormData();
  //   if (this.file != null && this.file != undefined) {
  //     window.scrollTo({
  //       top: 0, // Code is used for scroll top after event done
  //     });
  //     formData.append("file", this.file, this.file.name);
  //     var api = 'Leadbank/LeadReportImport'
  //     this.service.postfile(api, formData).subscribe((result: any) => {
  //       this.responsedata = result;
  //       this.NgxSpinnerService.hide();
  //       this.ToastrService.success("Excel Uploaded Successfully");
  //       window.location.reload();

  //     });
  //   }
  // }
  importexcel(evt: any) {
    let header;
    const target: DataTransfer = <DataTransfer>(evt.target);
    this.isExcelFile = !!this.file.name.match(/(.xls|.xlsx)/);

    if (this.isExcelFile) {

      debugger
      this.spinnerEnabled = true;
      const reader: FileReader = new FileReader();

      reader.onload = (e: any) => {
        /* read workbook */
        const bstr: string = e.target.result;
        const wb: XLSX.WorkBook = XLSX.read(bstr, { type: 'binary' });

        /* grab first sheet */
        const wsname: string = wb.SheetNames[0];
        const ws: XLSX.WorkSheet = wb.Sheets[wsname];

        /* save data */
        this.data = XLSX.utils.sheet_to_json(ws);
      };

      reader.readAsBinaryString(this.file);
      reader.onloadend = (e) => {
        if (this.data && this.data.length > 0) {
          debugger;
          let formData = new FormData();
          for (let rowIndex = 0; rowIndex < this.data.length; rowIndex++) {
            let row = this.data[rowIndex];
            if (!row['Company Name']) {
              this.fileInputs = null;
              this.ToastrService.warning(`Company Name cannot be null (Row: ${rowIndex + 1})`);
              return;
            }
            if (row['Customer Type'] !== 'Individual' && row['Customer Type'] !== 'Corporate') {
              this.fileInputs = null;
              this.ToastrService.warning(`Customer Type must be either Individual or Corporate (Row: ${rowIndex + 1})`);
              return;
            }
          }


          // Proceed with file upload if file is present
          if (this.file != null && this.file != undefined) {
            window.scrollTo({
              top: 0, // Scroll to top after the event
            });

            formData.append("file", this.file, this.file.name);
            this.NgxSpinnerService.show();

            var api = 'Leadbank/LeadReportImport';
            this.service.postfile(api, formData).subscribe((result: any) => {
              this.responsedata = result;

              if (result.status == false) {
                this.NgxSpinnerService.hide();
                this.fileInputs = null;
                this.ToastrService.warning('Error occurred while uploading the Excel file');
                this.GetLeadbankSummary2();
                window.location.reload();
              } else {
                this.NgxSpinnerService.hide();
                this.fileInputs = null;
                this.ToastrService.success("Excel uploaded successfully");
                this.GetLeadbankSummary2();
                window.location.reload();
              }
            });
          }
        } else {
          this.ToastrService.warning('No data found in the Excel file');
        }


      };
    } else {
      //this.resetInputFile();
      alert("Please select a valid Excel file.");
    }
  }


  exportExcel1(): void {
    const LeadReport = this.leadbank_list2.map(item => ({
      'Leadbank Name': item.leadbank_name || '',
      'Leadbankcontact Name': item.leadbankcontact_name || '',
      'Mobile Number': item.mobile || '',  // Ensure mobile number is treated as text
      'Email Address': item.email || '',
      Region: item.region_name || '',
      'Address Line 1': item.address1 || '',
      'Address Line 2': item.address2 || '',
      City: item.leadbank_city || '',
      State: item.leadbank_state || '',
      PinCode: item.leadbank_pin || '',
      Country: item.country_name || '',
      'Created Date': item.created_date || '',
      Source: item.source_name || '',
      'Customer Type': item.customer_type || '',
      Remarks: item.remarks || '',

    }));
  
    // Create a worksheet from the data
    const worksheet = XLSX.utils.json_to_sheet(LeadReport);
  
    // Apply styling to the header (first row)
    const header = Object.keys(LeadReport[0]);
    header.forEach((col, idx) => {
      const cellAddress = XLSX.utils.encode_cell({ r: 0, c: idx });
      if (worksheet[cellAddress]) {
        worksheet[cellAddress].s = {
          font: { bold: true, color: { rgb: "FFFFFF" } },  // White text
          fill: { fgColor: { rgb: "00317a" } },  // Dark blue background
          alignment: { horizontal: 'center', vertical: 'center' },
          border: {
            top: { style: "thin", color: { rgb: "000000" } },  // Border - top
            bottom: { style: "thin", color: { rgb: "000000" } },  // Border - bottom
            left: { style: "thin", color: { rgb: "000000" } },  // Border - left
            right: { style: "thin", color: { rgb: "000000" } },  // Border - right
          },
        };
      }
    });
  
    // Format the Mobile_Number column as text
    const mobileNumberColumnIndex = header.indexOf('Mobile Number');
    LeadReport.forEach((_, rowIndex) => {
      const cellAddress = XLSX.utils.encode_cell({ r: rowIndex + 1, c: mobileNumberColumnIndex });
      const cell = worksheet[cellAddress];
      if (cell) {
        cell.z = '@';  // Set the format to text for mobile numbers
      }
    });
  
    // Set column widths
    worksheet['!cols'] = header.map(() => ({ wch: 20 }));  // Set column width
  
    // Create a new workbook and add the styled worksheet
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, 'Lead');
  
    // Export the workbook as an Excel file
    XLSX.writeFile(workbook, 'Lead.xlsx');
  }
  
  oncontact(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    //console.log(param);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    console.log(encryptedParam);
    this.route.navigate(['/crm/CrmTrnLeadbankcontact', encryptedParam])
  }
  downloadfileformat() {
    let link = document.createElement("a");
    link.download = "Lead Template";
    link.href = "assets/media/Excels/leadbanktemplate/Lead_Template.xlsx";
    link.click();
  }

  onclose() {
    this.fileInputs = null;
  }
  onadd() {
    const secretKey = 'storyboarderp';
    const lspage1 = 'LeadBankdistributor';
    const lspage = AES.encrypt(lspage1, secretKey).toString();
    this.route.navigate(['/crm/CrmTrnLeadbankadd', lspage]);
  }
  popmodal(parameter: string, parameter1: string) {
    this.parameterValue1 = parameter;
    this.remarks = parameter;
    this.leadbank_name = parameter1;
  }

  canceluploadexcel() {
    this.fileInputs = null;
  }
}


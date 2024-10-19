import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';


interface Enquiry{
  leadstage_gid: string;
  enquiry_gid: string;
  leadstage_name: string;
  internal_notes: string;
  campaign_title: string;
  employee_name: string;
  campaign_gid: string;
  branch_gid: string;
  campaign: string;
  created_by: string;
  vendor_gid:string;


}


@Component({
  selector: 'app-pmr-trn-raise-enquiry',
  templateUrl: './pmr-trn-raise-enquiry.component.html',
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
export class PmrTrnRaiseEnquiryComponent {

  customerenquiryForm: FormGroup | any;
  ReassignForm: FormGroup | any;
  cusenquiry_list: any [] = [];
  leads_list: any [] = [];
  Team_list: any [] = [];
  Employee_list: any [] = []; 
  enquiry!:Enquiry;
  responsedata: any;
  selectstage: any;
  selectteam: any;
  selectemp:any;
  parameterValue1: any;
  getData: any;
  showOptionsDivId: any; 
  rows: any[] = [];



  constructor(private formBuilder: FormBuilder,private router:Router, private ToastrService: ToastrService, public service: SocketService, private route: ActivatedRoute,private NgxSpinnerService:NgxSpinnerService) {
    this.enquiry = {} as Enquiry;
  }
  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }
  
  ngOnInit(): void {
    this.GetCustomerEnquirySummary();

    this.customerenquiryForm = new FormGroup ({
      enquiry_gid: new FormControl(''),
      leadstage_gid: new FormControl(''),
      leadstage_name : new FormControl (this.enquiry.leadstage_name, [Validators.required]),
      internal_notes: new FormControl (this.enquiry. internal_notes, [Validators.required])
    });

    this.ReassignForm = new FormGroup ({
      enquiry_gid: new FormControl(''),
      campaign_gid: new FormControl(''),
      employee_gid: new FormControl(''),
      campaign_title: new FormControl(''),
      branch_name: new FormControl(''),
      employee_name : new FormControl ('') 
    });
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
 
  }

  GetCustomerEnquirySummary() {
    this.NgxSpinnerService.show();
    var url = 'PmrTrnRaiseEnquiry/GetVendorEnquirySummary'
    this.service.get(url).subscribe((result: any) => {
      // Check if the API response contains the expected array
      if (result && Array.isArray(result.cusenquiry_list)) {
        // If the API response is as expected, assign the array to cusenquiry_list
        this.cusenquiry_list = result.cusenquiry_list;
  
        // Destroy the DataTable if it exists
        if ($.fn.dataTable.isDataTable('#cusenquiry_list')) {
          $('#cusenquiry_list').DataTable().clear().destroy();
        }
  
        // Render the DataTable after a short delay to ensure data is processed
        setTimeout(() => {
          $('#cusenquiry_list').DataTable();
          this.NgxSpinnerService.hide();
        }, 100); // Increase the delay if needed
      } else {
        // Handle unexpected API response or empty data
        console.error('Unexpected API response or empty data');
        this.NgxSpinnerService.hide();
      }
    });
  }
  

    ////////// Close Popup Validation ////////////
    get leadstage_name() {
      return this.customerenquiryForm.get('leadstage_name')!;
    }
   
  /////// Re-Assign Popup Validation ////////
  get campaign_title(){
    return this.ReassignForm.get('campaign_title')!;
  }

  get employee_name(){
    return this.ReassignForm.get('employee_name')!;
  }
  

  onadd()
  {
        this.router.navigate(['/pmr/PmrTrnRaiseEnquiryaddNew']);

  }
  onview(params:any){
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.router.navigate(['/pmr/PmrTrnVendorenquiryView',encryptedParam]) 
  }
  // openModaledit(){
  //   // const secretKey = 'storyboarderp';
  //   //   const param = (params);
  //   //   const encryptedParam = AES.encrypt(param,secretKey).toString();,encryptedParam
  //     this.router.navigate(['/smr/SmrTrnEditCustomerEnquiry'])
  // }
  openModalclose(){

  }

  ////// Close Enquiry starts ////////


  // onsubmit(){
  //   if (this.customerenquiryForm.value.leadstage_name != null && this.customerenquiryForm.value.leadstage_name != '') {
  //     for (const control of Object.keys(this.customerenquiryForm.controls)) {
  //       this.customerenquiryForm.controls[control].markAsTouched();
  //     }
  //     this.customerenquiryForm.value;

      
  //     var url = 'SmrTrnCustomerEnquiry/GetUpdatedCloseEnquiry'

  //     this.service.post(url,this.customerenquiryForm.value).pipe().subscribe((result:any)=>{
  //       this.responsedata=result;
  //       if(result.status ==false){
  //         this.ToastrService.warning(result.message)
  //         this.GetCustomerEnquirySummary();
  //       }
  //       else{
  //         this.ToastrService.success(result.message)
  //         this.GetCustomerEnquirySummary();
  //       }
  //   }); 

  //   }
  //   else {
  //     this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
  //   }

  // }
  ///// Close Enquiry Ends //////


  ////// Re-assing Starts //////

  openModal(parameter: string){
    this.parameterValue1 = parameter;
    this.ReassignForm.get("branch_name")?.setValue(this.parameterValue1.branch_name);
    this.ReassignForm.get("enquiry_gid")?.setValue(this.parameterValue1.enquiry_gid);
  }
 
  // onupdate(){

  //   if (this.ReassignForm.value.employee_name != null && this.ReassignForm.value.employee_name != '') {
  //     for (const control of Object.keys(this.ReassignForm.controls)) {
  //       this.ReassignForm.controls[control].markAsTouched();
  //     }
  //     this.ReassignForm.value;

      
  //     var url = 'SmrTrnCustomerEnquiry/GetUpdatedReAssignEnquiry'

  //     this.service.post(url,this.ReassignForm.value).pipe().subscribe((result:any)=>{
  //       this.responsedata=result;
  //       if(result.status ==false){
  //         this.ToastrService.warning(result.message)
  //         this.GetCustomerEnquirySummary();
  //       }
  //       else{
  //         this.ToastrService.success(result.message)
  //         this.GetCustomerEnquirySummary();
  //       }
  //   }); 

  //   }
  //   else {
  //     this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
  //   }


  // }

  onclose(){
    this.customerenquiryForm.reset();
  }
  
  openrequest(params:any){
    const secretKey = 'storyboarderp';
    const param = (params);
    console.log(param)
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['./smr/SmrTrnRaiseproposal', encryptedParam])
  }
  oncloses(){
    this.ReassignForm.reset();
  }
  
  openquote(params:any){
    debugger
    const secretKey = 'storyboarderp';
    const param = (params);
    console.log(param)
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['./smr/SmrTrnRaisequote', encryptedParam])
  }

  splitIntoLines(text: string, lineLength: number): string[] {
    const lines = [];
    for (let i = 0; i < text.length; i += lineLength) {
      lines.push(text.substr(i, lineLength));
    }
    return lines;
  }
  
}


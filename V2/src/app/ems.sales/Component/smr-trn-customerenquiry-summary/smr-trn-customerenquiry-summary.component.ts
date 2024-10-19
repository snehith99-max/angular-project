import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';


interface Enquiry {
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


}

@Component({
  selector: 'app-smr-trn-customerenquiry-summary',
  templateUrl: './smr-trn-customerenquiry-summary.component.html',
  styleUrls: ['./smr-trn-customerenquiry-summary.component.scss'],
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
export class SmrTrnCustomerenquirySummaryComponent {
  customerenquiryForm: FormGroup | any;
  ReassignForm: FormGroup | any;
  cusenquiry_list: any[] = [];
  cusaddress_list: any[] = [];
  leads_list: any[] = [];
  Team_list: any[] = [];
  Employee_list: any[] = [];
  enquiry!: Enquiry;
  responsedata: any;
  selectstage: any;
  selectteam: any;
  selectemp: any;
  parameterValue1: any;
  address : any;
  showOptionsDivId:any;
  rows:any []=[];
  constructor(private formBuilder: FormBuilder, private router: Router, public NgxSpinnerService: NgxSpinnerService,
    private ToastrService: ToastrService, public service: SocketService, private route: ActivatedRoute) {
    this.enquiry = {} as Enquiry;
  }

  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.GetCustomerEnquirySummary();

    this.customerenquiryForm = new FormGroup({
      enquiry_gid: new FormControl(''),
      leadstage_gid: new FormControl(''),
      leadstage_name: new FormControl(this.enquiry.leadstage_name, [Validators.required]),
      internal_notes: new FormControl(this.enquiry.internal_notes, [Validators.required])
    });

    this.ReassignForm = new FormGroup({
      enquiry_gid: new FormControl(''),
      campaign_gid: new FormControl(''),
      employee_gid: new FormControl(''),
      campaign_title: new FormControl(''),
      branch_name: new FormControl(''),
      employee_name: new FormControl('')
    });

    ///// Lead Dropdrown /////
    var url = 'SmrTrnCustomerEnquiry/GetLeadDtl'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.leads_list = this.responsedata.GetLeadDtl;

    });

    ////// Team Dropdown ////////
    var url = 'SmrTrnCustomerEnquiry/GetTeamDtl'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.Team_list = this.responsedata.GetTeamDtl;
    });

    ////// Employee Dropdown ////////
    var url = 'SmrTrnCustomerEnquiry/GetEmployeeDtl'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.Employee_list = this.responsedata.GetEmployeeDtl;
    });

  }

  onLead(leadbank_gid : any){
    debugger
    const secretKey = 'storyboarderp';
    const param = (leadbank_gid);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/smr/SmrTrnLeadtocustomer', encryptedParam])
  }

  GetCustomerEnquirySummary() {
    
    var api = 'SmrTrnCustomerEnquiry/GetCustomerEnquirySummary';
    //this.NgxSpinnerService.show()
    this.service.get(api).subscribe((result: any) => {
      $('#cusenquiry_list').DataTable().destroy();
      this.responsedata = result;
      this.cusenquiry_list = this.responsedata.cusenquiry_list;
      console.log(this.cusenquiry_list)
      setTimeout(() => {
        $('#cusenquiry_list').DataTable({
        });
      }, 1);
    });

  }

  GetCustomerAddress(customer_gid: any) {
    debugger
    var api = 'SmrTrnCustomerEnquiry/GetOnChangeLead';
    let param = {
      leadbank_gid: customer_gid
    }
    this.service.getparams(api, param).subscribe((result: any) => {
      $('#cusaddress_list').DataTable().destroy();
      this.responsedata = result;
      this.cusaddress_list = this.responsedata.GetLead;
      this.address = `${this.cusaddress_list[0].leadbankbranch_name}<br>` + 
               `${this.cusaddress_list[0].contact_email}<br>` +
               `${this.cusaddress_list[0].contact_number}<br>` +
               `${this.cusaddress_list[0].contact_address}<br>` +
               `${this.cusaddress_list[0].address2}<br>` +
               `${this.cusaddress_list[0].city}<br>` +
               `${this.cusaddress_list[0].state}<br>` +
               `${this.cusaddress_list[0].country_name}`;
      setTimeout(() => {
        $('#cusaddress_list').DataTable({
        });
      }, 1);
    });

  }


  ////////// Close Popup Validation ////////////
  get internal_notes() {
    return this.customerenquiryForm.get('internal_notes')!;
  }

  /////// Re-Assign Popup Validation ////////
  get campaign_title() {
    return this.ReassignForm.get('campaign_title')!;
  }

  get employee_name() {
    return this.ReassignForm.get('employee_name')!;
  }
  getColor(customerRating: string): string {
    switch (customerRating) {
      case 'Hot':
        return '#e20505';
      case 'Cold':
        return '#0041bb';
      case 'Warm':
        return '#ffd900';
      default:
        return 'default-row'; // Default CSS class
    }
  }

  onadd() {
    this.router.navigate(['/smr/SmrTrnCustomerraiseenquiry']);

  }

  openModalclose(parameter: any) {
    debugger
    this.parameterValue1 = parameter;
    this.customerenquiryForm.get("enquiry_gid")?.setValue(this.parameterValue1);
  }
  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }

  ////// Close Enquiry starts ////////


  onsubmit() {
    debugger
    if (this.customerenquiryForm.value.internal_notes != null && this.customerenquiryForm.value.internal_notes != '') {
      for (const control of Object.keys(this.customerenquiryForm.controls)) {
        this.customerenquiryForm.controls[control].markAsTouched();
      }


      var url = 'SmrTrnCustomerEnquiry/GetUpdatedCloseEnquiry'

      this.service.post(url, this.customerenquiryForm.value).pipe().subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          this.ToastrService.warning(result.message)
          this.customerenquiryForm.reset()
          this.GetCustomerEnquirySummary();
        }
        else {
          this.ToastrService.success(result.message)
          this.customerenquiryForm.reset()
          this.GetCustomerEnquirySummary();
        }
      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }

  }

  onview(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    console.log(param)
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/smr/SmrTrnEnquiryView', encryptedParam])

  }



  ////// Re-assing Starts //////

  openModal(parameter: string) {
    this.parameterValue1 = parameter;
    this.ReassignForm.get("branch_name")?.setValue(this.parameterValue1.branch_name);
    this.ReassignForm.get("enquiry_gid")?.setValue(this.parameterValue1.enquiry_gid);
  }

  onupdate() {

    if (this.ReassignForm.value.employee_name != null && this.ReassignForm.value.employee_name != '') {
      for (const control of Object.keys(this.ReassignForm.controls)) {
        this.ReassignForm.controls[control].markAsTouched();
      }
      this.ReassignForm.value;


      var url = 'SmrTrnCustomerEnquiry/GetUpdatedReAssignEnquiry'

      this.service.post(url, this.ReassignForm.value).pipe().subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          this.ToastrService.warning(result.message)
          this.GetCustomerEnquirySummary();
        }
        else {
          this.ToastrService.success(result.message)
          this.GetCustomerEnquirySummary();
        }
      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }


  }

  onclose() {
    this.customerenquiryForm.reset();
  }

  openrequest(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    console.log(param)
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/smr/SmrTrnRaiseproposal', encryptedParam])
  }
  oncloses() {
    this.ReassignForm.reset();
  }

  openquote(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    console.log(param)
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['./smr/SmrTrnRaisequote', encryptedParam])
  }

  openModaledit(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    console.log(param)
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['./smr/SmrTrnEditCustomerEnquiry', encryptedParam])
  }
}

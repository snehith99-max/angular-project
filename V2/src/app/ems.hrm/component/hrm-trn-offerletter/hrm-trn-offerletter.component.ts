import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';

@Component({
  selector: 'app-hrm-trn-offerletter',
  templateUrl: './hrm-trn-offerletter.component.html',
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

export class HrmTrnOfferletterComponent {
  // showOptionsDivId: any; 
  offer_list: any[] = [];
  consider_list: any[] = [];
  responsedata: any;
  reactiveFormadd!: FormGroup;
  company_code: any;
  parameterValue1: any;
  reactiveFormEdit!: FormGroup;
  reactiveForm!: FormGroup;
  jobtype_list: any[] = [];
  first_name: any;
  designation_name: any;
  branch_name: any;
  SocketService: any;

  constructor(private formBuilder: FormBuilder, private route: ActivatedRoute, private NgxSpinnerService: NgxSpinnerService, private router: Router, private ToastrService: ToastrService, public service: SocketService) {
  }

  ngOnInit(): void {
    var url = 'OfferLetter/OfferLetterSummary'

    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.offer_list = this.responsedata.Offersummary_list;
      setTimeout(() => {
        $('#offer_list').DataTable();
      },);

      this.first_name = this.offer_list[0].first_name;
      this.designation_name = this.offer_list[0].designation_name;
      this.branch_name = this.offer_list[0].branch_name;
    });

    this.reactiveForm = new FormGroup({
      offer_gid: new FormControl(''),
      employee_gid: new FormControl(''),
      user_code: new FormControl(''),
      user_password: new FormControl(''),
      confirmpassword: new FormControl(''),
      jobtype: new FormControl(''),
      active_flag: new FormControl(''),
      user_status: new FormControl('')
    });

    var api1 = 'HrmTrnAdmincontrol/Getjobtypedropdown'
    this.service.get(api1).subscribe((result: any) => {
      this.jobtype_list = result.Getjobtypenamedropdown;
    });

  }

  openModaledit(parameter: string) {
    debugger;
    this.parameterValue1 = parameter
    this.first_name = this.parameterValue1.first_name;
    this.designation_name = this.parameterValue1.designation_name;
    this.branch_name = this.parameterValue1.branch_name;

    this.reactiveForm.get("offer_gid")?.setValue(this.parameterValue1.offer_gid);
  }

  update() {
    var url = 'OfferLetter/ConfirmEmployee';
    var params = {
      offer_gid: this.reactiveForm.value.offer_gid,
    }

    this.NgxSpinnerService.show();
    this.service.getparams(url, params).subscribe((result: any) => {
      if (result.status == true) {
        this.ToastrService.success(result.message);
        this.NgxSpinnerService.hide();
        this.ngOnInit();
      }
      else {
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();
      }
    })
  }

  onconfirmation(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/hrm/HrmMstEmployeeconfirmation', encryptedParam])
  }

  PrintPDF(offer_gid: string) {
    debugger
    const api = 'OfferLetter/GetOfferletterpdf';
    this.NgxSpinnerService.show()
    let param = {
      offer_gid: offer_gid,
    }

    this.service.getparams(api, param).subscribe((result: any) => {
      if (result != null) {
        this.service.filedownload1(result);
      }
      this.NgxSpinnerService.hide()
    });
  }
}

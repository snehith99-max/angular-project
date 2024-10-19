import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from 'src/environments/environment';
import { AES } from 'crypto-js';

interface IAppraisal {

  employee_name: string;
}

@Component({
  selector: 'app-hrm-trn-appraisalmanagement',
  templateUrl: './hrm-trn-appraisalmanagement.component.html',
  styleUrls: ['./hrm-trn-appraisalmanagement.component.scss']
})
export class HrmTrnAppraisalmanagementComponent {
  reactiveForm!: FormGroup;
  appraisal!: IAppraisal;
  employee_list: any[] = [];
  review_list: any[] = [];
  responsedata: any;

  constructor(private formBuilder: FormBuilder, private route: ActivatedRoute, private router: Router, public NgxSpinnerService:NgxSpinnerService, private ToastrService: ToastrService, public service: SocketService) {
    this.appraisal = {} as IAppraisal;
  }

  ngOnInit(): void {

    this.reactiveForm = new FormGroup({
      employee_name: new FormControl(this.appraisal.employee_name, [
        Validators.required,
        
      ]),
    });
    var url = 'HrmTrnAppraisalManagement/GetEmployeeDetail';
    this.service.get(url).subscribe((result: any) => {
      this.employee_list = result.GetEmployeeDetail;
    
    });
    var url = 'HrmTrnAppraisalManagement/GetAppraisalSummary'
    this.service.get(url).subscribe((result: any) => {

      this.responsedata = result;
      this.review_list = this.responsedata.reviewlist;
      setTimeout(() => {
        $('#review_list').DataTable();
      }, );


    });
  }

  get employee_name() {
    return this.reactiveForm.get('employee_name')!;
  }

  onsubmit() {
    const secretKey = 'storyboarderp';
    const param = this.reactiveForm.value.employee_name;
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.router.navigate(['/hrm/HrmTrnAppraisaladd',encryptedParam])
   
  }

  openModalappraisal(params:any){
    
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.router.navigate(['/hrm/HrmTrnAppraisal360',encryptedParam])
  }

  onclose(){
  this.reactiveForm.reset();
  }

}

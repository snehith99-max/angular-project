import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';



@Component({
  selector: 'app-pay-trn-pfmanagement',
  templateUrl: './pay-trn-pfmanagement.component.html',
  styleUrls: ['./pay-trn-pfmanagement.component.scss']
})
export class PayTrnPfmanagementComponent {

  reactiveFormEdit: FormGroup | any;
  employee_list: any[] = [];
  responsedata: any;
  addpfdetails_list: any;
  parameterValue1: any;
  

  constructor(private formBuilder: FormBuilder, private route: ActivatedRoute, private router: Router, private ToastrService: ToastrService, public service: SocketService) {
    this.reactiveFormEdit = new FormGroup({
      pf_no : new FormControl(''),
      pf_doj : new FormControl(''),
      experience:new FormControl(''),
      remarks:new FormControl(''),
      employee_gid:new FormControl('')
    })
   
  
  }
  ngOnInit(): void {
    this.reactiveFormEdit = new FormGroup({
      pf_no : new FormControl(''),
      pf_doj : new FormControl(''),
      experience:new FormControl(''),
      remarks:new FormControl(''),
      employee_gid:new FormControl('')
    })
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);
    //// Summary Grid//////
    var url = 'PayTrnPfManagement/GetPfManagementSummary'

    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.employee_list = this.responsedata.GetPfManagement_list;
      setTimeout(() => {
        $('#employee_list').DataTable();
      },);
    });
  }

  employeeassign() {
    this.router.navigate(['/payroll/PayTrnPfemployeeassign'])
  }
  onpfSubmit() {
    debugger
    var params = {
      employee_gid: this.employee_list[0].employee_gid,
      pf_no: this.reactiveFormEdit.value.pf_no,
      pf_doj: this.reactiveFormEdit.value.pf_doj,
      experience: this.reactiveFormEdit.value.experience,
      remarks : this.reactiveFormEdit.value.remarks
    }
    var url = 'PayTrnPfManagement/PostemployeeAssignSubmit'
   
    this.service.post(url, params).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
      }
      else {
        this.ToastrService.success(result.message)
      }
      this.reactiveFormEdit.reset();
    });

  }

  addpfpopup(employee_gid: any) {
    debugger
    var url = 'PayTrnPfManagement/GetEmployeePfSummary'
    let param = {
      employee_gid: employee_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.addpfdetails_list = this.responsedata.GetAddpfDetails_list;
      this.reactiveFormEdit.get("pf_no")?.setValue(this.addpfdetails_list[0].pf_no);
      this.reactiveFormEdit.get("pf_doj")?.setValue(this.addpfdetails_list[0].pf_doj);
      this.reactiveFormEdit.get("experience")?.setValue(this.addpfdetails_list[0].experience);
      this.reactiveFormEdit.get("remarks")?.setValue(this.addpfdetails_list[0].remarks);
    });
  }
}

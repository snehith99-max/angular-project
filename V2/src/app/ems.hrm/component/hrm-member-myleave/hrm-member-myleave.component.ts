import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup,Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

interface Ifileupload {
  leavetypegidfrodocupload: string;
}

@Component({
  selector: 'app-hrm-member-myleave',
  templateUrl: './hrm-member-myleave.component.html',
  styleUrls: ['./hrm-member-myleave.component.scss']
})

export class HrmMemberMyleaveComponent {
  radioSelected: any;
  leaveperiod: any;
  responsedata: any;
  leave_details: any;
  leavetypelist: any;
  leavetype_list: any[] = [];
  leavegid_list: any[] = [];
  reactiveFormadd: FormGroup;
  count_Sickavailable: any;
  count_Sickleavetaken: any;
  count_Casualleavetaken: any;
  count_casualavailable: any;
  parameterValue: any;
  leavesummary: any;
  leave: any;
  file1!: File;
  data: any;
  SocketService: any;
  leave_datefrom: any;
  parameterValue2: any;
  leave_days: any;
  fileupload!: Ifileupload;
  leaveFrom: any;
  applyleavecountlist: any;
  leavedays:any;

  constructor(private fb: FormBuilder, private route: ActivatedRoute, private NgxSpinnerService: NgxSpinnerService,  private router: Router, private service: SocketService, private ToastrService: ToastrService) {
    this.reactiveFormadd = new FormGroup({
      leave_gid: new FormControl(''),
      leave_from: new FormControl('',[Validators.required,Validators.pattern(/^\S.*$/)]),
      leave_to: new FormControl('',[Validators.required,Validators.pattern(/^\S.*$/)]),
      leave_days: new FormControl(''),
      leave_session: new FormControl(''),
      leavetype_gid: new FormControl(''),
      leavetype_name: new FormControl(''),
      leave_reason: new FormControl('', [Validators.required,Validators.pattern(/^\S.*$/)]),
      file1:new FormControl(''),
      leave_period:new FormControl('')
      
    })
    this.fileupload = {} as Ifileupload;
  }

  ngOnInit(): void {
    var api = 'ApplyLeave/GetLeaveCount';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.applyleavecountlist = this.responsedata.Applyleavecountlist;      
    });

    var api = 'ApplyLeave/getleavetype_name';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.leavetypelist = this.responsedata.leave_type_list;
    });

    var api = 'ApplyLeave/leavesummary';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.leave_details = this.responsedata.leave_list;
      setTimeout(() => {
        $('#leave_details').DataTable();
      }, 1);
    });

    var api = 'ApplyLeave/leavetype';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.leavetype_list = this.responsedata.leavetype_list;
      this.count_Sickleavetaken = this.leavetype_list[0].count_leavetaken;
      this.count_Sickavailable = this.leavetype_list[0].count_leaveavailable;
      this.count_casualavailable = this.leavetype_list[1].count_leaveavailable;
      this.count_Casualleavetaken = this.leavetype_list[1].count_leavetaken;
    });

    this.reactiveFormadd.get('leave_from')?.valueChanges.subscribe(() => {
      this.calculateLeaveDays();
    });
  
    this.reactiveFormadd.get('leave_to')?.valueChanges.subscribe(() => {
      this.calculateLeaveDays();
    });
  }
  onChange1(event: any) {
    this.file1 = event.target.files[0];
  }

  myModaladd(parameter: string) {
    this.parameterValue = parameter;
   // Check the value in the console
    this.reactiveFormadd.get('leavetype_name')?.setValue(this.parameterValue);
    this.reactiveFormadd.get('leavetype_gid')?.setValue(this.parameterValue);
  }
  onapplyleave() {
    var url = 'ApplyLeave/applyleavesubmit';
    this.service.post(url, this.reactiveFormadd.value).subscribe((result: any) => {
      this.fileupload.leavetypegidfrodocupload = result.leavetypegidfrodocupload;
      if (result.status == true) 
      {
        if (this.file1 != null && this.file1 != undefined){
        var url1 = "ApplyLeave/uploaddocument";
        const leavegid1 = this.fileupload.leavetypegidfrodocupload;
        let formData = new FormData();
        formData.append("photo", this.file1, this.file1.name);
        formData.append("leavetypegidfrodocupload",leavegid1);
        this.service.postfile(url1,formData).subscribe((result:any)=>{
          if (result.status == false){
            this.ToastrService.warning(result.message)
            this.reactiveFormadd.reset();

          }
          else{
            this.ToastrService.success(result.message)
            this.ngOnInit()


          }
          this.reactiveFormadd.reset();
        });
       }
       else{
        this.ToastrService.success(result.message)
        this.ngOnInit()

       }
      }
      else
      {
        this.ToastrService.warning(result.message)

      }
      this.reactiveFormadd.reset();
    });
  }

  delete(leave_gid: any) {
    this.parameterValue = leave_gid
  }

  ondelete() {
    this.NgxSpinnerService.show()
    var params = {
      leave_gid: this.parameterValue
    }
    var url3 = 'ApplyLeave/leavePendingDelete'
    this.service.postparams(url3, params).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide()

      }
      else {
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide()

      }
      this.ngOnInit();
    });
  }

  calculateLeaveDays() {
    if (this.radioSelected === 'Half') {
      this.reactiveFormadd.get('leave_days')?.setValue('0.5');
      const fromDate = this.reactiveFormadd.get('leave_from')?.value;
      this.reactiveFormadd.get('leave_to')?.setValue(fromDate);
    }
    else if (this.radioSelected === 'Full') {
      const fromDate = this.reactiveFormadd.get('leave_from')?.value;
      const toDate = this.reactiveFormadd.get('leave_to')?.value;
      if (fromDate && toDate) {
        const startDate = new Date(fromDate);
        const endDate = new Date(toDate);
        const diffTime = Math.abs(endDate.getTime() - startDate.getTime());
        const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24)) + 1;
        this.reactiveFormadd.get('leave_days')?.setValue(diffDays.toString());
      }
    
    }
  }


  leavePeriod() {
    this.leaveperiod = this.radioSelected;
    this.calculateLeaveDays();
  } 


  back() {
    this.router.navigate(['/hrm/HrmMemberDashboard'])
  }
  ondocumentdownload(leave_gid:any){
    var url = "ApplyLeave/documentdownload";
    let param ={
      leave_gid:leave_gid
    }
    this.service.getparams(url,param).subscribe((result:any)=>{
      if(result!=null){
        this.service.filedownload1(result);
      }
    });
  }
  close(){
    this.reactiveFormadd.reset();
  }


  leavechange() {
    if (this.radioSelected != null && this.reactiveFormadd.value.leave_to != '' && this.reactiveFormadd.value.leave_from != '') {
    var url = 'ApplyLeave/checkleavedate';
      let param = {
        radioSelected:this.radioSelected,
        leavetype_gid:this.parameterValue,
        leave_from: this.reactiveFormadd.value.leave_from,
        leave_to: this.reactiveFormadd.value.leave_to
      }
      this.service.postparams(url, param).subscribe((result: any) => {
        // this.leavedays = result.leavedays;        
        if (result.status == true) {
          this.ToastrService.warning(result.message)
        }


      });
    }
  }

  get leave_reason() {
    return this.reactiveFormadd.get('leave_reason')!;
  }

  get leave_from() {
    return this.reactiveFormadd.get('leave_from')!;
  }
  get leave_to() {
    return this.reactiveFormadd.get('leave_to')!;
  }
}

import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormGroupName, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { SelectionModel } from '@angular/cdk/collections';


export class Iholidaygrade {
  Holidaygradeassign_list:any
  holidaygrade1_list:any;
  holidaygrade_code:any;
  holidaygrade_name:any;

}
@Component({
  selector: 'app-hrm-trn-addholidayasign',
  templateUrl: './hrm-trn-addholidayasign.component.html',
})

export class HrmTrnAddholidayasignComponent {
  Holidaygradeassign_list: any[] = [];
  reactiveFormadd!: FormGroup;
  responsedata: any;
  selection = new SelectionModel<Iholidaygrade>(true, []);
  pick:Array<any> = [];
  CurObj: Iholidaygrade = new Iholidaygrade();

  constructor(private formBuilder: FormBuilder, private route: ActivatedRoute, private router: Router, private ToastrService: ToastrService, public service: SocketService,public NgxSpinnerService:NgxSpinnerService) {
    this.reactiveFormadd = new FormGroup({
      holidaygrade_code :new FormControl('',[Validators.required,Validators.pattern(/^\S.*$/)]),
      holidaygrade_name :new FormControl('',[Validators.required,Validators.pattern(/^\S.*$/)]),     
      Holidaygradeassign_list: this.formBuilder.array([])
    });
  }
  ngOnInit(): void {
    // var url = 'HolidayGradeManagement/Addholidaysummary'
    // this.service.get(url).subscribe((result: any) => {
  
    //   this.responsedata = result;
    //   this.Holidaygradeassign_list = this.responsedata.holidaygrade1_list;
    // });
    this.holidayassignsummary()
    const options: Options = {
          dateFormat: 'd-m-Y',    
        };
        flatpickr('.date-picker', options);
    }
    holidayassignsummary(){
    var url = 'HolidayGradeManagement/Addholidaysummary'
    this.service.get(url).subscribe((result: any) => {
  
      this.responsedata = result;
      this.Holidaygradeassign_list = this.responsedata.holidaygrade1_list;
    });
  }

  get holidaygrade_code() {
    return this.reactiveFormadd.get('holidaygrade_code')!;
  }
  get holidaygrade_name() {
    return this.reactiveFormadd.get('holidaygrade_name')!;
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.Holidaygradeassign_list.length;
    return numSelected === numRows;
  }
  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.Holidaygradeassign_list.forEach((row: Iholidaygrade) => this.selection.select(row));
  }
  
    // submit(){
    //   const selectedData = this.selection.selected; // Get the selected items
    //   if (selectedData.length === 0) {
    //     this.ToastrService.warning("Select Atleast one leave grade to Added");
    //     return;
    //   } 
      
    //   for (const data of selectedData) {
    //     this.Holidaygradeassign_list.push(data);
    // }
    // var params={ 
    //   // holidaygrade_list : this.Holidaygradeassign_list,
    //   holidaygrade_code : this.reactiveFormadd.value.holidaygrade_code,
    //   holidaygrade_name : this.reactiveFormadd.value.holidaygrade_name,       
    // }

    // console.log(params)
  
    // var url = 'HolidayGradeManagement/HolidayAssignSubmit'
    //   this.service.postparams(url,params).subscribe((result: any) => {
    //     if (result.status == false) {
    //       this.ToastrService.warning(result.message)
    //       this.router.navigate(['/hrm/HrmMstHolidaygradeManagement']);
    //    }
    //    else{
    //     this.ToastrService.success(result.message)
    //     this.router.navigate(['/hrm/HrmMstHolidaygradeManagement']);
    //    }
    //   });

    // }

    submit(){
        this.pick = this.selection.selected  
        this.CurObj.holidaygrade1_list = this.pick
        // this.CurObj.leavegrade_gid=this.leavegrade_gid
        this.CurObj.holidaygrade_code = this.reactiveFormadd.value.holidaygrade_code
        this.CurObj.holidaygrade_name = this.reactiveFormadd.value.holidaygrade_name
          if (this.CurObj.holidaygrade1_list.length === 0) {
          this.ToastrService.warning("Select Atleast one holiday grade to Added");
          return;
        } 
       
          var url = 'HolidayGradeManagement/HolidayAssignSubmit';  
          this.service.post(url, this.CurObj).subscribe((result: any) => {
            if (result.status === false) {
              this.ToastrService.warning(result.message);
              
            } else {
              this.ToastrService.success(result.message);
              this.router.navigate(['/hrm/HrmMstHolidaysummary'])
             

            }
          });
        this.selection.clear();    
    }    




}

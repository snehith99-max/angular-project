import { Component, OnInit, OnDestroy, ChangeDetectorRef, Renderer2, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { SelectionModel } from '@angular/cdk/collections';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { Subscription, Observable } from 'rxjs';
import { first } from 'rxjs/operators';
import { NgbTimepickerModule, NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
export class IEditassignEmployee {
  holidaygrade_gid:any;
  holiday_gid:any;
  // holidayeditassign:any;
  holidayeditunassign:any;
}
@Component({
  selector: 'app-hrm-mst-editholidaygrade',
  templateUrl: './hrm-mst-editholidaygrade.component.html',
})
export class HrmMstEditholidaygradeComponent {
  reactiveFormadd!: FormGroup;
  responsedata: any;
  Holidaygradeassign_list: any[] = [];
  Holidaygradeunassign_list: any[] = [];
  // holidayeditunassign: any[] =[];
  parameterValue1:any;
  parameterValue2:any;
  holidaygrade_gid: any;
  holiday_gid: any;
  Holidayassign_type: any;
  holidaygrade_code: any;
  holidaygrade_name: any;
  pick:Array<any> = [];
  CurObj: IEditassignEmployee = new IEditassignEmployee();
  selection = new SelectionModel<IEditassignEmployee>(true, []);


  constructor(private formBuilder: FormBuilder, private route: ActivatedRoute, private router: Router, private ToastrService: ToastrService, public service: SocketService,public NgxSpinnerService:NgxSpinnerService)
  {
 
 }
  ngOnInit(): void { 

    //// Summary Grid//////
    const holidaygrade_gid = this.route.snapshot.paramMap.get('holidaygrade_gid');
    this.holidaygrade_gid = holidaygrade_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.holidaygrade_gid, secretKey).toString(enc.Utf8);    
    this.holidaygrade_gid = deencryptedParam
    let param = {
      holidaygrade_gid: deencryptedParam
    }

      var url = 'HolidayGradeManagement/HolidayEditAssign'
      this.service.getparams(url,param).subscribe((result: any) => {
        $('#Holidaygradeassign_list').DataTable().destroy();
      this.responsedata = result;
      this.Holidaygradeassign_list = this.responsedata.holidayeditassign;
      setTimeout(() => {
        $('#Holidaygradeassign_list').DataTable();
      },);
    });  

      var url = 'HolidayGradeManagement/HolidayEditUnassign'
      this.service.getparams(url,param).subscribe((result: any) => {
        $('#Holidaygradeunassign_list').DataTable().destroy();
      this.responsedata = result;
      this.Holidaygradeunassign_list = this.responsedata.holidayeditunassign;
      setTimeout(() => {
        $('#Holidaygradeunassign_list').DataTable();
      },);
      
      });        

      var url = 'HolidayGradeManagement/Holidayassign'
      let param1={
        holidaygrade_gid:this.holidaygrade_gid
      }
   this.service.getparams(url,param1).subscribe((result: any) => {
   this.responsedata = result;  
   this.Holidayassign_type = this.responsedata.Holidayassign_type;
   this.holidaygrade_code = this.Holidayassign_type[0].holidaygrade_code;
   this.holidaygrade_name = this.Holidayassign_type[0].holidaygrade_name;
 });
    }

//   holidaygradesedit(){

//   var url = 'HolidayGradeManagement/HolidayEditAssign'
//   this.service.get(url).subscribe((result: any) => {  
//     this.responsedata = result;
//     this.Holidaygradeassign_list = this.responsedata.holidayeditassign;
//   });
// }


Unassign(){

  this.pick = this.selection.selected  
  this.CurObj.holidayeditunassign = this.pick
  this.CurObj.holidaygrade_gid=this.holidaygrade_gid
    if (this.CurObj.holidayeditunassign.length === 0) {
    this.ToastrService.warning("Select atleast one employee");
    return;
  } 

    var url = 'HolidayGradeManagement/HolidayEditUnAssignsubmit';  
    this.service.post(url, this.CurObj).subscribe((result: any) => {
      $('#Holidaygradeunassign_list').DataTable().destroy();
      if (result.status === false) {
        this.ToastrService.warning(result.message);
        
      } else {
        this.ToastrService.success(result.message);
        this.ngOnInit();              
      }
    });       
  this.selection.clear();
}
isAllSelected() {
  const numSelected = this.selection.selected.length;
  const numRows = this.Holidaygradeunassign_list.length;
  return numSelected === numRows;
}
masterToggle() {
  this.isAllSelected() ?
    this.selection.clear() :
    this.Holidaygradeunassign_list.forEach((row: IEditassignEmployee) => this.selection.select(row));
}
openModaldelete(parameter1: string) {  
  this.parameterValue1 = parameter1
}
// get Holiday_date() {
//   return this.reactiveFormadd.get('Holiday_date')!;
// }
// get holiday_name() {
//   return this.reactiveFormadd.get('holiday_name')!;
// }

ondelete() {
  console.log(this.parameterValue1);
  let param1={
    holiday_gid:this.parameterValue1
  }
  var url3 = 'HolidayGradeManagement/DeleteEditholiday'
  this.service.getparams(url3,  param1).subscribe((result: any) => {
    $('#Holidaygradeassign_list').DataTable().destroy();
    if (result.status == false) {
      this.ToastrService.warning('Error While Deleting Holiday')
    }
    else {
      this.ToastrService.success('Holiday Deleted Successfully')
      this.ngOnInit()
    }
  }); 
}

}

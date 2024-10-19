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

export class IAssignEmployee {
  holidaygrade_gid:any;
  holidayassignemployee:any;
}
@Component({
  selector: 'app-hrm-mst-holidayassignemployee',
  templateUrl: './hrm-mst-holidayassignemployee.component.html',
})
export class HrmMstHolidayassignemployeeComponent {
  Holidayassign_type: any;
  holidaygrade_code: any;
  holidaygrade_name: any;
  responsedata:any;
  holidayemployee_list:any[] = [];
  selection = new SelectionModel<IAssignEmployee>(true, []);
  leave_name:any;
  leave_code:any;
  holidaygrade_gid: any;
  Leaveassign_type: any;
  pick:Array<any> = [];
  CurObj: IAssignEmployee = new IAssignEmployee();
  constructor(    
    private renderer: Renderer2,
    private el: ElementRef,
    public service: SocketService,
    private ToastrService: ToastrService,
    private route: Router,
    private router: ActivatedRoute,
    private formBuilder: FormBuilder,
    private NgxSpinnerService: NgxSpinnerService
     ){}
     ngOnInit(): void { 

  
      //// Summary Grid//////
      const holidaygrade_gid = this.router.snapshot.paramMap.get('holidaygrade_gid');
      this.holidaygrade_gid = holidaygrade_gid;
      const secretKey = 'storyboarderp';
      const deencryptedParam = AES.decrypt(this.holidaygrade_gid, secretKey).toString(enc.Utf8);    
      this.holidaygrade_gid = deencryptedParam
      let param = {
        holidaygrade_gid: deencryptedParam
      }
      this.NgxSpinnerService.hide();
        var url = 'HolidayGradeManagement/HolidaygradeAssignemployee'
        this.service.getparams(url,param).subscribe((result: any) => {
        this.responsedata = result;
        this.holidayemployee_list = this.responsedata.holidayassignemployee;
        this.NgxSpinnerService.hide();
          setTimeout(() => {
            $('#holidayemployee_list').DataTable();
            }, );
        
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
      isAllSelected() {
        const numSelected = this.selection.selected.length;
        const numRows = this.holidayemployee_list.length;
        return numSelected === numRows;
      }
      masterToggle() {
        this.isAllSelected() ?
          this.selection.clear() :
          this.holidayemployee_list.forEach((row: IAssignEmployee) => this.selection.select(row));
      }

      assign(){
        var param = {
          holidayassignemployee: this.selection.selected,  // Assign selected employees
          holidaygrade_gid: this.holidaygrade_gid          // Assign holiday grade ID
        };
         
        if (param.holidayassignemployee.length === 0) {
          this.ToastrService.warning("Select at least one employee");
          return;
        }
            this.NgxSpinnerService.show();
            var url = 'HolidayGradeManagement/HolidayAssignSubmitemploye';  
            this.service.post(url, param).subscribe((result: any) => {
              if (result.status === false) {
                this.ToastrService.warning(result.message);
                
                
              } else {
                this.ToastrService.success(result.message);
                this.route.navigate(['/hrm/HrmMstHolidaysummary'])
                this.NgxSpinnerService.hide();

                }
            });        
          this.selection.clear();
      
      }
      

}

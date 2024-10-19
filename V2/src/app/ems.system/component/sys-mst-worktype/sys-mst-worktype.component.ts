import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ApexChart, ApexNonAxisChartSeries, ApexResponsive } from 'ng-apexcharts';
import { NgxSpinnerService } from 'ngx-spinner';


@Component({
  selector: 'app-sys-mst-worktype',
  templateUrl: './sys-mst-worktype.component.html',
  styleUrls: ['./sys-mst-worktype.component.scss']
})
export class SysMstWorktypeComponent {
  worktype_list: any[] = [];
  parameterValue1: any;
  parameterValue: any;
  WorkType: FormGroup | any;
  

  constructor(private SocketService: SocketService, private NgxSpinnerService: NgxSpinnerService, public service: SocketService, private ToastrService: ToastrService, private FormBuilder: FormBuilder) {
    this.WorkType = new FormGroup({
      
      Worktype_Name: new FormControl('', [Validators.required]),
      worktype_gid: new FormControl(''),
    
      })
    }

  

  ngOnInit(): void{
    this.worktypesummary()
  }
  worktypesummary() {
      var api = 'Worktype/GetWorktypeSummary';
      this.service.get(api).subscribe((result: any) => {
        this.worktype_list = result.WorktypeLists;
        setTimeout(() => {
          $('#worktype_list').DataTable();
          }, );
      });
    }
    addWorktype() {
      this.NgxSpinnerService.show();
      var url = 'Worktype/PostWorktype';
      this.service.postparams(url, this.WorkType.value).subscribe((result: any) => {
        if (result.status != false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message);
          this.WorkType.reset();
  
          this.worktypesummary()
        }
        else {
          this.ToastrService.warning(result.message);
          this.NgxSpinnerService.hide();
          this.WorkType.reset();
        }
      })
    }
    openModaledit(parameter: string) {
      debugger;
      this.parameterValue1 = parameter
    
      
      this.WorkType.get("Worktype_Name")?.setValue(this.parameterValue1.WorkType_Name);
      this.WorkType.get("worktype_gid")?.setValue(this.parameterValue1.Worktype_gid );
    }
    updateWorktype(){
      this.NgxSpinnerService.show();
      var url = 'Worktype/getUpdatedWorktype';
      this.SocketService.post(url, this.WorkType.value).subscribe((result:any) => {
        if(result.status == true){
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message);
          this.WorkType.reset();
        }
        else {
              
          this.ToastrService.warning(result.message);
          this.NgxSpinnerService.hide();
          this.WorkType.reset();
    
          
        }
        this.ngOnInit();
      })
        
        
    
    }
    ondelete(){
    
      this.NgxSpinnerService.show();
    
      var url = 'Worktype/DeleteWorktype';
    
      let params = {
    
        Worktype_gid: this.parameterValue
    
      }
    
      this.SocketService.getparams(url, params).subscribe((result:any) => {
    
        if(result.status == true){
    
          this.NgxSpinnerService.hide();
    
          this.ToastrService.success(result.message);
    
          this.ngOnInit();
    
        }
    
        else {
    
          this.ToastrService.warning(result.message);
    
          this.NgxSpinnerService.hide();
    
          this.ngOnInit();
    
        }  
    
      })
    
    }
    deletemodal(parameter: string) {
      this.parameterValue = parameter
    }
    
    close() {
      this.WorkType.reset();
    }
}

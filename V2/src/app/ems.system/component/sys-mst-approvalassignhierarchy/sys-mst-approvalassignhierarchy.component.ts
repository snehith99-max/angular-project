import { Component,OnInit,TemplateRef,ElementRef, ViewChild ,ChangeDetectorRef } from '@angular/core'; 
import { Router,ActivatedRoute } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AES, enc } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import {SelectionModel} from '@angular/cdk/collections';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from 'src/environments/environment.development';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms'; 
interface objInterface { module_gid: string; employee_gid: string,module_checked:boolean   } 
interface Sub2MenuItem { module_gid: string; text:string ,module_checked:boolean}

@Component({
  selector: 'app-sys-mst-approvalassignhierarchy',
  templateUrl: './sys-mst-approvalassignhierarchy.component.html',
  styleUrls: ['./sys-mst-approvalassignhierarchy.component.scss']
})
export class SysMstApprovalassignhierarchyComponent {
  employeelist: any
  cboselectedEmployee: any;
  module_name:any;
  reactiveFormReset!: FormGroup;
  approval_type:any;
  responsedata:any;
  module_gid:any;
  employee:any;
  hierarchy_level: any;
  constructor(public router:Router,private route: ActivatedRoute,public NgxSpinnerService:NgxSpinnerService,
    private SocketService: SocketService,private ToastrService: ToastrService,private FormBuilder: FormBuilder,
    private changeDetectorRef: ChangeDetectorRef) {
      
    } 
    ngOnInit(): void {
    
      //   this.reactiveFormReset = this.FormBuilder.group({  
      //     cboselectedEmployee:[null,[Validators.required]],
      // });

      const modulegid = this.route.snapshot.paramMap.get('module_gid');
      this.module_gid = modulegid;
      const secretKey = 'storyboarderp';
      const deencryptedParam = AES.decrypt(this.module_gid, secretKey).toString(enc.Utf8);
      console.log(deencryptedParam);
      debugger;
      const [module_gid, module_name,approval_type] = deencryptedParam.split('+');
      this.module_gid = module_gid;
      this.module_name = module_name;
      this.approval_type = approval_type;

      var params = {
        module_gid: this.module_gid,
        module_name: this.module_name,
        approval_type: this.approval_type

        }

     var url = 'SysMstModuleManage/GetEmployeeAssignlist';
    this.SocketService.getparams(url,params).subscribe((result: any) => { 
      this.employeelist  = result.employeelist;  
    });
debugger;
    var param = {
      
      module_gid: this.module_gid     

      }
    var url = 'SysMstModuleManage/GetApprovalAssignHierarchysummary'
    this.SocketService.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.hierarchy_level = this.responsedata.level_list;
    });

    }
    
    Approvalassign(data: any): void {
    debugger;
    var api1 = 'ShiftType/GetshiftTimepopup';
 
    
    let params = {
      module_gid: this.module_gid,
      module_name: this.module_name,
      approval_type: this.approval_type,
      // employeelist:this.reactiveForm
       };
 
    this.SocketService.getparams(api1, params).subscribe((result: any) => {
        this.responsedata = result;
        // this.Shifttime_list = this.responsedata.Time_list;
    });
  }
  submit(){
    debugger;
    var employeeList = this.cboselectedEmployee.map(function(employeeId: any) {
      return { employee_gid: employeeId };
      });
    let params = {
      module_gid: this.module_gid,
      module_name: this.module_name,
      approval_type: this.approval_type,
      employeelist: employeeList 
     
       }
       this.NgxSpinnerService.show();
       var url = 'SysMstModuleManage/PostEmployeeAssignSubmit';
       this.SocketService.postparams(url,params).subscribe((result: any) => { 
         if(result.status ==true){
           this.ToastrService.success(result.message)
           this.NgxSpinnerService.hide(); 
           this.ngOnInit();
           this.cboselectedEmployee = null;
         }
         else{
           this.ToastrService.warning(result.message)
           this.NgxSpinnerService.hide();   
         }
       });

  }

  isLevelSelected(level: string): boolean {
    return this.cboselectedEmployee.some((employee: any) => employee.employee_name === level);
}


  back(){
    this.router.navigate(['/system/SysMstApprovalhierarchy']); 

  }
}

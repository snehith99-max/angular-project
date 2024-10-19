import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { environment } from 'src/environments/environment';
import { NgxSpinnerService } from 'ngx-spinner';


@Component({
  selector: 'app-sys-mst-approvalhierarchy',
  templateUrl: './sys-mst-approvalhierarchy.component.html',
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
  `]})
export class SysMstApprovalhierarchyComponent {
  approval_list: any[] = []
  responsedata: any;
  module_gid: any;
  reactiveForm!: FormGroup;
  approval_limit:any;
  employeelist : any;
  module_name:any;


  constructor(private formBuilder: FormBuilder,private route: Router,
    private router: ActivatedRoute, private ActivatedRoute: ActivatedRoute,public NgxSpinnerService:NgxSpinnerService, private ToastrService: ToastrService,private SocketService: SocketService,) {
  }
  ngOnInit(): void { 

    this.reactiveForm = new FormGroup({
      module_name: new FormControl(''),
      Sequence: new FormControl(''),
      sequence_hierarchy: new FormControl(''),
      parallel: new FormControl(''),
      approval_type: new FormControl(''),
      module_gid: new FormControl(''),


    });
    debugger;

    this.ActivatedRoute.queryParams.subscribe(params => {
      const urlparams = params['hash'];
      if (urlparams) {
        const decryptedParam = AES.decrypt(urlparams, environment.secretKey).toString(enc.Utf8);
        const paramvalues = decryptedParam.split('&');
        this.module_gid = paramvalues[0];
      }
    });
    let param = {
      module_gid : this.module_gid
    }
    // this.NgxSpinnerService.hide();
      var url = 'SysMstModuleManage/ApprovalSummary'
      this.SocketService.getparams(url,param).subscribe((result: any) => {
      this.responsedata = result;
      this.approval_list = this.responsedata.Approvalsummary;
      // this.NgxSpinnerService.hide();
        setTimeout(() => {
          $('#approval_list').DataTable();
          }, );
      
      });    
    // const module_gid = this.router.snapshot.paramMap.get('module_gid');
    // this.module_gid = module_gid;
    // const secretKey = 'storyboarderp';
    // const deencryptedParam = AES.decrypt(this.module_gid, secretKey).toString(enc.Utf8);    
    // this.module_gid = deencryptedParam
    // let param = {
    //   holidaygrade_gid: deencryptedParam
    // }
    // // this.NgxSpinnerService.hide();
    //   var url = 'SysMstModuleManage/ApprovalSummary'
    //   this.service.getparams(url,param).subscribe((result: any) => {
    //   this.responsedata = result;
    //   this.approval_list = this.responsedata.Approvalsummary;
    //   // this.NgxSpinnerService.hide();
    //     setTimeout(() => {
    //       $('#holidayemployee_list').DataTable();
    //       }, );
      
    //   });   
    
    
    }

    // assign(module_name:any){
    //   this.module_name=module_name;
    //   var url = 'SystemMaster/GetEmployeelist';
    //   this.SocketService.get(url).subscribe((result: any) => {
    //     this.employeelist  = result.employeelist;
    //   });
    // }
    onedit(module_gid:any,module_name:any){
     this.module_gid=module_gid;
     this.reactiveForm.get("module_name")?.setValue(module_name);
     //  this.approval_limit=approval_limit
    }
    onsubmit(): void {
      debugger;
      var params={ 
        module_gid : this.module_gid,
        module_name : this.reactiveForm.value.module_name,
        approval_type : this.reactiveForm.value.approval_type,

      }
      console.log(params)
    
      var url = 'SysMstModuleManage/Approvalsubmit'
      this.NgxSpinnerService.show();
        this.SocketService.postparams(url,params).subscribe((result: any) => {
          this.NgxSpinnerService.hide();
          if (result.status == false) {
            this.ToastrService.warning(result.message)
            this.route.navigate(['/system/SysMstApprovalhierarchy']);
           
            this.NgxSpinnerService.hide();
         }
         else{
          this.NgxSpinnerService.show();
          this.ToastrService.success(result.message)
          this.route.navigate(['/system/SysMstApprovalhierarchy']);
          this.NgxSpinnerService.hide();
         }
        //  window.location.reload();
        this.reactiveForm.reset();

        });
      }
onback(){
  this.reactiveForm.reset();
}
      // assign(module_gid: any,module_name:any){ 
        
      //   this.module_name=module_name;
      //   const parameter1 = `${module_gid}`; 
      //   const encryptedParam = AES.encrypt(parameter1,environment.secretKey).toString();
      //   var url = '/system/SysMstAssignHierarchy?hash=' + encodeURIComponent (encryptedParam);
      //   this.route.navigateByUrl(url)
      // }
      assign(params:any,params1: any,params2: any){
        debugger;
        const secretKey = 'storyboarderp';
        const param = (params+'+'+params1+'+'+params2);
        const encryptedParam = AES.encrypt(param,secretKey).toString();
        this.route.navigate(['/system/SysMstAssignHierarchy',encryptedParam]) 
        
        
      }
}

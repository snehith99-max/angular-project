import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { SelectionModel } from '@angular/cdk/collections';
import { ToastrService } from 'ngx-toastr';
import { FormControl, FormGroup, Validators, FormBuilder } from '@angular/forms';
import { AES } from 'crypto-js';
import { Router } from '@angular/router';

interface objInterface {employee_name:string,employee_gid:string,user_gid:string }

@Component({
  selector: 'app-tsk-trn-manager',
  templateUrl: './tsk-trn-manager.component.html',
  styleUrls: ['./tsk-trn-manager.component.scss']
})
export class TskTrnManagerComponent {
  customer_list:any
  searchText1=''
  searchText=''
  list: any[]=[];
  team:any
  selection = new SelectionModel<objInterface>(true, []);
  selection2 = new SelectionModel<objInterface>(true, []);
  taskpending_list: any;
  team_gid: any;
  employee_list: any[]=[];
  team_name: any;
  assignteamform!:FormGroup
  member: any;
  module_name: any;
  client: any;
  taskassigned_list: any;
  task_title: any;
  task_code: any;
  assgn_manager: any;
  assigned_member: any;
  pending_count: any;
  assigned_count: any;
  completed_list: any;
  completed_count: any;
  task_gid: any;
  task_name: any;
  holdmanager_count: any;
  show_stopper: any;
  mandatory: any;
  non_mandatory: any;
  nice_to_count: any;
  testmanager_count: any;
  tasks_gid: any;
  txthold_reason: any;
  holdform: FormGroup | any;
  total: any;
  team_count: any;
  module_gid: any;
  constructor(private Router:Router,public FormBuilder: FormBuilder,private ToastrService:ToastrService,private NgxSpinnerService:NgxSpinnerService,private SocketService: SocketService){
    this.assignform()
    this.createstsform()
  }
  createstsform() {
    this.holdform = this.FormBuilder.group({
      txholdtremarks: new FormControl(null,
        [    Validators.required,
          Validators.pattern(/^(?!\s*$).+/)
        ]),
    });
  }
  assignform(){
    this.assignteamform=this.FormBuilder.group({
      team_name: ['' ,Validators.required],
    })
  }
  sortColumn(columnKey: string): void {
    return this.SocketService.sortColumn(columnKey);
  }

  getSortIconClass(columnKey: string) {
    return this.SocketService.getSortIconClass(columnKey);
  }
  ngOnInit() {
    var url = 'TskTrnTaskManagement/Managerpendingsummary';
    this.NgxSpinnerService.show();
    this.SocketService.get(url).subscribe((result: any) => {
      if (result.taskpending_list != null) {
        $('#manager').DataTable().destroy();
        this.taskpending_list = result.taskpending_list;
        this.pending_count=result.pending_count

        this.NgxSpinnerService.hide();
        setTimeout(() => {
          $('#manager').DataTable();
        }, 1);
      }
      else {
        this.taskpending_list = result.taskpending_list;
        this.pending_count=result.pending_count

        setTimeout(() => {
          var table = $('#manager').DataTable();
        }, 1);
        this.NgxSpinnerService.hide();
        $('#manager').DataTable().destroy();
        this.NgxSpinnerService.hide();
      }
    });
    var url = 'TskTrnTaskManagement/Assignedpendingsummary';
    this.NgxSpinnerService.show();
    this.SocketService.get(url).subscribe((result: any) => {
      this.assigned_count=result.assigned_count
    })
    var url = 'TskTrnTaskManagement/Assignedcompletedsummary';
    this.NgxSpinnerService.show();
    this.SocketService.get(url).subscribe((result: any) => {
      this.completed_count=result.completed_count
    })
    var url = 'TskTrnTaskManagement/Assignedholdsummary';
    this.NgxSpinnerService.show();
    this.SocketService.get(url).subscribe((result: any) => {
      this.holdmanager_count=result.holdmanager_count
    })
    var url = 'TskTrnTaskManagement/Assignedtestingsummary';
    this.NgxSpinnerService.show();
    this.SocketService.get(url).subscribe((result: any) => {
      this.testmanager_count=result.testmanager_count
    })
    var url = 'TskTrnTaskManagement/teamlistsummary';
    this.NgxSpinnerService.show();
    this.SocketService.get(url).subscribe((result: any) => {
      this.team_count=result.team_count

    });
    var url = 'TskTrnTaskManagement/managerallcount';
    this.SocketService.get(url).subscribe((result: any) => {
      this.total = result.row_count
    });
    this.count()
  }
  edit(data: any) {debugger
    this.assignteamform.reset()
    this.team_gid = data.module_gid;
    this.task_gid = data.task_gid;
    this.task_name=data.task_name
    this.module_name=data.module_name
    this.module_gid=data.module_gid
    this.client=data.client_name
    var param = {
      team_gid: this.team_gid
    }
    this.NgxSpinnerService.show();
    var url = 'TskMstCustomer/managerlist';
    this.SocketService.getparams(url, param).subscribe((result: any) => {
      this.member=result.assignmem_list
      this.NgxSpinnerService.hide();
    });
    // var url = 'TskMstCustomer/editmanager';
    // this.SocketService.getparams(url, param).subscribe((result: any) => {
    //   this.team=result.assignmem_list[0].employee_name
    //   this.NgxSpinnerService.hide();
    // });
  }




submitassign(){debugger
  var params={
    task_gid:this.task_gid,
    team_gid: this.team_gid,
    assigned_member_gid:this.assignteamform.value.team_name.employee_gid,
    assigned_member:this.assignteamform.value.team_name.employee_name,
     }
  this.NgxSpinnerService.show();
    var url = 'TskTrnTaskManagement/Addmember';
    this.SocketService.post(url, params).subscribe((result: any) => {
      this.NgxSpinnerService.hide();

      if (result.status == true) {
        this.ToastrService.success(result.message);
        this.ngOnInit()
      } else {
        this.ToastrService.warning(result.message);
        this.ngOnInit()

      }
      this.ngOnInit();
    });
}

count(){
  var url = 'TskTrnTaskManagement/managercount';
  this.SocketService.get(url).subscribe((result: any) => {
    this.show_stopper=result.show_stopper_count
    this.mandatory=result.mandatory_count
    this.non_mandatory=result.non_mandatory_count
    this.nice_to_count=result.nice_to_have_count
  });
}
holdtask(task_gid:any,task_code:any,task_name:any){
  this.tasks_gid=task_gid
  this.task_code=task_code
  this.task_name=task_name
  this.holdform.reset();

}
Submithold(){
  this.NgxSpinnerService.show();
  var params = {
    task_gid: this.tasks_gid,
    taskhold_reason: this.txthold_reason,
    status:'Hold'
}
var url = "TskTrnTaskManagement/GetHoldtask";
this.SocketService.getparams(url,params).subscribe((result:any)=>{
  if(result.status= true){
    this.ToastrService.success(result.message);
    this.NgxSpinnerService.hide();
    this.ngOnInit()
  }
  else{
    this.NgxSpinnerService.hide();
    this.ToastrService.warning(result.message);
  } 
  });
 }
}

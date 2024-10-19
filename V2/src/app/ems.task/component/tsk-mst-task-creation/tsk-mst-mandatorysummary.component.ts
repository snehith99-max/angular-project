import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { AES } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { SelectionModel } from '@angular/cdk/collections';
interface objInterface {project_name:string,project_gid:string,task_gid:string}
@Component({
  selector: 'app-tsk-mst-mandatorysummary',
  templateUrl: './tsk-mst-mandatorysummary.component.html',
  styles: [`.badge-blue {
    color: #2cacbd;
    background-color: transparent;
   
    &.badge-outline-blue {
      color: r#2cacbd;
      border: 1px solid;
      background-color: transparent;
      border-radius: 5px;
      padding: 5px;
      margin-right: 50px;
      padding-top: 2px;
      padding-bottom: 2px;
      // margin: 25px;
    }
  }  .selected-row {
    background-color: #e7f4ff; /* You can change this color as needed */
  }
  .bg-tableheaderblue{
  background-color: #cad8e7 !important;
  
  }
  .selectable-name {
  cursor: pointer;
  }
  .selectable-name:hover{
  background-color: #e7f4ff;
  
  }.text-pending {
    color: #009ef7;
    font-weight: bold;
    font-family: "Times New Roman", Times, serif;
  }
  
  .text-in-progress {
    color: #009ef7;
    font-weight: bold;
    font-family: "Times New Roman", Times, serif;
  }
  
  .text-hold {
    color: #de9800;
    font-weight: bold;
    font-family: "Times New Roman", Times, serif;
  }
  
  .text-testing {
    color: green;
    font-weight: bold;
    font-family: "Times New Roman", Times, serif;
  }
  .count3{
    padding: 6px 10px;
      color: #000000;
      background-color: rgb(205 232 255);
      border-radius: 6px;
      position: relative;
  }.cardHeadtime {

    position: relative;
  
    display: flex;
  
    flex-direction: column;
  
    min-width: 0;
  
    word-wrap: break-word;
  
    padding: 18px;
  
    background-clip: border-box;
  
    border: 0px solid rgba(0, 0, 0, 0);
  
    border-radius: .25rem;
  
    margin-bottom: 1.0rem;
  
    box-shadow: 0 2px 6px 0 rgb(218 218 253 / 65%), 0 2px 6px 0 rgb(206 206 238 / 54%);
  
    transition: all .3s ease-in-out;
  
    background-color: white;
  
  }
  .view_count {
    padding: 3px 6px;
    color: #ffffff;
    background-color: #50cd89;
    border-radius: 5px;
  }
   `
  ]
})
export class TskMstMandatorysummaryComponent {
  show_stopper: any;
  non_mandatory: any;
  searchText1=''
  searchText=''
  selection = new SelectionModel<objInterface>(true, []);
  selection2 = new SelectionModel<objInterface>(true, []);
  module_gid: any;
  module_name: any;
  GetAssignedlist: any[]=[];
  GetUnassignedlist: any[]=[];
  task_gid: any;
  parametervalue: any;
  nice_to_count: any;
  mandatory: any;
  mandatory_list: any;
  completed: any;
  time = new Date();
  rxTime = new Date();
  currentDayName: any;
  toDate: any;
  timerInterval: any;
  clientview: any;
  constructor(private ToastrService:ToastrService,private Router:Router,private NgxSpinnerService:NgxSpinnerService,private SocketService: SocketService) {
  }
  sortColumn(columnKey: string): void {
    return this.SocketService.sortColumn(columnKey);
  }

  getSortIconClass(columnKey: string) {
    return this.SocketService.getSortIconClass(columnKey);
  }
  ngOnInit() {
    const today = new Date();
    this.currentDayName = today.toLocaleDateString('en-US', { weekday: 'long' });
    this.rxTime = new Date();
    this.timerInterval = setInterval(() => {
        this.rxTime = new Date();
    }, 1000);
    var url = 'TskTrnTaskManagement/Mandatorysummary';
    this.NgxSpinnerService.show();
    this.SocketService.get(url).subscribe((result: any) => {
      if (result.taskpending_list != null) {
        $('#mandatory').DataTable().destroy();
        this.mandatory_list = result.taskpending_list;
        this.mandatory=result.mandatory
        this.NgxSpinnerService.hide();
        setTimeout(() => {
          $('#mandatory').DataTable();
        }, 1);
      }
      else {
        this.mandatory_list = result.taskpending_list;
        this.mandatory=result.mandatory
        setTimeout(() => {
          var table = $('#mandatory').DataTable();
        }, 1);
        this.NgxSpinnerService.hide();
        $('#mandatory').DataTable().destroy();
        this.NgxSpinnerService.hide();
      }
    });
    var url = 'TskTrnTaskManagement/showstoppersummary';
    this.SocketService.get(url).subscribe((result: any) => {
      this.show_stopper=result.show_stopper
    });
    var url = 'TskTrnTaskManagement/nonmandatorytsummary';
    this.SocketService.get(url).subscribe((result: any) => {
      this.non_mandatory=result.non_mandatory
    });
    var url = 'TskTrnTaskManagement/nicetohavesummary';
    this.SocketService.get(url).subscribe((result: any) => {
      this.nice_to_count=result.nice_to_count
    });
    var url = 'TskTrnTaskManagement/completed';
    this.SocketService.get(url).subscribe((result: any) => {
      this.completed=result.completed
    });
  }
  delete(task_gid: any) {
    this.parametervalue = task_gid
  
  }
  ondelete() {
    this.NgxSpinnerService.show();
    var params = {
      task_gid: this.parametervalue
    }
    var url = 'TskTrnTaskManagement/taskassigndelete';
    this.SocketService.getparams(url, params).subscribe((result: any) => {
      if (result.status == true) { 
        this.ToastrService.success(result.message);
        this.NgxSpinnerService.hide();
        this.ngOnInit();
      }
      else {
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide(); 
      }
    });
  }
  view(params:any){debugger
    const parameter1 = `${params}&Pending`;
    const secretKey = 'storyboarderp';
    const encryptedParam = AES.encrypt(parameter1,secretKey).toString();
      var url = '/ITS/ItsMstTaskStatusView?hash=' + encodeURIComponent (encryptedParam);
    this.Router.navigateByUrl(url)
    
  }
  tags(data:any) {debugger
    this.module_gid =data.module_gid
    this.task_gid =data.task_gid
    this.module_name=data.module_name
    this.clientlist()
  }
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.GetUnassignedlist.length;
    return numSelected === numRows;
  }
  masterToggle() {
  
    this.isAllSelected() ? this.selection.clear() :
      this.GetUnassignedlist.forEach((row: objInterface) => this.selection.select(row));
  }
  isSelected2() {
    const numSelected = this.selection2.selected.length;
    const numRows = this.GetAssignedlist.length;
    return numSelected === numRows;
  }
  
  
  masterToggle2(){
    this.isSelected2() ? this.selection2.clear() :
    this.GetAssignedlist.forEach((row: objInterface) => this.selection2.select(row));
  }
  tag() {debugger
    this.selection.selected.forEach(selectedEmployee => {
        const index = this.GetUnassignedlist.findIndex(employee => employee.project_name === selectedEmployee.project_name);
        if (index !== -1) {
            this.GetUnassignedlist.splice(index, 1);
            this.NgxSpinnerService.show()
            this.GetAssignedlist.push({
              project_name: selectedEmployee.project_name,
              project_gid: selectedEmployee.project_gid,
  
                index: index,
            });
        }
        setTimeout(() => {
        this.NgxSpinnerService.hide()
      }, 500);
    });
    this.selection.clear();
    this.searchText=''
  }
  matchesSearch(item: any): boolean {
    const searchString = this.searchText.toLowerCase();
    return item.project_name.toLowerCase().includes(searchString);
  }
  matchesSearchs(item: any): boolean {
    const searchString = this.searchText1.toLowerCase();
    return item.project_name.toLowerCase().includes(searchString);
  }
  untag() {debugger
    this.selection2.selected.forEach(selectedEmployee => {
        const index = this.GetAssignedlist.findIndex(employee1 => employee1.project_name === selectedEmployee.project_name);
        if (index !== -1) {
            var params = {
              task_gid: selectedEmployee.task_gid
            };
            this.NgxSpinnerService.show()
            var url = 'TskTrnTaskManagement/conditionclient';
            this.SocketService.getparams(url, params).subscribe((result: any) => {
                if (result.status == true) {
                    const deletedMember = this.GetAssignedlist.splice(index, 1)[0];
                    this.GetUnassignedlist.splice(deletedMember.index, 0, deletedMember);
                    this.NgxSpinnerService.hide()
  
                } else {
                  this.ToastrService.warning(result.message)
                  this.NgxSpinnerService.hide()
  
                }
            });
        }
    });
    this.selection2.clear();
    this.searchText1 = ''
  
  }
  // untag() {
  //   this.selection2.selected.forEach(selectedEmployee => {
  //       const index = this.GetAssignedlist.findIndex(employee1 => employee1.project_name === selectedEmployee.project_name);
  //       if (index !== -1) {
  //           this.GetAssignedlist.splice(index, 1);
  //           this.NgxSpinnerService.show()
  //           this.GetUnassignedlist.push({
  //             project_name: selectedEmployee.project_name,
  //             project_gid: selectedEmployee.project_gid,
  
  //               index: index,
  //           });
  //           setTimeout(() => {
  //             this.NgxSpinnerService.hide()
  //           }, 500);
  //       }
  //   });
  //   this.selection2.clear();
  //   this.searchText1=''
  // }
  resetTaggedValues() {
    this.GetAssignedlist = []; // As
    this.GetUnassignedlist=[]
  
    // this.ngOnInit()
  }
  clientlist() {debugger
    this.NgxSpinnerService.show();
    let param = {
      team_gid: this.module_gid,
      task_gid:this.task_gid
    };
  
    var url = 'TskTrnTaskManagement/GetUnassignedclientlist';
    this.SocketService.getparams(url, param).subscribe((result: any) => {
      this.GetUnassignedlist = result.GetUnassignedclientlist || [];
      var url1 = 'TskTrnTaskManagement/GetAssignedclientlist';
      this.SocketService.getparams(url1, param).subscribe((result: any) => {
        this.GetAssignedlist = result.GetAssignedclient_list || [];
        this.NgxSpinnerService.hide();
      });
    });
  }
  submitassign(){
    const params = {
      task_gid:this.task_gid,
      team_gid:this.module_gid,
      client_list:this.GetAssignedlist
    };
    console.log(params)
    this.NgxSpinnerService.show();
    var url = 'TskTrnTaskManagement/getclient';
    this.SocketService.post(url, params).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
  
      if (result.status == true) {
        this.ToastrService.success(result.message);
        this.ngOnInit();
      } else {
        this.ToastrService.warning(result.message);
      }
    });
  }
  Edit(task_gid: any) {debugger
    this.task_gid = task_gid;
    var param = {
      task_gid: task_gid
    }
    this.NgxSpinnerService.show();
    var url = 'TskTrnTaskManagement/clientview';
    this.SocketService.getparams(url, param).subscribe((result: any) => {
      this.clientview=result.clientview
      this.NgxSpinnerService.hide();
    });
  }
}

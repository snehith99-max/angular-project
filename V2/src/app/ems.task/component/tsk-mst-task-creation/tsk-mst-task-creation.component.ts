import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { AES } from 'crypto-js';
import { SelectionModel } from '@angular/cdk/collections';
import { ToastrService } from 'ngx-toastr';
import { FormControl, FormGroup, Validators, FormBuilder, FormsModule } from '@angular/forms';
interface objInterface {project_name:string,project_gid:string,task_gid:string}

@Component({
  selector: 'app-tsk-mst-task-creation',
  templateUrl: './tsk-mst-task-creation.component.html',
  styleUrls: ['./tsk-mst-task-creation.component.scss']
})
export class TskMstTaskCreationComponent {
  taskpending_list: any;
  taskhold_list: any;
  task_list: any;
  show_stopper: any;
  mandatory: any;
  searchText1=''
  searchText=''
  Addstatus!: FormGroup;
  non_mandatory: any;
  parametervalue: any;
  nice_to_count: any;
  customer_list: any;
  selection = new SelectionModel<objInterface>(true, []);
  selection2 = new SelectionModel<objInterface>(true, []);
  module_gid: any;
  module_name: any;
  GetAssignedlist: any[]=[];
  GetUnassignedlist: any[]=[];
  task_gid: any;
  completed: any;
  time = new Date();
  rxTime = new Date();
  currentDayName: any;
  toDate: any;
  timerInterval: any;
  clientview: any;
  constructor(private FormBuilder: FormBuilder,private ToastrService:ToastrService,private Router:Router,private NgxSpinnerService:NgxSpinnerService,private SocketService: SocketService) {
    this.status()
  }
  status() {
    this.Addstatus = this.FormBuilder.group({
      txt_Client: [null, [Validators.required]]
    })
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
    var url = 'TskTrnTaskManagement/showstoppersummary';
    this.NgxSpinnerService.show();
    this.SocketService.get(url).subscribe((result: any) => {
      if (result.taskpending_list != null) {
        $('#Show').DataTable().destroy();
        this.taskpending_list = result.taskpending_list;
        this.show_stopper=result.show_stopper
        this.NgxSpinnerService.hide();
        setTimeout(() => {
          $('#Show').DataTable();
        }, 1);
      }
      else {
        this.taskpending_list = result.taskpending_list;
        this.show_stopper=result.show_stopper
        setTimeout(() => {
          var table = $('#Show').DataTable();
        }, 1);
        this.NgxSpinnerService.hide();
        $('#Show').DataTable().destroy();
        this.NgxSpinnerService.hide();
      }
    });
    var url = 'TskTrnTaskManagement/Mandatorysummary';
    this.SocketService.get(url).subscribe((result: any) => {
      this.mandatory=result.mandatory
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
  view(params:any){debugger
    const parameter1 = `${params}&Pending`;
    const secretKey = 'storyboarderp';
    const encryptedParam = AES.encrypt(parameter1,secretKey).toString();
      var url = '/ITS/ItsMstTaskStatusView?hash=' + encodeURIComponent (encryptedParam);
    this.Router.navigateByUrl(url)
    
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
untag() {
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

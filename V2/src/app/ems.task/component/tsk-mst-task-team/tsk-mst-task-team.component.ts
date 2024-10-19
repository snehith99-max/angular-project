import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
interface objInterface { employee_name: string, employee_gid: string, user_gid: string, team_gid: string }
import { SelectionModel } from '@angular/cdk/collections';
@Component({
  selector: 'app-tsk-mst-task-team',
  templateUrl: './tsk-mst-task-team.component.html',
  styleUrls: ['./tsk-mst-task-team.component.scss']
})
export class TskMstTaskTeamComponent {
  Team_list: any;
  parametervalue: any;
  employee_list: any[] = [];
  list: any[] = [];
  selection = new SelectionModel<objInterface>(true, []);
  selection2 = new SelectionModel<objInterface>(true, []);
  team_gid: any;
  searchText1 = ''
  searchText = ''
  selection3 = new SelectionModel<objInterface>(true, []);
  selection4 = new SelectionModel<objInterface>(true, []);
  unasignedlist: any[] = [];
  employeemanager_list: any[] = [];
  managerlist: any;
  member: any;
  menulevel: any;
  hide: boolean = false
  show: boolean = false
  teamname_gid: any;
  txt_process: any
  constructor(private SocketService: SocketService, private NgxSpinnerService: NgxSpinnerService, private ToastrService: ToastrService, public router: Router, private FormBuilder: FormBuilder) {
    this.createEditForm();

    this.createForm();
  }
  AddForm!: FormGroup | any;
  EditForm!: FormGroup | any;
  editformdata = {
    txteditcode: '',
    txteditteamname: '',
    txteditteamnames: '',
    txteditprocess: ''
  };
  createForm() {
    this.AddForm = this.FormBuilder.group({
      txtteamname: ['', [Validators.required]],
      txtteamnames: ['', [Validators.required, Validators.pattern(/^(\S+\s*)*(?!\s).*$/)]],
      txt_process: [''],

    });
  }
  createEditForm() {
    this.EditForm = this.FormBuilder.group({
      txteditteamnames: ['', [Validators.required, Validators.pattern(/^(\S+\s*)*(?!\s).*$/)]],
      txt_editprocess: [''],
      txteditteamname: ['', [Validators.required]],
    });
  }
  module() {
    this.hide = true
    this.show = false
    this.AddForm.get('txtteamname').setValidators([Validators.required]);
    this.AddForm.get('txtteamname').updateValueAndValidity();
    this.EditForm.get('txteditteamname').setValidators([Validators.required]);
    this.EditForm.get('txteditteamname').updateValueAndValidity();
    this.AddForm.get('txtteamnames').reset();
    this.AddForm.get('txtteamnames').clearValidators();
    this.AddForm.get('txtteamnames').updateValueAndValidity();
    this.EditForm.get('txteditteamnames').reset();
    this.EditForm.get('txteditteamnames').clearValidators();
    this.EditForm.get('txteditteamnames').updateValueAndValidity();
  }
  other() {
    this.hide = false
    this.show = true
    this.AddForm.get('txtteamnames').setValidators([Validators.required, Validators.pattern(/^(\S+\s*)*(?!\s).*$/)]);
    this.AddForm.get('txtteamnames').updateValueAndValidity();
    this.AddForm.get('txtteamname').reset();
    this.AddForm.get('txtteamname').clearValidators();
    this.AddForm.get('txtteamname').updateValueAndValidity();
    this.EditForm.get('txteditteamnames').setValidators([Validators.required,Validators.pattern(/^(\S+\s*)*(?!\s).*$/)]);
    this.EditForm.get('txteditteamnames').updateValueAndValidity();
    this.EditForm.get('txteditteamname').reset();
    this.EditForm.get('txteditteamname').clearValidators();
    this.EditForm.get('txteditteamname').updateValueAndValidity();
  }
  ngOnInit() {
    this.NgxSpinnerService.show();
    var url = 'TskMstCustomer/TeamSummary';
    this.SocketService.get(url).subscribe((result: any) => {
      if (result.team_list != null) {
        $('#bsopssummary').DataTable().destroy();
        this.Team_list = result.team_list;
        this.NgxSpinnerService.hide();
        setTimeout(() => {
          $('#bsopssummary').DataTable();
        }, 1);
      }
      else {
        this.Team_list = result.team_list;
        setTimeout(() => {
          var table = $('#bsopssummary').DataTable();
        }, 1);
        this.NgxSpinnerService.hide();
        $('#bsopssummary').DataTable().destroy();
        this.NgxSpinnerService.hide();
      }
    });
    var url = 'TskMstCustomer/levelone_menu';
    this.SocketService.get(url).subscribe((result: any) => {
      this.menulevel = result.menulevel;
    });
  }
  addteam() {
    debugger;
    if (this.AddForm.valid) {
      let params;
      if (this.AddForm.value.txt_process === 'Other') {
        params = {
          team_name: this.AddForm.value.txtteamnames,
          teamname_gid: '',
          process: this.AddForm.value.txt_process,
        };
      } else {
        params = {
          team_name: this.AddForm.value.txtteamname.module_name,
          teamname_gid: this.AddForm.value.txtteamname.module_gid,
          process: this.AddForm.value.txt_process,
        };
      }
      this.NgxSpinnerService.show();
      const url = 'TskMstCustomer/teamadd';
      this.SocketService.post(url, params).subscribe((result: any) => {
        this.NgxSpinnerService.hide();
        if (result.status === true) {
          this.ToastrService.success(result.message);
        } else {
          this.ToastrService.warning(result.message);
        }
        this.ngOnInit();
      });
    }
  }
  
  sortColumn(columnKey: string): void {
    return this.SocketService.sortColumn(columnKey);
  }

  getSortIconClass(columnKey: string) {
    return this.SocketService.getSortIconClass(columnKey);
  }
  delete(team_gid: any) {
    this.parametervalue = team_gid
  }
  ondelete() {
    this.NgxSpinnerService.show();
    var params = {
      team_gid: this.parametervalue
    }
    var url = 'TskMstCustomer/taskdelete';
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
  openteam() {
    this.AddForm.reset()
    this.hide = false
    this.show = false

  }
  Updateteam() {debugger
    this.NgxSpinnerService.show();
    let params;
      if (this.editformdata.txteditprocess === 'Other') {
        params = {
          team_gid:this.team_gid,
          team_name: this.EditForm.value.txteditteamnames,
          teamname_gid: '',
        };
      } else {
        params = {
          team_gid:this.team_gid,
          team_name: this.EditForm.value.txteditteamname.module_name,
          teamname_gid: this.EditForm.value.txteditteamname.module_gid,
        };
      }
    var url = 'TskMstCustomer/Updateteam';
    this.SocketService.post(url, params).subscribe((result: any) => {
      if (result.status == true) {
        this.ToastrService.success(result.message);
        this.NgxSpinnerService.hide();
        this.ngOnInit();
      }
      else {
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();
      }

    })
  }
  edit(team_gid: any) {
    debugger
    this.team_gid = team_gid;
    var param = {
      team_gid: team_gid
    }
    this.NgxSpinnerService.show();
    var url = 'TskMstCustomer/teamedit';
    this.SocketService.getparams(url, param).subscribe((result: any) => {
      this.editformdata.txteditteamname = result.team_name;
      this.editformdata.txteditteamnames = result.team_name;
      this.editformdata.txteditcode = result.team_code;
      this.editformdata.txteditprocess = result.process
      if (this.editformdata.txteditprocess === 'Other') {
        this.show = true
        this.other()
      }
      else {
        this.hide = true
        this.module()
      }
      this.teamname_gid = result.teamname_gid
      this.managerlist = result.assignman_list
      this.member = result.assignmem_list
      this.NgxSpinnerService.hide();
    });
 
  }
  assignmanager(team_gid: any) {
    debugger
    this.team_gid = team_gid;
    var param = {
      team_gid: team_gid,
    }
    this.NgxSpinnerService.show()
    var url = 'TskMstCustomer/teamedit';
    this.SocketService.getparams(url, param).subscribe((result: any) => {
      this.editformdata.txteditteamname = result.team_name;
      this.NgxSpinnerService.hide()

    });
    this.searchText = ''
    this.searchText1 = ''

    this.teammanager();

    this.selection4.clear();
    this.selection3.clear();

  }
  teammanager() {
    debugger
    this.NgxSpinnerService.show();
    let param = {
      team_gid: this.team_gid,
    };

    var url = 'TskMstCustomer/GetUnassignedmanagerlist';
    this.SocketService.getparams(url, param).subscribe((result: any) => {
      this.employeemanager_list = result.GetUnassignedlist || [];
      var url1 = 'TskMstCustomer/GetAssignedmanagerlist';
      this.SocketService.getparams(url1, param).subscribe((result: any) => {
        this.unasignedlist = result.GetAssignedlist || [];
        this.NgxSpinnerService.hide();
      });
    });
  }
  masterToggle3() {
    this.isAllSelected3() ? this.selection3.clear() :
      this.employeemanager_list.forEach((row: objInterface) => this.selection3.select(row));
  }
  isAllSelected3() {
    const numSelected = this.selection3.selected.length;
    const numRows = this.employeemanager_list.length;
    return numSelected === numRows;
  }
  isSelected4() {
    const numSelected = this.selection4.selected.length;
    const numRows = this.unasignedlist.length;
    return numSelected === numRows;
  }


  masterToggle4() {
    this.isSelected4() ? this.selection4.clear() :
      this.unasignedlist.forEach((row: objInterface) => this.selection4.select(row));
  }
  resetValues() {
    this.employeemanager_list = []
    this.unasignedlist = []
  }
  manageruntag() {
    this.selection4.selected.forEach(selectedEmployee => {
      const index = this.unasignedlist.findIndex(employee1 => employee1.employee_name === selectedEmployee.employee_name);
      if (index !== -1) {
        this.NgxSpinnerService.show()

        const deletedMember = this.unasignedlist.splice(index, 1)[0];
        this.employeemanager_list.splice(deletedMember.index, 0, deletedMember);
      }
      setTimeout(() => {
        this.NgxSpinnerService.hide()
      }, 500);
    });
    this.searchText1 = ''

    this.selection4.clear();
    this.selection3.clear();
  }
  managertag() {
    this.selection3.selected.forEach(selectedEmployee => {
      const index = this.employeemanager_list.findIndex(employee1 => employee1.employee_name === selectedEmployee.employee_name);
      if (index !== -1) {
        this.employeemanager_list.splice(index, 1);
        this.NgxSpinnerService.show()
        this.unasignedlist.push({
          employee_name: selectedEmployee.employee_name,
          employee_gid: selectedEmployee.employee_gid,

          index: index,
        });
        setTimeout(() => {
          this.NgxSpinnerService.hide()
        }, 500);
      }
    });
    this.selection4.clear();
    this.selection3.clear();
    this.searchText = ''
  }
  assignmanagersubmit() {
    var params = {
      team_gid: this.team_gid,
      assignman_list: this.unasignedlist
    }
    console.log(params)
    this.NgxSpinnerService.show();
    var url = 'TskMstCustomer/Addassignmanager';
    this.SocketService.post(url, params).subscribe((result: any) => {
      this.NgxSpinnerService.hide();

      if (result.status == true) {
        this.ToastrService.success(result.message);
      } else {
        this.ToastrService.warning(result.message);
      }
      this.ngOnInit();
    });
    this.employeemanager_list = []
  }
  assignsubmit() {
    var params = {
      team_gid: this.team_gid,
      assignmem_list: this.list
    }
    console.log(params)
    this.NgxSpinnerService.show();
    var url = 'TskMstCustomer/Addassignmember';
    this.SocketService.post(url, params).subscribe((result: any) => {
      this.NgxSpinnerService.hide();

      if (result.status == true) {
        this.ToastrService.success(result.message);
      } else {
        this.ToastrService.warning(result.message);
      }
      this.ngOnInit();
    });
    this.employee_list = []
  }

  teammemmber() {
    debugger
    this.NgxSpinnerService.show();
    let param = {
      team_gid: this.team_gid,
    };

    var url = 'TskMstCustomer/GetUnassignedlist';
    this.SocketService.getparams(url, param).subscribe((result: any) => {
      this.employee_list = result.GetUnassignedlist || [];
      var url1 = 'TskMstCustomer/GetAssignedlist';
      this.SocketService.getparams(url1, param).subscribe((result: any) => {
        this.list = result.GetAssignedlist || [];

        this.NgxSpinnerService.hide();
      });
    });
  }
  resetTaggedValues() {
    this.list = []; // As
    this.employee_list = []

    // this.ngOnInit()
  }
  isSelected2() {
    const numSelected = this.selection2.selected.length;
    const numRows = this.list.length;
    return numSelected === numRows;
  }


  masterToggle2() {
    this.isSelected2() ? this.selection2.clear() :
      this.list.forEach((row: objInterface) => this.selection2.select(row));
  }
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.employee_list.length;
    return numSelected === numRows;
  }
  masterToggle() {

    this.isAllSelected() ? this.selection.clear() :
      this.employee_list.forEach((row: objInterface) => this.selection.select(row));
  }
  tag() {
    debugger
    this.selection.selected.forEach(selectedEmployee => {
      const index = this.employee_list.findIndex(employee => employee.employee_name === selectedEmployee.employee_name);
      if (index !== -1) {
        this.employee_list.splice(index, 1);
        this.NgxSpinnerService.show()
        this.list.push({
          employee_name: selectedEmployee.employee_name,
          employee_gid: selectedEmployee.employee_gid,

          index: index,
        });
      }
      setTimeout(() => {
        this.NgxSpinnerService.hide()
      }, 500);
    });
    this.selection.clear();
    this.searchText = ''
  }
  // untag() {

  //   this.selection2.selected.forEach(selectedEmployee => {
  //     const index = this.list.findIndex(employee => employee.employee_name === selectedEmployee.employee_name);
  //     if (index !== -1) {
  //       this.NgxSpinnerService.show()
  //       const deletedMember = this.list.splice(index, 1)[0]; 
  //       this.employee_list.splice(deletedMember.index, 0, deletedMember);
  //   }
  //   setTimeout(() => {
  //     this.NgxSpinnerService.hide()
  //   }, 500);
  // });
  // this.selection2.clear();
  // this.searchText1 = ''
  // }
  untag() {
    this.selection2.selected.forEach(selectedEmployee => {
      const index = this.list.findIndex(employee => employee.employee_name === selectedEmployee.employee_name);
      if (index !== -1) {
        var params = {
          team_gid: selectedEmployee.team_gid
        };
        this.NgxSpinnerService.show()
        var url = 'TskMstCustomer/conditionmember';
        this.SocketService.getparams(url, params).subscribe((result: any) => {
          if (result.status == true) {
            const deletedMember = this.list.splice(index, 1)[0];
            this.employee_list.splice(deletedMember.index, 0, deletedMember);
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
  matchesSearch(item: any): boolean {
    const searchString = this.searchText.toLowerCase();
    return item.employee_name.toLowerCase().includes(searchString);
  }
  matchesSearchs(item: any): boolean {
    const searchString = this.searchText1.toLowerCase();
    return item.employee_name.toLowerCase().includes(searchString);
  }
  assignsmember(team_gid: any) {
    debugger
    this.team_gid = team_gid;
    var param = {
      team_gid: team_gid,
    }
    this.NgxSpinnerService.show()
    var url = 'TskMstCustomer/teamedit';
    this.SocketService.getparams(url, param).subscribe((result: any) => {
      this.editformdata.txteditteamname = result.team_name;
      this.NgxSpinnerService.hide()
    });
    this.searchText = ''
    this.searchText1 = ''

    this.teammemmber();
    this.selection2.clear();
    this.selection.clear();

  }
}

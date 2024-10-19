import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { FormControl, FormGroup, Validators, FormBuilder } from '@angular/forms';
import { AES } from 'crypto-js';
import { SelectionModel } from '@angular/cdk/collections';
interface objInterface1 { status_log: string }
interface objInterface {team_name:string,team_gid:string}
@Component({
  selector: 'app-tsk-mst-customer',
  templateUrl: './tsk-mst-customer.component.html',
  styleUrls: ['./tsk-mst-customer.component.scss'],
})
export class TskMstCustomerComponent {
  assignteamform!:FormGroup
  EditForm!: FormGroup;
  AddForm!: FormGroup;
  stsform!: FormGroup;
  searchText1=''
  searchText=''
  customer_list: any;
  selection = new SelectionModel<objInterface>(true, []);
  selection2 = new SelectionModel<objInterface>(true, []);
  editformdata = {
    txteditproject: '',
    txteditprojectcode: '',
  };
  statusFormData = {
    txtOccupationtypename: '',
    rbo_status: '',
    txtremarks: '',
  };
  project_gid: any;
  parametervalue: any;
  Occupationtypeinactivelog_data: any;
  current_status : any;
  editloglist: any;
  Team_list: any;
  team: any;
  project_list:any
  projectname_gid: any;
  GetUnassignedmodulelist: any[]=[];
  GetAssignedmodulelist: any[]=[];
  listteam: any;
  constructor(public router: Router, private ToastrService: ToastrService, private NgxSpinnerService: NgxSpinnerService, private SocketService: SocketService, public FormBuilder: FormBuilder) {
    this.createForm();
    this.createEditForm();
    this.createstsform();
    this.assignform()
  }
  assignform(){
    this.assignteamform=this.FormBuilder.group({
      team_name: ['' ,Validators.required],
    })
  }
  // Typescript for Getting customer summary Details from API// 
  //code written by sathish//
  ngOnInit(): void {
    var url = 'TskMstCustomer/CustomerSummary';
    this.NgxSpinnerService.show();
    this.SocketService.get(url).subscribe((result: any) => {
      if (result.project_list != null) {
        $('#OccupationtypeSummary').DataTable().destroy();
        this.project_list = result.project_list;
        this.NgxSpinnerService.hide();
        setTimeout(() => {
          $('#OccupationtypeSummary').DataTable();
        }, 1);
      }
      else {
        this.project_list = result.project_list;
        setTimeout(() => {
          var table = $('#OccupationtypeSummary').DataTable();
        }, 1);
        this.NgxSpinnerService.hide();
        $('#OccupationtypeSummary').DataTable().destroy();
        this.NgxSpinnerService.hide();
      }
    });
    var url = 'TskMstCustomer/TeamSummary';
    this.SocketService.get(url).subscribe((result: any) => {
      this.Team_list = result.team_list;
      if (this.Team_list != null) {
        this.Team_list = this.Team_list.filter((item: objInterface1) => item.status_log === 'Y');
      }
    });
    var url = 'TskMstCustomer/projectlist';
    this.SocketService.get(url).subscribe((result: any) => {
      this.customer_list = result.customer_list;
    });
  }
  createForm() {
    this.AddForm = this.FormBuilder.group({
      txtproject: ['', [Validators.required]],
    });
  }
  createEditForm() {
    this.EditForm = this.FormBuilder.group({
      txteditproject: ['', [Validators.required]],
    });
  }
  createstsform() {
    this.stsform = this.FormBuilder.group({
      txtremarks: ['', [Validators.required, Validators.pattern(/^(\S+\s*)*(?!\s).*$/)]],
      rbo_status: ['']
    })
  }

  click() {
    this.AddForm.reset();
  }
  // Typescript for Getting customer Add Details from API// 
  //code written by sathish//
  addcustomer() {
    if (this.AddForm.valid) {
      const params = {
        project_name: this.AddForm.value.txtproject.customer_name,
        projectname_gid: this.AddForm.value.txtproject.customer_gid,
      };
      this.NgxSpinnerService.show();
      var url = 'TskMstCustomer/Customeradd';
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
  }
  // Typescript for Getting customer Edit Details from API// 
  //code written by sathish//  
  Edit(project_gid: any) {debugger
    this.project_gid = project_gid;
    var param = {
      project_gid: project_gid
    }
    this.NgxSpinnerService.show();
    var url = 'TskMstCustomer/Customeredit';
    this.SocketService.getparams(url, param).subscribe((result: any) => {
      this.editformdata.txteditprojectcode = result.project_code;
      this.editformdata.txteditproject = result.project_name;
      this.listteam=result.listteam
      this.projectname_gid = result.projectname_gid;
      this.NgxSpinnerService.hide();
    });
  }
  // Typescript for Getting customer Update Details from API// 
  //code written by sathish//
  Updatecustomer() {debugger
    this.NgxSpinnerService.show();
    var params = {
      project_gid: this.project_gid,
      projectname_gid  :(this.EditForm.value.txteditproject.customer_gid == undefined) ? this.projectname_gid : this.EditForm.value.txteditproject.customer_gid,
      project_name  :(this.EditForm.value.txteditproject.customer_name == undefined) ? this.EditForm.value.txteditproject : this.EditForm.value.txteditproject.customer_name,
    }
    var url = 'TskMstCustomer/Updatecustomer';
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
 

    
  // Typescript for Getting customer delete Details from API// 
  //code written by sathish//
  delete(project_gid: any) {
    this.parametervalue = project_gid

  }
  ondelete() {
    this.NgxSpinnerService.show();
    var params = {
      project_gid: this.parametervalue
    }
    var url = 'TskMstCustomer/Customerdelete';
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
    // Typescript for Getting sorting Details from UI// 
  //code written by sathish//
  sortColumn(columnKey: string): void {
    return this.SocketService.sortColumn(columnKey);
  }

  getSortIconClass(columnKey: string) {
    return this.SocketService.getSortIconClass(columnKey);
  }
  assignteam(project_gid:any){
    this.assignteamform.reset()
    this.project_gid=project_gid
    var param = {
      project_gid: project_gid
    }
    this.NgxSpinnerService.show();
    var url = 'TskMstCustomer/Customeredit';
    this.SocketService.getparams(url, param).subscribe((result: any) => {
      this.editformdata.txteditproject = result.project_name;
      this.NgxSpinnerService.hide();
    })
    this.teammanager()
  }
  teammanager() {debugger
    this.NgxSpinnerService.show();
    let param = {
      project_gid: this.project_gid,
    };

    var url = 'TskMstCustomer/GetUnassignedmodulelist';
    this.SocketService.getparams(url, param).subscribe((result: any) => {
      this.GetUnassignedmodulelist = result.GetUnassignedmodule_list || [];
      var url1 = 'TskMstCustomer/GetAssignedmodulelist';
      this.SocketService.getparams(url1, param).subscribe((result: any) => {
        this.GetAssignedmodulelist = result.GetAssignedmodule_list || [];
        this.NgxSpinnerService.hide();
      });
    });
  }
  submitassign(){
      const params = {
        project_gid:this.project_gid,
        listteam:this.GetAssignedmodulelist
      };
      console.log(params)
      this.NgxSpinnerService.show();
      var url = 'TskMstCustomer/getassignteam';
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
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.GetUnassignedmodulelist.length;
    return numSelected === numRows;
  }
  masterToggle() {
  
    this.isAllSelected() ? this.selection.clear() :
      this.GetUnassignedmodulelist.forEach((row: objInterface) => this.selection.select(row));
  }
  isSelected2() {
    const numSelected = this.selection2.selected.length;
    const numRows = this.GetAssignedmodulelist.length;
    return numSelected === numRows;
  }
  
  
  masterToggle2(){
    this.isSelected2() ? this.selection2.clear() :
    this.GetAssignedmodulelist.forEach((row: objInterface) => this.selection2.select(row));
  }
  tag() {debugger
    this.selection.selected.forEach(selectedEmployee => {
        const index = this.GetUnassignedmodulelist.findIndex(employee => employee.team_name === selectedEmployee.team_name);
        if (index !== -1) {
            this.GetUnassignedmodulelist.splice(index, 1);
            this.NgxSpinnerService.show()
            this.GetAssignedmodulelist.push({
              team_name: selectedEmployee.team_name,
              team_gid: selectedEmployee.team_gid,
  
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
    return item.team_name.toLowerCase().includes(searchString);
  }
  matchesSearchs(item: any): boolean {
    const searchString = this.searchText1.toLowerCase();
    return item.team_name.toLowerCase().includes(searchString);
  }
untag() {
    this.selection2.selected.forEach(selectedEmployee => {
        const index = this.GetAssignedmodulelist.findIndex(employee1 => employee1.team_name === selectedEmployee.team_name);
        if (index !== -1) {
            this.GetAssignedmodulelist.splice(index, 1);
            this.NgxSpinnerService.show()
            this.GetUnassignedmodulelist.push({
              team_name: selectedEmployee.team_name,
                team_gid: selectedEmployee.team_gid,

                index: index,
            });
            setTimeout(() => {
              this.NgxSpinnerService.hide()
            }, 500);
        }
    });
    this.selection2.clear();
    this.searchText1=''
  }
  resetTaggedValues() {
    this.GetAssignedmodulelist = []; // As
    this.GetUnassignedmodulelist=[]

    // this.ngOnInit()
}
}




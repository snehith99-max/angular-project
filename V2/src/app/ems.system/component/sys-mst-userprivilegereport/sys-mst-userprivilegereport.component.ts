import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES, enc } from 'crypto-js';
import { SelectionModel } from '@angular/cdk/collections';


interface objInterface { module_gid: string; employee_gid: string, module_checked: boolean }
interface Sub2MenuItem { module_gid: string; text: string, module_checked: boolean }
interface Sub1MenuItem { sub2menu: Sub2MenuItem[]; }
interface SubMenuItem { sub1menu: Sub1MenuItem[]; }
interface UserRoleMenuItem { submenu: SubMenuItem[]; }

@Component({
  selector: 'app-sys-mst-userprivilegereport',
  templateUrl: './sys-mst-userprivilegereport.component.html',
  styleUrls: ['./sys-mst-userprivilegereport.component.scss'],
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
`]
})
export class SysMstUserprivilegereportComponent {
  Userprivilege: FormGroup | any;
  mdlempcode: any;
  Employee_list: any[] = [];
  user_list: any[] = [];
  responsedata: any;
  usergrouptemplateform!: FormGroup;
  selection = new SelectionModel<objInterface>(true, []);
  showOptionsDivId: any;
  user_gid: any;


  UserMenuList: any;

  constructor(private SocketService: SocketService,private route:Router,private NgxSpinnerService: NgxSpinnerService, public service: SocketService, private ToastrService: ToastrService, private FormBuilder: FormBuilder) {
    this.Userprivilege = new FormGroup({
      user_gid: new FormControl(''),
    })
  }
  ngOnInit(): void {

    this.GetUserPrivilegeSummary();


    // var api = 'UserPrivilege/GetEmployeedropdown';
    // this.service.get(api).subscribe((result: any) => {
    //   this.responsedata = result;
    //   this.Employee_list = this.responsedata.employeelists;
    // });
    //this.worktypesummary();
  }
  employeeDetailsFetch() {
    let user_gid = this.Userprivilege.get('user_gid')?.value;


    let param = {
      user_gid: user_gid
    }
  var url = 'UserPrivilege/GetUserPrivilegeList';
    this.SocketService.getparams(url,param).subscribe((result: any) => {
      if (result.status != null) {
        this.UserMenuList = result.menu_list;

        console.log(this.UserMenuList);
        this.NgxSpinnerService.hide();
        this.initializeSelection();
      }
    });

  }
  initializeSelection() {
    for (const data of this.UserMenuList) {
      for (const j of data.submenu) {
        for (const k of j.sub1menu) {
          if (k.menu_access === 'Y') {
            this.updateSelection(k);
          }
        }
      }
    }
  }

  updateSelection(item: any) {
    if (item.menu_access === 'Y') {
      this.selection.select(item);
    }
  }

  masterToggle() {
    if (this.isAllSelected())
      this.selection.clear()
    else {
      const ModuleCheckedDate = this.UserMenuList.flatMap((item1: UserRoleMenuItem) =>
        item1.submenu.flatMap((item2: SubMenuItem) =>
          item2.sub1menu
        )

      );
      this.selection.select(...ModuleCheckedDate);
    }
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    return numSelected;
  }

  // employeeDetailsFetch() {
  //   debugger
  //   let user_gid = this.Userprivilege.get('user_gid')?.value;


  //   let param = {
  //     user_gid: user_gid
  //   }
  //   this.NgxSpinnerService.show()
  //   var url = 'UserPrivilege/GetOnChangeEmployee';
  //   this.service.getparams(url, param).subscribe((result: any) => {
  //     this.responsedata = result;
  //     this.user_list = this.responsedata.GetEmployeeonchangedetails;
  //     this.NgxSpinnerService.hide()
  //     this.Userprivilege.get("user_gid")?.setValue(this.user_list[0].Name);
  //     this.NgxSpinnerService.hide()

  //   });
  // }


  GetUserPrivilegeSummary() {
    var url = 'UserPrivilege/GetUserPrivilegeSummary'
    this.service.get(url).subscribe((result: any) => {
    this.responsedata = result;
      this.Employee_list = this.responsedata.employeelists;
      setTimeout(() => {
        $('#Employee_list').DataTable();
      }, 1);
    });
  }


  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }


  userprivilege(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.route.navigate(['/system/SysMstUserPriRep', encryptedParam])
  }


}
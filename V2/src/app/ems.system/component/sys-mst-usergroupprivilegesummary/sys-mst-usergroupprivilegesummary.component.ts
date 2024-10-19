import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-sys-mst-usergroupprivilegesummary',
  templateUrl: './sys-mst-usergroupprivilegesummary.component.html',
  styleUrls: ['./sys-mst-usergroupprivilegesummary.component.scss'],
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

export class SysMstUsergroupprivilegesummaryComponent {
  showOptionsDivId: any;
  response_data: any;
  UserGroupList: any;
  parameterValue: any;
  usergrouppopup_list:any;
  menu_access: any;
  i:any;
  constructor(private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private SocketService: SocketService,
    private ToastrService: ToastrService,
    public service: SocketService,
    private NgxSpinnerService: NgxSpinnerService) {
  }

  ngOnInit(): void {

    this.GetUserGroupTempSummary();
    // var api = 'SysMstUserGroupTemp/GetUserGroupTempSummary';
    // this.NgxSpinnerService.show();
    // this.SocketService.get(api).subscribe((result: any) => {
    //   this.NgxSpinnerService.hide();
    //   this.response_data = result;
    //   this.UserGroupList = this.response_data.MdlSysMstUserGroupList;

    //   setTimeout(() => {
    //     $('#usergrouptemp_list').DataTable();
    //   }, 1);
    // });
  }

  GetUserGroupTempSummary() {
    var api = 'SysMstUserGroupTemp/GetUserGroupTempSummary';
    this.NgxSpinnerService.show();
    this.SocketService.get(api).subscribe((result: any) => {
      $('#usergrouptemp_list').DataTable().destroy();
      this.NgxSpinnerService.hide();
      this.response_data = result;
      this.UserGroupList = this.response_data.MdlSysMstUserGroupList;

      setTimeout(() => {
        $('#usergrouptemp_list').DataTable();
      }, 1);
    });
  }

  toggleStatus(data: any) {
    if (data.statuses === 'Active') {
      this.openModalinactive(data);
    } else {
      this.openModalactive(data);
    }
  }
  openModalactive(parameter: string) {
    this.parameterValue = parameter;
  }

  onActivate() {
    console.log(this.parameterValue);
    var url = 'SysMstUserGroupTemp/GetusergroupActive'
    this.NgxSpinnerService.show();
    this.SocketService.getid(url, this.parameterValue).subscribe((result: any) => {

      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide();
      }
      else {
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
      }  
      this.GetUserGroupTempSummary();
    });
  }

openModalinactive(parameter: string) {
  this.parameterValue = parameter;
}

oninactive() {
  console.log(this.parameterValue);
  var url = 'SysMstUserGroupTemp/GetusergroupInactive'
  this.NgxSpinnerService.show();
  this.SocketService.getid(url, this.parameterValue).subscribe((result: any) => {

    if (result.status == false) {
      this.ToastrService.warning(result.message)
      this.NgxSpinnerService.hide();

    }
    else {
      this.ToastrService.success(result.message)
      this.NgxSpinnerService.hide();
    }
    this.GetUserGroupTempSummary();

  });
}
templatepopup(usergrouptemplate_gid:any) {
  const scrollContainer = document.getElementById('scroll-container');
  if (scrollContainer) {
    scrollContainer.scrollTop = 0;
  }
  var params = {
    usergrouptemplate_gid: usergrouptemplate_gid
  }
  var url = 'SysMstUserGroupTemp/Getuesrgroupdetails';
  this.SocketService.getparams(url, params).subscribe((result: any) => {
    if (result.status != null) {
      this.usergrouppopup_list = result.menu_list;

      console.log(this.usergrouppopup_list);
      this.NgxSpinnerService.hide();
      this.initializeSelection();
      this.assignMenuAccess();
    }
  });
}
initializeSelection() {
  for (const data of this.usergrouppopup_list) {
    for(const i of data.sub2menu) {
    for (const j of data.submenu) {
      for (const k of j.sub1menu) {
      
      }
    }
  }
  }

}

assignMenuAccess(): void {
  if (this.usergrouppopup_list && Array.isArray(this.usergrouppopup_list)) {
    this.usergrouppopup_list.forEach((menuItem) => {
      if (menuItem.submenu && Array.isArray(menuItem.submenu)) {
        menuItem.submenu.forEach((submenuItem: { sub1menu: any[]; }) => {
          if (submenuItem.sub1menu && Array.isArray(submenuItem.sub1menu)) {
            submenuItem.sub1menu.forEach((sub1menuItem) => {
             
              this.menu_access = sub1menuItem.menu_access;

           
              console.log('Assigned menu_access:', this.menu_access);

             
            });
          }
        });
      }
    });
  } else {
    console.log('usergrouppopup_list is empty or not defined.');
  }
}

onedit(params: any) {
  const secretKey = 'storyboarderp';
  const param = (params); // Stringify if params is an object
  const encryptedParam = AES.encrypt(param, secretKey).toString();
  this.router.navigate(['/system/SysMstUsergroupprivilegeedit', encryptedParam]);
}

  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }

  ondelete() {
    let param = {
      usergrouptemplate_gid: this.parameterValue
    }
    var url = 'SysMstUserGroupTemp/DeleteUserGroupDetails'
    this.SocketService.getparams(url, param).subscribe((result: any) => {
      if (result.status == true) {
        this.ToastrService.success(result.message)
        this.GetUserGroupTempSummary();
      }
      else {
        this.ToastrService.warning(result.message)
        
      }
      
    });    
  } 
}

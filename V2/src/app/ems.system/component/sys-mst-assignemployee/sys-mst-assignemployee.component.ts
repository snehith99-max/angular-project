import { Component, OnInit, TemplateRef, ElementRef, ViewChild, ChangeDetectorRef } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AES, enc } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { SelectionModel } from '@angular/cdk/collections';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from 'src/environments/environment.development';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

interface objInterface { module_gid: string; employee_gid: string, module_checked: boolean }
interface Sub2MenuItem { module_gid: string; text: string, module_checked: boolean }
interface Sub1MenuItem { sub2menu: Sub2MenuItem[]; }
interface SubMenuItem { sub1menu: Sub1MenuItem[]; }
interface UserRoleMenuItem { submenu: SubMenuItem[]; }

@Component({
  selector: 'app-sys-mst-assignemployee',
  templateUrl: './sys-mst-assignemployee.component.html',
  styleUrls: ['./sys-mst-assignemployee.component.scss']
})

export class SysMstAssignemployeeComponent implements OnInit {
  @ViewChild('checkAll', { static: true }) checkAll!: ElementRef<HTMLInputElement>;
  module_gid: any
  user_gid: any;
  employeelist: any
  cboselectedEmployee: any
  cboassignhierarchy: any
  ModuleAssignedemployeeinfo: any
  ModuleHierarchy: any;
  userRoleMenulist: any;
  selectedCheckboxData: any[] = [];
  selection = new SelectionModel<objInterface>(true, []);
  assignempform!: FormGroup;
  usergrouptemplist: any;
  cbousergrouptemp: any;
  checkAllState: boolean = false;
  menu_access: any;
  
  constructor(public router: Router, private ActivatedRoute: ActivatedRoute, public NgxSpinnerService: NgxSpinnerService,
    private SocketService: SocketService, private ToastrService: ToastrService, private FormBuilder: FormBuilder,
    private changeDetectorRef: ChangeDetectorRef) {

    this.assignempform = new FormGroup({
      employee_name: new FormControl('', [Validators.required]),
      hierarchy: new FormControl('', [Validators.required]),
      usergrouptemp: new FormControl(''),
    })
  }
  
  ngOnInit(): void {
    this.NgxSpinnerService.show();
    
    this.ActivatedRoute.queryParams.subscribe(params => {
      const urlparams = params['hash'];
      if (urlparams) {
        const decryptedParam = AES.decrypt(urlparams, environment.secretKey).toString(enc.Utf8);
        const paramvalues = decryptedParam.split('&');
        this.module_gid = paramvalues[0];
      }
    });

    var params = {
      module_gid: this.module_gid
    }

    var url = 'SysMstModuleManage/GetEmployeeAssignlist';
    this.SocketService.getparams(url, params).subscribe((result: any) => {
      this.employeelist = result.employeelist;
    });

    var url = 'SysMstModuleManage/GetModuleAssignedEmployee';
    this.SocketService.getparams(url, params).subscribe((result: any) => {
      $('#assignemployeetable').DataTable().destroy();
      if (result.status != null) {
        this.ModuleAssignedemployeeinfo = result.mdlModuleAssigneddtl;
        this.ModuleHierarchy = result.mdlModuleHierarchy;
        this.NgxSpinnerService.hide();
      }
      setTimeout(() => {
        $('#assignemployeetable').DataTable();
      }, 1);
    });

    var url = 'SysMstModuleManage/GetUserGroupTemplist';
    this.SocketService.getparams(url, params).subscribe((result: any) => {
      this.usergrouptemplist = result.usergrouptemplist;
    });
  }

  addmoduleuser() {
    var employeeList = this.cboselectedEmployee.map(function (employeeId: any) {
      return { employee_gid: employeeId };
    });
    var params = {
      module_gid: this.module_gid,
      assign_hierarchy: this.cboassignhierarchy,
      Mdlassignemployeelist: employeeList,
      usergrouptemplate_gid: this.cbousergrouptemp,
    }

    this.NgxSpinnerService.show();
    var url = 'SysMstModuleManage/PostModuleEmployeeAssign';
    this.SocketService.post(url, params).subscribe((result: any) => {
      if (result.status == true) {
        this.ToastrService.success(result.message)
        this.assignempform.reset();
        this.cboselectedEmployee = null;
        this.cboassignhierarchy = null;
        this.cbousergrouptemp = null;
        this.NgxSpinnerService.hide();
        this.ngOnInit();
      }
      else {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide();
      }
    });
    // setTimeout(function () {
    //   window.location.reload();
    // }, 2000);
  }

  UserRoleClick(user_gid: any, user_code: any, user_name: any) {
    this.selection.clear();
    this.user_gid = user_gid;
    const scrollContainer = document.getElementById('scroll-container');
    if (scrollContainer) {
      scrollContainer.scrollTop = 0;
    }
    var params = {
      module_parentgid: this.module_gid,
      user_gid: user_gid
    }
    var url = 'SysMstModuleManage/GetUserRoleList';
    this.SocketService.post(url, params).subscribe((result: any) => {
      if (result.status != null) {
        this.userRoleMenulist = result.menu_list;

        console.log(this.userRoleMenulist);
        this.NgxSpinnerService.hide();
        this.initializeSelection();
        this.assignMenuAccess();
      }
    });
  }
  initializeSelection() {
    for (const data of this.userRoleMenulist) {
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

  isAllSelected(): boolean {
    const totalItems = this.userRoleMenulist
      .flatMap((item1: UserRoleMenuItem) => item1.submenu
      .flatMap((item2: SubMenuItem) => item2.sub1menu)).length;
    return this.selection.selected.length === totalItems;
  }

  updateCheckAllState(): void {
    this.checkAllState = this.isAllSelected();
  }

  masterToggle(): void {
    const allItems = this.userRoleMenulist
      .flatMap((item1: UserRoleMenuItem) => item1.submenu
      .flatMap((item2: SubMenuItem) => item2.sub1menu));

    if (this.checkAllState) {
      this.selection.clear();
    } else {
      this.selection.select(...allItems);
    }

    this.updateCheckAllState();
  }

  onItemSelect(item: any): void {
    if (this.selection.isSelected(item)) {
      this.selection.deselect(item);
      item.menu_access = 'N';
    } else {
      this.selection.select(item);
      item.menu_access = 'Y'; 
    }
    this.updateCheckAllState();
  }
  
  UserRoleselected() {
    const moduleListGid = this.selection.selected.map(data => data.module_gid).join(',');
    var params = {
      module_gid: moduleListGid,
      module_parentgid: this.module_gid,
      user_gid: this.user_gid
    }
    this.NgxSpinnerService.show();
    var url = 'SysMstModuleManage/PostPrivilege';
    this.SocketService.post(url, params).subscribe((result: any) => {
      if (result.status == true) {
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
        this.ngOnInit();
      }
      else {
        this.ToastrService.warning(result.message);   
        this.NgxSpinnerService.hide();
      }
    });
  }
  backbutton() {
    this.router.navigate(['/system/SysMstModuleManager']);
  }

  assignMenuAccess(): void {
    if (this.userRoleMenulist && Array.isArray(this.userRoleMenulist)) {
      this.userRoleMenulist.forEach((menuItem) => {
        if (menuItem.submenu && Array.isArray(menuItem.submenu)) {
          menuItem.submenu.forEach((submenuItem: { sub1menu: any[]; }) => {
            if (submenuItem.sub1menu && Array.isArray(submenuItem.sub1menu)) {
              submenuItem.sub1menu.forEach((sub1menuItem) => {
               
                this.menu_access = sub1menuItem.menu_access;
 
             
                console.log('Assigned menu_access:', this.menu_access);
 
                if(this.menu_access === 'Y'){
                  this.checkAllState = true;
                }
                else if(this.menu_access === 'N'){
                  this.checkAllState = false;
                }
                else{
                  this.checkAllState = true;
                }
              });
            }
          });
        }
      });
    } else {
      console.log('userRoleMenulist is empty or not defined.');
    }
  }

}


import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { SelectionModel } from '@angular/cdk/collections';
import { AES, enc } from 'crypto-js';
interface objInterface { module_gid: string; employee_gid: string, module_checked: boolean, menu_level: string }
interface Sub2MenuItem { module_gid: string; text: string, module_checked: boolean }
interface Sub3MenuItem { menu_level: string; text: string, module_checked: boolean }
interface Sub1MenuItem { sub2menu: Sub2MenuItem[]; }
interface SubMenuItem { sub1menu: Sub1MenuItem[]; }
interface Sub1MenuItem { sub3menu: Sub3MenuItem[]; }
interface UserRoleMenuItem { submenu: SubMenuItem[]; }

@Component({
  selector: 'app-sys-mst-usergroupprivilegeedit',
  templateUrl: './sys-mst-usergroupprivilegeedit.component.html',
  styleUrls: ['./sys-mst-usergroupprivilegeedit.component.scss']
})
export class SysMstUsergroupprivilegeeditComponent {

  usergrouptemplateform!: FormGroup;
  selection = new SelectionModel<objInterface>(true, []);

  UserMenuList: any;
  module_gid: any;
  user_gid: any;
  menu_level: any;
  module_list: any;
  response_data: any;
  usergrouptemplate_gid:any;
  MdlEditUserGroupTempList: any;
  MdlEditUserGroupTempListNew: any;

  constructor(private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private SocketService: SocketService,
    private ToastrService: ToastrService,
    private NgxSpinnerService: NgxSpinnerService) {

      this.usergrouptemplateform = new FormGroup({
        menu_level: new FormControl(''),
        menu_list: new FormControl(''),
        GetEditUserGroupTempSummary: new FormControl(''),
        usergrouptemplate_code: new FormControl('', [Validators.required, Validators.pattern(/^\S.*$/)]),
        usergrouptemplate_name: new FormControl('',[Validators.required, Validators.pattern(/^\S.*$/)])
      })
  }  
  get usergrouptemplate_code() {
    return this.usergrouptemplateform.get('usergrouptemplate_code')
  }

  get usergrouptemplate_name() {
    return this.usergrouptemplateform.get('usergrouptemplate_name')
  }

  ngOnInit(): void {
    debugger
    const usergrouptemplate_gid = this.route.snapshot.paramMap.get('usergrouptemplate_gid');
    this.usergrouptemplate_gid = usergrouptemplate_gid;

    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.usergrouptemplate_gid, secretKey).toString(enc.Utf8);

    debugger
    var url = 'SysMstUserGroupTemp/GetEditUserMenuList';
    var params = {
      usergrouptemplate_gid: deencryptedParam,
    }
    this.SocketService.getparams(url,params).subscribe((result: any) => {
      debugger;
    
        this.UserMenuList = result.menu_list;
        this.NgxSpinnerService.hide();
        this.initializeSelection();

        var url = 'SysMstUserGroupTemp/GetEditUserGroupTempSummary';
    var params = {
      usergrouptemplate_gid: deencryptedParam,
    }
    this.SocketService.getparams(url,params).subscribe((result: any) => {
      debugger;
      if (result.status != null) {
        this.MdlEditUserGroupTempListNew = result.MdlEditUserGroupTempListNew;
        debugger;
        this.usergrouptemplateform.get("usergrouptemplate_code")?.setValue(this.MdlEditUserGroupTempListNew[0].usergrouptemplate_code);
        this.usergrouptemplateform.get("usergrouptemplate_name")?.setValue(this.MdlEditUserGroupTempListNew[0].usergrouptemplate_name);
        console.log(this.MdlEditUserGroupTempListNew);
        this.NgxSpinnerService.hide();
      }
    });  
      
    }); 
    debugger;
        
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
    if (this.isAllSelected()) {
      this.selection.clear();
    } else {
  
      const ModuleCheckedData = this.UserMenuList.flatMap((item1: UserRoleMenuItem) =>
        item1.submenu.flatMap((item2: SubMenuItem) =>
          item2.sub1menu
        )
      );
      this.selection.select(...ModuleCheckedData);
    }
  }
  isAllSelected(): boolean {
    const totalItems = this.UserMenuList.flatMap((item1: UserRoleMenuItem) =>
      item1.submenu.flatMap((item2: SubMenuItem) =>
        item2.sub1menu
      )
    ).length;
    
    return this.selection.selected.length === totalItems;
  }
  masterToggleForSection(section: any) {
    const allItemsInSection = section.submenu.flatMap((submenu: any) => submenu.sub1menu);
  
    if (this.isAllSelectedForSection(section)) {
      allItemsInSection.forEach((item: objInterface) => this.selection.deselect(item));
    } else {
      allItemsInSection.forEach((item: objInterface) => this.selection.select(item));
    }
  }
  
  isAllSelectedForSection(section: any): boolean {
    const allItemsInSection = section.submenu.flatMap((submenu: any) => submenu.sub1menu);
    return allItemsInSection.every((item: objInterface) => this.selection.isSelected(item));
  }
  
  isAnySelected(): boolean {
    return this.selection.selected.length > 0;
  }  
  submit() {
    debugger;
    const usergrouptemplate_gid = this.route.snapshot.paramMap.get('usergrouptemplate_gid');
    this.usergrouptemplate_gid = usergrouptemplate_gid;

    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.usergrouptemplate_gid, secretKey).toString(enc.Utf8);
    const moduleListGid = this.selection.selected.map(data => data.module_gid).join(',');
    const menuListGid = this.selection.selected.map(data => data.menu_level).join(',');

    var params = {
      module_gid: moduleListGid,
      menu_level: menuListGid,
      usergrouptemplate_code: this.usergrouptemplateform.value.usergrouptemplate_code,
      usergrouptemplate_name: this.usergrouptemplateform.value.usergrouptemplate_name,
      usergrouptemplate_gid:deencryptedParam
    }

    this.NgxSpinnerService.show();
    var url = 'SysMstUserGroupTemp/UpdateUserGroupTemp';
    this.SocketService.post(url, params).subscribe((result: any) => {
      if (result.status == true) {
        debugger
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
        this.ngOnInit();
        this.router.navigate(['/system/SysMstusergroupprivilegesummary']); 
      }
      else {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide();
      }
    });
  }
  backbutton(){
    this.router.navigate(['/system/SysMstusergroupprivilegesummary']); 
  }

}

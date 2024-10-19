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
  selector: 'app-sys-mst-userprivilegereprt',
  templateUrl: './sys-mst-userprivilegereprt.component.html',
  styleUrls: ['./sys-mst-userprivilegereprt.component.scss']
  
})
export class SysMstUserprivilegereprtComponent {

  Userprivilege: FormGroup | any;
  mdlempcode: any;
  Employee_list: any[] = [];
  user_list: any[] = [];
  responsedata: any;
  usergrouptemplateform!: FormGroup;
  selection = new SelectionModel<objInterface>(true, []);
  showOptionsDivId: any;
  user_gid: any;
  usergid: any;
  user_name:any;


  UserMenuList: any;
j: any;

  constructor(private SocketService: SocketService,private route:Router,private router: ActivatedRoute,private NgxSpinnerService: NgxSpinnerService, public service: SocketService, private ToastrService: ToastrService, private FormBuilder: FormBuilder) {
    this.Userprivilege = new FormGroup({
      user_gid: new FormControl(''),
    })
  }

  ngOnInit(): void {

    const user_gid = this.router.snapshot.paramMap.get('user_gid');

    this.usergid = user_gid;

    const secretKey = 'storyboarderp';

    const deencryptedParam = AES.decrypt(this.usergid, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)
    this.employeeprivilege(deencryptedParam)
  }

  employeeprivilege(user_gid:any) {

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
}

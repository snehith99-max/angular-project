import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
interface ISearchL1 {
  module_name: string;
  
}
interface ISearchL2 {
  level2_menu: string;
  
}

@Component({
  selector: 'app-sys-rpt-screenprivilegereport',
  templateUrl: './sys-rpt-screenprivilegereport.component.html',
  styleUrls: ['./sys-rpt-screenprivilegereport.component.scss']
})
export class SysRptScreenprivilegereportComponent {
  level1!: ISearchL1;
  level2!: ISearchL2;
  reactiveForm!: FormGroup;
  level1menulist: any[] = [];
  level2menulist: any[] = [];
  level3menulist: any[] = [];
  level1_menu: any;
  level2_menu: any;
  level3_menu: any;
  responsedata: any;
  screenprivilege_list: any[] = [];
  employeelevel1_list: any[] = [];
  showlevel1Detail: boolean = false;
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService){
    this.level1 = {} as ISearchL1;
    this.level2 = {} as ISearchL2;
  }

  ngOnInit(): void {
    // this.level2menulist = [{ module_name2: 'All', module_gid: 'all' }, ...this.level2menulist];

    this.reactiveForm = new FormGroup({
      modulegid: new FormControl(''),
      module_name: new FormControl(''),
      module_name2: new FormControl(''),
      module_name3: new FormControl(''),
    });
    var url = 'SysRptScreenPrivilege/GetLevel1Menu';
    this.service.get(url).subscribe((result: any) => {
    this.level1menulist = result.GetLevel1Menu;     
    // this.level1menulist = [{ module_name: 'All', module_gid: 'all' }, ...this.level1menulist];
    
   
        });
        var url = 'SysRptScreenPrivilege/GetLevel2Menu';
        this.service.get(url).subscribe((result: any) => {
        this.level2menulist = result.GetLevel2Menu;     
        // this.level1menulist = [{ module_name: 'All', module_gid: 'all' }, ...this.level1menulist];
        
       
            });
            var url = 'SysRptScreenPrivilege/GetLevel3Menu';
            this.service.get(url).subscribe((result: any) => {
            this.level3menulist = result.GetLevel3Menu;     
            // this.level1menulist = [{ module_name: 'All', module_gid: 'all' }, ...this.level1menulist];
            
           
                });
 
  }

  GetScreenPrivilegeSummary() {
     debugger;
    // const selectedPrivilege = this.reactiveForm.value.level1_menu || 'null';
    
    // for (const control of Object.keys(this.reactiveForm.controls)) {
    //   this.reactiveForm.controls[control].markAsTouched();
    // }
    // const params = {
    //   module_gid : selectedPrivilege,
     
    // };
    var url = 'SysRptScreenPrivilege/GetScreenPrivilegeSummary'
    this.service.get(url).subscribe((result) => {
      this.responsedata = result;
      this.screenprivilege_list = this.responsedata.screenprivilegedatalist;
      setTimeout(() => {
        $('#screenprivilegedatalist').DataTable();
      },1);
    });
  }

  level1detail() {
    debugger;
   
    let module_gid = this.reactiveForm.get('modulegid')?.value;
    let param = {
      module_gid: module_gid
    }
  
  
  var url = 'SysRptScreenPrivilege/GetEmployeeLevel1Detail'
 
  this.service.getparams(url, param).subscribe((result: any) => {
  this.responsedata=result;
    this.employeelevel1_list = result.GetEmployeeData_Detail;
    this.reactiveForm.get("module_name2")?.setValue(this.employeelevel1_list[0].module_name);
  });
  }
}

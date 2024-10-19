import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { SelectionModel } from '@angular/cdk/collections';
import { NgxSpinnerService } from 'ngx-spinner';

export class Itemplateassign {
  assignmodule: string[] = [];
  template_gid: any;
  shift_type:any;
}
@Component({
  selector: 'app-sys-mst-template-summary',
  templateUrl: './sys-mst-template-summary.component.html',
  styleUrls: ['./sys-mst-template-summary.component.scss'],
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

export class SysMstTemplateSummaryComponent {
  showOptionsDivId: any;
  responsedata: any;
  template_list: any;
  parameterValue:any;
  assign_listt:any;
  selection = new SelectionModel<Itemplateassign>(true, []);
  CurObj: Itemplateassign = new Itemplateassign();
  pick: Array<any> = [];
  templatpopup_list:any;
  constructor(private router: Router,     private NgxSpinnerService: NgxSpinnerService,
    private route: ActivatedRoute, private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService) { }

  ngOnInit() : void {

    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.templatesummary();
  }

  templatesummary()
  {
    var url = 'SysMstTemplate/GetTemplateSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#template_list').DataTable().destroy();
      this.responsedata = result;
      this.template_list = this.responsedata.templatesummarylist;
     
      setTimeout(() => {
        $('#template_list').DataTable();
      }, 1);
    });
  }
  assignsummary()
  {
    var url = 'SysMstTemplate/Getassignmodule'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.assign_listt = this.responsedata.assignmodule;
     
    });
  }

  edittemplate(params: string){
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.router.navigate(['/system/SysMstTemplateEdit',encryptedParam])
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
  this.NgxSpinnerService.show();
var url = 'SysMstTemplate/GettemplateActive'
this.service.getid(url, this.parameterValue).subscribe((result: any) => {

  if (result.status == false) {
    this.ToastrService.warning(result.message)
    this.NgxSpinnerService.hide();

  }
  else {
    this.ToastrService.success(result.message)
    this.NgxSpinnerService.hide();
  }  
  this.templatesummary();
});
}

openModalinactive(parameter: string) {
this.parameterValue = parameter;
}

onInactivate() {

this.NgxSpinnerService.show();
var url = 'SysMstTemplate/GettemplateInactive'
this.service.getid(url, this.parameterValue).subscribe((result: any) => {

  if (result.status == false) {
    this.ToastrService.warning(result.message)
    this.NgxSpinnerService.hide();

  }
  else {
    this.ToastrService.success(result.message)
    this.NgxSpinnerService.hide();

  }
  this.templatesummary();

});
}

  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }


  ondelete() {
    console.log(this.parameterValue);
    var url3 = 'SysMstTemplate/DeleteTemplate'
    this.NgxSpinnerService.show();
    this.service.getid(url3, this.parameterValue).subscribe((result: any) => {
      $('#template_list').DataTable().destroy();
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide();
        this.templatesummary();

      }
      else {
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
        this.templatesummary();
      }
    });
    // setTimeout(function () {
    //   window.location.reload();
    // }, 2000);
  }

  // assign() {
  //   console.log(this.parameterValue);
  //   var url3 = 'SysMstTemplate/Getassignmodule'
  //   this.service.getid(url3, this.parameterValue).subscribe((result: any) => {
  //     if (result.status == false) {
  //       this.ToastrService.warning(result.message)
  //       this.templatesummary();
  //     }
  //     else {
  //       this.ToastrService.success(result.message)
  //       this.templatesummary();
  //     }
  //   });
  //   setTimeout(function () {
  //     window.location.reload();
  //   }, 2000);
  // }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.assign_listt.length;
    return numSelected === numRows;
  }

  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.assign_listt.forEach((row: Itemplateassign) => this.selection.select(row));
  }

  openassign(parameter: string){

    this.parameterValue = parameter,
    this.assignsummary()
    
  }
  assign(){
    debugger
    this.pick = this.selection.selected
    this.CurObj.assignmodule = this.pick
    this.CurObj.template_gid=this.parameterValue

    // this.CurObj.shift_type = this.reactiveFormadd.value.shift_type
    if ( this.CurObj.assignmodule.length === 0) {
      this.ToastrService.warning("Select Atleast one Module to assign");
      return;
    }  
    this.NgxSpinnerService.show();
    var url = 'SysMstTemplate/Asiignmoduletotemplate'
      this.service.post(url, this.CurObj).subscribe((result: any) => {
        this.NgxSpinnerService.hide();
        if (result.status == false) {
          this.ToastrService.warning(result.message)
       }
       else{
        this.ToastrService.success(result.message)
       }  
      }); 
      this.selection.clear(); 
  }
  isAnySelected() {
    return this.selection.selected.length > 0; // Check if any modules are selected
}
    templatepopup(data: any): void {
      debugger;
      var api1 = 'SysMstTemplate/Gettemplatedetails';
  
      let params = {
        template_gid: data.template_gid,
      };
  
      this.service.getparams(api1, params).subscribe((result: any) => {
        this.responsedata = result;
        this.templatpopup_list = this.responsedata.assignmodule;
      });
    }

    toggleOptions(account_gid: any) {
      if (this.showOptionsDivId === account_gid) {
        this.showOptionsDivId = null;
      } else {
        this.showOptionsDivId = account_gid;
      }
    }
  
}

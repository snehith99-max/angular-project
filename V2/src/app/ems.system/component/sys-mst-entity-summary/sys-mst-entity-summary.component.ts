import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, MaxLengthValidator, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';

interface IEntity {
  entity_name: string;
  entity_description: string;
  entity_gid: string;
  entity_prefix: string;
  entityedit_name: string;
}

@Component({
  selector: 'app-sys-mst-entity-summary',
  templateUrl: './sys-mst-entity-summary.component.html',
  styleUrls: ['./sys-mst-entity-summary.component.scss'],
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

export class SysMstEntitySummaryComponent {
  showOptionsDivId: any;
  reactiveForm!: FormGroup;
  reactiveFormEdit!: FormGroup;
  responsedata: any;
  parameterValue: any;
  // editedValues: any = {};
  remainingChars: any | number = 1000

  parameterValue1: any;
  entity_list: any[] = [];
  entity!: IEntity;
  constructor(private SocketService: SocketService, private NgxSpinnerService: NgxSpinnerService, public service: SocketService, private ToastrService: ToastrService, private FormBuilder: FormBuilder) {
    this.entity = {} as IEntity;
  }

  ngOnInit(): void {

    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.GetEntitySummary();
    // Form values for Add popup //
    this.reactiveForm = new FormGroup({
      entity_code: new FormControl('', [Validators.required, Validators.pattern('^[a-zA-Z0-9]{1,8}$'), Validators.pattern(/^\S.*$/), Validators.maxLength(8)]),

      // entity_prefix: new FormControl('', [Validators.required, Validators.pattern('^[a-zA-Z0-9]{5}$'), Validators.pattern(/^\S.*$/), Validators.maxLength(5)]),
      entity_prefix: new FormControl('',[Validators.required,Validators.pattern(/^\S.*$/),Validators.maxLength(8)]),
      entity_name: new FormControl('', [Validators.required,Validators.pattern(/^\S.*$/)]),
      entity_description: new FormControl(''),
    });

    // Form values for Edit popup //
    this.reactiveFormEdit = new FormGroup({
      entity_codeedit: new FormControl(''),

      entityedit_name: new FormControl(this.entity.entityedit_name,[Validators.required,Validators.pattern(/^\S.*$/)]),
      entityedit_description: new FormControl(''),
      entity_prefixedit: new FormControl('',[Validators.required,Validators.pattern(/^\S.*$/)]),
      entity_gid: new FormControl(''),
    });
  }

  updateRemainingCharsadd() {
    this.remainingChars = 1000 - this.reactiveForm.value.entity_description.length;
  }

  updateRemainingCharsadd1() {
    this.remainingChars = 1000 - this.reactiveFormEdit.value.entityedit_description.length;
  }

  // Summary Grid //
  GetEntitySummary() {
    var url = 'Entitylist/GetEntitySummary'
    this.service.get(url).subscribe((result: any) => {
    this.responsedata = result;
      this.entity_list = this.responsedata.entity_lists;
      setTimeout(() => {
        $('#entity_list').DataTable();
      }, 1);
    });
  }

  // Add popup validtion //
  get entity_code() {
    return this.reactiveForm.get('entity_code')!;
  }
  get entity_prefix() {
    return this.reactiveForm.get('entity_prefix')!;
  }
  get entity_name() {
    return this.reactiveForm.get('entity_name')!;
  }

  // Edit popup validtion //
  get entity_codeedit() {
    return this.reactiveFormEdit.get('entity_codeedit')!;
  }
  get entityedit_name() {
    return this.reactiveFormEdit.get('entityedit_name')!;
  }
  get entity_prefixedit() {
    return this.reactiveFormEdit.get('entity_prefixedit')!;
  }

  // Add popup //
  public onsubmit(): void {
    if (this.reactiveForm.value.entity_name != null && this.reactiveForm.value.entity_name != '') {

      for (const control of Object.keys(this.reactiveForm.controls)) {
        this.reactiveForm.controls[control].markAsTouched();
      }
      this.reactiveForm.value;
      var url = 'Entitylist/PostEntity'
      this.NgxSpinnerService.show();
      this.service.post(url, this.reactiveForm.value).subscribe((result: any) => {
        $('#entity_list').DataTable().destroy();
        if (result.status == false) {
          this.ToastrService.warning(result.message)
          this.NgxSpinnerService.hide();
          // this.GetEntitySummary();
          this.reactiveForm.reset();
        }
        else {
          this.reactiveForm.get("entity_prefix")?.setValue(null);
          this.reactiveForm.get("entity_name")?.setValue(null);
          this.reactiveForm.get("entity_description")?.setValue(null);
          this.NgxSpinnerService.hide();

          this.ToastrService.success(result.message)
        this.reactiveForm.reset();
        this.NgxSpinnerService.hide();
        this.GetEntitySummary();
      }
      });
      // setTimeout(function () {
      //   window.location.reload();
      // }, 2000);
    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
        this.NgxSpinnerService.hide();
    }
   this.reactiveForm.reset();
   
  }



  
   // Edit popup //
   openModaledit(parameter: string) {
    this.parameterValue1 = parameter
    this.reactiveFormEdit.get("entity_codeedit")?.setValue(this.parameterValue1.entity_code);

    this.reactiveFormEdit.get("entityedit_name")?.setValue(this.parameterValue1.entity_name);
    this.reactiveFormEdit.get("entityedit_description")?.setValue(this.parameterValue1.entity_description);
    this.reactiveFormEdit.get("entity_gid")?.setValue(this.parameterValue1.entity_gid);
    this.reactiveFormEdit.get("entity_prefixedit")?.setValue(this.parameterValue1.entity_prefix);
  }
  isFormEdited(): boolean {
    return !Object.keys(this.reactiveFormEdit.controls).every(control => {
      return this.reactiveFormEdit.controls[control].pristine;
    });
  }

  getEditedValues(): any {
    const editedValues: any = {};
    const entityGid = this.reactiveFormEdit.value.entity_gid;
  if (entityGid) {
    editedValues['entity_gid'] = entityGid;
  }
    Object.keys(this.reactiveFormEdit.controls).forEach(control => {
      if (control !== 'entity_gid' && !this.reactiveFormEdit.controls[control].pristine) {
        editedValues[control] = this.reactiveFormEdit.value[control];
      }
    });
   
    return editedValues;
  }

  // Update popup //
  onupdate() {
    if (this.reactiveFormEdit.value.entityedit_name != null && this.reactiveFormEdit.value.entityedit_name != '') {
      for (const control of Object.keys(this.reactiveFormEdit.controls)) {
        this.reactiveFormEdit.controls[control].markAsTouched();
      }
      this.reactiveFormEdit.value;
      // if (!this.isFormEdited()) {
      //   this.ToastrService.warning('No changes detected.');
      //   return;
      // }
    //   if(Object.keys(editedValues).length > 0){
    //   var url = 'Entitylist/Getupdateentitydetails'
    //   this.service.postparams(url, editedValues).pipe().subscribe((result: any) => {
    //     this.responsedata = result;
    //     if (result.status == false) {
    //       this.ToastrService.warning(result.message)
    //       this.GetEntitySummary();
    //       Object.keys(editedValues).forEach(key => editedValues[key] = null);
    //     }
    //     else {
    //       this.ToastrService.success(result.message)
    //       this.GetEntitySummary();
    //       Object.keys(editedValues).forEach(key => editedValues[key] = null);
    //       console.log('After modification:', editedValues);
    //     } 
    //   });
    // }
    var url = 'Entitylist/Getupdateentitydetails'
    this.NgxSpinnerService.show();
    this.service.postparams(url, this.reactiveFormEdit.value).pipe().subscribe(result => {
      $('#entity_list').DataTable().destroy();
      this.responsedata = result;
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.GetEntitySummary();
        this.NgxSpinnerService.hide();
      }
      else {
        this.ToastrService.success(result.message)
        this.GetEntitySummary();
        this.NgxSpinnerService.hide();
      }
    });
    // setTimeout(function () {
    //   window.location.reload();
    // }, 2000);
    }  
  }

  toggleStatus(data: any) {
    debugger
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
    var url = 'Entitylist/GetentityActive'
    this.service.getid(url, this.parameterValue).subscribe((result: any) => {

      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide();

      }
      else {
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
      }  
      this.GetEntitySummary();
    });
  }

  openModalinactive(parameter: string) {
    this.parameterValue = parameter;
  }

  onInactivate() {

    this.NgxSpinnerService.show();
    var url = 'Entitylist/GetentityInactive'
    this.service.getid(url, this.parameterValue).subscribe((result: any) => {

      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide();

      }
      else {
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();

      }
      this.GetEntitySummary();

    });
  }




  // Delete popup //
  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }

  ondelete() {
    console.log(this.parameterValue);
    var url = 'Entitylist/Getdeleteentitydetails'
    this.NgxSpinnerService.show();
    let param = {
      entity_gid: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      $('#entity_list').DataTable().destroy();
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide();
      }
      else {
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
      }
      this.GetEntitySummary();
    });
    // setTimeout(function () {
    //   window.location.reload();
    // }, 2000);
  }
  onclose() {
    this.reactiveForm.reset();
  }
  oncloseedit() {
    this.reactiveFormEdit.reset()
  }

  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }
  
}

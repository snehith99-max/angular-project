import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

interface IBranch {
  branch_gid: string;
  branch_gid1: string;
  branch_code: string;
  branch_name: string;
  branch_prefix: string;
  branchmanager_gid: string;
  // Postal_code_add : string;
  branch_code_edit: string;
  branch_name_edit: string;
  branch_prefix_edit: string;

  branch_code_add: string;
  Branch_address_add: string;
  branch_logo_path: string;
  City: string;
  State: string;
  Postal_code: string;
  Phone_no_add: any;
  Email_address_add: any;
  GST_no_add: any;
}

@Component({
  selector: 'app-sys-mst-branch',
  templateUrl: './sys-mst-branch.component.html',
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

export class SysMstBranchComponent {
  invalidFileFormat: boolean = false;
  branch_logo_path: string | null = null;
  Images: File | null = null;
  showOptionsDivId: any;
  reactiveForm!: FormGroup;
  reactiveFormEdit!: FormGroup;
  reactiveFormadd!: FormGroup;
  responsedata: any;
  editedValues: any = {};
  parameterValue: any;
  branch_logo: any;
  parameterValue1: any;
  branch_list: any[] = []
  branch!: IBranch;
  route: any;
  file: any;
  parameterValue2: any;
  branchdetails: any[] = [];
  form: FormGroup;
  selectedFileName : string ='';
  filename : string = '';

  constructor(private fb: FormBuilder, private SocketService: SocketService, private NgxSpinnerService: NgxSpinnerService, public service: SocketService, private ToastrService: ToastrService, private FormBuilder: FormBuilder) {
    this.branch = {} as IBranch;
    this.form = this.fb.group({
      branch_logo_path: ['']
    });
  }

  ngOnInit(): void {

    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.BranchSummary();

    // Form values for Add popup //
    this.reactiveForm = new FormGroup({
      branch_code: new FormControl(this.branch.branch_code, [Validators.required, Validators.pattern(/^\S.*$/)]),
      branch_name: new FormControl(this.branch.branch_name, [Validators.required, Validators.pattern(/^\S.*$/)]),
      branch_prefix: new FormControl(this.branch.branch_prefix, [Validators.required, Validators.pattern(/^\S.*$/)])
    });

    // const postalCodePattern = '^[A-Za-z0-9]{1,6}(?:\\s[A-Za-z0-9]{1,6})*$';

    this.reactiveFormadd = new FormGroup({
      branch_code_add: new FormControl(''),
      Branch_address_add: new FormControl('', [Validators.required, Validators.pattern(/^\S.*$/)]),
      City: new FormControl('', [Validators.required, Validators.pattern(/^\S.*$/)]),

      State: new FormControl(''),
      Postal_code: new FormControl('', [Validators.minLength(6)]),
      branch_gid: new FormControl(''),
      Phone_no_add: new FormControl('',  [Validators.required,Validators.minLength(10), Validators.maxLength(12)]),
      Email_address_add: new FormControl('', [Validators.required, Validators.pattern('^([a-z0-9]+(?:[-.][a-z0-9]+)*)@([a-z0-9]+\\.[a-z]{2,20}(\\.[a-z]{2})?|,\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\]|localhost)$')]),
      GST_no_add: new FormControl('',[ Validators.pattern(/[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[0-9]{1}[A-Z]{1}[0-9A-Z]{1}$/), Validators.maxLength(15)]),
      branch_logo_path: new FormControl(''),
    });

    // Form values for Edit popup //
    this.reactiveFormEdit = new FormGroup({
      branch_code_edit: new FormControl(this.branch.branch_code_edit, [Validators.required,]),
      branch_name_edit: new FormControl(this.branch.branch_name_edit, [Validators.required, Validators.pattern(/^\S.*$/)]),
      branch_prefix_edit: new FormControl(this.branch.branch_prefix_edit, [Validators.required, Validators.pattern(/^\S.*$/)]),
      branch_gid: new FormControl(''),
    });
  }

  BranchSummary() {
    var url = 'SysMstBranch/BranchSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#branch_list').DataTable().destroy();
      this.responsedata = result;
      this.branch_list = this.responsedata.branch_list1;

      
      setTimeout(() => {
        $('#branch_list').DataTable();
      }, 1);
    });
  }

  onChange(event: any) {
    this.file = event.target.files[0];
    const validImageTypes = ['image/jpeg', 'image/png', 'image/gif'];
    
    if (this.file && validImageTypes.includes(this.file.type)) {
      this.invalidFileFormat = false;
      this.reactiveFormadd.get('branch_logo_path')?.setValue(this.file);
    } else {
      this.invalidFileFormat = true;
      this.reactiveFormadd.get('branch_logo_path')?.reset();
      event.target.value = ''; // Clear the file input field
    }
    if (this.file) {
      this.branch_logo_path = this.file.name;
      this.Images = this.file;
      this.form.patchValue({
        branch_logo_path: this.file.name
      });
    }
   else {
      this.branch_logo_path = null;
      this.Images = null;
      this.form.patchValue({
        branch_logo_path: null
      });
    }
  }

  getFileName(filePath: string): string {
    return filePath ? filePath.split('/').pop() || '' : '';
  }

  // Add validtion //
  get branch_code() {
    return this.reactiveForm.get('branch_code')!;
  }
  get branch_name() {
    return this.reactiveForm.get('branch_name')!;
  }
  get branch_prefix() {
    return this.reactiveForm.get('branch_prefix')!;
  }

  // Add popup validtion //
  get branch_code_add() {
    return this.reactiveFormadd.get('branch_code_add')!;
  }
  get Branch_address_add() {
    return this.reactiveFormadd.get('Branch_address_add')!;
  }
  get Phone_no_add() {
    return this.reactiveFormadd.get('Phone_no_add')!;
  }
  get Email_address_add() {
    return this.reactiveFormadd.get('Email_address_add')!;
  }
  get Postal_code() {
    return this.reactiveFormadd.get('Postal_code')!;
  }
  get GST_no_add() {
    return this.reactiveFormadd.get('GST_no_add')!;
  }

  // Edit popup validtion //
  get branch_code_edit() {
    return this.reactiveFormEdit.get('branch_code_edit')!;
  }
  get branch_name_edit() {
    return this.reactiveFormEdit.get('branch_name_edit')!;
  }
  get branch_prefix_edit() {
    return this.reactiveFormEdit.get('branch_prefix_edit')!;
  }

  // Add popup API //
  public onsubmit(): void {
    if (this.reactiveForm.value.branch_code != null && this.reactiveForm.value.branch_code != '')
      if (this.reactiveForm.value.branch_name != null && this.reactiveForm.value.branch_name != '')
        if (this.reactiveForm.value.branch_prefix != null && this.reactiveForm.value.branch_prefix != '') {
          for (const control of Object.keys(this.reactiveForm.controls)) {
            this.reactiveForm.controls[control].markAsTouched();
          }
          this.reactiveForm.value;
          var url1 = 'SysMstBranch/PostBranch'
          this.NgxSpinnerService.show();
          this.service.postparams(url1, this.reactiveForm.value).subscribe((result: any) => {
            if (result.status == false) {
              this.NgxSpinnerService.hide();
              this.ToastrService.warning(result.message)
              this.BranchSummary();
              this.reactiveForm.reset();
              this.NgxSpinnerService.hide();
              
            }
            else {
              this.reactiveForm.get("branch_code")?.setValue(null);
              this.reactiveForm.get("branch_name")?.setValue(null);
              this.reactiveForm.get("branch_prefix")?.setValue(null);
              this.ToastrService.success(result.message)
              this.BranchSummary();
              this.NgxSpinnerService.hide();
            }
          });
          // setTimeout(function () {
          //   window.location.reload();
          // }, 2000);
        }
        else {
          this.ToastrService.warning('Kindly Fill All Mandatory Fields !!')
        }
  }

  // Edit popup API //
  openModaledit(parameter: string) {
    this.parameterValue1 = parameter
    this.reactiveFormEdit.get("branch_code_edit")?.setValue(this.parameterValue1.branch_code);
    this.reactiveFormEdit.get("branch_name_edit")?.setValue(this.parameterValue1.branch_name);
    this.reactiveFormEdit.get("branch_prefix_edit")?.setValue(this.parameterValue1.branch_prefix);
    this.reactiveFormEdit.get("branch_gid")?.setValue(this.parameterValue1.branch_gid);
  }

  isFormEdited(): boolean {
    return !Object.keys(this.reactiveFormEdit.controls).every(control => {
      return this.reactiveFormEdit.controls[control].pristine;
    });
  }

  getEditedValues(): any {
    const editedValues: any = {};
    const entityGid = this.reactiveFormEdit.value.branch_gid;
    if (entityGid) {
      editedValues['branch_gid'] = entityGid;
    }
    Object.keys(this.reactiveFormEdit.controls).forEach(control => {
      if (control !== 'branch_gid' && !this.reactiveFormEdit.controls[control].pristine) {
        editedValues[control] = this.reactiveFormEdit.value[control];
      }
    });
    return editedValues;
  }
  
  onupdate() {
    var url = 'SysMstBranch/getUpdatedBranch'
    this.NgxSpinnerService.show();
    this.service.post(url, this.reactiveFormEdit.value).subscribe((result: any) => {
      if (result.status == true) {
       
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
        this.BranchSummary();        
      }
      else {
        this.ToastrService.warning(result.message)

        this.NgxSpinnerService.hide();

        this.BranchSummary();

      }
    });
    // setTimeout(function () {
    //   window.location.reload();
    // }, 2000);
  }

  myModaladddetails(parameter: string) {
    this.parameterValue2 = parameter

    this.reactiveFormadd.get("branch_code_add")?.setValue(this.parameterValue2.branch_code);
    this.reactiveFormadd.get("branch_gid")?.setValue(this.parameterValue2.branch_gid);
    this.getbranchdetails(this.parameterValue2.branch_gid)
  }

  public validate(): void {
    console.log(this.reactiveFormadd.value)
    this.branch = this.reactiveFormadd.value;

    if (this.branch.branch_code_add != null && this.branch.Branch_address_add != null && this.branch.City != null && this.branch.State != null && this.branch.Postal_code != null && this.branch.Email_address_add != null && this.branch.Phone_no_add != null && this.branch.GST_no_add != null) {
      let formData = new FormData();
      if (this.file != null && this.file != undefined) {
        formData.append("branch_logo_path", this.file, this.file.name);
        formData.append("branch_gid", this.branch.branch_gid);
        formData.append("branch_code", this.branch.branch_code_add);
        formData.append("Branch_address", this.branch.Branch_address_add);
        formData.append("City", this.branch.City);
        formData.append("State", this.branch.State);
        formData.append("Postal_code", this.branch.Postal_code);
        formData.append("Email_address", this.branch.Email_address_add);
        formData.append("Phone_no", this.branch.Phone_no_add);
        formData.append("GST_no", this.branch.GST_no_add);

        var api = 'SysMstBranch/Updatedbranchlogo'
        this.service.postfile(api, formData,).subscribe((result: any) => {
          console.log(result);

          if (result.status == true) {
            this.ToastrService.success(result.message)
            setTimeout(() => {
              window.location.reload();
          }, 2000); // 2000 milliseconds = 2 seconds
          }
          else {
            this.ToastrService.warning(result.message)
            this.BranchSummary();
          }
          this.responsedata = result;
        });
      }
      else {
        var api = 'SysMstBranch/BranchSummarydetail'
        this.NgxSpinnerService.show();
        this.service.post(api, this.branch).subscribe((result: any) => {

          if (result.status == true) {
            this.ToastrService.success(result.message)
            this.NgxSpinnerService.hide();
            this.reactiveFormadd.reset();

          }
          else {
            this.ToastrService.warning(result.message)
            this.NgxSpinnerService.hide();
            this.BranchSummary();
            this.reactiveFormadd.reset();
          }
          this.responsedata = result;
          this.NgxSpinnerService.hide();

        });
      }
    }
    return;
    
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
    var url = 'SysMstBranch/GetbranchActive'
    this.service.getid(url, this.parameterValue).subscribe((result: any) => {

      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide();

      }
      else {
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
      }  
      this.BranchSummary();
    });
  }

  openModalinactive(parameter: string) {
    this.parameterValue = parameter;
  }

  onInactivate() {

    this.NgxSpinnerService.show();
    var url = 'SysMstBranch/GetbranchInactive'
    this.service.getid(url, this.parameterValue).subscribe((result: any) => {

      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide();

      }
      else {
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();

      }
      this.BranchSummary();

    });
  }

  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }

  ondelete() {
    console.log(this.parameterValue);
    var url3 = 'SysMstBranch/DeleteBranch'
    this.NgxSpinnerService.show();
    this.service.getid(url3, this.parameterValue).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide();
        this.BranchSummary();
      }
      else {
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
        this.BranchSummary();
      }
    });
    // setTimeout(function () {
    //   window.location.reload();
    // }, 2000);
  }

  onclose() {
    this.reactiveForm.reset();
  };
  
  onclose1() {
    this.reactiveFormadd.reset();
    this.invalidFileFormat = false;
  };

  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }
  openbranch(parameter: string) {
    this.parameterValue = parameter
  this.reactiveFormadd.get("branch_gid")?.setValue(this.parameterValue.branch_gid);
  console.log(this.reactiveFormadd)
  const branchgid = this.parameterValue.branch_gid;
  this.getbranchdetails(branchgid);
  }
  getbranchdetails(branch_gid:any){
    var url = 'SysMstBranch/PopupBranch'
    let param = {
      branch_gid : branch_gid 
        }
    this.service.getparams(url, param).subscribe((result: any) => {
   this.branchdetails = result.branch_list1;
   this.reactiveFormadd.get("branch_code_add")?.setValue(this.branchdetails[0].branch_code_add);
   this.reactiveFormadd.get("Branch_address_add")?.setValue(this.branchdetails[0].Branch_address_add);
   this.reactiveFormadd.get("City")?.setValue(this.branchdetails[0].city);
   this.reactiveFormadd.get("Postal_code")?.setValue(this.branchdetails[0].postal_code);
   this.reactiveFormadd.get("Phone_no_add")?.setValue(this.branchdetails[0].Phone_no_add);
   this.reactiveFormadd.get("Email_address_add")?.setValue(this.branchdetails[0].Email_address_add);
   this.reactiveFormadd.get("GST_no_add")?.setValue(this.branchdetails[0].GST_no_add);
   this.reactiveFormadd.get("State")?.setValue(this.branchdetails[0].State);
  this.selectedFileName = (this.branchdetails[0].branch_logo_path)
  this.filename = this.selectedFileName.split('/').pop() || '';

  
  });



  }
}

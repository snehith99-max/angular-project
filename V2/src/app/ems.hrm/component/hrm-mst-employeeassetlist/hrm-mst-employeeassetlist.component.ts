import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
interface IAssetlist {
  assetref_no: string;
  asset_name: string;
  asset_count: string;
  asset_countedit: string;
  asset_gid: string;
  assetref_noedit: string;
  asset_nameedit: string;
  active_flag: string;
}

@Component({
  selector: 'app-hrm-mst-employeeassetlist',
  templateUrl: './hrm-mst-employeeassetlist.component.html',
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
export class HrmMstEmployeeassetlistComponent {
  // showOptionsDivId: any;
  parameterValue1: any;
  parameterValue: any;
  reactiveForm!: FormGroup;
  reactiveFormEdit!: FormGroup;
  responsedata: any;
  asset_list: any[] = [];

  assetlist!: IAssetlist;

  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService) {
    this.assetlist = {} as IAssetlist;
  }
  ngOnInit(): void {
    this.GetAssetListSummary();
     // Form values for Add popup/////
     this.reactiveForm = new FormGroup({
      assetref_no: new FormControl(this.assetlist.assetref_no, [
        Validators.required,Validators.pattern(/^\S.*$/)]),
      
      asset_name: new FormControl(this.assetlist.asset_name, [Validators.required,Validators.pattern(/^\S.*$/)]),

      asset_count: new FormControl(this.assetlist.asset_count, [Validators.required,]),
      asset_gid: new FormControl(''),


      
    });

    this.reactiveFormEdit = new FormGroup({

      assetref_noedit: new FormControl(this.assetlist.assetref_noedit, [
        Validators.required,]),
       
        asset_nameedit: new FormControl(this.assetlist.asset_nameedit, [
        Validators.required,]),

        asset_countedit: new FormControl(this.assetlist.asset_countedit, [
          Validators.required,]),
      
        active_flag: new FormControl(''),
       

        asset_gid: new FormControl(''),
     


    });
   
    
  }

  GetAssetListSummary() {
    var url = 'HrmMstAssetList/GetAssetListSummary'
    this.service.get(url).subscribe((result: any) => {

      this.responsedata = result;
      this.asset_list = this.responsedata.asset_list;
      setTimeout(() => {
        $('#assetlist').DataTable();
      }, );


    });
  
  }

 
  get assetref_no() {
    return this.reactiveForm.get('assetref_no')!;
  }
  get asset_name() {
    return this.reactiveForm.get('asset_name')!;
  }
  get asset_count() {
    return this.reactiveForm.get('asset_count')!;
  }

  get assetref_noedit() {
    return this.reactiveFormEdit.get('assetref_noedit')!;
  }
  get asset_nameedit() {
    return this.reactiveFormEdit.get('asset_nameedit')!;
  }
  get asset_countedit() {
    return this.reactiveFormEdit.get('asset_countedit')!;
  }


  openModaledit(parameter: string){
    this.parameterValue1 = parameter
    this.reactiveFormEdit.get("assetref_noedit")?.setValue(this.parameterValue1.assetref_no);
    this.reactiveFormEdit.get("asset_nameedit")?.setValue(this.parameterValue1.asset_name);
    this.reactiveFormEdit.get("asset_countedit")?.setValue(this.parameterValue1.asset_count);
    this.reactiveFormEdit.get("active_flag")?.setValue(this.parameterValue1.active_flag);
    this.reactiveFormEdit.get("asset_gid")?.setValue(this.parameterValue1.asset_gid);
   


  }

  // openModaldelete(parameter: string){
  //   this.parameterValue = parameter
  // }

  public onsubmit(): void {
    debugger;
    if (this.reactiveForm.value.assetref_no != null && this.reactiveForm.value.assetref_no != '')

    if (this.reactiveForm.value.asset_name != null && this.reactiveForm.value.asset_name != '')
      {

          this.reactiveForm.value;
          var url = 'HrmMstAssetList/PostAssetList'
          this.service.postparams(url, this.reactiveForm.value).subscribe((result: any) => {

            if (result.status == false) {
              this.ToastrService.warning(result.message)
                
            }
            else {
              this.reactiveForm.get("asset_gid")?.setValue(null);
              this.reactiveForm.get("assetref_no")?.setValue(null);
              this.reactiveForm.get("asset_name")?.setValue(null);
              this.reactiveForm.get("asset_count")?.setValue(null);
              

              this.ToastrService.success(result.message)
              this.GetAssetListSummary();
             

            }

          });

        }
       
              // Display a notification
              //  new Notification("Asset List Added Successfully", {});
  
              // Reload the page after a short delay (you can adjust the delay as needed)
             this.reactiveForm.reset();
          }
      
  

  onclose(){

  }

  public onupdate(): void {
    debugger;
    if (this.reactiveFormEdit.value.asset_nameedit != null && this.reactiveFormEdit .value.asset_nameedit != '')

     {
          for (const control of Object.keys(this.reactiveFormEdit.controls)) {
            this.reactiveFormEdit.controls[control].markAsTouched();
          }
          this.reactiveFormEdit.value;

          var url = 'HrmMstAssetList/UpdatedAssetList'

          this.service.postparams(url, this.reactiveFormEdit.value).pipe().subscribe(result => {
            this.responsedata = result;
            if (result.status == false) {
              this.ToastrService.warning(result.message)
              this.GetAssetListSummary();
            }
            else {
              this.ToastrService.success(result.message)
              this.GetAssetListSummary();
            }

          });

     }

  }

  // ondelete(){
  //   console.log(this.parameterValue);
  //   var url = 'HrmMstAssetList/DeleteAssetList'
  //   this.service.getid(url, this.parameterValue).subscribe((result: any) => {
  //     if (result.status == false) {
  //       this.ToastrService.warning('Error While Deleting Asset List')
  //     }
  //     else {
  //       this.ToastrService.success('Asset List Deleted Successfully')
  //     }
  //     this.GetAssetListSummary();

  //   });
   

  // }

}

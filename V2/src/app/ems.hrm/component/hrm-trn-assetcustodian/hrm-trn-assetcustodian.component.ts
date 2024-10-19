import { Component, ElementRef, OnInit, Renderer2 } from '@angular/core';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpParams } from '@angular/common/http';
import { AES } from 'crypto-js';
interface ISearchD {
  department_name: string;
  
}
interface ISearchB {
  branch_name: string;
  
}
@Component({
  selector: 'app-hrm-trn-assetcustodian',
  templateUrl: './hrm-trn-assetcustodian.component.html',
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
export class HrmTrnAssetcustodianComponent {
  // showOptionsDivId: any;
  reactiveFormSubmit!: FormGroup;
  branch!: ISearchB;
  department!: ISearchD;
  department_list: any[] = [];
  branch_list: any[] = [];
  custodian_list: any[] = [];
 custodianadd_list: any[] = [];
 Document_list: any[] = [];
 file_path: any;
 selectedBranch: any = null;
 branch_name :any;
 department_name:any;
selectedDepartment: any = null;
  responsedata: any;
  parameterValue: any;
  parameterValue1: any;
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService,private route:Router) {
    this.branch = {} as ISearchB;
    this.department = {} as ISearchD;

  }

ngOnInit(): void {  
 
  this.department_list = [{ department_name: 'All', department_gid: 'all' }, ...this.department_list];

    this.reactiveFormSubmit = new FormGroup({
       department_gid: new FormControl(this.department.department_name, [ Validators.required,]),
       branch_gid: new FormControl(this.branch.branch_name, [ Validators.required,   ]),
       department_name: new FormControl(''),
       branch_name: new FormControl(''),
       asset_gid: new FormControl(''),
       employee_gid: new FormControl(''),

        });
    var url = 'HrmTrnAssetcustodian/GetBranch';
    this.service.get(url).subscribe((result: any) => {
    this.branch_list = result.GetBranch;     
    this.branch_list = [{ branch_name: 'All', branch_gid: 'all' }, ...this.branch_list];
    
   
        });

        const params = {
          branch_name: "all",
          department_name: "all"
        };
    
        const url2 = 'HrmTrnAssetcustodian/Getassetcustodiansummary';
    
        this.service.getparams(url2, params).subscribe((result) => {
          this.responsedata = result;
          this.custodian_list = this.responsedata.custodian_list;
    
    
          setTimeout(() => {
            $('#custodian_list').DataTable();
          }, 1);
        });

}

onBranchChange(branch_gid: any) {
  debugger;
  const branchValue = this.reactiveFormSubmit.get('branch_name')!.value;

  if (branchValue === 'all') {
    this.department_list = [{ department_name: 'All', department_gid: 'all' }];
  } else {
    var url1 = 'HrmTrnAssetcustodian/GetDepartment';
  let param: { branch_gid: any } = {
    branch_gid: branch_gid
};
  this.service.getparams(url1, param).subscribe((result: any) => {
    debugger;
    this.department_list = [{ department_name: 'All', department_gid: 'all' }];
    this.department_list.push(...result.GetDepartment); // Add the data to the existing array 
  });
  }
  this.GetCustodianSummary();
}






GetCustodianSummary() {
  let selectedBranch = this.reactiveFormSubmit.value.branch_name;
  let selectedDepartment = this.reactiveFormSubmit.value.department_name;

  if(selectedBranch==null || selectedBranch==""){
    selectedBranch='all'
  }
  if(selectedDepartment==null || selectedDepartment==""){
    selectedDepartment='all'
  }

    const params = {
      branch_name: selectedBranch,
      department_name: selectedDepartment
    };
    const url2 = 'HrmTrnAssetcustodian/Getassetcustodiansummary';
    this.service.getparams(url2, params).subscribe((result) => {
      this.responsedata = result;
      this.custodian_list = this.responsedata.custodian_list;
      setTimeout(() => {
        $('#custodian_list').DataTable();
      }, 1);
    });
}


ondetail(employee_gid: any) {
  var url = 'HrmTrnAssetcustodian/GetassetcustodianExpand'
  let param = {
    employee_gid : employee_gid 
  }
  this.service.getparams(url, param).subscribe((result: any) => {
    this.custodianadd_list = result.GetAddCustodian;
    });

}

addcustodian(params:any){
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.route.navigate(['/hrm/HrmTrnAddAssetcustodian',encryptedParam]) 
  }


  openModaldtl(parameter: string,parameter1:string) {
    this.parameterValue = parameter
    this.parameterValue1 = parameter1

    this.reactiveFormSubmit.get("asset_gid")?.setValue(this.parameterValue1.asset_gid);
    this.reactiveFormSubmit.get("employee_gid")?.setValue(this.parameterValue.employee_gid);
    console.log(this.reactiveFormSubmit)
    const Assetgid = this.reactiveFormSubmit.value.asset_gid;
    const Employeegid = this.reactiveFormSubmit.value.employee_gid;
    if (this.reactiveFormSubmit.value.asset_gid != null && this.reactiveFormSubmit.value.employee_gid != '') {
      for (const control of Object.keys(this.reactiveFormSubmit.controls)) {
        this.reactiveFormSubmit.controls[control].markAsTouched();
      }
      const params = {
       asset_gid: Assetgid,
       employee_gid: Employeegid
      };
    
      var url = 'HrmTrnAssetcustodian/AssetDocument'
      this.service.getparams(url, params).subscribe((result: any) => {
        this.Document_list = result.Assetcustodian;
     debugger;

      
      });
    }

  }
  get asset_gid() {
    return this.reactiveFormSubmit.get('asset_gid')!
  }
  get employee_gid() {
    return this.reactiveFormSubmit.get('employee_gid')!
  }

  onclose(){

  }




  downloadDocument(document_gid: any) {
    var api = 'HrmTrnAssetcustodian/downloadFile';
    let param = { document_gid: document_gid };
  
    this.service.download(api, param).subscribe((result: any) => {
      this.responsedata = result;
      this.file_path = this.responsedata.DownloadFile; // Access the first value in the DownloadFile array
  
      if (!this.file_path || this.file_path.length === 0) {
        window.scrollTo({
          top: 0,
        });
      } else {
        // Assuming the file content is in this.file_path (adjust this as needed)
        const fileContent = this.file_path;
        const documentname=this.file_path[0].document_name;

        // Extract the file name from the response or use a default name
        const documentName1 = 'Document'+documentname;

        // Create a Blob from the file content
        const blob = new Blob([fileContent], { type: 'application/octet-stream' });
        const url = window.URL.createObjectURL(blob);

        // Create a temporary link element and trigger the download
        const link = document.createElement('a');
        link.href = url;
        link.download = documentName1;
        link.click();

        // Clean up the object URL after the download is initiated
        window.URL.revokeObjectURL(url);
      }
    });
  }
  

}

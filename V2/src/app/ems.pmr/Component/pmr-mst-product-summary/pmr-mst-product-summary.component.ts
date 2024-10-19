import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators,FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { ExcelService } from 'src/app/Service/excel.service';
import { Subscription, Observable } from 'rxjs';
import { first } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { environment } from 'src/environments/environment';
import { NgxSpinnerService } from 'ngx-spinner';
interface IEmployee {
}
@Component({
  selector: 'app-pmr-mst-product-summary',
  templateUrl: './pmr-mst-product-summary.component.html',
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
export class PmrMstProductSummaryComponent {
  employee!: IEmployee;
  log_list : any[]=[];
  private unsubscribe: Subscription[] = [];
  file!:File;
  reactiveForm!: FormGroup;
  responsedata: any;
  parameterValue: any;
  fileInputs: any;
  products: any[] = [];
  response_data :any;
  showOptionsDivId :any;

  constructor(private fb: FormBuilder,private route: ActivatedRoute,private router: Router,private excelService: ExcelService,private service: SocketService,private ToastrService: ToastrService,private NgxSpinnerService:NgxSpinnerService,) {} 
  
 
  
  ngOnInit(): void {
    this.GetProductSummary();

    this.reactiveForm = new FormGroup({
      file: new FormControl(''),

      // password: new FormControl(this.employee.password, [
      //   Validators.required,
      // ]),
      // confirmpassword: new FormControl(''),
      // employee_gid: new FormControl(''),

    });
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
  }

  importlog()
  {
    var api = 'PmrMstProduct/GetImportLog';
    this.service.get(api).subscribe((result: any) => {
      this.response_data = result;
      this.log_list = result.product_list;

    });
 
  }

  myModalLogDelete(parameter: string)
  {
    this.parameterValue = parameter
  }
  deletelog()
  {
    var url = 'PmrMstProduct/DeleteLog'
    let param = {
      log_id: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.GetProductSummary();

      }
      else {
        this.ToastrService.success(result.message)
        this.GetProductSummary();
      }
      this.GetProductSummary();
    });
  }

  ProductexportExcel() {

    var api7 = 'PmrMstProduct/GetProductReportExport';
    this.service.generateexcel(api7).subscribe((result: any) => {
        this.responsedata = result;

        if (this.responsedata && this.responsedata.productexport_list && this.responsedata.productexport_list.length > 0) {
            var phyPath = this.responsedata.productexport_list[0].lspath1;
            var relPath = phyPath.split("src");
            var hosts = window.location.host;
            var prefix = location.protocol + "//";
            var str = prefix.concat(hosts, relPath[1]);

            var link = document.createElement("a");
            var name = this.responsedata.productexport_list[0].lsname2;
            link.download = name; 
            link.href = str;
            link.click();
        } else {
            console.error('No export data available');
        }
    }, error => {
        console.error('Error generating Excel file', error);
    });

    // const ProductExcel = this.products.map(item => ({
    //   ProductCode: item.product_code || '',
    //   ProductType: item.producttype_name || '',
    //   ProductGroup: item.productgroup_name || '',
    //   Product: item.product_name || '',
    //   Unit: item.productuomclass_name || '',
    //   CostPrice: item.cost_price || '',
    //   CreatedDate: item.created_date || '',
    //   CreatedBy: item.created_by || ''
    // }));


    // this.excelService.exportAsExcelFile(ProductExcel, 'Product_Excel');

  }

  GetProductSummary(){
 
    var api = 'PmrMstProduct/GetProductSummary';
    this.NgxSpinnerService.show();
    this.service.get(api).subscribe((result:any) => {
      this.response_data = result;
      this.products = this.response_data.product_list;
      setTimeout(()=>{  
        $('#product_list').DataTable();
      }, 1);
      this.NgxSpinnerService.hide();
    });
  
  }
  onChange2(event:any) {
    this.file =event.target.files[0];
   
    }
    onedit(params:any){
      const secretKey = 'storyboarderp';
      const param = (params);
      const encryptedParam = AES.encrypt(param,secretKey).toString();
      this.router.navigate(['/pmr/PmrMstProductEdit',encryptedParam]) 
    }
    onview(params:any){
      const secretKey = 'storyboarderp';
      const param = (params);
      const encryptedParam = AES.encrypt(param,secretKey).toString();
      this.router.navigate(['/pmr/PmrMstProductView',encryptedParam])
    }
  
  onadd()
  {
        this.router.navigate(['/pmr/PmrMstProductAdd'])

  }
  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  
  }

  public validate(): void {
    this.employee = this.reactiveForm.value;
    let formData = new FormData();
    if(this.file !=null &&  this.file != undefined){

    formData.append("file", this.file,this.file.name);
    var api='Product/ProductimageUpload'

    this.service.postfile(api,formData).subscribe((result:any) => {
      this.responsedata=result;
      if(result.status ==false){
        this.ToastrService.warning(result.message)
      }
      else{
        this.reactiveForm.reset();
        this.ToastrService.success(result.message)
      }
    });
  }
    else{
      var api7='Product/Postimage'
        this.service.post(api7,this.employee).subscribe((result:any) => {

          if(result.status ==false){
            this.ToastrService.warning(result.message)
          }
          else{
            this.router.navigate(['/crm/CrmMstProductsummary']);
            this.ToastrService.success(result.message)
          }
          this.responsedata=result;
        });
    }
  }
  ondelete() {
    this.NgxSpinnerService.show()
    var url = 'PmrMstProduct/GetDeleteProductdetails'
    let param = {
      product_gid : this.parameterValue 
    }
    this.service.getparams(url,param).subscribe((result: any) => {
      if(result.status ==false){
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide();
      }
      else{
           
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
        this.GetProductSummary();
    }

  
  
    });
  }
  onclose() {
    window.location.reload();

  }
  // importexcel()
  //  {
  //   let formData = new FormData();
  //   if (this.file != null && this.file != undefined) {
  //     window.scrollTo({
  //       top: 0, // Code is used for scroll top after event done
  //     });
  //     formData.append("file", this.file, this.file.name);
  //     var api = 'PmrMstProduct/ProductImportExcel'
  //     this.service.postfile(api, formData).subscribe((result: any) => {
  //       this.response_data = result;
        
  //       window.location.reload();
  //       this.ToastrService.success("Excel Uploaded Successfully")
  //     });
  //   }
  // }

  importexcel() {

    
     this.NgxSpinnerService.show();
     let formData = new FormData();
     if (this.file != null && this.file != undefined) {
       window.scrollTo({
         top: 0, // Code is used for scroll top after event done
       });
       formData.append("file", this.file, this.file.name);
       var api = 'PmrMstProduct/productImportExcel'
       this.service.postfile(api, formData).subscribe((result: any) => {
         this.responsedata = result;
         if (result.status == false) {
 
           this.NgxSpinnerService.hide();
           this.ToastrService.warning('Error While Occured Excel Upload')
         }
         else {
 
           this.NgxSpinnerService.hide();
           // window.location.reload();
           this.fileInputs= null;
           this.ToastrService.success("Excel Uploaded Successfully")
 
         }
         this.GetProductSummary();
 
       });
     }
   }

  //  ProductexportExcel() {
  //   const ProductExcel = this.products.map(item => ({
  //     ProductCode: item.product_code || '',
  //     ProductType: item.producttype_name || '',
  //     ProductGroup: item.productgroup_name || '',
  //     Product: item.product_name || '',
  //     Unit: item.productuomclass_name || '',
  //     CostPrice: item.cost_price || '',
  //     CreatedDate: item.created_date || '',
  //     CreatedBy: item.created_by || ''
  //   }));


  //   this.excelService.exportAsExcelFile(ProductExcel, 'Product_Excel');

  // }


  downloadfileformat() {
 
    let link = document.createElement("a"); 
    link.download = "Product Template";
    link.href = "assets/media/Excels/producttemplate/Product Import Excel.xlsx";
    link.click();
  }
  // onChange1(event: any) {
  //   this.file = event.target.files[0];
  // }
  taxSegMap(params: any)
  {
    
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/smr/SmrMstMapTaxSegment', encryptedParam])
  }

  // toggleOptions(account_gid: any) {
  //   if (this.showOptionsDivId === account_gid) {
  //     this.showOptionsDivId = null;
  //   } else {
  //     this.showOptionsDivId = account_gid;
  //   }
  // }

  // oninactive() {
  //   debugger
  //   console.log(this.parameterValue);
  //   var url3 = 'SmrMstProduct/GetcustomerInactive'
  //   this.service.getid(url3, this.parameterValue).subscribe((result: any) => {

  //     if (result.status == false) {
  //       this.ToastrService.warning('Error While Product Inactivated')
  //     }
  //     else {
  //       this.ToastrService.success('Product Inactivated Successfully')
  //     }
  //     // window.location.reload();
  //     this.GetProductSummary();
  //   });
  // }

  // openModalactive(parameter: string) {
  //   this.parameterValue = parameter
  // }



  // onactive() {
  //   debugger
  //   console.log(this.parameterValue);
  //   var url3 = 'SmrMstProduct/GetcustomerActive'
  //   this.service.getid(url3, this.parameterValue).subscribe((result: any) => {

  //     if (result.status == false) {
  //       this.ToastrService.warning('Error While Product Activated')
  //     }
  //     else {
  //       this.ToastrService.success('Product Activated Successfully')
  //     }
  //     // window.location.reload();
  //     this.GetProductSummary();
  //   });
  // }

  // openModalinactive(parameter: string) {
  //   this.parameterValue = parameter
  // }

  // toggleStatus(data: any) {

  //   if (data.statuses === 'Active') {
  //     this.openModalinactive(data);
  //   } else {
  //     this.openModalactive(data);
  //   }
  // }

  toggleStatus(data: any) {
    debugger
    console.log(data.Status); // Check if it's actually 'InActive' or 'Inactive'
    if (data.statuses === 'Active') {
      this.openModalinactive(data);
    } else {
      this.openModalactive(data);
    }
  }
  

  /// Product Active - Inactive
  onChange1(event: any) {
    this.file = event.target.files[0];
  }
  openModalinactive(parameter: string) {
    this.parameterValue = parameter
  }


  oninactive() {
    console.log(this.parameterValue);
    var url3 = 'PmrMstProduct/GetcustomerInactive'
    this.service.getid(url3, this.parameterValue).subscribe((result: any) => {

      if (result.status == false) {
        this.ToastrService.warning('Error While Product Inactivated')
      }
      else {
        this.ToastrService.success('Product Inactivated Successfully')
      }
      // window.location.reload();
      this.GetProductSummary();
    });
  }

  openModalactive(parameter: string) {
    this.parameterValue = parameter
  }
  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }

  onactive() {
    console.log(this.parameterValue);
    var url3 = 'PmrMstProduct/GetcustomerActive'
    this.service.getid(url3, this.parameterValue).subscribe((result: any) => {

      if (result.status == false) {
        this.ToastrService.warning('Error While Product Activated')
      }
      else {
        this.ToastrService.success('Product Activated Successfully')
      }
      // window.location.reload();
      this.GetProductSummary();
    });
  }

}





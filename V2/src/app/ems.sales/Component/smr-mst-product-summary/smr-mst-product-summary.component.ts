import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Subscription, Observable } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { environment } from 'src/environments/environment';
import { ExcelService } from 'src/app/Service/excel.service';
import { NgxSpinnerService } from 'ngx-spinner';
interface IEmployee {

  product_code: string
  product_gid: string
}

@Component({
  selector: 'app-smr-mst-product-summary',
  templateUrl: './smr-mst-product-summary.component.html',
  styleUrls: ['./smr-mst-product-summary.component.scss']
})
export class SmrMstProductSummaryComponent {
  employee!: IEmployee;

  private unsubscribe: Subscription[] = [];
  file: File | null = null;;
  data:any;
  reactiveForm!: FormGroup;
  responsedata: any;
  log_list:any[] = [];
  parameterValue: any;
  fileInputs: any;
  showOptionsDivId:any;
  products: any[] = [];
  response_data: any;
  product_list: any[] = [];
  mdlproducttype: any;
  productform: any;
  producttype_list: any;
  constructor(private fb: FormBuilder,private sanitizer: DomSanitizer, private excelService: ExcelService, public NgxSpinnerService: NgxSpinnerService, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService,) { }

  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.GetProductSummary();

    this.reactiveForm = new FormGroup({
      file: new FormControl(''),
      fileExtension: new FormControl(''),
      fileName: new FormControl(''),
      imagePath: new FormControl(''),
      product_gid: new FormControl('')
    });
    this.productform = new FormGroup({
      producttype_gid: new FormControl(''),
      producttype_name: new FormControl('')
    });


    // Product Type DropDown
    var api = 'SmrMstProduct/GetProducttype';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.producttype_list = this.responsedata.GetProducttype;

    });

  }
  sortColumn(columnKey: string): void {
    return this.service.sortColumn(columnKey);
  }
  getSortIconClass(columnKey: string) {
    return this.service.getSortIconClass(columnKey);
  }

  GetProductSummary() {

    var api = 'SmrMstProduct/GetSalesProductSummary';
    this.NgxSpinnerService.show()
    this.service.get(api).subscribe((result: any) => {
      $('#product_list').DataTable().destroy();
      this.response_data = result;
      this.products = this.response_data.product_list;
      this.NgxSpinnerService.hide()
      setTimeout(() => {
        $('#product_list').DataTable();
       
      }, 1);
    });

  }
  // Method to sanitize product_image URLs
sanitizeProductImages(products: any[]): any[] {
  return products.map(product => {
    if (product.product_image) {
      // Sanitize the product_image URL
      product.product_image = this.sanitizeImageUrl(product.product_image);
    }
    return product;
  });
}

// Method to sanitize image URLs
sanitizeImageUrl(url: string): SafeUrl {
  return this.sanitizer.bypassSecurityTrustUrl(url);
}
  onChange2(event: any) {
    this.file = event.target.files[0];
    // var api='Employeelist/EmployeeProfileUpload'
    // //console.log(this.file)
    //   this.service.EmployeeProfileUpload(api,this.file).subscribe((result:any) => {
    //     this.responsedata=result;
    //   });
  }
  onedit(params: any) {

    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/smr/SmrMstProductEdit', encryptedParam])
  }
  onview(params: any) {

    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/smr/SmrMstProductView', encryptedParam])
  }
  taxSegMap(params: any)
  {
    
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/smr/SmrMstMapTaxSegment', encryptedParam])


  }
  onadd() {
    this.router.navigate(['/smr/SmrMstProductAdd'])

  }
  openModaldelete(parameter: string) {
    this.parameterValue = parameter

  }

  // public validate(): void {
  //   this.employee = this.reactiveForm.value;
  //   let formData = new FormData();
  //   if(this.file !=null &&  this.file != undefined){

  //   formData.append("file", this.file,this.file.name);
  //   var api='Product/ProductimageUpload'

  //   this.service.postfile(api,formData).subscribe((result:any) => {
  //     this.responsedata=result;
  //     if(result.status ==false){
  //       this.ToastrService.warning(result.message)
  //     }
  //     else{
  //       this.reactiveForm.reset();
  //       this.ToastrService.success(result.message)
  //     }
  //   });
  // }
  //   else{
  //     var api7='Product/Postimage'
  //     //console.log(this.file)
  //       this.service.post(api7,this.employee).subscribe((result:any) => {

  //         if(result.status ==false){
  //           this.ToastrService.warning(result.message)
  //         }
  //         else{
  //           this.router.navigate(['/smr/SmrMstProductsummary']);
  //           this.ToastrService.success(result.message)
  //         }
  //         this.responsedata=result;
  //       });
  //   }
  // }
  ondelete() {
    console.log(this.parameterValue);
    var url = 'SmrMstProduct/GetDeleteSalesProductdetails'
    let param = {
      product_gid: this.parameterValue
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
      this.NgxSpinnerService.show();





    });
  }
  onclose() {
    window.location.reload();

  }

  importexcel() {
    let formData = new FormData();
    if (this.file != null && this.file != undefined) {
      window.scrollTo({
        top: 0, // Code is used for scroll top after event done
      });
      formData.append("file", this.file, this.file.name);
      var api = 'SmrMstProduct/ProductImportExcel'
      this.service.postfile(api, formData).subscribe((result: any) => {
        this.response_data = result;
      if( result.status == false)
      {
        this.response_data = result;
        this.ToastrService.warning("Excel Import Un Successfull! Kindly Check the Import Log For Details!")

      }
      else
      {
        window.location.reload();
        this.ToastrService.success(result.message)
      }
      });
    }
  }
  downloadfileformat() {

    let link = document.createElement("a");
    link.download = "Sales ProductExcel";
    window.location.href = "https://" + environment.host + "/Templates/Sales ProductExcel.xls";
    link.click();
  }
  toggleStatus(data: any) {

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
    var url3 = 'SmrMstProduct/GetcustomerInactive'
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
    var url3 = 'SmrMstProduct/GetcustomerActive'
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

  ProductexportExcel() {
debugger
    var api7 = 'SmrMstProduct/GetProductReportExport';
    this.service.generateexcel(api7).subscribe((result: any) => {
        this.responsedata = result;

        if (this.responsedata && this.responsedata.productexport_list && this.responsedata.productexport_list.length > 0) {
            var phyPath = this.responsedata.productexport_list[0].lspath1;
            var relPath = phyPath.split("EMS_Base");
            var hosts = environment.host;
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
  // Product Image Upload
  myModaladddetails(parameter: string) {
    this.parameterValue = parameter
    this.reactiveForm.get("product_gid")?.setValue(this.parameterValue.product_gid);

  }
   onsubmit(): void {
    debugger
    this.employee = this.reactiveForm.value;
    let formData = new FormData();
    if(!this.file){
      this.ToastrService.warning('Please select a file');
      return;
    }
    if (this.file != null && this.file != undefined) {
      formData.append("file", this.file, this.file.name);
      formData.append("product_gid", this.employee.product_gid);
     
      var api7 = 'SmrMstProduct/ProductImage'
      this.NgxSpinnerService.show();
      this.service.postfile(api7, formData).subscribe((result: any) => {
        if (result.status == false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.GetProductSummary()
        }
        else {
          // this.router.navigate(['/crm/CrmMstProductsummary']);
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.GetProductSummary()
         

        }

        this.responsedata = result;


      });

    }
  }

  //for Filter Option
  productSearch() {
    try {
      var params = {
        producttype_gid: this.productform.value.producttype_name
      }

      var api = 'SmrMstProduct/GetFilterProduct';
      this.service.getparams(api, params).subscribe((result: any) => {
        this.responsedata = result;
        this.products = this.responsedata.product_list;
      });
    }
    catch (error) {
      console.log(error)
    }
  }
  // onClearProductSummary(){
  //   this.mdlproducttype='';
  //   var api = 'SmrMstProduct/GetSalesProductSummary';
  //   this.service.get(api).subscribe((result: any) => {
  //     this.response_data = result;
  //     this.products = this.response_data.product_list;
  //   });
  // }
  importlog()
  {
    var api = 'SmrMstProduct/GetImportLog';
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
    var url = 'SmrMstProduct/DeleteLog'
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
}





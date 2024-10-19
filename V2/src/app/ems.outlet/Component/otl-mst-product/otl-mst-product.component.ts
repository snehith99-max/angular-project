import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { ExcelService } from 'src/app/Service/excel.service';
import { Subscription, Observable } from 'rxjs';
import { first } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { environment } from 'src/environments/environment';
import { NgxSpinnerService } from 'ngx-spinner';
interface IEmployee {

}
interface IProduct {
  product_gid: string;

}

@Component({
  selector: 'app-otl-mst-product',
  templateUrl: './otl-mst-product.component.html',
  styleUrls: ['./otl-mst-product.component.scss']
})
export class OtlMstProductComponent {

  employee!: IEmployee;

  private unsubscribe: Subscription[] = [];
  file!: File;
  reactiveForm!: FormGroup;
  responsedata: any;
  parameterValue: any;
  fileInputs: any;
  products: any[] = [];
  response_data: any;
  showOptionsDivId: any;
  product!: IProduct;
  parameterValue1 : any;
  constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private excelService: ExcelService, private service: SocketService, private ToastrService: ToastrService, private NgxSpinnerService: NgxSpinnerService,) { }

  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }

  ngOnInit(): void {
    this.GetProductSummary();

    this.reactiveForm = new FormGroup({
      file: new FormControl(''),
      fileExtension: new FormControl(''),
      fileName: new FormControl(''),
      imagePath: new FormControl(''),
      product_gid: new FormControl('')
    });
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
  }
  GetProductSummary() {

    var api = 'OtlMstProduct/GetProductSummary';
    this.NgxSpinnerService.show();
    this.service.get(api).subscribe((result: any) => {
      $('#products').DataTable().destroy();
      this.response_data = result;
      this.products = this.response_data.Products_list;
      setTimeout(() => {
        $('#products').DataTable();
      }, 1);
    
      this.NgxSpinnerService.hide();
    });

  }
  onChange2(event: any) {
    this.file = event.target.files[0];

  }
  onedit(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/outlet/OtlMstProductEdit', encryptedParam])
  }
  onview(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/outlet/OtlMstProductview', encryptedParam])
  }

  onadd() {
    this.router.navigate(['/outlet/OtlMstProductAdd'])

  }
  openModaldelete(parameter: string) {
    this.parameterValue = parameter

  }

  public validate(): void {
    this.employee = this.reactiveForm.value;
    let formData = new FormData();
    if (this.file != null && this.file != undefined) {

      formData.append("file", this.file, this.file.name);
      var api = 'Product/ProductimageUpload'

      this.service.postfile(api, formData).subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          this.ToastrService.warning(result.message)
        }
        else {
          this.reactiveForm.reset();
          this.ToastrService.success(result.message)
        }
      });
    }
    else {
      var api7 = 'Product/Postimage'
      this.service.post(api7, this.employee).subscribe((result: any) => {

        if (result.status == false) {
          this.ToastrService.warning(result.message)
        }
        else {
          this.router.navigate(['/crm/CrmMstProductsummary']);
          this.ToastrService.success(result.message)
        }
        this.responsedata = result;
      });
    }
  }
  ondelete() {
    this.NgxSpinnerService.show()
    var url = 'Product/GetDeleteProductdetails'
    let param = {
      product_gid: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide();
      }
      else {

        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
        this.GetProductSummary();
      }



    });
  }
  onclose() {
    window.location.reload();

  }


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
          this.fileInputs = null;
          this.ToastrService.success("Excel Uploaded Successfully")

        }
        this.GetProductSummary();

      });
    }
  }

  ProductexportExcel() {
    const ProductExcel = this.products.map(item => ({
      ProductCode: item.product_code || '',
      Product: item.product_name || '',
      Description : item.product_desc || '',
      SellingPrice: item.cost_price || '',
     
    }));


    this.excelService.exportAsExcelFile(ProductExcel, 'Product_Excel');

  }


  downloadfileformat() {

    let link = document.createElement("a");
    link.download = "Product Template";
    link.href = "assets/media/Excels/producttemplate/product.xlsx";
    link.click();
  }
  onChange1(event: any) {
    this.file = event.target.files[0];
  }
  taxSegMap(params: any) {

    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/smr/SmrMstMapTaxSegment', encryptedParam])


  }

  // image
  myModaladddetails(parameter: string) {
    this.parameterValue1 = parameter
    this.reactiveForm.get("product_gid")?.setValue(this.parameterValue1.product_gid);   

  }
  public onsubmit(): void {
    console.log(this.reactiveForm.value)
    this.product = this.reactiveForm.value;
    let formData = new FormData();
    if (this.file != null && this.file != undefined) {
      formData.append("file", this.file, this.file.name);
      formData.append("product_gid", this.product.product_gid);
      this.NgxSpinnerService.show();
      var api7 = 'OtlMstProduct/GetProductImage'
      this.service.postfile(api7, formData).subscribe((result: any) => {
        if(result.status ==false){
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
        }
        else{
          // this.router.navigate(['/crm/CrmMstProductsummary']);
          this.NgxSpinnerService.hide();
          this.GetProductSummary();
          this.ToastrService.success(result.message)
           window.location.reload();

        }

        this.responsedata = result;
       

      });
      
    }
  }

  
}






